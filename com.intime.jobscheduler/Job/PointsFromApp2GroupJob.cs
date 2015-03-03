using com.intime.fashion.common;
using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    class PointsFromApp2GroupJob : IJob
    {

        private class LinqInner
        {
            public UserAccountEntity U { get; set; }
            public CardEntity C { get; set; }
        }
        private void Query(YintaiHangzhouContext db, int minPoints, Action<IQueryable<LinqInner>> callback)
        {
            var accounts = from p in db.UserAccounts
                           join c in db.Cards on p.User_Id equals c.User_Id
                           where p.AccountType == (int)AccountType.Point
                             && p.Status == (int)DataStatus.Normal
                             && p.Amount >= minPoints
                             && c.CardNo.Length > 0
                             && (!c.IsLocked.HasValue || c.IsLocked.Value==false)
                           select new LinqInner()
                           {
                               U = p,
                               C = c
                           };
            if (callback != null)
                callback(accounts);
        }
        public void Execute(IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            JobDataMap data = context.JobDetail.JobDataMap;
            var privatekey = data.GetString("privatekey");
            var publickey = data.GetString("publickey");
            var minPoints = data.GetInt("minpoints");
            var groupPointConvertUrl = data.GetString("pointconverturl");
            int point2GroupRatio = int.Parse(ConfigurationManager.AppSettings["point2groupratio"]);
            string appStoreNo = ConfigurationManager.AppSettings["appStoreNoInGroup"];
            if (string.IsNullOrEmpty(appStoreNo))
            {
                log.Info("app store no in group is empty!");
                return;
            }

            int cursor = 0;
            int successCount = 0;
           int size = JobConfig.DEFAULT_PAGE_SIZE;
            int totalCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Type linqType = new { U = (UserAccountEntity)null, C = (CardEntity)null }.GetType();

            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {

                Query(db, minPoints, a => totalCount = a.Count());
                
            }
            int lastMaxId = 0;
            while (cursor < totalCount)
            {
                List<LinqInner> accounts = null;
                using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
                {
                    Query(db, minPoints, acs => accounts = acs.Where(a=>a.U.Id>lastMaxId).OrderBy(a => a.U.Id).Take(size).ToList());
                }
                foreach (var account in accounts)
                {
                    using (var ts = new TransactionScope())
                    {
                        // step1: check account balanced
                        using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
                        {
                            var correctPoints = db.PointHistories.Where(p => p.User_Id == account.U.User_Id && p.Status != (int)DataStatus.Deleted).Sum(p => p.Amount);
                            var convertPoints = correctPoints - correctPoints % point2GroupRatio;
                            if (convertPoints < minPoints)
                                continue;


                            //step 2: insert deduct point history
                            db.PointHistories.Add(new PointHistoryEntity()
                               {
                                   Amount = -convertPoints,
                                   CreatedDate = DateTime.Now,
                                   CreatedUser = 0,
                                   Description = string.Format("转换为集团积点{0}", -convertPoints),
                                   Name = string.Format("转换为集团积点{0}", -convertPoints),
                                   PointSourceType = (int)PointSourceType.System,
                                   PointSourceId = 0,
                                   Status = (int)DataStatus.Normal,
                                   Type = (int)PointType.Convert2Group,
                                   UpdatedDate = DateTime.Now,
                                   UpdatedUser = 0,
                                   User_Id = account.U.User_Id
                               });
                            db.SaveChanges();

                            // step 3: call group service to convert points
                            string errorMsg;
                            bool canConvert = GroupServiceHelper.SendHttpMessage(groupPointConvertUrl,
                                publickey,
                                privatekey,
                                new
                                {
                                    cardno = account.C.CardNo,
                                    amount = convertPoints / point2GroupRatio,
                                    storeno = appStoreNo
                                },
                                out errorMsg);
                            if (canConvert)
                            {
                                ts.Complete();
                                successCount++;
                            }
                            else
                            {
                                log.Info(string.Format("convert points failed for cardno:{0},msg:{1}", account.C.CardNo, errorMsg));
                            }
                        }

                    }

                }
                cursor += size;
                lastMaxId = accounts.Max(a => a.U.Id);

            }
            sw.Stop();
            log.Info(string.Format("{0} points from app2group in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));


        }

    }
}
