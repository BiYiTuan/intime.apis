using com.intime.fashion.data.sync.Wgw.Request.Item;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder.Stock
{
    public class StockBuilder
    {
        private readonly ISyncRequest _request;
        private readonly ProductEntity _item;
        public StockBuilder(ISyncRequest request, ProductEntity item)
        {
            this._request = request;
            this._item = item;
        }

        /// <summary>
        /// 构建库存参数
        /// </summary>
        public void BuildStockInfo(string stockName,bool buildTotalNumber = true)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                var inventories = db.Inventories.Where(i => i.ProductId == _item.Id);
                var stocks = new List<dynamic>();

                foreach (var inventory in inventories)
                {
                    Map4InventoryEntity inventoryMap = db.Map4Inventory.FirstOrDefault(
                        m => m.InventoryId == inventory.Id && m.Channel == ConstValue.WGW_CHANNEL_NAME && m.ProductId == inventory.ProductId);
                    var colorEntity = db.ProductPropertyValues.FirstOrDefault(t => t.Id == inventory.PColorId);
                    var sizeEntity = db.ProductPropertyValues.FirstOrDefault(t => t.Id == inventory.PSizeId);
                    var color = db.ProductProperties.FirstOrDefault(t => t.Id == colorEntity.PropertyId);
                    var size = db.ProductProperties.FirstOrDefault(t => t.Id == sizeEntity.PropertyId);

                    if (colorEntity == null || color == null || sizeEntity == null || size == null)
                    {
                        continue;
                    }

                    var saleAttr = string.Format("{0}:{1}|{2}:{3}",
                        colorEntity.PropertyId,
                        colorEntity.Id,
                        sizeEntity.PropertyId,
                        sizeEntity.Id);

                    var attr = string.Format("{0}:{1}|{2}:{3}",
                        color.PropertyDesc,
                        colorEntity.ValueDesc,
                        size.PropertyDesc,
                        sizeEntity.ValueDesc);
                    stocks.Add(new
                    {
                        stockId = inventory.Id,
                        price = (int)Math.Ceiling(_item.Price * 100),
                        num = inventory.Amount,
                        desc = attr,
                        saleAttr,
                        attr,
                        specAttr = string.Empty,
                        status = (int)(_item.Is4Sale.HasValue && _item.Is4Sale.Value && _item.Status == 1 && inventory.Amount > 0 ? StockStatus.IS_FOR_SALE : StockStatus.IS_IN_STORE),
                        itemId = inventoryMap == null ? null : inventoryMap.itemId,
                        skuId = inventoryMap == null ? 0 : inventoryMap.skuId,
                    });
                }
                if (stocks.Count > 0)
                {
                    _request.Put(stockName, JsonConvert.SerializeObject(new { stockJsonStr = stocks }));
                }
                if (buildTotalNumber)
                {
                    _request.Put("num", inventories.Any() ? inventories.Sum(q => q.Amount) : 0);
                }
            }
        }
    }
}
