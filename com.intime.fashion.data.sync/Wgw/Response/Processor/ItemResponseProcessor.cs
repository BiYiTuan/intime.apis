using System;
using System.Linq;
using Yintai.Hangzhou.Data.Models;
using TransactionScope = System.Transactions.TransactionScope;

namespace com.intime.fashion.data.sync.Wgw.Response.Processor
{
    public class ItemResponseProcessor : IProcessor
    {
        public bool Process(dynamic response,dynamic otherInfo)
        {
            if (response.errorCode != 0)
            {
                this.ErrorMessage = response.errorMessage;
                return false;
            }
            this.MarkItem(otherInfo, response.itemInfo);
            return true;
        }

        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 新同步到微购物的商品需要标记到数据库表里
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="itemInfo"></param>
        private void MarkItem(int productId, dynamic itemInfo)
        {
            using (var tran = new TransactionScope())
            {
                using (var db = DbContextHelper.GetDbContext())
                {
                    string itemId = itemInfo.itemId.ToString();
                    var mapping =
                        db.Map4Product.FirstOrDefault(
                            m =>
                                m.ProductId == productId && m.ChannelProductId == itemId &&
                                m.Channel == ConstValue.WGW_CHANNEL_NAME);
                    var product = db.Products.FirstOrDefault(p => p.Id == productId);
                    if (mapping == null)
                    {
                        db.Map4Product.Add(new Map4ProductEntity
                        {
                            Channel = ConstValue.WGW_CHANNEL_NAME,
                            ChannelProductId = itemId,
                            ProductId = productId,
                            //UpdateDate修改为三年前，以便后续更新以添加颜色和图片
                            UpdateDate = DateTime.Now.AddYears(-3),
                            CreateDate = DateTime.Now,
                            Status = product.Status,
                            IsImageUpload = 0
                        });
                    }
                    else
                    {
                        mapping.UpdateDate = DateTime.Now;
                    }

                    foreach (var stock in itemInfo.stockNew)
                    {

                        var skuId = (long)stock.skuId;
                        var inventoryId = (long)stock.stockId;
                        var map = db.Map4Inventory.FirstOrDefault(m =>
                            m.ProductId == productId &&
                            m.Channel == ConstValue.WGW_CHANNEL_NAME &&
                            m.skuId == skuId &&
                            m.InventoryId == inventoryId);
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
                                InventoryId = (long)stock.stockId,
                                stockId = stock.stockId,
                                soldNum = (int)stock.soldNum,
                                num = stock.num,
                                specAttr = stock.specAttr.ToString(),
                                price = (decimal)stock.price/100,
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
                            map.price = (decimal)stock.price/100;
                            map.status = (int)stock.status;
                            map.saleAttr = stock.saleAttr;
                            map.pic = stock.pic;
                        }

                    }

                    db.SaveChanges();
                }
                tran.Complete();
            }
        }

    }
}
