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
    class ESIMSTagService : ESServiceSingle<ESIMSTag>
    {
        protected override ESIMSTag entity2Model(int entityId)
        {
            var db = Context;
            var tagEntity = Context.Set<IMS_TagEntity>().Find(entityId);
            return new ESIMSTag()
            {
                Id = tagEntity.Id,
                Name = tagEntity.Name,
                SortOrder = tagEntity.SortOrder ?? 0,
                Status = tagEntity.Status
            };
        }
        
    }
}
