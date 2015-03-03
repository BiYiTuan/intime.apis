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
    public class SectionBrandHandler : MessageHandler
    {
        private ESServiceBase _esService = null;
        private DbContext _db = null;
        public SectionBrandHandler()
        {
            _esService = SearchLogic.GetService(IndexSourceType.Brand);
            _db = ServiceLocator.Current.Resolve<DbContext>();
        }
        public override int SourceType
        {
            get { return (int)MessageSourceType.SectionBrand; }
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
            var brand = _db.Set<IMS_SectionBrandEntity>().Find(message.EntityId);
            if (brand == null)
                return true;

            return _esService.IndexSingle(brand.Id);

        }
    }
}
