
using com.intime.fashion.common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace com.intime.fashion.service
{
    public partial  class ComboService
    {
        
      
        public  bool IfCanOnline(int userId)
        {
            var onlineCount = _db.Set<IMS_AssociateItemsEntity>().Where(ia=>ia.Status==(int)DataStatus.Normal && 
                                    ia.ItemType==(int)ComboType.Product)
                               .Join(_db.Set<IMS_ComboEntity>().Where(ic=>ic.ExpireDate>DateTime.Now),o=>o.ItemId,i=>i.Id,(o,i)=>o)
                               .Join(_db.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == userId), o => o.AssociateId, i => i.Id,
                                (o, i) => o).Count();
            return onlineCount < ConfigManager.MAX_COMBO_ONLINE;
        }

        public void OfflineComboOne(int authuid)
        {
            //use the LFLO policy to offline the oldest combo
            var associateItemRepo = ServiceLocator.Current.Resolve<IEFRepository<IMS_AssociateItemsEntity>>();
            var comboRepo = ServiceLocator.Current.Resolve<IEFRepository<IMS_ComboEntity>>();
            var associateItemEntity = _db.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                               .Join(_db.Set<IMS_AssociateItemsEntity>().Where(iai => iai.ItemType == (int)ComboType.Product && iai.Status==(int)DataStatus.Normal), o => o.Id, i => i.AssociateId, (o, i) => i)
                               .OrderBy(ia=>ia.UpdateDate)
                               .FirstOrDefault();
            associateItemEntity.Status = (int)DataStatus.Default;
            associateItemEntity.UpdateDate = DateTime.Now;
            associateItemRepo.Update(associateItemEntity);

            var comboEntity = _db.Set<IMS_ComboEntity>().Find(associateItemEntity.ItemId);
            comboEntity.Status = (int)DataStatus.Default;
            comboEntity.UpdateDate = DateTime.Now;
            comboRepo.Update(comboEntity);
        }

        public IMS_ComboEntity CreateComboFromProduct(ProductEntity productEntity, IMS_AssociateEntity associateEntity)
        {
           
         
            var createUserId = associateEntity.UserId;
            //step1: offline one other combo
            if (!IfCanOnline(createUserId))
            {
                OfflineComboOne(createUserId);
            }
            //step1.1: create combo

            var immediateTagEntity = _db.Set<Product2IMSTagEntity>().Where(pi => pi.ProductId == productEntity.Id)
                                .Join(_db.Set<IMS_TagEntity>().Where(it=>it.ImmediatePublic.HasValue && it.ImmediatePublic.Value==true),o=>o.IMSTagId,i=>i.Id,(o,i)=>i)
                                .FirstOrDefault();

            var comboEntity = _comboRepo.Insert(new IMS_ComboEntity()
            {
                CreateDate = DateTime.Now,
                CreateUser = createUserId,
                Desc = productEntity.Description,
                OnlineDate = DateTime.Now,
                UnitPrice = productEntity.UnitPrice,
                Price = productEntity.Price,
                Private2Name = string.Empty,
                Status = (int)DataStatus.Normal,
                UpdateDate = DateTime.Now,
                UpdateUser = createUserId,
                UserId = createUserId,
                ProductType = (int)ProductType.FromSelf,
                IsPublic = immediateTagEntity!=null,
                IsInPromotion = false,
                ExpireDate = DateTime.Now.AddDays(ConfigManager.COMBO_EXPIRED_DAYS)
            });

            //step2: create combo2product
            _combo2productRepo.Insert(new IMS_Combo2ProductEntity()
                {
                    ComboId = comboEntity.Id,
                    ProductId = productEntity.Id
                });


            //step2.1 associate combo
            _associateItemRepo.Insert(new IMS_AssociateItemsEntity()
            {
                AssociateId = associateEntity.Id,
                CreateDate = DateTime.Now,
                CreateUser = createUserId,
                ItemId = comboEntity.Id,
                ItemType = (int)ComboType.Product,
                Status = (int)DataStatus.Normal,
                UpdateDate = DateTime.Now,
                UpdateUser = createUserId
            });

            //step3: bind images

            foreach (var resource in _db.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Product &&
                                            r.SourceId == productEntity.Id &&
                                            r.Status == (int)DataStatus.Normal &&
                                            r.Type == (int)ResourceType.Image))
            {
                _resourceRepo.Insert(new ResourceEntity()
                {
                    Size = resource.Size,
                    SortOrder = resource.SortOrder,
                    Name = resource.Name,
                    Height = resource.Height,
                    ExtName = resource.ExtName,
                    Domain = resource.Domain,
                    CreatedUser = resource.CreatedUser,
                    CreatedDate = DateTime.Now,
                    ContentSize = resource.ContentSize,
                    SourceId = comboEntity.Id,
                    SourceType = (int)SourceType.Combo,
                    Type = resource.Type,
                    Status = resource.Status,
                    UpdatedDate = DateTime.Now,
                    UpdatedUser = resource.UpdatedUser,
                    Width = resource.Width
                });
            }

          
            return comboEntity;
        }
    }
}
