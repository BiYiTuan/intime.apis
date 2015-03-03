using com.intime.fashion.common;
using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    class ExpireStoreCouponReturnPointsJob:IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap data = context.JobDetail.JobDataMap;
            var privatekey = data.GetString("privatekey");
            var publickey = data.GetString("publickey");
            var awsExpireurl = data.GetString("awsexpireurl");
            int point2GroupRatio = int.Parse(ConfigurationManager.AppSettings["point2groupratio"]);
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int successCount = 0;
           int size = JobConfig.DEFAULT_PAGE_SIZE;
            int totalCount = 0;
            DateTime fromDate =  DateTime.Today.AddDays(-1);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                Query(db, fromDate, coupons => totalCount = coupons.Count());
            }
            int lastMaxId = 0;
            while (cursor < totalCount)
            {
                List<StoreCouponEntity> oneTimeList = null;

                using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
                {
                    Query(db, fromDate, coupons => oneTimeList = coupons.Where(c=>c.Id>lastMaxId).OrderBy(c=>c.Id).Take(size).ToList());
                }
                foreach (var coupon in oneTimeList)
                {
                    bool canRebate = HttpClientUtil.SendHttpMessage(awsExpireurl, new { code = coupon.Code }
                            , publickey
                            , privatekey
                            , null
                            , null);
                    if (!canRebate)
                    {
                        continue;
                    }
                    using (var ts = new TransactionScope())
                    {
                        using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
                        {
                            var point = db.PointHistories.Where(u => u.User_Id == coupon.UserId.Value && u.Type == (int)PointType.VoidCoupon && u.Description == coupon.Code).FirstOrDefault();
                            if (point == null)
                            {
                                db.PointHistories.Add(new PointHistoryEntity()
                                {
                                    Amount = (decimal)coupon.Points * point2GroupRatio,
                                    CreatedDate = DateTime.Now,
                                    CreatedUser = 0,
                                    Description = coupon.Code,
                                    Name = string.Format("代金券过期返回积点{0}", coupon.Points * point2GroupRatio),
                                    PointSourceType = (int)PointSourceType.System,
                                    PointSourceId = 0,
                                    Status = (int)DataStatus.Normal,
                                    Type = (int)PointType.VoidCoupon,
                                    UpdatedDate = DateTime.Now,
                                    UpdatedUser = 0,
                                    User_Id = coupon.UserId.Value

                                });
                                db.SaveChanges();
                            }
                            ts.Complete();
                            successCount++;
                        }
                    }
    
                }
                cursor += size;
                lastMaxId = oneTimeList.Max(c => c.Id);
            }
            sw.Stop();
            log.Info(string.Format("{0} rebate points in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));


        }
        private void Query(YintaiHangzhouContext db,DateTime fromDate,Action<IQueryable<StoreCouponEntity>> callback)
        {
            var accounts = from p in db.StoreCoupons
                           where p.ValidEndDate >= fromDate && p.ValidEndDate < DateTime.Today
                             && p.Status == (int)CouponStatus.Normal
                           select p;
            if (callback != null)
                callback(accounts);
        }
    }
}
