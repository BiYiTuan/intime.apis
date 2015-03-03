using System;
using System.Linq;
using com.intime.o2o.data.exchange.Tmall.Core;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.o2o.data.exchange.Tmall.Product.Mappers.Support
{
    public class DefaultProductMapper : IProductMapper
    {
        public long? ToChannel(int? innerId)
        {
            if (!innerId.HasValue)
            {
                return null;
            }

            using (var db = new YintaiHangzhouContext())
            {
                var extProduct = db.Map4Product.FirstOrDefault(b => b.ProductId == innerId && b.Channel == ConstValue.Tmall);
                return extProduct == null ? null : extProduct.ChannelProductId.ConvertTo<long>();
            }
        }

        public int? FromChannel(long? outerId)
        {
            if (!outerId.HasValue)
            {
                return null;
            }

            using (var db = new YintaiHangzhouContext())
            {
                var extProduct = db.Map4Product.FirstOrDefault(b => b.ChannelProductId == outerId.ToString() && b.Channel == ConstValue.Tmall);
                return extProduct == null ? null : extProduct.ProductId.ConvertTo<int>();
            }
        }

        public void Save(int? innerId, long? outerId)
        {
            if (!innerId.HasValue)
            {
                throw new ArgumentNullException("innerId");
            }

            if (!outerId.HasValue)
            {
                throw new ArgumentNullException("outerId");
            }

            using (var db = new YintaiHangzhouContext())
            {
                db.Map4Product.Add(new Map4ProductEntity()
                {
                    ChannelProductId = outerId.ToString(),
                    ProductId = innerId.Value,
                    Channel = ConstValue.Tmall,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
                db.SaveChanges();
            }
        }
    }
}
