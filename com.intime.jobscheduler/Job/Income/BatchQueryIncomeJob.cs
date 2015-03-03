using com.intime.fashion.common.Tencent;
using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.jobscheduler.Job.Income
{
    [DisallowConcurrentExecution]
    class BatchQueryIncomeJob:IJob
    {
        private const int batchTransferPage = 10000;
        private void Query( Action<IQueryable<IMS_AssociateIncomeTransferEntity>> callback)
        {

            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var orders = db.Set<IMS_AssociateIncomeTransferEntity>().Where(ot => 
                            ot.Status == (int)AssociateIncomeTransferStatus.RequestSent
                           );

                if (callback != null)
                    callback(orders);
            }
        }
        public void Execute(IJobExecutionContext context)
        {
            DoQuery();
        }
        private ILog Log
        {
            get
            {
                return
                    LogManager.GetLogger(this.GetType());
            }
        }
      
        private void DoQuery()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var totalCount = 0;
            Query(orders => totalCount = orders.Count());

            int cursor = 0;
            int successCount = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE; ;
            int lastCursor = 0;


            while (cursor < totalCount)
            {
                List<IMS_AssociateIncomeTransferEntity> oneTimeList = null;
                Query(orders =>
                {
                    oneTimeList = orders.Where(a => a.Id > lastCursor).OrderBy(a => a.Id).Take(size).ToList();
                });

                foreach (var order in oneTimeList)
                {
                    using (var ts = new TransactionScope())
                    {
                        bool canComplete = true;
                        using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
                        {
                            var fullPackageId = string.Concat(order.PackageId, order.SerialNo);
                           // var keyEntity = db.Set<Group_WeixinKeysEntity>().Where(gw => gw.GroupId == order.GroupId && gw.Status == (int)DataStatus.Normal)
                            //        .First();
                            BatchQueryResponse response = AutoBankTransfer.Instance.Query(new BatchQueryRequest()
                            {
                                PackageId = fullPackageId,
                                ServiceVersion = "1.2",
                                GroupId = order.GroupId

                            });
                            if (response != null && response.IsSuccess)
                            {
                                switch (response.Result.TradeState)
                                {

                                    case 4://all failed
                                    case 7:
                                        doFailAll(db, order, response,int.Parse(response.PackageId));
                                        break;
                                    case 6:
                                        doPartialSuccess(db, order, response);
                                        successCount++;
                                        break;
                                    default:

                                        canComplete = false; 
                                        break;
                                }
                            }
                            else
                            {
                                if (response != null && response.IsAllFail)
                                    doFailAll(db, order, response, int.Parse(fullPackageId));
                                else
                                    canComplete = false;
                                
                            }

                        }
                      if (canComplete)
                         ts.Complete();
                      else
                          Log.Info(AutoBankTransfer.Instance.GetDebugLine());

                    }
                }

                cursor += size;
                if (oneTimeList != null && oneTimeList.Count > 0)
                    lastCursor = oneTimeList.Max(o => o.Id);
            }
            sw.Stop();

            Log.Info(string.Format("total query transfer jobs:{0},{1} query complete job in {2} => {3} docs/s", totalCount, successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }

        private void doPartialSuccess(YintaiHangzhouContext db, IMS_AssociateIncomeTransferEntity order, BatchQueryResponse response)
        {
            order.Status = (int)AssociateIncomeTransferStatus.Complete;
            order.QueryRetCode = response.Result.TradeState.ToString();
            order.QueryRetMsg = response.RetMsg;
            db.Entry(order).State = System.Data.EntityState.Modified;

            int fullPackageId = int.Parse(response.PackageId);
            //modify all transfer requesting
            if (response.Result.SuccessResult.Records != null)
            {
                foreach (var succRequest in response.Result.SuccessResult.Records)
                {
                    int requestId = int.Parse(succRequest.Serial);
                    var request = db.Set<IMS_AssociateIncomeRequestEntity>().Find(requestId);
                    request.Status = (int)AssociateIncomeRequestStatus.Transferred;
                    request.UpdateDate = DateTime.Now;
                    request.TransferErrorCode = order.QueryRetCode;
                    request.TransferErrorMsg = order.QueryRetMsg;
                    db.Entry(request).State = System.Data.EntityState.Modified;

                    var incomeAccount = db.Set<IMS_AssociateIncomeEntity>().Where(iai => iai.UserId == request.UserId && iai.GroupId == order.GroupId).First();
                    incomeAccount.RequestAmount -= request.Amount;
                    incomeAccount.ReceivedAmount += request.Amount;
                    incomeAccount.UpdateDate = DateTime.Now;
                    db.Entry(incomeAccount).State = System.Data.EntityState.Modified;
                }
            }
            if (response.Result.FailResult.Records != null)
            {
                foreach (var failRequest in response.Result.FailResult.Records)
                {
                    if (failRequest != null)
                    {
                        int requestId = int.Parse(failRequest.Serial);
                        var request = db.Set<IMS_AssociateIncomeRequestEntity>().Find(requestId);
                        request.Status = (int)AssociateIncomeRequestStatus.Failed;
                        request.UpdateDate = DateTime.Now;
                        request.TransferErrorCode = failRequest.ErrorCode;
                        request.TransferErrorMsg = failRequest.ErrorMsg;
                        db.Entry(request).State = System.Data.EntityState.Modified;

                        var incomeAccount = db.Set<IMS_AssociateIncomeEntity>().Where(iai => iai.UserId == request.UserId && iai.GroupId == order.GroupId).First();
                        incomeAccount.RequestAmount -= request.Amount;
                        incomeAccount.AvailableAmount += request.Amount;
                        incomeAccount.UpdateDate = DateTime.Now;
                        db.Entry(incomeAccount).State = System.Data.EntityState.Modified;
                    }

                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    if (!error.IsValid)
                    {
                        foreach (var internalError in error.ValidationErrors)
                        {
                            Log.Info(string.Format("{0}_{1}", internalError.PropertyName, internalError.ErrorMessage));
                        }
                    }
                }
            }
        }

        private void doFailAll(YintaiHangzhouContext db,
            IMS_AssociateIncomeTransferEntity order,
            BatchQueryResponse response,
            int fullPackageId)
        {
            order.Status = (int)AssociateIncomeTransferStatus.Fail;
            order.QueryRetCode = response.Result.TradeState.ToString();
            order.QueryRetMsg = response.RetMsg;
            db.Entry(order).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            //modify all transfer requesting
            foreach (var request in db.Set<IMS_AssociateIncomeRequestEntity>().Where(iar=>iar.Status==(int)AssociateIncomeRequestStatus.Transferring)
                                    .Join(db.Set<IMS_AssociateIncomeTran2ReqEntity>().Where(iait => iait.FullPackageId == fullPackageId),
                                            o => o.Id,
                                            i => i.RequestId,
                                            (o, i) => o))
            {
                request.Status = (int)AssociateIncomeRequestStatus.Failed;
                request.UpdateDate = DateTime.Now;
                request.TransferErrorCode = order.QueryRetCode;
                request.TransferErrorMsg = order.QueryRetMsg;
                db.Entry(request).State = System.Data.EntityState.Modified;

                var incomeAccount = db.Set<IMS_AssociateIncomeEntity>().Where(iai => iai.UserId == request.UserId && iai.GroupId == order.GroupId).First();
                incomeAccount.RequestAmount -= request.Amount;
                incomeAccount.AvailableAmount += request.Amount;
                incomeAccount.UpdateDate = DateTime.Now;
                db.Entry(incomeAccount).State = System.Data.EntityState.Modified;
            }
            db.SaveChanges();                             
        }

      
    }
}
