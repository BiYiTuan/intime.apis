using com.intime.fashion.common.message;
using com.intime.fashion.common.message.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class InventoryRepository : RepositoryBase<InventoryEntity, int>,IInventoryRepository
    {
        public override void Update(InventoryEntity entity)
        {
            base.Update(entity);

            this.NotifyMessage<InventoryEntity>(() => {
                var messageProvider = ServiceLocator.Current.Resolve<IMessageCenterProvider>();
                messageProvider.GetSender().SendMessageReliable(new UpdateMessage()
                {
                    SourceType = (int)MessageSourceType.Inventory,
                    EntityId = entity.Id
                });
            });

            
           
        }
        public override Yintai.Hangzhou.Data.Models.InventoryEntity Insert(Yintai.Hangzhou.Data.Models.InventoryEntity entity)
        {
            var newEntity =  base.Insert(entity);

            this.NotifyMessage<InventoryEntity>(() =>
            {
                var messageProvider = ServiceLocator.Current.Resolve<IMessageCenterProvider>();
                messageProvider.GetSender().SendMessageReliable(new CreateMessage()
                {
                    SourceType = (int)MessageSourceType.Inventory,
                    EntityId = newEntity.Id
                });
            });
           

            return newEntity;
        }

      
    }
}
