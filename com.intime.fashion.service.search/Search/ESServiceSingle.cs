using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.fashion.common;
using Yintai.Architecture.Framework.ServiceLocation;

namespace com.intime.fashion.service.search
{
    public class ESServiceSingle<TModel> : ESServiceBase where TModel : class
    {
        protected DbContext _db = null;

        public override bool IndexSingle(int entityId)
        {
            var esCombo = entity2Model(entityId);
            return SearchLogic.IndexSingle<TModel>(esCombo);
        }

        protected virtual TModel entity2Model(int entityId)
        {
            throw new NotImplementedException();
        }

        protected DbContext Context
        {
            get
            {
                if (_db == null)
                    _db = ServiceLocator.Current.Resolve<DbContext>();
                return _db;
            }
        }
    }
}
