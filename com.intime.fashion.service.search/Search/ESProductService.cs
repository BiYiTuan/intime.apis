using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.ES;
using Yintai.Hangzhou.Model.ESModel;

namespace com.intime.fashion.service.search
{
    class ESProductService:ESServiceSingle<ESProduct>
    {
        protected override ESProduct entity2Model(int id)
        {
            var db = Context;
            var linq = from p in db.Set<ProductEntity>().Where(pe => pe.Id == id).AsQueryable()
                       join s in db.Set<StoreEntity>().AsQueryable() on p.Store_Id equals s.Id
                       join b in db.Set<BrandEntity>().AsQueryable() on p.Brand_Id equals b.Id
                       join t in db.Set<TagEntity>().AsQueryable() on p.Tag_Id equals t.Id
                       let resource = (from r in db.Set<ResourceEntity>().AsQueryable()
                                       where r.SourceId == p.Id
                                       && r.SourceType == 1
                                       select new ESResource()
                                       {
                                           Domain = r.Domain,
                                           Name = r.Name,
                                           SortOrder = r.SortOrder,
                                           IsDefault = r.IsDefault,
                                           Type = r.Type,
                                           Width = r.Width,
                                           Height = r.Height,
                                           ColorId = r.ColorId,
                                           Status = r.Status
                                       })
                       let specials = from psp in db.Set<SpecialTopicProductRelationEntity>().AsQueryable()
                                      where psp.Product_Id == p.Id
                                      join sp in db.Set<SpecialTopicEntity>().AsQueryable() on psp.SpecialTopic_Id equals sp.Id
                                      select new ESSpecialTopic
                                      {
                                          Id = sp.Id,
                                          Name = sp.Name,
                                          Description = sp.Description
                                      }
                       let promotions = db.Set<Promotion2ProductEntity>().AsQueryable().Where(pp => pp.ProdId == p.Id)
                                         .Join(db.Set<PromotionEntity>().AsQueryable(), o => o.ProId, i => i.Id, (o, i) => i)
                                         .GroupJoin(db.Set<ResourceEntity>().AsQueryable().Where(pr => pr.SourceType == (int)SourceType.Promotion && pr.Type == (int)ResourceType.Image)
                                                    , o => o.Id
                                                    , i => i.SourceId
                                                    , (o, i) => new { Pro = o, R = i.OrderByDescending(r => r.SortOrder) })
                                         .Select(ppr => new ESPromotion
                                         {
                                             Id = ppr.Pro.Id,
                                             Name = ppr.Pro.Name,
                                             Description = ppr.Pro.Description,
                                             CreatedDate = ppr.Pro.CreatedDate,
                                             StartDate = ppr.Pro.StartDate,
                                             EndDate = ppr.Pro.EndDate,
                                             Status = ppr.Pro.Status,
                                             Resource = ppr.R.Select(r => new ESResource()
                                             {
                                                 Domain = r.Domain,
                                                 Name = r.Name,
                                                 SortOrder = r.SortOrder,
                                                 IsDefault = r.IsDefault,
                                                 Type = r.Type,
                                                 Width = r.Width,
                                                 Height = r.Height,
                                                 ColorId = r.ColorId,
                                                 Status = r.Status

                                             })
                                         })
                       let section = (from section in db.Set<SectionEntity>().AsQueryable()
                                      where section.BrandId == p.Brand_Id && section.StoreId == p.Store_Id
                                      select new ESSection()
                                      {
                                          ContactPerson = section.ContactPerson,
                                          ContactPhone = section.ContactPhone,
                                          Id = section.Id,
                                          Location = section.Location,
                                          Name = section.Name,
                                          Status = section.Status
                                      })
                       let propertyValues = (from property in db.Set<ProductPropertyEntity>()
                                             where property.ProductId == p.Id
                                             join v in db.Set<ProductPropertyValueEntity>() on property.Id equals v.PropertyId
                                             select new ESProductPropertyValue
                                             {
                                                 ProductId = p.Id,
                                                 Id = v.Id,
                                                 IsColor = property.IsColor.HasValue && property.IsColor.Value,
                                                 IsSize = property.IsSize.HasValue && property.IsSize.Value,
                                                 PropertyDesc = property.PropertyDesc,
                                                 PropertyId = property.Id,
                                                 ValueDesc = v.ValueDesc
                                             })

                       let stockPropertyValues = (from inv in db.Set<InventoryEntity>()
                                                  where inv.ProductId == p.Id
                                                  join val in db.Set<OPC_StockPropertyValueRawEntity>() on inv.Id equals val.InventoryId
                                                  select new ESStockPropertyValue()
                                                  {
                                                      Id = val.Id,
                                                      InventoryId = inv.Id,
                                                      PropertyData = val.PropertyData,
                                                      UpdateTime = val.UpdateDate,
                                                      BrandSizeCode = val.BrandSizeCode,
                                                      BrandSizeName = val.BrandSizeName
                                                  })
                       let category = (from map in db.Set<ProductMapEntity>() where map.ProductId == p.Id && map.Channel == "intime" select map.ChannelCatId)
                       select new ESProduct()
                       {
                           Id = p.Id,
                           Name = p.Name,
                           Description = p.Description,
                           CreatedDate = p.CreatedDate,
                           Price = p.Price,
                           RecommendedReason = p.RecommendedReason,
                           Status = p.Status,
                           CreateUserId = p.CreatedUser,
                           SortOrder = p.SortOrder,
                           Tag = new ESTag()
                           {
                               Id = t.Id,
                               Name = t.Name,
                               Description = t.Description
                           },
                           Store = new ESStore()
                           {
                               Id = s.Id,
                               Name = s.Name,
                               Description = s.Description,
                               Address = s.Location,
                               Location = new Location
                               {
                                   Lon = s.Longitude,
                                   Lat = s.Latitude
                               },
                               GpsAlt = s.GpsAlt,
                               GpsLat = s.GpsLat,
                               GpsLng = s.GpsLng,
                               Tel = s.Tel
                           },
                           Brand = new ESBrand()
                           {
                               Id = b.Id,
                               Name = b.Name,
                               Description = b.Description,
                               EngName = b.EnglishName
                           },
                           Resource = resource,
                           PropertyValues = propertyValues,
                           StockPropertyValues = stockPropertyValues,
                           SpecialTopic = specials,
                           Promotion = promotions,
                           Is4Sale = p.Is4Sale ?? false,
                           UnitPrice = p.UnitPrice,
                           FavoriteCount = p.FavoriteCount,
                           InvolvedCount = p.InvolvedCount,
                           ShareCount = p.ShareCount,
                           RecommendUserId = p.RecommendUser,
                           Section = section.FirstOrDefault(),
                           UpcCode = p.SkuCode,
                           UpdatedDate = p.UpdatedDate,
                           CategoryId = category.FirstOrDefault() == null ? 0 : (category.FirstOrDefault().HasValue ? category.FirstOrDefault().Value : 0),
                           IsSystem = (!p.ProductType.HasValue) || p.ProductType == (int)ProductType.FromSystem

                       };
            return linq.FirstOrDefault();
        }
       
    }
}
