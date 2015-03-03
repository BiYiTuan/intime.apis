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
    class ESBrandService:ESServiceSingle<ESBrand>
    {
        protected override ESBrand entity2Model(int entityId)
        {
            var db = Context;
            var storeBrands = db.Set<IMS_SectionBrandEntity>().Join(db.Set<SectionEntity>(), o => o.SectionId, i => i.Id, (o, i) => new { SB = o, Sec = i })
                                     .Join(db.Set<StoreEntity>(), o => o.Sec.StoreId, i => i.Id, (o, i) => new { SB = o.SB, S = i });
            var linq = db.Set<BrandEntity>().Where(p => p.Id == entityId)
                      .GroupJoin(storeBrands
                      , o => o.Id, i => i.SB.BrandId, (o, i) => new { B = o, S = i });
            return linq.Select(p => new ESBrand()
            {
                Id = p.B.Id,
                Name = p.B.Name,
                Description = p.B.Description,
                Status = p.B.Status,
                Group = p.B.Group,
                EngName = p.B.EnglishName,
                Stores = p.S.Select(s => new ESStore()
                {
                    Id = s.S.Id
                })
            }).FirstOrDefault();
        }
       
    }
}
