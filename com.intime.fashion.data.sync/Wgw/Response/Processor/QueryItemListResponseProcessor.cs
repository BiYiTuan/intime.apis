using System;
using System.Linq;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync.Wgw.Response.Processor
{
    public class QueryItemListResponseProcessor : IProcessor
    {
        public bool Process(dynamic response, dynamic otherInfo=null)
        {
            if (response.errorCode != 0)
            {
                this.ErrorMessage = response.errorMessage;
                return false;
            }
            try
            {
                if (otherInfo != null)
                {
                    return BackupProducts(response);
                }
                Sync(response);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        private bool BackupProducts(dynamic rsp)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                foreach (var item in rsp.o2oItemInfos)
                {
                    string itemId = item.itemId;

                    if(db.Set<MappedProductBackupEntity>().Any(t=>t.ChannelProductId == itemId))
                        continue;

                    db.Set<MappedProductBackupEntity>().Add(new MappedProductBackupEntity()
                    {
                        Channel = ConstValue.WGW_CHANNEL_NAME,
                        ChannelProductId = itemId,
                        CreateDate = DateTime.Now,
                        ProductId = item.stockId,
                        UpdateDate = DateTime.Now,
                       
                    });
                    db.SaveChanges();
                }
            }
            return true;
        }

        private void Sync(dynamic response)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                foreach (var item in response.o2oItemInfos)
                {
                    string itemId = item.itemId;

                    int productId = int.Parse(item.stockId.ToString());
                    var map = db.Map4Product.FirstOrDefault(
                        t => t.Channel == ConstValue.WGW_CHANNEL_NAME && t.ChannelProductId == itemId);
                    if (map == null)
                    {
                        db.Map4Product.Add(new Map4ProductEntity()
                        {
                            Channel = ConstValue.WGW_CHANNEL_NAME,
                            ChannelProductId = itemId,
                            CreateDate = DateTime.Now,
                            ProductId = productId,
                            UpdateDate = DateTime.Now,
                            IsImageUpload = 0,
                            Status = item.itemState
                        });
                    }
                    else
                    {
                        map.Status = item.itemState;
                        map.UpdateDate = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }
        }

        public string ErrorMessage
        {
            get; private set;
        }
    }
}
