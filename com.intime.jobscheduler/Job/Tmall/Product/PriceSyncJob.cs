using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.o2o.data.exchange.Tmall.Core;
using com.intime.o2o.data.exchange.Tmall.Core.Support;
using com.intime.o2o.data.exchange.Tmall.Product.Models;
using Common.Logging;
using Quartz;
using Top.Api;
using Top.Api.Request;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.jobscheduler.Job.Tmall.Product
{
    [DisallowConcurrentExecution]
    public class PriceSyncJob : IJob
    {
        private ITopClient _client;
        private string _sessionKey;
        private static string CONSUMER_KEY = ConfigurationManager.AppSettings["CONSUMER_KEY"] ?? "intime";
        private ILog _logger;

        public PriceSyncJob()
        {
            ITopClientFactory factory = new DefaultTopClientFactory();
            _logger = LogManager.GetCurrentClassLogger();
            _client = factory.Get(CONSUMER_KEY);
            _sessionKey = factory.GetSessionKey(CONSUMER_KEY);
        }

        public void Execute(IJobExecutionContext context)
        {
            int total = 0;
            int succeedCount = 0;

            int cursor = 0;
#if !DEBUG
            JobDataMap data = context.JobDetail.JobDataMap;
            int size = data.ContainsKey("pageSize") ? data.GetInt("pageSize") : 20;
            int interval = data.ContainsKey("intervalOfMins") ? data.GetInt("intervalOfMins") : 20;
#else
            int size = 30;
            int interval = 100;
#endif
            var benchTime = DateTime.Now.AddMinutes(-interval);
            DoQuery(benchTime, items => total = items.Count());
            while (cursor < total)
            {
                List<Map4InventoryEntity> oneTimeList = null;
                DoQuery(benchTime, skus =>
                {
                    oneTimeList = skus.Where(a => a.Id > cursor).OrderBy(a => a.Id).Take(size).ToList();
                });

                succeedCount += oneTimeList.Count(SyncItemPrice);
                cursor += size;
            }
        }

        private bool SyncItemPrice(Map4InventoryEntity arg)
        {
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var items = from product in db.Set<ProductEntity>()
                            from stock in db.Set<InventoryEntity>()
                            from map in db.Set<Map4InventoryEntity>()
                            where product.Price != map.price && product.Id == stock.ProductId && stock.Id == map.InventoryId
                            select new { Price = product.Price, Amount = stock.Amount, map };
                if (!items.Any()) return false;
                bool allSucceed = true;
                foreach (var item in items)
                {
                    if (!SyncSku(item.Price, item.Amount, item.map))
                    {
                        allSucceed = false;
                    }
                }
                return allSucceed;
            }
        }

        private bool SyncSku(decimal price, int amount, Map4InventoryEntity map)
        {
            var p = price.ToString();
            var req = new ItemSkuPriceUpdateRequest
            {
                NumIid = Convert.ToInt64(map.itemId),
                OuterId = map.ProductId.ToString(),
                Price = p,
                Properties = map.saleAttr,
                Quantity = amount,
                ItemPrice = p
            };

            var rsp = _client.Execute(req, _sessionKey);

            if (rsp.IsError)
            {
                return false;
            }
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var entity = db.Set<Map4InventoryEntity>().FirstOrDefault(x => x.Id == map.Id);
                entity.price = price;
                entity.num = amount;
                entity.UpdateDate = DateTime.Now;
                db.SaveChanges();
            }
            return true;
        }


        private void DoQuery(DateTime benchTime, Action<IQueryable<Map4InventoryEntity>> callback)
        {
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = from product in db.Set<ProductEntity>()
                           from stock in db.Set<InventoryEntity>()
                           from map in db.Set<Map4InventoryEntity>()
                           where product.UpdatedDate > benchTime && product.Price != map.price && product.Id == stock.ProductId && stock.Id == map.InventoryId
                           select map;

                if (callback != null)
                {
                    callback(linq);
                }
            }
        }

    }
}
