using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Yintai.Architecture.Framework;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.ES;
using Yintai.Hangzhou.Model.ESModel;

namespace com.intime.fashion.service.search
{
    class ESGroupService : ESServiceSingle<ESGroup>
    {
        protected override ESGroup entity2Model(int entityId)
        {
            var db = Context;
            var entity = Context.Set<GroupEntity>().Find(entityId);
            return Mapper.Map<GroupEntity, ESGroup>(entity);
        }
       
    }
}
