using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.fashion.data.sync.Tmall.Executor;
using com.intime.o2o.data.exchange.Ims;
using com.intime.o2o.data.exchange.IT;
using com.intime.o2o.data.exchange.Tmall.Core;
using com.intime.o2o.data.exchange.Tmall.Core.Support;
using Common.Logging;
using Quartz;
using Top.Api;
using ConstValue = com.intime.fashion.data.sync.Tmall.ConstValue;

namespace com.intime.jobscheduler.Job.Tmall.Order
{
    public class LogisticInfo2TmallJob : IJob
    {
        private ITopClient _client;
        private string _sessionKey;
        private static string CONSUMER_KEY = ConfigurationManager.AppSettings["CONSUMER_KEY"] ?? "intime";
        private ILog _logger;
        public LogisticInfo2TmallJob()
        {
            ITopClientFactory factory = new DefaultTopClientFactory();
            _logger = LogManager.GetCurrentClassLogger();
            _client = factory.Get(CONSUMER_KEY);
            _sessionKey = factory.GetSessionKey(CONSUMER_KEY);
        }
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

            var logisticExecutor = new LogisticsExecutor(benchTime, pageSize, _client, imsClient, _sessionKey);

            try
            {
                logisticExecutor.Execute();
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Error(e);
            }
        }
    }
}
