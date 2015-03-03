using com.intime.fashion.common.message;
using com.intime.fashion.common.message.Messages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class OrderTransactionRepository : EFRepository<OrderTransactionEntity>
    {
        public OrderTransactionRepository() : base(ServiceLocator.Current.Resolve<DbContext>()) { }


        public override Yintai.Hangzhou.Data.Models.OrderTransactionEntity Insert(Yintai.Hangzhou.Data.Models.OrderTransactionEntity entity)
        {
            var newEntity = base.Insert(entity);
            this.NotifyMessage<OrderTransactionEntity>(() =>
            {
                var messageProvider = ServiceLocator.Current.Resolve<IMessageCenterProvider>();
                var validSources = new Dictionary<int, int>();
                validSources.Add((int)PaidOrderType.GiftCard, (int)MessageSourceType.GiftCard);
                validSources.Add((int)PaidOrderType.Self, (int)MessageSourceType.Order);
                validSources.Add((int)PaidOrderType.Self_ProductOfSelf, (int)MessageSourceType.Order);
                if (validSources.Keys.Contains(newEntity.OrderType.Value))
                {
                    messageProvider.GetSender().SendMessageReliable(new PaidMessage()
                    {
                        SourceType = validSources[newEntity.OrderType.Value],
                        SourceNo = newEntity.OrderNo
                    });
                }
               
            });
           
            return newEntity;
        }
    }
}
