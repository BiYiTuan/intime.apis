using System;
using System.Linq;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.o2o.data.exchange.Tmall.Product.Mappers.Support
{
    public class DefaultItemMapper : IItemMapper
    {
        /// <summary>
        /// 根据商品Id获取itemId(mini银一个商品对应一个Item)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public long? ToChannel(int? productId)
        {
            using (var db = new YintaiHangzhouContext())
            {
                var extMap = db.Map4Inventory.ThenByDescending(m => m.Id).FirstOrDefault(m => m.ProductId == productId);
                return extMap == null ? 0 : Convert.ToInt64(extMap.itemId);
            }
        }

        public int? FromChannel(long? outerId)
        {
            throw new NotImplementedException();
        }

        public void Save(int? innerId, long? outerId)
        {
            throw new NotImplementedException();
        }
    }
}
