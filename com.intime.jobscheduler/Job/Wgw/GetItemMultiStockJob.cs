using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.fashion.data.sync.Wgw.Executor;
using Common.Logging;
using Quartz;

namespace com.intime.jobscheduler.Job.Wgw
{
    [DisallowConcurrentExecution]
    public class GetItemMultiStockJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
#if DEBUG
            DateTime benchTime = DateTime.Now.AddYears(-1);
#else
            JobDataMap data = context.JobDetail.JobDataMap;
            var interval = data.ContainsKey("intervalofmins") ? data.GetInt("intervalofmins") : 1;
            if (!data.ContainsKey("benchtime"))
            {
                data.Put("benchtime", DateTime.Now.AddMinutes(-interval));
            }
            else
            {
                data["benchtime"] = data.GetDateTimeValue("benchtime").AddMinutes(interval);
            }
            var benchTime = data.GetDateTime("benchtime");
#endif
            ILog logger = LogManager.GetLogger(this.GetType());
            ExecuteResult executeInfo = null;
            var analytic = ActionTimer.Perform(() =>
            {
                var inventorySyncExecutor = new GetItemMultiStockExecutor(benchTime, logger);
                executeInfo = inventorySyncExecutor.Execute();
            });


            if (executeInfo.Status != ExecuteStatus.Succeed)
            {
                foreach (var msg in executeInfo.MessageList)
                {
                    logger.Error(msg);
                }
            }
            else
            {
                logger.Info(string.Format("Sync stocks succeed : {0}, failed :{1}, total: {2}, elapsed:{3}, ratio: {4}",
                    executeInfo.SucceedCount,
                    executeInfo.FailedCount,
                    executeInfo.TotalCount,
                    analytic.TotalSeconds,
                    executeInfo.SucceedCount / analytic.TotalSeconds));
            }

        }
    }
}
