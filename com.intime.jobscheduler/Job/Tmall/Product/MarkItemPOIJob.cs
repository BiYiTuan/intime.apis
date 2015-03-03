using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using com.intime.o2o.data.exchange.Tmall.Core;
using com.intime.o2o.data.exchange.Tmall.Core.Support;
using com.intime.o2o.data.exchange.Tmall.Product.Models;
using Common.Logging;
using log4net.Repository.Hierarchy;
using Quartz;
using Quartz.Impl.Matchers;
using Top.Api;
using Top.Api.Request;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.jobscheduler.Job.Tmall.Product
{
    [DisallowConcurrentExecution]
    public class MarkItemPOIJob : IJob
    {
        private ITopClient _client;
        private string _sessionKey;
        private static string CONSUMER_KEY = ConfigurationManager.AppSettings["CONSUMER_KEY"] ?? "intime";
        private ILog _logger;
        public MarkItemPOIJob()
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
            DoQuery(items => total = items.Count());
            int cursor = 0;
#if !DEBUG
            JobDataMap data = context.JobDetail.JobDataMap;
            int size = data.ContainsKey("pageSize") ? data.GetInt("pageSize") : 20;
#else
            int size = 30;
#endif
            while (cursor < total)
            {
                List<ProductPoolEntity> oneTimeList = null;
                DoQuery(products =>
                {
                    oneTimeList = products.Where(a => a.Id > cursor).OrderBy(a => a.Id).Take(size).ToList();
                });

                succeedCount += oneTimeList.Count(MarkPoi);
                cursor += size;
            }
        }

        private bool MarkPoi(ProductPoolEntity entity)
        {
            using (var db = getDbContext())
            {
                var products = from p in db.Set<ProductEntity>()
                               from i in db.Set<InventoryEntity>()
                               from m in
                                   (from inventory in db.Set<InventoryEntity>()
                                    from map in db.Set<Map4InventoryEntity>()
                                    where
                                        map.Channel == "tmall" && inventory.Id == map.InventoryId &&
                                        inventory.ProductId == entity.ProductId
                                    select map)
                               from map in db.Set<Map4InventoryEntity>()
                               from map4store in db.Set<Map4StoreEntity>()
                               where
                                   p.Id == i.ProductId && i.Id == map.InventoryId && map.itemId == m.itemId &&
                                   map4store.StoreId == p.Store_Id
                               select new { ItemId = m.itemId, StoreId = map4store.ChannelStoreId };

                var aggregatedItem = products.GroupBy(p => p.ItemId, p => p.StoreId);
                if (!aggregatedItem.Any()) return false;
                bool allSucceed = true;
                foreach (var item in aggregatedItem)
                {
                    var succeed = MarkOne(item.Key, item.ToList());
                    if (!succeed)
                    {
                        allSucceed = false;
                    }
                    else
                    {

                    }
                }
                if (allSucceed)
                {
                    var pp = db.Set<ProductPoolEntity>().FirstOrDefault(x => x.Id == entity.Id);
                    pp.POIUploaded = true;
                    pp.UpdateDate = DateTime.Now;
                    db.SaveChanges();
                }
                return allSucceed;
            }
        }

        private bool MarkOne(string item, IEnumerable<string> storeIds)
        {
            var req = new O2oItemStoresBindRequest
            {
                ItemId = item,
                StoreIds = string.Format("['{0}']", string.Join("','", storeIds))
            };
            var rsp = _client.Execute(req, _sessionKey);
            return !rsp.IsError;
        }


        private void DoQuery(Action<IQueryable<ProductPoolEntity>> callback)
        {
            using (var db = getDbContext())
            {
                var linq =
                    db.Set<ProductPoolEntity>()
                        .Where(x => !x.POIUploaded && x.Status == (int)ProductPoolStatus.AddItemFinished);
                if (callback != null)
                {
                    callback(linq);
                }
            }
        }

        private YintaiHangzhouContext getDbContext()
        {
            return new YintaiHangzhouContext("YintaiHangzhouContext");
        }
    }
}
