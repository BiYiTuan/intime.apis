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
using com.intime.fashion.service;
using Yintai.Architecture.Framework.ServiceLocation;
using com.intime.fashion.service.contract;

namespace com.intime.jobscheduler.Job.Income
{
    [DisallowConcurrentExecution]
    class AutoChangeIncomeStatusJob:IJob
    {
        private void Query(DateTime benchTime, Action<IQueryable<IMS_AssociateIncomeHistoryEntity>> callback)
        {
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var orders = db.Set<IMS_AssociateIncomeHistoryEntity>().Where(ot => ot.Status == (int)AssociateIncomeStatus.Frozen
                            && ot.CreateDate < benchTime);

                if (callback != null)
                    callback(orders);
            }
        }
        public void Execute(IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(this.GetType());

            JobDataMap data = context.JobDetail.JobDataMap;
           
            var totalCount = 0;
            var interval = data.ContainsKey("intervalOfDays") ? data.GetInt("intervalOfDays") : 1;
            if (!data.ContainsKey("benchtime"))
            {
                data.Put("benchtime", DateTime.Now.AddDays(-interval));
            }
            else
            {
                data["benchtime"] = data.GetDateTimeValue("benchtime").AddDays(interval);
            }
            var benchTime = data.GetDateTime("benchtime");

            Query(benchTime, orders => totalCount = orders.Count());

            int cursor = 0;
            int successCount = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int lastCursor = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (cursor < totalCount)
            {
                List<IMS_AssociateIncomeHistoryEntity> oneTimeList = null;
                Query(benchTime, orders =>
                {
                    oneTimeList = orders.Where(a => a.Id > lastCursor).OrderBy(a => a.Id).Take(size).ToList();
                });
                foreach (var order in oneTimeList)
                {
                    try
                    {
                        using (var slt = new ScopedLifetimeDbContextManager())
                        {
                            using (var ts = new TransactionScope())
                            {
                                using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
                                {
                                    var rmaEntity = db.Set<RMAEntity>().Where(r => r.OrderNo == order.SourceNo
                                                && r.Status != (int)RMAStatus.Void
                                                && r.Status != (int)RMAStatus.Reject).FirstOrDefault();
                                    bool incomeIsAvail = rmaEntity == null ? true : false;
                                    bool isSuccess = false;
                                    var associateIncomeService = ServiceLocator.Current.Resolve<IAssociateIncomeService>();
                                    if (incomeIsAvail)
                                    {
                                        isSuccess = associateIncomeService.Avail(order);
                                    }
                                    else
                                    {
                                        isSuccess = associateIncomeService.Void(order);
                                    }

                                    if (isSuccess)
                                    {
                                        ts.Complete();
                                        successCount++;
                                    }
                                    else
                                        log.Error(string.Format("order:{0} cannot convert income to avail/void", order.SourceNo));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Info(ex);
                    }
                }
                cursor += size;
                if (oneTimeList != null && oneTimeList.Count > 0)
                    lastCursor = oneTimeList.Max(o => o.Id);
            }

            sw.Stop();
            log.Info(string.Format("total income history:{0},{1} converted incomes in {2} => {3} docs/s", totalCount, successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));
        }
    }
}
