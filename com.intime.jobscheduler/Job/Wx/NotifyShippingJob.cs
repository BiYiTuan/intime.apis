using com.intime.fashion.common;
using com.intime.fashion.common.Wxpay;
using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.jobscheduler.Job.Wx
{
    [DisallowConcurrentExecution]
    class NotifyShippingJob:IJob
    {
         private void Query(Action<IQueryable<OrderEntity>> callback)
        {
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {

                var prods = db.Set<OrderEntity>().Where(o => o.Status >= (int)OrderStatus.Shipped
                            && o.OrderSource == WxPayConfig.ORDER_SOURCE
                            && !db.Set<JobSuccessHistoryEntity>().Any(j => j.JobType == (int)JobType.Wx_Shipping && j.JobId == o.Id));


                if (callback != null)
                    callback(prods);
            }
        }
        public void Execute(IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(this.GetType());

            JobDataMap data = context.JobDetail.JobDataMap;
            var benchDate = data.ContainsKey("benchdate") ? data.GetDateTime("benchdate") : DateTime.Today.AddDays(-1);
            var interval = data.ContainsKey("intervalOfSecs") ? data.GetInt("intervalOfSecs") : 5 * 60;
            int successCount = 0;
            int cursor = 0;
           int size = JobConfig.DEFAULT_PAGE_SIZE;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            int totalCount = 0;
            Query(o => totalCount = o.Count());
            while (cursor < totalCount)
                {
                   var orderList = new List<OrderEntity>();
                    Query(o=>orderList = o.OrderBy(p=>p.Id).Skip(cursor).Take(size).ToList());
                   
                    foreach (var l in orderList)
                    {
                        using(var ts = new TransactionScope())
                        {
                            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
                            {
                                db.JobSuccessHistories.Add(new JobSuccessHistoryEntity() { 
                                     CreateDate = DateTime.Now,
                                      JobId = l.Id,
                                       JobType = (int)JobType.Wx_Shipping
                                });
                                var linq = db.Set<Order2ExEntity>().Where(o => o.OrderNo == l.OrderNo)
                                            .Join(db.Set<OrderTransactionEntity>().Where(ot => ot.PaymentCode == WxPayConfig.PaymentCode)
                                                    , o => o.OrderNo
                                                    , i => i.OrderNo
                                                    , (o, i) => new { OE = o, OT = i }).FirstOrDefault();
                                if (linq == null)
                                {
                                    log.Info(string.Format("order has not transaction:{0}",l.OrderNo));
                                    continue;
                                }
                             
                                var requestData = new WxNotify() {
                                     DeliverMsg = "已提货",
                                      DeliverStatus="1",
                                       DeliverTS = DateTime.Now.TicksOfWx().ToString(),
                                        OutTradeNo = linq.OE.ExOrderNo,
                                         OpenId = linq.OT.OutsiteUId,
                                          TransactionId = linq.OT.TransNo
                                          
                                };
                                bool notifyResult = WxServiceHelper.Notify(requestData.EncodedRequest, null, null);
                                if (notifyResult)
                                {
                                    db.SaveChanges();
                                    ts.Complete();
                                    successCount++;
                                }
                                
                            }
                        }
                       
                    }

                    cursor += size;
                }

            sw.Stop();
            log.Info(string.Format("{0} wx orders notified in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }
       
    }
}
