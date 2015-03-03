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
    class ESStoreService : ESServiceSingle<ESStore>
    {
        protected override ESStore entity2Model(int entityId)
        {
            var db = Context;
            var entity = Context.Set<StoreEntity>().Find(entityId);
            var resource = Context.Set<ResourceEntity>().Where(r => r.SourceId == entityId
                              && r.SourceType == (int)SourceType.StoreLogo)
                          .Select(r => new ESResource()
                          {
                              Domain = r.Domain,
                              Name = r.Name,
                              SortOrder = r.SortOrder,
                              IsDefault = r.IsDefault,
                              Type = r.Type,
                              Width = r.Width,
                              Height = r.Height
                          });


            return new ESStore()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Address = entity.Location,
                Location = new Location
                {
                    Lon = entity.Longitude,
                    Lat = entity.Latitude
                },
                GpsAlt = entity.GpsAlt,
                GpsLat = entity.GpsLat,
                GpsLng = entity.GpsLng,
                Tel = entity.Tel,
                Status = entity.IsOnLine ?? 0,
                Resource = resource,
                Departments = Context.Set<DepartmentEntity>().Where(d => d.StoreId == entityId)
                            .OrderBy(d => d.SortOrder)
                            .ToList()
                            .Select(d => new ESDepartment()
                            {
                                Id = d.Id,
                                Name = d.Name,
                                CreateDate = d.CreateDate,
                                UpdateDate = d.UpdateDate,
                                CreateUser = d.CreateUser,
                                UpdateUser = d.UpdateUser,
                                SortOrder = d.SortOrder
                            }),
                GroupId = entity.Group_Id

            };

        }
    }
}
