using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    public class ExternalProductSyncJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(this.GetType());

            JobDataMap data = context.JobDetail.JobDataMap;
            var benchDate = data.ContainsKey("benchdate") ? data.GetDateTime("benchdate") : DateTime.Today.AddDays(-1);
            if (!data.ContainsKey("intervalofsec"))
            {
                log.Info("intervalofsec not set");
                return;
            }
            var secondsInterval = data.ContainsKey("intervalofsec");
            int successCount = 0;
            int cursor = 0;
           int size = JobConfig.DEFAULT_PAGE_SIZE;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                
                /*
                int totalCount = 0;
                while (cursor < totalCount)
                {
                    var linq = prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size).ToList();
                    foreach (var l in linq)
                    {
                        var sumPoints = db.PointHistories.Where(p => p.User_Id == l.User_Id && p.Status != (int)DataStatus.Deleted).Sum(p => p.Amount);
                        if (sumPoints == l.Amount)
                            continue;
                        l.Amount = sumPoints > 0 ? sumPoints : 0;
                        l.UpdatedDate = DateTime.Now;
                        db.Entry(l).State = System.Data.EntityState.Modified;
                        db.SaveChanges();
                        successCount++;
                    }

                    cursor += size;
                }
                 * */

            }
            sw.Stop();
            log.Info(string.Format("{0} external products in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }
    }
}
