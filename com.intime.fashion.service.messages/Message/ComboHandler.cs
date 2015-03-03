using com.intime.fashion.common.message;
using com.intime.fashion.service.search;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.service.messages.Message
{
   public class ComboHandler:MessageHandler
    {
        private ESServiceBase _esService = null;
        private DbContext _db;

        public ComboHandler()
        {
            _esService = SearchLogic.GetService(IndexSourceType.Combo);
            _db = ServiceLocator.Current.Resolve<DbContext>();
        }
        public override int SourceType
        {
            get { return (int)MessageSourceType.Combo; }
        }

        public override int ActionType
        {
            get
            {
                return (int)(MessageAction.CreateEntity |
                      MessageAction.UpdateEntity |
                      MessageAction.DeleteEntity);
            }
        }
        public override bool Work(BaseMessage message)
        {
            var comboId = message.EntityId;
           var only4Tmall = _db.Set<IMS_Combo2ProductEntity>().Where(icp => icp.ComboId == comboId)
                .Join(_db.Set<Product2IMSTagEntity>(), o => o.ProductId, i => i.ProductId, (o, i) => i)
                .Join(_db.Set<IMS_TagEntity>().Where(it => (it.Only4Tmall ?? false) == true), o => o.IMSTagId, i => i.Id, (o, i) => i)
                .Any();
            if (!only4Tmall)
                return _esService.IndexSingle(comboId);
            return true;
            
        }
    }
}
