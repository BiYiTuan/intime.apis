using com.intime.fashion.data.sync.Wgw.Request.Builder;
using com.intime.fashion.data.sync.Wgw.Request.Item;
using Common.Logging;
using System;
using System.Linq;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync.Wgw.Executor
{
    public class InventorySyncExecutor : ExecutorBase
    {
        private int _failedCount;
        private int _succeedCount;
        private int _totalCount;
        public InventorySyncExecutor(DateTime benchTime, ILog logger)
            : base(benchTime, logger)
        {
        }
        
        /// <summary>
        /// 成功数量
        /// </summary>
        protected override int SucceedCount
        {
            get { return _succeedCount; }
        }

        /// <summary>
        /// 失败数量
        /// </summary>
        protected override int FailedCount
        {
            get { return _failedCount; }
        }

        /// <summary>
        /// 总数
        /// </summary>
        protected override int TotalCount
        {
            get { return _totalCount; }
        }

        protected override void ExecuteCore(dynamic extraParameter = null)
        {
            SyncItemInventoriesDifferInStockRecordsCount();
            SyncInventoryDifferInAmount();
        }

        /// <summary>
        /// 更新数量不一致的库存
        /// </summary>
        private void SyncInventoryDifferInAmount()
        {
            #region
            using (var db = DbContextHelper.GetDbContext())
            {
                var stocks =
                    db.Inventories.Where(i => i.UpdateDate >= BenchTime)
                        .Join(db.Products.Where(p => p.IsHasImage && p.Is4Sale.HasValue && p.Is4Sale.Value),
                            i => i.ProductId, p => p.Id, (i, p) => new { i.Amount, p.Price, i.UpdateDate, i.Id })
                        .Join(db.Map4Inventory.Where(m => m.Channel == ConstValue.WGW_CHANNEL_NAME), s => s.Id,
                            m => m.InventoryId, (s, m) => new { s, m })
                        .Where(s => s.s.UpdateDate > s.m.UpdateDate || s.s.Amount != s.m.num || s.s.Price != s.m.price)
                        .Select(m => new
                        {
                            m.m.itemId,
                            m.m.skuId,
                            m.s.Amount,
                            m.s.Price,
                            m.m.Id
                        });
                foreach (var stock in stocks)
                {
                    var price = Math.Ceiling(stock.Price * 100); //微购物价格单位为"分"
                    var num = stock.Amount;
                    var mapId = stock.Id;
                    try
                    {
                        var updateStockRequest = new UpdateItemStockRequest(stock.itemId, stock.skuId);
                        updateStockRequest.Put("num", stock.Amount);
                        updateStockRequest.Put("price", price);
                        var result = Client.Execute<dynamic>(updateStockRequest);
                        if (result.errorCode == 0)
                        {
                            UpdateInventoryMap(mapId, price, num);
                            _succeedCount += 1;
                        }
                        else
                        {
                            _failedCount += 1;
                            Logger.Error(string.Format("Failed to update stock {0}(stockID),Error message from wgw:{1}", mapId,
                                result.errorMessage));
                        }
                    }
                    catch (Exception ex)
                    {
                        _failedCount += 1;
                        Logger.Error(string.Format("Failed to update stock {0}(stock ID),Error Message:{1}", mapId,
                            ex.Message));
                    }
                }
            }

            _totalCount = _succeedCount + _failedCount;


            #endregion
        }

        /// <summary>
        /// 同步单品库存记录数和微购物不一致的商品库存
        /// </summary>
        private void SyncItemInventoriesDifferInStockRecordsCount()
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                var pIds = db.Inventories.Where(i=>i.Amount > 0).GroupBy(t => t.ProductId)
                        .Select(o => new {iid = o.Key, countOfLocalStockRecords = o.Count()})
                        .Join(
                            db.Map4Inventory.Where(t => t.Channel == ConstValue.WGW_CHANNEL_NAME)
                                .GroupBy(t => t.ProductId)
                                .Select(g => new { pid = g.Key, countOfWgwStockRecords = g.Count()}), i => i.iid,
                            m => m.pid,
                            (o, m) =>
                                new
                                {
                                    //o.iid,
                                    o.countOfLocalStockRecords,
                                    m.pid,
                                    m.countOfWgwStockRecords
                                })
                        .Where(r => r.countOfLocalStockRecords > r.countOfWgwStockRecords)
                        .Select(r => r.pid)
                        .Union(
                            db.Products.Where(
                                p =>
                                    p.IsHasImage && p.Is4Sale.HasValue && p.Is4Sale.Value &&
                                    db.Map4Product.Any(
                                        m => m.Channel == ConstValue.WGW_CHANNEL_NAME && m.ProductId == p.Id) &&
                                    !db.Map4Inventory.Any(
                                        m => m.ProductId == p.Id && m.Channel == ConstValue.WGW_CHANNEL_NAME))
                                .Select(p => p.Id));
                foreach (var pid in pIds)
                {
                    var request = new UpdateItemMultiStockRequest(null);
                    var builder = RequestParamsBuilderFactory.CreateBuilder(request);
                    var rsp = Client.Execute<dynamic>(builder.BuildParameters(pid));
                    if (rsp.errorCode == 0)
                    {
                        Process(rsp.returnInfo, pid);
                    }
                    else
                    {
                        Logger.Info(string.Format("Failed to update stock (id) = {0},Error Message:{1}", pid, rsp.errorMessage));
                    }
                }
            }
        }

        private void Process(dynamic returnInfo, int productId)
        {
            ProcessSuccessList(returnInfo.successList, productId);
            ProcessFailList(returnInfo.failList);
        }

        private void ProcessFailList(dynamic failList)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                foreach (var stock in failList)
                {
                    string itemId = stock.itemId;
                    var inventoryId = (long)stock.stockId;
                    var inventory =
                        db.Map4Inventory.FirstOrDefault(
                            m =>
                                m.Channel == ConstValue.WGW_CHANNEL_NAME && m.InventoryId == inventoryId &&
                                m.itemId == itemId);
                    if (inventory != null)
                    {
                        db.Map4Inventory.Remove(inventory);
                    }
                    _failedCount += 1;
                    Logger.Error(string.Format("Failed to update stock, stock ID={0}", stock.stockId));
                }
                db.SaveChanges();
            }
        }

        private void ProcessSuccessList(dynamic successList, int productId)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                foreach (var stock in successList)
                {
                    string itemId = stock.itemId;
                    try
                    {
                        var skuId = (long)stock.skuId;
                        var inventoryId = (long)stock.stockId;
                        var map =
                            db.Map4Inventory.FirstOrDefault(
                                m =>
                                    m.Channel == ConstValue.WGW_CHANNEL_NAME && m.itemId == itemId && m.InventoryId == inventoryId);
                        if (map == null)
                        {
                            db.Map4Inventory.Add(new Map4InventoryEntity
                            {
                                ProductId = productId,
                                itemId = itemId,
                                attr = stock.attr,
                                Channel = ConstValue.WGW_CHANNEL_NAME,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                desc = stock.desc.ToString(),
                                InventoryId = inventoryId,
                                stockId = stock.stockId,
                                soldNum = (int)stock.soldNum,
                                num = stock.num,
                                specAttr = stock.specAttr.ToString(),
                                price = (decimal)stock.price / 100,
                                status = (int)stock.status,
                                saleAttr = stock.saleAttr,
                                skuId = skuId,
                                pic = stock.pic
                            });
                        }
                        else
                        {
                            map.attr = stock.attr;
                            map.UpdateDate = DateTime.Now;
                            map.desc = stock.desc;
                            map.soldNum = stock.soldNum;
                            map.specAttr = stock.specAttr;
                            map.price = (decimal)stock.price / 100;
                            map.status = (int)stock.status;
                            map.saleAttr = stock.saleAttr;
                            map.pic = stock.pic;
                        }
                        db.SaveChanges();
                        _succeedCount += 1;
                    }
                    catch (Exception ex)
                    {
                        _failedCount += 1;
                        Logger.Error(string.Format("Failed to save stock mapping to Map4Inventory:stockID:{0},Error Message:{1}", stock.stockId, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="price"></param>
        /// <param name="num"></param>
        private void UpdateInventoryMap(int mapId, decimal price, int num)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                var map =
                    db.Map4Inventory.FirstOrDefault(t => t.Channel == ConstValue.WGW_CHANNEL_NAME && t.Id == mapId);
                if (map != null)
                {
                    map.price = price/100; //价格单位 : 元
                    map.num = num;
                    map.UpdateDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
    }
}
