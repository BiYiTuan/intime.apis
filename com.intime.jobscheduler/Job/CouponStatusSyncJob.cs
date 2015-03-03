using com.intime.fashion.common;
using Common.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    public class CouponStatusSyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(this.GetType());

            JobDataMap data = context.JobDetail.JobDataMap;
             var interval = data.ContainsKey("interval") ? data.GetIntValue("interval") : 24 * 60;
            var benchDate = DateTime.Now.AddMinutes(-interval);
            var host = data.GetString("awshost");
           var public_key = data.GetString("publickey");
            var private_key = data.GetString("privatekey");

            dynamic jsonResponse = null;
            HttpClientUtil.SendHttpMessage(host, new
                {
                    benchdate = benchDate.ToUniversalTime()
                },public_key,private_key,r=>jsonResponse = r,null);

            if (jsonResponse == null )
            {
                log.Info("request error!" );
                return;
            }
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                foreach (var dynamicObject in jsonResponse.data)
                {
                    try
                    {
                        string code = dynamicObject.code;
                        int? status = dynamicObject.status;
                        DateTime opeDate = dynamicObject.created_at;
                        CouponStatus targetStatus = CouponStatus.Used;
                        CouponActionType targetActionType = CouponActionType.Consume;
                        if (status.HasValue && status.Value == -1)
                        {
                            targetStatus = CouponStatus.Normal;
                            targetActionType = CouponActionType.Rebate;
                        }
                        switch ((int)dynamicObject.coupontype)
                        {
                            case 1:
                                var coupon = db.StoreCoupons.Where(s => s.Code == code && s.Status != (int)CouponStatus.Deleted).FirstOrDefault();
                                if (coupon != null)
                                {
                                    coupon.Status = (int)targetStatus;
                                    coupon.UpdateDate = opeDate.ToLocalTime();
                                    coupon.UpdateUser = 0;
                                    db.Entry(coupon).State = EntityState.Modified;

                                    db.CouponLogs.Add(new CouponLogEntity()
                                    {
                                        ActionType = (int)targetActionType,
                                        BrandNo = dynamicObject.brandno,
                                        Code = code,
                                        ConsumeStoreNo = dynamicObject.storeno,
                                        CreateDate = opeDate.ToLocalTime(),
                                        CreateUser = 0,
                                        ReceiptNo = dynamicObject.receiptno,
                                        Type = 1
                                    });
                                    db.SaveChanges();
                                    successCount++;
                                }
                               
                                break;
                            case 2:
                                 var coupon2 = db.CouponHistories.Where(s => s.CouponId == code && s.Status != (int)CouponStatus.Deleted).FirstOrDefault();
                                 if (coupon2 != null)
                                {
                                    coupon2.Status = (int)targetStatus;
                                    db.Entry(coupon2).State = EntityState.Modified;

                                    db.CouponLogs.Add(new CouponLogEntity()
                                    {
                                        ActionType = (int)targetActionType,
                                        BrandNo = dynamicObject.brandno,
                                        Code = code,
                                        ConsumeStoreNo = dynamicObject.storeno,
                                        CreateDate = opeDate.ToLocalTime(),
                                        CreateUser = 0,
                                        ReceiptNo = dynamicObject.receiptno,
                                        Type = 2
                                    });
                                    db.SaveChanges();
                                    successCount++;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }
            }
            sw.Stop();
            log.Info(string.Format("{0} status logs in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }
      
    }
}
