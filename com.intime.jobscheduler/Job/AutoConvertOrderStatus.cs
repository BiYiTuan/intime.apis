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
    class AutoConvertOrderStatus : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            internalVoidOrderWhenRMAComplete();
            //            internalCompleteOrderWhenRMARequest();

        }



        private void internalVoidOrderWhenRMAComplete()
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            var benchDate = DateTime.Today;
            var toBenchDate = benchDate.AddDays(1);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {

                var rmas = db.RMAs.Where(r => r.Status == (int)RMAStatus.Complete && r.UpdateDate >= benchDate && r.UpdateDate < toBenchDate);
                int totalCount = rmas.Count();
                while (cursor < totalCount)
                {
                    var result = rmas.OrderByDescending(r => r.Id).Skip(cursor).Take(size);
                    foreach (var rma in result)
                    {
                        var rmaQuantity = db.RMAs.Where(r => r.OrderNo == rma.OrderNo && r.Status == (int)RMAStatus.Complete)
                                         .Join(db.RMAItems, o => o.RMANo, i => i.RMANo, (o, i) => i).Sum(i => i.Quantity);
                        var totalQuantity = db.OrderItems.Where(o => o.OrderNo == rma.OrderNo).Sum(i => i.Quantity);
                        if (rmaQuantity != totalQuantity)
                            continue;
                        var orderEntity = db.Orders.Where(o => o.OrderNo == rma.OrderNo).First();
                        if (orderEntity.Status != (int)OrderStatus.RMAd)
                        {
                            orderEntity.Status = (int)OrderStatus.RMAd;
                            orderEntity.UpdateDate = DateTime.Now;

                            db.OrderLogs.Add(new OrderLogEntity()
                            {
                                CreateDate = DateTime.Now,
                                CreateUser = 0,
                                CustomerId = orderEntity.CustomerId,
                                Operation = "订单全部退货",
                                OrderNo = orderEntity.OrderNo,
                                Type = (int)OrderOpera.FromSystem
                            });

                            db.SaveChanges();
                            successCount++;
                        }
                    }
                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} order convert to rmad in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }
    }
}
