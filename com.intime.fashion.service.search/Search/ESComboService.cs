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
    class ESComboService : ESServiceSingle<ESCombo>
    {
        protected override ESCombo entity2Model(int entityId)
        {
            var db = Context;
            var brandLinq = db.Set<IMS_Combo2ProductEntity>().Where(ic => ic.ComboId == entityId)
                            .Join(db.Set<ProductEntity>(), o => o.ProductId, i => i.Id, (o, i) => i)
                            .Join(db.Set<BrandEntity>(), o => o.Brand_Id, i => i.Id, (o, i) => i).ToList();

            var tagLinq = db.Set<IMS_Combo2ProductEntity>().Where(ic => ic.ComboId == entityId)
                            .Join(db.Set<ProductEntity>(), o => o.ProductId, i => i.Id, (o, i) => i)
                            .Join(db.Set<Product2IMSTagEntity>(), o => o.Id, i => i.ProductId, (o, i) => i)
                            .Join(db.Set<IMS_TagEntity>().Where(it => it.Status == (int)DataStatus.Normal), o => o.IMSTagId, i => i.Id, (o, i) => i)
                            .Distinct().ToList();
            return db.Set<IMS_ComboEntity>().Where(ic => ic.Id == entityId)
                    .Join(db.Set<IMS_AssociateItemsEntity>().Where(iai => iai.ItemType == (int)ComboType.Product), o => o.Id, i => i.ItemId, (o, i) => new { C = o, AI = i })
                    .Join(db.Set<IMS_AssociateEntity>(), o => o.AI.AssociateId, i => i.Id, (o, i) => new { C = o.C, A = i })
                    .GroupJoin(db.Set<ResourceEntity>().Where(r => r.Status == (int)DataStatus.Normal && r.SourceType == (int)SourceType.Combo),
                                o => o.C.Id,
                                i => i.SourceId,
                                (o, i) => new { C = o.C, A = o.A, R = i.OrderByDescending(ri => ri.SortOrder) })
                     .ToList()
                     .Select(l => Mapper.Map<IMS_ComboEntity, ESCombo>(l.C, target =>
                     {
                         target.Resources = l.R == null ? null : l.R.Select(r => Mapper.Map<ResourceEntity, ESResource>(r));
                         target.AssociateId = l.A.Id;
                         target.AssociateName = db.Set<UserEntity>().Find(l.A.UserId).Nickname;
                         target.StoreId = l.A.StoreId;
                         target.Brands = brandLinq.Select(b => Mapper.Map<BrandEntity, ESBrand>(b));
                         target.IsInPromotion = target.IsInPromotion ?? false;
                         target.DiscountAmount = target.DiscountAmount ?? 0m;
                         target.OriginPrice = l.C.UnitPrice ?? l.C.Price;
                         target.Tags = tagLinq.Select(t => Mapper.Map<IMS_TagEntity, ESIMSTag>(t));
                         target.Group = Mapper.Map<GroupEntity, ESGroup>(Context.Set<StoreEntity>().Where(s => s.Id == l.A.StoreId)
                                        .Join(Context.Set<GroupEntity>(), o => o.Group_Id, i => i.Id, (o, i) => i)
                                        .FirstOrDefault());
                         if ((l.C.IsPublic ?? true) == false)
                         {
                             target.Status = (int)DataStatus.Default;
                         }
                     })).FirstOrDefault();
        }
        
    }
}
