using System;
using System.Linq;
using System.Web.Mvc;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.WebApiCore.Areas.Gg.Controllers
{
    public class StockController:GgController
    {
        [ValidateParameters]
        public ActionResult Map(dynamic request, string channel)
        {
            var db = Context;
            foreach (var item in request)
            {
                string itemId = item.num_iid.ToString();
                int productId = Convert.ToInt32(item.outer_id);
                
                foreach (var sku in item.skus)
                {
                    int inventoryId = Convert.ToInt32(sku.outer_id);
                    var map =
                        db.Set<Map4InventoryEntity>()
                            .FirstOrDefault(m => m.itemId == itemId && m.ProductId == productId && m.InventoryId == inventoryId);
                    if (map == null)
                    {
                        db.Set<Map4InventoryEntity>().Add(new Map4InventoryEntity()
                        {
                            attr = string.Empty,
                            Channel = channel,
                            ChannelId = null,
                            CreateDate = DateTime.Now,
                            desc = string.Empty,
                            InventoryId = inventoryId,
                            itemId = itemId,
                            num = sku.quantity,
                            price = sku.price,
                            ProductId = productId,
                            saleAttr = sku.properties,
                            sellerUin = 0,
                            skuId = sku.sku_id,
                            soldNum = 0,
                            specAttr = string.Empty,
                            status = 0,
                            stockId = inventoryId.ToString(),
                            UpdateDate = DateTime.Now
                        });
                    }
                    else
                    {
                        map.InventoryId = inventoryId;
                        map.UpdateDate = DateTime.Now;
                        map.num = sku.quantity;
                        map.price = sku.price;
                        map.itemId = itemId;
                        map.saleAttr = sku.properties;
                    }
                    db.SaveChanges();
                }

            }
            return this.RenderSuccess<dynamic>(r => r.Data = null);
        }
    }
}
