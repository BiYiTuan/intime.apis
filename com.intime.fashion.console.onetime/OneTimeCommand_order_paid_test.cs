using System.Data;
using CLAP;
using com.intime.fashion.service;
using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace com.intime.fashion.console.onetime
{
    public partial class OneTimeCommand
    {
        [Verb(IsDefault = false, Aliases = "pay", Description = "pay for order")]
        static void Pay(string orderno)
        {
            var Context = ServiceLocator.Current.Resolve<DbContext>();
            var _orderTranRepo = ServiceLocator.Current.Resolve<IEFRepository<OrderTransactionEntity>>();
            var _associateIncomeService = new AssociateIncomeService(ServiceLocator.Current.Resolve<IEFRepository<IMS_AssociateIncomeHistoryEntity>>(), ServiceLocator.Current.Resolve<IEFRepository<IMS_AssociateIncomeEntity>>());
            var guid = new Guid();
            
            string trade_no = guid.ToString();

        
            using (var ts = new TransactionScope())
            {
                var orderEntity = Context.Set<OrderEntity>().FirstOrDefault(o => o.OrderNo == orderno);

                var orderType = (orderEntity.OrderProductType ?? (int)OrderProductType.SystemProduct) == (int)OrderProductType.SelfProduct ? (int)PaidOrderType.Self_ProductOfSelf : (int)PaidOrderType.Self;
        
                _orderTranRepo.Insert(new OrderTransactionEntity()
                {
                    Amount = orderEntity.TotalAmount,
                    OrderNo = orderno,
                    CreateDate = DateTime.Now,
                    IsSynced = false,
                    PaymentCode = "270",
                    TransNo = trade_no,
                    OutsiteType = (int)OutsiteType.WX,
                    OutsiteUId = "o7z1Pt5YIGRlI5Nstqf4P_VNOCsQ",
                    OrderType = orderType
                });
                if (orderEntity.Status != (int)OrderStatus.Paid)
                {
                    orderEntity.Status = (int)OrderStatus.Paid;
                    orderEntity.UpdateDate = DateTime.Now;
                    orderEntity.RecAmount = orderEntity.TotalAmount;
                    Context.Entry(orderEntity).State = EntityState.Modified;

                    try
                    {
                        _associateIncomeService.Froze(orderEntity.OrderNo);
                        Context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine(e.InnerException);
                        Console.WriteLine(e.Message);
                    }

                }
                ts.Complete();
            }
        }
    }
}
