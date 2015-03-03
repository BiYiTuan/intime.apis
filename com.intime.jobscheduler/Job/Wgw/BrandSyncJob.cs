using com.intime.fashion.data.sync.Wgw.Executor;
using Common.Logging;
using Quartz;
using System;

namespace com.intime.jobscheduler.Job.Wgw
{
    [DisallowConcurrentExecution]
    public class BrandSyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
#if DEBUG
            DateTime benchTime = DateTime.Now.AddYears(-1);
#else
            JobDataMap data = context.JobDetail.JobDataMap;
            var interval = data.ContainsKey("intervalOfDays") ? data.GetInt("intervalOfDays") : 2;
            if (!data.ContainsKey("benchtime"))
            {
                data.Put("benchtime", DateTime.Now.AddDays(-interval));
            }
            else
            {
                data["benchtime"] = data.GetDateTimeValue("benchtime").AddDays(interval);
            }
            var benchTime = data.GetDateTime("benchtime");
#endif
            ILog logger = LogManager.GetLogger(this.GetType());
            ExecuteResult executeInfo = null;
            var analytic = ActionTimer.Perform(() =>
            {
                var brandSyncExecutor = new BrandSyncExecutor(benchTime, logger);
                executeInfo = brandSyncExecutor.Execute();
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
                logger.Info(string.Format("Map product succeed :{0}, failed :{1},total :{2}, elapsed {3},ratio {4}", 
                    executeInfo.SucceedCount,
                    executeInfo.FailedCount, 
                    executeInfo.TotalCount, 
                    analytic.TotalSeconds,
                    executeInfo.SucceedCount/analytic.TotalSeconds));
            }
        }
    }
}
