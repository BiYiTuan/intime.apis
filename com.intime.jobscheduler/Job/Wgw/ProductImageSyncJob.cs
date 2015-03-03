using com.intime.fashion.data.sync.Wgw.Executor;
using Common.Logging;
using Quartz;
using System;

namespace com.intime.jobscheduler.Job.Wgw
{
    public class ProductImageSyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
#if DEBUG
            DateTime benchTime = DateTime.Now.AddYears(-1);
#else
            JobDataMap data = context.JobDetail.JobDataMap;
            var interval = data.ContainsKey("intervalOfHrs") ? data.GetInt("intervalOfHrs") : 1;
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
                var productSyncExecutor = new ProductImageSyncExecutor(benchTime, logger);
                executeInfo = productSyncExecutor.Execute();
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
                logger.Info(string.Format("Sync order succeed : {0}, failed :{1}, total: {2}, elapsed:{3}, ratio: {4}",
                    executeInfo.SucceedCount,
                    executeInfo.FailedCount,
                    executeInfo.TotalCount,
                    analytic.TotalSeconds,
                    executeInfo.SucceedCount / analytic.TotalSeconds));
            }
        }
    }
}
