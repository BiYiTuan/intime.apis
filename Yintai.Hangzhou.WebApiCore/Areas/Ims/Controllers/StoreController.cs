using com.intime.fashion.common;
using com.intime.fashion.webapi.domain.Request.Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.Customer;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Request.Customer;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public class StoreController : RestfulController
    {
        private IEFRepository<IMS_AssociateEntity> _associateRepo;
        private IEFRepository<IMS_AssociateBrandEntity> _associateBrandRepo;
        private IEFRepository<IMS_AssociateSaleCodeEntity> _associateSaleCodeRepo;
        private IEFRepository<IMS_AssociateItemsEntity> _associateItemRepo;
        private ICustomerRepository _customerRepo;
        private ICustomerDataService _customerService;
        private IEFRepository<IMS_InviteCodeEntity> _inviteRepo;
        private IEFRepository<IMS_InviteCodeRequestEntity> _inviteRequestRepo;
        public StoreController(IEFRepository<IMS_AssociateEntity> associateRepo,
            IEFRepository<IMS_AssociateBrandEntity> associateBrandRepo,
            IEFRepository<IMS_AssociateSaleCodeEntity> associateSaleCodeRepo,
            IEFRepository<IMS_AssociateItemsEntity> associateItemRepo,
            ICustomerRepository customerRepo,
            ICustomerDataService customerService,
            IEFRepository<IMS_InviteCodeEntity> inviteRepo,
            IEFRepository<IMS_InviteCodeRequestEntity> inviteRequestRepo
        )
        {
            _associateRepo = associateRepo;
            _associateBrandRepo = associateBrandRepo;
            _associateSaleCodeRepo = associateSaleCodeRepo;
            _associateItemRepo = associateItemRepo;
            _customerRepo = customerRepo;
            _customerService = customerService;
            _inviteRepo = inviteRepo;
            _inviteRequestRepo = inviteRequestRepo;
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult My(int? authuid)
        {
            var linq = Context.Set<UserEntity>().Where(u => u.Id == authuid.Value)
                       .Join(Context.Set<IMS_AssociateEntity>(), o => o.Id, i => i.UserId, (o, i) => i).First();

            var store = GetStoreById(linq.Id, authuid.Value);
            return this.RenderSuccess<IMSStoreDetailResponse>(m => m.Data = store);

        }

        [RestfulAuthorize]
        public ActionResult Create(string invite_code, int? authuid)
        {
            if (string.IsNullOrEmpty(invite_code))
                return this.RenderError(r => r.Message = "邀请码错误！");
            var inviteEntity = Context.Set<IMS_InviteCodeEntity>().Where(ii => ii.Code == invite_code && ii.Status == (int)DataStatus.Normal)
                                 .Join(Context.Set<IMS_SectionOperatorEntity>(), o => o.SectionOperatorId, i => i.Id, (o, i) => new { Inv = o, Sec = i })
                                 .FirstOrDefault();
            if (inviteEntity == null)
            {
                return this.RenderError(r => r.Message = "邀请码不存在！");
            }
            if (Convert.ToBoolean(inviteEntity.Inv.IsBinded))
                return this.RenderError(r => r.Message = "邀请码已绑定！");

            var exitUserEntity = Context.Set<UserEntity>().Find(authuid);
            if (exitUserEntity.UserLevel == (int)UserLevel.DaoGou)
                return this.RenderError(r => r.Message = "用户已经开过店了！");
            //steps:
            // 1. read initial info from invite code tables
            // 2. initialize associate tables
            using (var ts = new TransactionScope())
            {
                var initialBrands = Context.Set<IMS_SectionBrandEntity>().Where(isb => isb.SectionId == inviteEntity.Sec.SectionId);
                var initialSaleCodes = Context.Set<IMS_SalesCodeEntity>().Where(isc => isc.SectionId == inviteEntity.Sec.SectionId);
                var sectionEntity = Context.Set<SectionEntity>().Find(inviteEntity.Sec.SectionId);
                //2.0 disable invite code
                inviteEntity.Inv.IsBinded = 1;
                inviteEntity.Inv.UpdateDate = DateTime.Now;
                inviteEntity.Inv.UserId = authuid.Value;
                _inviteRepo.Update(inviteEntity.Inv);

                //2.1 update user level to daogou
                exitUserEntity.UserLevel = (int)UserLevel.DaoGou;
                exitUserEntity.UpdatedDate = DateTime.Now;
                exitUserEntity.UpdatedUser = exitUserEntity.Id;
                if (string.IsNullOrEmpty(exitUserEntity.Logo))
                    exitUserEntity.Logo = ConfigManager.IMS_DEFAULT_LOGO;
                _customerRepo.Update(exitUserEntity);

                //2.2 create daogou's associate store
                var assocateEntity = _associateRepo.Insert(new IMS_AssociateEntity()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = authuid.Value,
                    OperateRight = inviteEntity.Inv.AuthRight.Value,
                    Status = (int)DataStatus.Normal,
                    TemplateId = ConfigManager.IMS_DEFAULT_TEMPLATE,
                    UserId = authuid.Value,
                    StoreId = sectionEntity.StoreId ?? 0,
                    SectionId = sectionEntity.Id,
                    OperatorCode = inviteEntity.Sec.OperatorCode
                });
                //2.3 create daogou's brands
                foreach (var brand in initialBrands)
                {
                    _associateBrandRepo.Insert(new IMS_AssociateBrandEntity()
                    {
                        AssociateId = assocateEntity.Id,
                        BrandId = brand.BrandId,
                        CreateDate = DateTime.Now,
                        CreateUser = authuid.Value,
                        Status = (int)DataStatus.Normal,
                        UserId = authuid.Value
                    });
                }
                //2.4 create daogou's sales code
                foreach (var saleCode in initialSaleCodes)
                {
                    _associateSaleCodeRepo.Insert(new IMS_AssociateSaleCodeEntity()
                    {
                        AssociateId = assocateEntity.Id,
                        Code = saleCode.Code,
                        CreateDate = DateTime.Now,
                        CreateUser = authuid.Value,
                        Status = (int)DataStatus.Normal,
                        UserId = authuid.Value

                    });
                }
                //2.5 create daogou's giftcard
                var groupEntity = Context.Set<StoreEntity>().Where(s=>s.Id == sectionEntity.StoreId)
                                .Join(Context.Set<GroupEntity>(),o=>o.Group_Id,i=>i.Id,(o,i)=>i).FirstOrDefault();
                if (groupEntity != null)
                {
                    var giftCardEntity = Context.Set<IMS_GiftCardEntity>().Where(igc => igc.Status == (int)DataStatus.Normal &&igc.GroupId == groupEntity.Id)
                                        .FirstOrDefault();
                    if (giftCardEntity != null)
                    {
                        _associateItemRepo.Insert(new IMS_AssociateItemsEntity()
                        {
                            AssociateId = assocateEntity.Id,
                            CreateDate = DateTime.Now,
                            CreateUser = authuid.Value,
                            ItemId = giftCardEntity.Id,
                            ItemType = (int)ComboType.GiftCard,
                            Status = (int)DataStatus.Normal,
                            UpdateDate = DateTime.Now,
                            UpdateUser = authuid.Value
                        });
                    }
                }
                ts.Complete();

            }
            var response = _customerService.GetUserInfo(new GetUserInfoRequest
            {
                AuthUid = authuid.Value
            });
            return this.RenderSuccess<CustomerInfoResponse>(c => c.Data = response.Data);

        }

        [RestfulAuthorize]
        public ActionResult RequestCode_Basic(IMSStoreInviteCodeBasicRequest request, int? authuid)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var requestEntity = Context.Set<IMS_InviteCodeRequestEntity>().Where(iv => iv.UserId == authuid.Value
                                                && iv.Status != (int)InviteCodeRequestStatus.Reject).FirstOrDefault();
            if (requestEntity != null)
            {
                return this.RenderError(r => r.Message = "用户已申请邀请码，正在处理中...");
            }

            var associateEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid.Value).FirstOrDefault();
            if (associateEntity != null)
            {
                return this.RenderError(r => r.Message = "用户已开店！");
            }

            _inviteRequestRepo.Insert(new IMS_InviteCodeRequestEntity()
            {
                ContactMobile = request.Mobile,
                CreateDate = DateTime.Now,
                CreateUser = authuid.Value,
                Name = request.Name,
                UpdateDate = DateTime.Now,
                UpdateUser = authuid.Value,
                RequestType = (int)InviteCodeRequestType.Basic,
                Status = (int)InviteCodeRequestStatus.Requesting

            });

            return this.RenderSuccess<dynamic>(null);

        }

        [RestfulAuthorize]
        public ActionResult RequestCode_DG(IMSStoreInviteCodeDaogouRequest request, int? authuid)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var requestEntity = Context.Set<IMS_InviteCodeRequestEntity>().Where(iv => iv.UserId == authuid.Value
                                                && iv.Status != (int)InviteCodeRequestStatus.Reject).FirstOrDefault();
            if (requestEntity != null)
            {
                return this.RenderError(r => r.Message = "用户已申请，正在处理中...");
            }

            var associateEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid.Value).FirstOrDefault();
            if (associateEntity != null)
            {
                return this.RenderError(r => r.Message = "用户已开店！");
            }
            var sectionEntity = Context.Set<SectionEntity>().Where(s => s.Status == (int)DataStatus.Normal
                    && s.StoreId == request.StoreId
                    && s.SectionCode == request.SectionCode)
                    .FirstOrDefault();
            if (sectionEntity == null)
                return this.RenderError(r => r.Message = "专柜码不存在！");
            _inviteRequestRepo.Insert(new IMS_InviteCodeRequestEntity()
            {
                ContactMobile = request.Mobile,
                CreateDate = DateTime.Now,
                CreateUser = authuid.Value,
                Name = request.Name,
                UpdateDate = DateTime.Now,
                UpdateUser = authuid.Value,
                OperatorCode = request.OperatorCode,
                RequestType = (int)InviteCodeRequestType.Daogou,
                SectionCode = request.SectionCode,
                SectionName = sectionEntity.Name,
                Status = (int)InviteCodeRequestStatus.Requesting,
                StoreId = request.StoreId,
                DepartmentId = request.DepartId,
                UserId = authuid.Value
            });

            return this.RenderSuccess<dynamic>(null);

        }

        [RestfulAuthorize]
        public ActionResult RequestCode_Upgrade(IMSStoreInviteCodeUpgradeRequest request, int? authuid)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var associateEntity = Context.Set<IMS_AssociateEntity>().Where(ia=>ia.UserId ==authuid && ia.Status==(int)DataStatus.Normal)
                    .FirstOrDefault();
            if ((associateEntity.OperateRight & (int)UserOperatorRight.SelfProduct) ==
                (int)UserOperatorRight.SelfProduct)
                return this.RenderError(r => r.Message = "用户已有最高权限！");
          
            var requestEntity = Context.Set<IMS_InviteCodeRequestEntity>().Where(iv => iv.UserId == authuid.Value
                                                && iv.Status != (int)InviteCodeRequestStatus.Reject
                                                && iv.Status !=(int)InviteCodeRequestStatus.Approved).FirstOrDefault();
            if (requestEntity != null)
            {
                return this.RenderError(r => r.Message = "用户已申请，正在处理中...");
            }

            var sectionEntity = Context.Set<SectionEntity>().Where(s => s.Status == (int)DataStatus.Normal
                    && s.StoreId == request.StoreId
                    && s.SectionCode == request.SectionCode)
                    .FirstOrDefault();
            if (sectionEntity == null)
                return this.RenderError(r => r.Message = "专柜码不存在！");

            _inviteRequestRepo.Insert(new IMS_InviteCodeRequestEntity()
            {
                ContactMobile = request.Mobile,
                CreateDate = DateTime.Now,
                CreateUser = authuid.Value,
                Name = request.Name,
                UpdateDate = DateTime.Now,
                UpdateUser = authuid.Value,
                OperatorCode = request.OperatorCode,
                RequestType = (int)InviteCodeRequestType.Daogou,
                SectionCode = request.SectionCode,
                SectionName = sectionEntity.Name,
                Status = (int)InviteCodeRequestStatus.Requesting,
                StoreId = request.StoreId,
                DepartmentId = request.DepartId,
                UserId = authuid.Value
            });

            return this.RenderSuccess<dynamic>(null);

        }

        [RestfulAuthorize]
        public ActionResult Detail(int id, int? authuid)
        {
            var store = GetStoreById(id, authuid.Value);
            return this.RenderSuccess<IMSStoreDetailResponse>(m => m.Data = store);

        }

        private IMSStoreDetailResponse GetStoreById(int storeId, int authuid)
        {
            var linq = Context.Set<UserEntity>()
                       .Join(Context.Set<IMS_AssociateEntity>().Where(ia => ia.Id == storeId), o => o.Id, i => i.UserId, (o, i) => new { U = o, Store = i })
                       .First();
            var groupLinq = Context.Set<IMS_AssociateEntity>().Where(ia => ia.Id == storeId)
                           .Join(Context.Set<StoreEntity>(), o => o.StoreId, i => i.Id, (o, i) => i)
                           .Join(Context.Set<GroupEntity>(), o => o.Group_Id, i => i.Id, (o, i) => i)
                           .Join(Context.Set<IMS_GiftCardEntity>().Where(igc=>igc.Status==(int)DataStatus.Normal),o=>o.Id,i=>i.GroupId,(o,i)=>i)
                           .FirstOrDefault();
            
            var giftCards = Context.Set<IMS_AssociateItemsEntity>().Where(iai => iai.AssociateId == linq.Store.Id
                                             && iai.ItemType == (int)ComboType.GiftCard
                                             && iai.Status == (int)DataStatus.Normal)
                                 .Join(Context.Set<IMS_GiftCardEntity>(), o => o.ItemId, i => i.Id, (o, i) => i)
                                 .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.GiftCard && r.Status == (int)DataStatus.Normal),
                                                 o => o.Id, i => i.SourceId, (o, i) => new { G = o, R = i.OrderByDescending(ir => ir.SortOrder).ThenBy(ir => ir.Id).FirstOrDefault() })
                                 .Select(l => new IMSGiftCard()
                                 {
                                     Id = l.G.Id,
                                     Desc = l.G.Name,
                                     ImageUrl = l.R == null ? string.Empty : l.R.Name
                                 }).ToList();
            var combos = Context.Set<IMS_AssociateItemsEntity>().Where(iai => iai.AssociateId == linq.Store.Id
                                             && iai.ItemType == (int)ComboType.Product
                                             && iai.Status == (int)DataStatus.Normal)
                                 .Join(Context.Set<IMS_ComboEntity>().Where(c => (!c.ExpireDate.HasValue || c.ExpireDate > DateTime.Now) &&
                                            !Context.Set<IMS_Combo2ProductEntity>().Where(icp=>icp.ComboId == c.Id)
                                                    .Join(Context.Set<Product2IMSTagEntity>(),o=>o.ProductId,i=>i.ProductId,(o,i)=>i)
                                                    .Join(Context.Set<IMS_TagEntity>().Where(it=>(it.Only4Tmall??false)==true),o=>o.IMSTagId,i=>i.Id,(o,i)=>i)
                                                    .Any())
                                        , o => o.ItemId, i => i.Id, (o, i) => i)
                                 .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Combo && r.Status == (int)DataStatus.Normal),
                                                 o => o.Id, i => i.SourceId, (o, i) => new { C = o, R = i.OrderByDescending(ir => ir.SortOrder).ThenBy(ir => ir.Id).FirstOrDefault() })
                                 .GroupJoin(Context.Set<IMS_Combo2ProductEntity>()
                                             .GroupJoin(Context.Set<ResourceEntity>()
                                                         .Where(r => r.SourceType == (int)SourceType.Product && r.Status == (int)DataStatus.Normal && r.Type == (int)ResourceType.Image)
                                                             , o => o.ProductId,
                                                             i => i.SourceId,
                                                             (o, i) => new { P = o, R = i.OrderByDescending(ir => ir.SortOrder).FirstOrDefault() })
                                          , o => o.C.Id
                                          , i => i.P.ComboId
                                          , (o, i) => new { C = o.C, CR = o.R, P = i })
                                .GroupJoin(Context.Set<IMS_Combo2ProductEntity>()
                                                .Join(Context.Set<Product2IMSTagEntity>(), o => o.ProductId, i => i.ProductId, (o, i) => new { ICP = o, PIT = i })
                                                .Join(Context.Set<IMS_TagEntity>().Where(it => it.Status == (int)DataStatus.Normal), o => o.PIT.IMSTagId, i => i.Id, (o, i) => new { ICP = o.ICP, IT = i }),
                                            o => o.C.Id,
                                            i => i.ICP.ComboId,
                                            (o, i) => new { C = o.C, CR = o.CR, P = o.P, IT = i })
                                .GroupJoin(Context.Set<IMS_Combo2ProductEntity>()
                                            .Join(Context.Set<ProductEntity>(),o=>o.ProductId,i=>i.Id,(o,i)=>new {ICP=o,Product=i})
                                            .Join(Context.Set<BrandEntity>(), o => o.Product.Brand_Id, i => i.Id, (o, i) => new { ICP = o.ICP, Brand = i }),
                                           o=>o.C.Id,
                                           i=>i.ICP.ComboId,
                                           (o,i)=>new {C = o.C, CR = o.CR, P = o.P, IT = o.IT,Brands=i })
                                 .OrderByDescending(ia => ia.C.CreateDate)
                                 .ToList()
                                 .Select(l => new IMSCombo()
                                  {
                                      Id = l.C.Id,
                                      Desc = l.C.Desc,
                                      ImageUrl = l.CR==null?string.Empty:l.CR.Name,
                                      Price = l.C.Price,
                                      //ProductImageUrls = l.P.Select(lpr => lpr.R.Name),
                                      ExpireDate = l.C.ExpireDate,
                                      IsInPromotion = l.C.IsInPromotion,
                                      DiscountAmount = l.C.DiscountAmount,
                                      Tags = l.IT==null?null:l.IT.Select(it => new IMSTagResponse()
                                      {
                                          Id = it.IT.Id,
                                          Name = it.IT.Name
                                      }).Distinct(),
                                      Brands = l.Brands==null?null: l.Brands.Select(b=>new 
                                            {Id = b.Brand.Id,Name = b.Brand.Name})
                                            .Distinct()
                                  });
  
            return new IMSStoreDetailResponse()
            {
                Id = linq.Store.Id,
                Name = linq.U.Nickname,
                Logo = linq.U.Logo,
                GiftCardSaling = giftCards,
                ComboSaling = combos,
                Mobile = linq.U.Mobile,
                Is_Owner = authuid == linq.U.Id,
                Is_Favored = Context.Set<FavoriteEntity>().Any(f => f.User_Id == authuid &&
                                    f.FavoriteSourceType == (int)SourceType.Store &&
                                    f.FavoriteSourceId == storeId &&
                                    f.Status == (int)DataStatus.Normal),
                Template_Id = linq.Store.TemplateId ?? 1,
                IsSupportGiftCard = groupLinq!=null
            };
        }
    }
}
