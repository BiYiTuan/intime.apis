
using System.Linq;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.o2o.data.exchange.Tmall.Product.Tools
{
    public class StoreTool
    {
        /// <summary>
        /// 根据门店Id,获取门店的省信息
        /// </summary>
        /// <param name="storeId">门店Id</param>
        /// <returns>门店省市信息</returns>
        public Location GetLocation(int storeId)
        {
            using (var db = new YintaiHangzhouContext())
            {
                var extMap4Store = db.Map4Store.FirstOrDefault(s => s.StoreId == storeId);
                return extMap4Store == null ? new Location() : new Location()
                {
                    Prov = extMap4Store.Province,
                    City = extMap4Store.City
                };
            }
        }
    }

    public class Location
    {
        public string Prov { get; set; }
        public string City { get; set; }
    }
}
