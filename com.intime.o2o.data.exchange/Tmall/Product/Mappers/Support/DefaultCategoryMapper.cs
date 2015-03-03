using System;
using System.Linq;
using com.intime.o2o.data.exchange.Tmall.Core;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.o2o.data.exchange.Tmall.Product.Mappers.Support
{
    public class DefaultCategoryMapper : ICategoryMapper
    {
        public long? ToChannel(int? innerId)
        {
            if (!innerId.HasValue)
            {
                return null;
            }

            using (var db = new YintaiHangzhouContext())
            {
                var innerIdStr = innerId.ToString();
                var extCategory = db.Map4Category.FirstOrDefault(c => c.CategoryCode == innerIdStr && c.Channel == ConstValue.Tmall);
                return extCategory == null ? null : extCategory.ChannelCategoryId.ConvertTo<long>();
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
                var extCategory =
                    db.Map4Category.FirstOrDefault(c => c.ChannelCategoryId == outerId.Value && c.Channel == ConstValue.Tmall);

                return extCategory == null ? null : extCategory.CategoryCode.ConvertTo<int>();
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
                db.Map4Category.Add(new Map4CategoryEntity()
                {
                    CategoryCode = innerId.ToString(),
                    ChannelCategoryId = (int)outerId.Value,
                    Channel = ConstValue.Tmall,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
                db.SaveChanges();
            }
        }
    }
}
