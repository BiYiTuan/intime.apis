using com.intime.fashion.data.sync.Wgw.Request.Item;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync.Wgw.Executor
{
    public class GetItemMultiStockExecutor:ExecutorBase
    {
        private int _totalCount;
        private int _succeedCount;
        private int _failCount;
        public GetItemMultiStockExecutor(DateTime benchTime, ILog logger) : base(benchTime, logger)
        {
        }

        protected override int SucceedCount
        {
            get { return _succeedCount; }
        }

        protected override int FailedCount
        {
            get { return _failCount; }
        }

        private void Query(Expression<Func<Map4ProductEntity, bool>> whereCondition, Action<IQueryable<Map4ProductEntity>> callBack)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                var linq = db.Map4Product.Where(m => m.UpdateDate >= BenchTime && m.Channel == ConstValue.WGW_CHANNEL_NAME);
                if (whereCondition != null)
                {
                    linq = linq.Where(whereCondition);
                }
                if (callBack != null)
                {
                    callBack(linq);
                }
            }
        }

        protected override void ExecuteCore(dynamic extraParameter = null)
        {
            int pageSize = extraParameter ?? 20;
            int cursor = 0;
            int lastCursor = 0;
            Expression<Func<Map4ProductEntity, bool>> whereCondition = null;

            Query(whereCondition, items => _totalCount = items.Count());
            while (cursor < _totalCount)
            {
                List<Map4ProductEntity> oneTimeList = null;
                Query(whereCondition,
                    items =>
                        oneTimeList = items.Where(i => i.Id > lastCursor).OrderBy(i => i.Id).Take(pageSize).ToList());
                foreach (var product in oneTimeList)
                {
                    if (SyncOne(product))
                    {
                        _succeedCount ++;
                    }
                    else
                    {
                        _failCount ++;
                    }
                }
                cursor += pageSize;
                lastCursor = oneTimeList.Max(i => i.Id);
            }
        }

        private bool SyncOne(Map4ProductEntity product)
        {
            try
            {
                var getItemMultiStockRequest = new GetItemMultiStockRequest(product.ChannelProductId);
                var rsp = Client.Execute<dynamic>(getItemMultiStockRequest);
                if (rsp.errorCode == 0)
                {
                    var stocks = JsonConvert.DeserializeObject<dynamic>(rsp.newData.ToString());

                    using (var db = DbContextHelper.GetDbContext())
                    {
                        foreach (var stock in stocks.stockJsonStr)
                        {
                            int stockId;
                            if (int.TryParse(stock.stockId.ToString(), out stockId))
                            {
                                var map =
                                db.Map4Inventory.FirstOrDefault(
                                    t => t.Channel == ConstValue.WGW_CHANNEL_NAME && t.InventoryId == stockId);

                                if (map == null)
                                {
                                    db.Map4Inventory.Add(new Map4InventoryEntity()
                                    {
                                        attr = stock.attr,
                                        skuId = stock.skuId,
                                        stockId = stock.stockId,
                                        num = stock.num,
                                        price = (decimal)stock.price / 100,
                                        pic = stock.pic,
                                        desc = stock.desc,
                                        //status = stock.status,
                                        //saleAttr = stock.saleAttr,
                                        //specAttr = stock.specAttr,
                                        //soldNum = (int)stock.soldNum,
                                        
                                        Channel = ConstValue.WGW_CHANNEL_NAME,
                                        ProductId = product.ProductId,
                                        itemId = product.ChannelProductId,
                                        CreateDate = DateTime.Now,
                                        UpdateDate = DateTime.Now,
                                        InventoryId = stockId
                                    });
                                }
                                else
                                {
                                    map.skuId = stock.skuId;
                                    //map.soldNum = (int)stock.soldNum;
                                    map.UpdateDate = DateTime.Now;
                                    map.num = stock.num;
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                    return true;
                }
                Logger.Error(rsp.errorMessage);
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
            }
            return false;
        }
    }
}
