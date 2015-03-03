using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.fashion.data.sync.Tmall;
using com.intime.fashion.data.sync.Tmall.Executor;
using com.intime.o2o.data.exchange.Ims;
using com.intime.o2o.data.exchange.IT;
using Common.Logging;
using Quartz;
using Top.Api;

namespace com.intime.jobscheduler.Job.Tmall.Product
{
    [DisallowConcurrentExecution]
    public class InventorySync2ImsJob:IJob
    {
        private ITopClient _client;
        private string _sessionKey;
        private static string CONSUMER_KEY = ConfigurationManager.AppSettings["CONSUMER_KEY"] ?? "intime";
        private ILog _logger = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
#if DEBUG
            var pageSize = 20;
            int interval = 15;
#else
            JobDataMap data = context.JobDetail.JobDataMap;
            var pageSize = data.ContainsKey("pageSize") ? data.GetInt("pageSize") : 20;
            var interval = data.ContainsKey("intervalOfMins") ? data.GetInt("intervalOfMins") : 15;
#endif
            var benchTime = DateTime.Now.AddMinutes(-interval);

            IApiClient imsClient = new ImsApiClient(ConstValue.IMS_SERVICE_URL, ConstValue.IMS_APP_KEY, ConstValue.IMS_APP_SECRET);
            _logger.Info("Begin sync inventory to ims");
            var executor = new ItemSyncExecutor(benchTime, pageSize, imsClient);
            executor.Execute();
            _logger.Info("End sync!");
        }
    }
}
