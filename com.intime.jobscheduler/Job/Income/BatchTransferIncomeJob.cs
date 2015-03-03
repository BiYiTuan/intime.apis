using com.intime.fashion.common;
using com.intime.fashion.common.Tencent;
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

namespace com.intime.jobscheduler.Job.Income
{
    [DisallowConcurrentExecution]
    public class BatchTransferIncomeJob:IJob
    {
        private const int batchTransferPage = 10000;
        private void Query(DateTime benchTime, Action<IQueryable<IMS_AssociateIncomeRequestEntity>> callback)
        {
            var toTime = benchTime.AddDays(1).Date;
            var fromTime = benchTime.Date;
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var orders = db.Set<IMS_AssociateIncomeRequestEntity>().Where(ot => ot.Status == (int)AssociateIncomeRequestStatus.Requesting
                            && ot.CreateDate >= fromTime
                            && ot.CreateDate <toTime);

                if (callback != null)
                    callback(orders);
            }
        }
        public void Execute(IJobExecutionContext context)
        {


            JobDataMap data = context.JobDetail.JobDataMap;

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

           
            DoTransfer(benchTime);

           
        }
        private ILog Log
        {
            get
            {
                return
                    LogManager.GetLogger(this.GetType());
            }
        }
        private IMS_AssociateIncomeTransferEntity CreateTransferRecord(DateTime benchTime,int? groupId)
        {
            var packageId = int.Parse(benchTime.ToString("yyyyMMdd"));
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var transferEntity = db.Set<IMS_AssociateIncomeTransferEntity>().Where(ia => ia.PackageId == packageId)
                                    .OrderByDescending(l => l.SerialNo).FirstOrDefault();
                if (transferEntity == null)
                {
                    transferEntity = db.IMS_AssociateIncomeTransfer.Add(new IMS_AssociateIncomeTransferEntity()
                    {
                        CreateDate = DateTime.Now,
                        IsSuccess = false,
                        PackageId = packageId,
                        SerialNo = transferEntity == null ? 1 : transferEntity.SerialNo + 1,
                        Status = (int)AssociateIncomeTransferStatus.NotStart,
                        TotalCount = 0,
                        TotalFee = 0,
                        TransferRetCode = "-1",
                        TransferRetMsg = string.Empty,
                        GroupId = groupId
                    });
                    db.SaveChanges();
                }
                return transferEntity;
            }
        }

        private void DoTransfer(DateTime benchTime)
        {
            var packageId = int.Parse(benchTime.ToString("yyyyMMdd"));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var totalCount = 0;
            Query(benchTime, orders => totalCount = orders.Count());

            int cursor = 0;
            int successCount = 0;
            int size = batchTransferPage;
            int lastCursor = 0;
            int serialNo = 1;
            

            while (cursor < totalCount)
            {
                List<IMS_AssociateIncomeRequestEntity> oneTimeList = null;
                List<BatchTransferItem> transferItems = new List<BatchTransferItem>();
                Query(benchTime, orders =>
                {
                    oneTimeList = orders.Where(a => a.Id > lastCursor).OrderBy(a => a.Id).Take(size).ToList();
                });
                using (var ts = new TransactionScope())
                {

                    using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
                    {
                        foreach (var group in oneTimeList.GroupBy(otl => otl.GroupId))
                        {
                            var groupId = group.Key;
                            var transferJobEntity = CreateTransferRecord(benchTime,groupId);
                            string fullPackageId = string.Concat(transferJobEntity.PackageId, transferJobEntity.SerialNo);
                            foreach (var order in oneTimeList.Where(ot=>ot.GroupId == groupId))
                            {
                                order.Status = (int)AssociateIncomeRequestStatus.Transferring;
                                order.UpdateDate = DateTime.Now;
                                db.Entry(order).State = System.Data.EntityState.Modified;

                                transferItems.Add(new BatchTransferItem()
                                {
                                    AccountName = order.BankAccountName,
                                    AccountType = 1,
                                    AmountOfFen = CommonUtil.Yuan2Fen(order.Amount - (order.TransferFee ?? 0m)),
                                    BankCode = order.BankCode,
                                    BankNo = order.BankNo,
                                    Desc = "银泰分成",
                                    NotifyMobile = string.Empty,
                                    Serial = order.Id.ToString()
                                });

                                db.IMS_AssociateIncomeTran2Req.Add(new IMS_AssociateIncomeTran2ReqEntity()
                                {
                                    FullPackageId = int.Parse(fullPackageId),
                                    RequestId = order.Id
                                });
                                db.SaveChanges();

                            }
                            if (transferItems.Count <= 0)
                            {
                                Log.Info(string.Format("this day:{0} has no cps records", benchTime));
                                return;
                            }


                            var totalNum = transferItems.Count;
                            var totalAmount = transferItems.Sum(l => l.AmountOfFen);
                           // var keyEntity = db.Set<Group_WeixinKeysEntity>().Where(gw => gw.GroupId == groupId && gw.Status == (int)DataStatus.Normal)
                            //        .First();
                            var transferResponse = AutoBankTransfer.Instance.BatchTransfer(new BatchTransferRequest()
                            {
                                Records = transferItems.ToArray(),
                                TotalNum = totalNum,
                                TotalAmountOfFen = totalAmount,
                                PackageId = string.Concat(transferJobEntity.PackageId, transferJobEntity.SerialNo),
                                GroupId = groupId
                              //  OperateUser = keyEntity.Outcome_OperatorId,
                               // OperatePwdMd5 = keyEntity.Outcome_OperatorPwd,
                                // SPId = keyEntity.Outcome_ParterId
                                
                            });
                            if (transferResponse != null)
                            {
                                transferJobEntity.Status = (int)AssociateIncomeTransferStatus.RequestSent;
                                transferJobEntity.IsSuccess = true;
                                transferJobEntity.TotalCount = totalNum;
                                transferJobEntity.TotalFee = totalAmount;
                                transferJobEntity.TransferRetCode = transferResponse.RetCode;
                                transferJobEntity.TransferRetMsg = transferResponse.RetMsg;
                                db.Entry(transferJobEntity).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                                ts.Complete();
                                successCount++;
                            }
                            else if (transferResponse != null)
                            {
                                Log.Error(string.Format("income transfer fail, code:{0},msg:{1}", transferResponse.RetCode, transferResponse.RetMsg));
                            }
                            else
                            {
                                Log.Error("income transfer fail, unknow problem!");
                            }
                        }
                        
                    }
                }


               
                cursor += size;
                serialNo++;
                if (oneTimeList != null && oneTimeList.Count > 0)
                    lastCursor = oneTimeList.Max(o => o.Id);
            }
            sw.Stop();

            Log.Info(string.Format("total income transfer jobs:{0},{1} sent out job in {2} => {3} docs/s", totalCount, successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }
    }
}
