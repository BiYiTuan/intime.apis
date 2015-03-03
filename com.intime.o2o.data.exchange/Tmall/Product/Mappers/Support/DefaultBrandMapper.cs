using System;
using System.Linq;
using com.intime.o2o.data.exchange.Tmall.Core;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.o2o.data.exchange.Tmall.Product.Mappers.Support
{
    public class DefaultBrandMapper : IBrandMapper
    {
        public long? ToChannel(int? innerId)
        {
            if (!innerId.HasValue)
            {
                return null;
            }

            using (var db = new YintaiHangzhouContext())
            {
                var extBrand = db.Map4Brand.FirstOrDefault(
                    b => b.BrandId == innerId
                        && b.Channel == ConstValue.Tmall
                    );
                return extBrand == null ? null : extBrand.ChannelBrandId.ConvertTo<long>();
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

                var outerIdStr = outerId.ToString();
                var extBrand = db.Map4Brand.FirstOrDefault(
                    b => b.ChannelBrandId == outerIdStr
                     && b.Channel == ConstValue.Tmall);
                return extBrand == null ? null : extBrand.BrandId.ConvertTo<int>();
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
                db.Map4Brand.Add(new Map4BrandEntity()
                {
                    ChannelBrandId = outerId.ToString(),
                    BrandId = innerId.Value,
                    Channel = ConstValue.Tmall,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
                db.SaveChanges();
            }
        }
    }
}
