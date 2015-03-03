using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    public class AccountSyncJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(this.GetType());

            JobDataMap data = context.JobDetail.JobDataMap;
            var benchDate = data.ContainsKey("benchdate")?data.GetDateTime("benchdate"):DateTime.Today.AddDays(-1);
         
            int successCount = 0;
            int cursor = 0;
           int size = JobConfig.DEFAULT_PAGE_SIZE;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var prods = from r in db.UserAccounts
                            where r.AccountType == (int)AccountType.Point 
                             && (from p in db.PointHistories
                                 where (p.CreatedDate>=benchDate || p.UpdatedDate>=benchDate)
                                 && p.User_Id == r.User_Id
                                  select p).Any()
                            select r;

                int totalCount = prods.Count();
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

            }
            sw.Stop();
            log.Info(string.Format("{0} accounts in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }
    }
}
