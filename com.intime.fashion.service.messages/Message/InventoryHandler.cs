using com.intime.fashion.common.message;
using com.intime.fashion.service.search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;


namespace com.intime.fashion.service.messages.Message
{
   public  class InventoryHandler:MessageHandler
    {
       private ESServiceBase _esService = null;
         public InventoryHandler()
        {
            _esService = SearchLogic.GetService(IndexSourceType.Inventory);
        }
        public override int SourceType
        {
            get { return (int)MessageSourceType.Inventory; }
        }

        public override int ActionType
        {
            get { return (int)(MessageAction.CreateEntity | MessageAction.UpdateEntity); }
        }

        public override bool Work(BaseMessage message)
        {
   
                return _esService.IndexSingle(message.EntityId);
        }
    }
}
