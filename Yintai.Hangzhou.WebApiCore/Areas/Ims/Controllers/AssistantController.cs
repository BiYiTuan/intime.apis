using com.intime.fashion.common;
using com.intime.fashion.common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using com.intime.fashion.service;
using Yintai.Hangzhou.WebSupport.Mvc;
using com.intime.fashion.service.contract;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public partial class AssistantController : RestfulController
    {
        private IEFRepository<IMS_AssociateSaleCodeEntity> _salescodeRepo;
        private IEFRepository<IMS_AssociateItemsEntity> _associateitemRepo;
        private IEFRepository<IMS_AssociateIncomeRequestEntity> _incomerequestRepo;
        private ICustomerRepository _userRepo;
        private IEFRepository<IMS_AssociateEntity> _associateRepo;
        private IFeedbackRepository _feedbackRepo;
        private IEFRepository<IMS_GiftCardEntity> _cardRepo;
        private IResourceRepository _resourceRepo;
        private IInventoryRepository _inventoryRepo;
        private IEFRepository<IMS_ComboEntity> _comboRepo;
        private IEFRepository<IMS_AssociateIncomeEntity> _incomeRepo;
        private IComboService _comboService;
        private IEFRepository<IMS_InviteCodeRequestEntity> _inviteCodeRequestRepo;
        private IEFRepository<SectionEntity> _sectionRepo;
        public AssistantController(IEFRepository<IMS_AssociateSaleCodeEntity> salescodeRepo,
            IEFRepository<IMS_AssociateItemsEntity> associateitemRepo,
            IEFRepository<IMS_AssociateIncomeRequestEntity> incomerequestRepo,
            ICustomerRepository userRepo,
            IEFRepository<IMS_AssociateEntity> associateRepo,
            IFeedbackRepository feedbackRepo,
            IEFRepository<IMS_GiftCardEntity> cardRepo,
            IResourceRepository resourceRepo,
            IInventoryRepository inventoryRepo,
            IEFRepository<IMS_ComboEntity> comboRepo,
            IEFRepository<IMS_AssociateIncomeEntity> incomeRepo,
            IComboService comboService,
            IEFRepository<IMS_InviteCodeRequestEntity> inviteCodeRequestRepo,
            IEFRepository<SectionEntity> sectionRepo
            )
        {
            _salescodeRepo = salescodeRepo;
            _associateitemRepo = associateitemRepo;
            _incomerequestRepo = incomerequestRepo;
            _userRepo = userRepo;
            _associateRepo = associateRepo;
            _feedbackRepo = feedbackRepo;
            _cardRepo = cardRepo;
            _resourceRepo = resourceRepo;
            _inventoryRepo = inventoryRepo;
            _comboRepo = comboRepo;
            _incomeRepo = incomeRepo;
            _comboService = comboService;
            _inviteCodeRequestRepo = inviteCodeRequestRepo;
            _sectionRepo = sectionRepo;
        }
        [RestfulAuthorize]
        public ActionResult Gift_Cards(PagerInfoRequest request, int authuid)
        {
            var groupLinq = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                            .Join(Context.Set<StoreEntity>(), o => o.StoreId, i => i.Id, (o, i) => i)
                            .Join(Context.Set<GroupEntity>(), o => o.Group_Id, i => i.Id, (o, i) => i);
            int page = request.Page <= 0 ? 0 : request.Page - 1;
            int pagesize = request.Pagesize >= 40 || request.Pagesize <= 0 ? 20 : request.Pagesize;
            var count = _cardRepo.Get(x => x.Status == 1).Count();
            var cards = new List<dynamic>();
            var linq =
                _cardRepo.Get(x => x.Status == 1)
                    .Join(groupLinq, o => o.GroupId, i => i.Id, (o, i) => i)
                    .OrderByDescending(x => x.Id)
                    .Skip(page * pagesize)
                    .Take(pagesize)
                    .GroupJoin(Context.Set<IMS_AssociateItemsEntity>().Where(x => x.ItemType == (int)ComboType.GiftCard && x.CreateUser == authuid), card => card.Id, item => item.ItemId, (c, i) => new { id = c.Id, desc = c.Name, sale = i.FirstOrDefault() })
                    .GroupJoin(_resourceRepo.Get(x => x.SourceType == (int)SourceType.GiftCard), c => c.id,
                        s => s.SourceId, (c, rs) => new { card = c, image = rs.FirstOrDefault() });
            foreach (var cr in linq)
            {
                cards.Add(new
                {
                    id = cr.card.id,
                    desc = cr.card.desc,
                    image = cr.image.Name.Image320Url(),
                    is_online = cr.card.sale != null && cr.card.sale.Status == (int)DataStatus.Normal,
                });
            }
            var rsp = new PagerInfoResponse<dynamic>(request.PagerRequest, count) { Items = cards };
            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = rsp);
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Combos(PagerInfoRequest request, int authuid)
        {
            var linq = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                       .Join(Context.Set<IMS_AssociateItemsEntity>().Where(ia => ia.ItemType == (int)ComboType.Product && ia.Status != (int)DataStatus.Deleted), o => o.Id, i => i.AssociateId, (o, i) => i)
                       .Join(Context.Set<IMS_ComboEntity>(), o => o.ItemId, i => i.Id, (o, i) => i)
                       .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Combo && r.Status == (int)DataStatus.Normal)
                                , o => o.Id
                                , i => i.SourceId
                                , (o, i) => new { A = o, R = i.OrderByDescending(ir => ir.SortOrder).ThenBy(ir => ir.Id).FirstOrDefault() });

            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.A.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList().Select(l => new Yintai.Hangzhou.Contract.DTO.Response.IMSComboDetailResponse().FromEntity<IMSComboDetailResponse>(l.A, o =>
            {
                if (l.R == null)
                    return;
                o.ImageUrl = l.R.Name;


            }));
            var response = new PagerInfoResponse<IMSComboDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };

            return this.RenderSuccess<PagerInfoResponse<IMSComboDetailResponse>>(c => c.Data = response);
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Combos_Online_Count(int authuid)
        {
            var linq = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                       .Join(Context.Set<IMS_AssociateItemsEntity>().Where(ia => ia.ItemType == (int)ComboType.Product && ia.Status == (int)DataStatus.Normal),
                                            o => o.Id, i => i.AssociateId,
                                            (o, i) => i);

            return this.RenderSuccess<dynamic>(c => c.Data = new
            {
                total_count = linq.Count()
            });
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Products(PagerInfoRequest request, int authuid)
        {
            var linq = Context.Set<ProductEntity>().Where(ia => ia.CreatedUser == authuid && ia.Status != (int)DataStatus.Deleted && ia.ProductType == (int)ProductType.FromSelf)
                       .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Product && r.Status == (int)DataStatus.Normal),
                                    o => o.Id,
                                    i => i.SourceId,
                                    (o, i) => new { P = o, R = i.OrderByDescending(ir => ir.SortOrder) });
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.P.CreatedDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList().Select(l => new IMSProductSelfDetailResponse().FromEntity<IMSProductSelfDetailResponse>(l.P, o =>
            {

                if (l.R == null)
                    return;
                o.Images = l.R.Select(pr => new IMSSelfImageResponse()
                {
                    Id = pr.Id,
                    Name = pr.Name
                });
            }));
            var response = new PagerInfoResponse<IMSProductSelfDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };

            return this.RenderSuccess<PagerInfoResponse<IMSProductSelfDetailResponse>>(c => c.Data = response);

        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult SalesCodes(int authuid)
        {
            var linq = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                            .GroupJoin(Context.Set<IMS_AssociateSaleCodeEntity>(),
                                        o => o.Id,
                                        i => i.AssociateId,
                                        (o, i) => i).FirstOrDefault();
            var salesCodes = linq == null ? new string[] { } : linq.Select(l => l.Code).ToArray();
            var response = new PagerInfoResponse<string>(new PagerRequest(), salesCodes.Count())
            {
                Items = salesCodes.ToList()
            };


            return this.RenderSuccess<PagerInfoResponse<string>>(c => c.Data = response);
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Avail_Banks()
        {
            var linq = Context.Set<IMS_BankEntity>().Where(ia => ia.Status == (int)DataStatus.Normal).OrderBy(l => l.Name);
            var response = new PagerInfoResponse<dynamic>(new PagerRequest(), linq.Count())
            {
                Items = linq.ToList().Select(l => new
                {
                    code = l.Code,
                    name = l.Name
                }).ToList<dynamic>()
            };


            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = response);
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult SalesCode_Add(string sale_code, int authuid)
        {
            if (string.IsNullOrEmpty(sale_code))
                return this.RenderError(r => r.Message = "销售码为空");
            var linq = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid).FirstOrDefault();
            if (linq == null)
                return this.RenderError(r => r.Message = "无权操作");

            var codeEntity = Context.Set<IMS_AssociateSaleCodeEntity>().Where(ias => ias.AssociateId == linq.Id && ias.Code == sale_code).FirstOrDefault();
            if (codeEntity != null)
                return this.RenderError(r => r.Message = "销售码已存在！");



            _salescodeRepo.Insert(new IMS_AssociateSaleCodeEntity()
            {
                AssociateId = linq.Id,
                Code = sale_code,
                CreateDate = DateTime.Now,
                CreateUser = authuid,
                Status = (int)DataStatus.Normal,
                UserId = authuid
            });


            return this.RenderSuccess<dynamic>(null);
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Brands(int authuid)
        {
            var linq = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                            .GroupJoin(Context.Set<IMS_SectionBrandEntity>().Join(Context.Set<BrandEntity>().Where(b => b.Status == (int)DataStatus.Normal),
                                                                    o => o.BrandId,
                                                                    i => i.Id,
                                                                    (o, i) => new { AB = o, B = i }),
                                        o => o.SectionId,
                                        i => i.AB.SectionId,
                                        (o, i) => i).FirstOrDefault();
            List<dynamic> brands = null;
            if (linq != null)
                brands = linq.Select(l => new
                {
                    id = l.AB.BrandId,
                    name = l.B.Name
                }).ToList<dynamic>();
            else
                brands = new List<dynamic>();
            var response = new PagerInfoResponse<dynamic>(new PagerRequest(), brands.Count())
            {
                Items = brands
            };


            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = response);

        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Combo_Status_Update(IMSComboStatusUpdateRequest request, int authuid)
        {
            var comboItemEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                                .Join(Context.Set<IMS_AssociateItemsEntity>().Where(iai => iai.ItemType == request.Item_Type && iai.ItemId == request.Item_Id), o => o.Id, i => i.AssociateId, (o, i) => i)
                                .FirstOrDefault();
            if (comboItemEntity == null)
                return this.RenderError(r => r.Message = "无权操作该组合");
            if (request.Item_Type == (int)ComboType.Product && request.Is_Online)
            {
                if (!_comboService.IfCanOnline(authuid))
                    return this.RenderError(r => r.Message = "店铺上线组合数量超出限制！");
            }
            using (var ts = new TransactionScope())
            {
                comboItemEntity.Status = request.Is_Online ? (int)DataStatus.Normal : (int)DataStatus.Default;
                comboItemEntity.UpdateDate = DateTime.Now;
                _associateitemRepo.Update(comboItemEntity);

                if (request.Item_Type == (int)ComboType.Product)
                {
                    var comboEntity = Context.Set<IMS_ComboEntity>().Find(request.Item_Id);
                    comboEntity.Status = comboItemEntity.Status;
                    comboEntity.UpdateDate = DateTime.Now;
                    comboEntity.ExpireDate = DateTime.Now.AddDays(ConfigManager.COMBO_EXPIRED_DAYS);
                    _comboRepo.Update(comboEntity);

                    if (request.Is_Online)
                    {
                        var inventories = Context.Set<ProductEntity>().Where(p => p.ProductType == (int)ProductType.FromSelf)
                                        .Join(Context.Set<IMS_Combo2ProductEntity>().Where(ic => ic.ComboId == request.Item_Id),
                                             o => o.Id,
                                             i => i.ProductId,
                                             (o, i) => o)
                                         .Join(Context.Set<InventoryEntity>().Join(Context.Set<ProductPropertyValueEntity>().Where(ppv => ppv.Status == (int)DataStatus.Normal),
                                                                                    o => o.PSizeId,
                                                                                    i => i.Id,
                                                                                    (o, i) => o),
                                                    o => o.Id,
                                                    i => i.ProductId,
                                                    (o, i) => i);
                        foreach (var inventory in inventories)
                        {
                            if (inventory.Amount < 1)
                            {
                                inventory.Amount = 1;
                                inventory.UpdateDate = DateTime.Now;
                                _inventoryRepo.Update(inventory);
                            }
                        }
                    }
                }
                ts.Complete();
            }
            return this.RenderSuccess<dynamic>(null);

        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Income(int authuid)
        {
            var storeEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                          .Join(Context.Set<StoreEntity>(), o => o.StoreId, i => i.Id, (o, i) => i).First();
            var groupId = storeEntity.Group_Id;
            var incomeEntity = Context.Set<IMS_AssociateIncomeEntity>().Where(iai => iai.UserId == authuid && iai.GroupId == groupId).FirstOrDefault();
            if (incomeEntity == null)
                incomeEntity = new IMS_AssociateIncomeEntity()
                {
                    ReceivedAmount = 0,
                    AvailableAmount = 0,
                    TotalAmount = 0,
                    RequestAmount = 0


                };
            return this.RenderSuccess<dynamic>(c => c.Data = new
            {
                received_amount = incomeEntity.ReceivedAmount,
                frozen_amount = incomeEntity.TotalAmount - incomeEntity.AvailableAmount - incomeEntity.RequestAmount - incomeEntity.ReceivedAmount,
                request_amount = incomeEntity.RequestAmount,
                avail_amount = incomeEntity.AvailableAmount,
                total_amount = incomeEntity.TotalAmount

            });
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Income_Received(PagerInfoRequest request, int authuid)
        {
            var storeEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                          .Join(Context.Set<StoreEntity>(), o => o.StoreId, i => i.Id, (o, i) => i).First();
            var groupId = storeEntity.Group_Id;
            var linq = Context.Set<IMS_AssociateIncomeRequestEntity>().
                        Where(iair => iair.UserId == authuid && iair.GroupId == groupId);
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList().Select(l => new IMSIncomeReqDetailResponse().FromEntity<IMSIncomeReqDetailResponse>(l));
            var response = new PagerInfoResponse<IMSIncomeReqDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };

            return this.RenderSuccess<PagerInfoResponse<IMSIncomeReqDetailResponse>>(c => c.Data = response);
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Income_Requesting(PagerInfoRequest request, int authuid)
        {
            var storeEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                           .Join(Context.Set<StoreEntity>(), o => o.StoreId, i => i.Id, (o, i) => i).First();
            var groupId = storeEntity.Group_Id;
            var linq = Context.Set<IMS_AssociateIncomeRequestEntity>().
                        Where(iair => iair.UserId == authuid &&
                               (iair.Status == (int)AssociateIncomeTransferStatus.RequestSent ||
                               iair.Status == (int)AssociateIncomeTransferStatus.NotStart)
                               && iair.GroupId == groupId);
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList().Select(l => new IMSIncomeReqDetailResponse().FromEntity<IMSIncomeReqDetailResponse>(l));
            var response = new PagerInfoResponse<IMSIncomeReqDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };

            return this.RenderSuccess<PagerInfoResponse<IMSIncomeReqDetailResponse>>(c => c.Data = response);
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Income_History(PagerInfoRequest request, int authuid)
        {
            var storeEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                           .Join(Context.Set<StoreEntity>(), o => o.StoreId, i => i.Id, (o, i) => i).First();
            var groupId = storeEntity.Group_Id;
            var linq = Context.Set<IMS_AssociateIncomeHistoryEntity>().
                            Where(iair => iair.AssociateUserId == authuid 
                                && iair.Status > (int)AssociateIncomeStatus.Create 
                                && iair.SourceType == (int)AssociateOrderType.GiftCard
                                && iair.GroupId == groupId)
                            .Join(Context.Set<IMS_GiftCardOrderEntity>(), o => o.SourceNo, i => i.No, (o, i) => new IMSIncomeDetailResponse
                            {
                                CreateDate = o.CreateDate,
                                Id = i.Id,
                                AssociateIncome = o.AssociateIncome,
                                SourceNo = o.SourceNo,
                                TotalAmount = i.Amount,
                                SourceType = o.SourceType,
                                Status = o.Status
                            });

            var linq2 = Context.Set<IMS_AssociateIncomeHistoryEntity>().
                        Where(iair => iair.AssociateUserId == authuid 
                                && iair.Status > (int)AssociateIncomeStatus.Create 
                                && iair.SourceType == (int)AssociateOrderType.Product
                                && iair.GroupId == groupId)
                        .Join(Context.Set<OrderEntity>(), o => o.SourceNo, i => i.OrderNo, (o, i) => new IMSIncomeDetailResponse
                        {
                            CreateDate = o.CreateDate,
                            Id = i.Id,
                            AssociateIncome = o.AssociateIncome,
                            SourceNo = o.SourceNo,
                            TotalAmount = i.TotalAmount,
                            SourceType = o.SourceType,
                            Status = o.Status
                        });
            linq = linq.Union(linq2);
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList();
            var response = new PagerInfoResponse<IMSIncomeDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };

            return this.RenderSuccess<PagerInfoResponse<IMSIncomeDetailResponse>>(c => c.Data = response);
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Income_Frozen(PagerInfoRequest request, int authuid)
        {
            var storeEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                           .Join(Context.Set<StoreEntity>(), o => o.StoreId, i => i.Id, (o, i) => i).First();
            var groupId = storeEntity.Group_Id;
            var linq = Context.Set<IMS_AssociateIncomeHistoryEntity>().
                        Where(iair => iair.AssociateUserId == authuid 
                                && iair.Status == (int)AssociateIncomeStatus.Frozen 
                                && iair.SourceType == (int)AssociateOrderType.GiftCard
                                && iair.GroupId == groupId)
                        .Join(Context.Set<IMS_GiftCardOrderEntity>(), o => o.SourceNo, i => i.No, (o, i) => new IMSIncomeDetailResponse
                         {
                             CreateDate = o.CreateDate,
                             Id = i.Id,
                             AssociateIncome = o.AssociateIncome,
                             SourceNo = o.SourceNo,
                             TotalAmount = i.Amount,
                             SourceType = o.SourceType,
                             Status = o.Status
                         });

            var linq2 = Context.Set<IMS_AssociateIncomeHistoryEntity>().
                        Where(iair => iair.AssociateUserId == authuid 
                            && iair.Status == (int)AssociateIncomeStatus.Frozen 
                            && iair.SourceType == (int)AssociateOrderType.Product
                            && iair.GroupId == groupId)
                        .Join(Context.Set<OrderEntity>(), o => o.SourceNo, i => i.OrderNo, (o, i) => new IMSIncomeDetailResponse
                        {
                            CreateDate = o.CreateDate,
                            Id = i.Id,
                            AssociateIncome = o.AssociateIncome,
                            SourceNo = o.SourceNo,
                            TotalAmount = i.TotalAmount,
                            SourceType = o.SourceType,
                            Status = o.Status
                        });
            linq = linq.Union(linq2);
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList();
            var response = new PagerInfoResponse<IMSIncomeDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };

            return this.RenderSuccess<PagerInfoResponse<IMSIncomeDetailResponse>>(c => c.Data = response);
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Income_Request(IMSIncomeReqRequest request, int authuid)
        {
            //CHECK THE income request rule
            if (string.IsNullOrEmpty(request.Bank_No))
                return this.RenderError(r => r.Message = "银行卡号不能为空");
            if (string.IsNullOrEmpty(request.User_Name))
                return this.RenderError(r => r.Message = "银行帐户名不能为空");
            if (string.IsNullOrEmpty(request.Id_Card))
                return this.RenderError(r => r.Message = "身份证号码不能为空");
            if (request.Amount <= ConfigManager.BANK_TRANSFER_FEE)
                return this.RenderError(r => r.Message = string.Format("提现最小金额须大于{0}", ConfigManager.BANK_TRANSFER_FEE));
            var storeEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid)
                            .Join(Context.Set<StoreEntity>(), o => o.StoreId, i => i.Id, (o, i) => i).First();
            var groupId = storeEntity.Group_Id;
           
            var bankEntity = Context.Set<IMS_BankEntity>().
                        Where(ib => ib.Status == (int)DataStatus.Normal && ib.Code == request.Bank_Code).
                        FirstOrDefault();
            if (bankEntity == null)
                return this.RenderError(r => r.Message = "银行不支持提现");
            var txOptions = new TransactionOptions();
            txOptions.IsolationLevel = IsolationLevel.Serializable;
            using (var ts = new TransactionScope(TransactionScopeOption.Required, txOptions))
            {
                var incomeAccountEntity = Context.Set<IMS_AssociateIncomeEntity>().Where(iai => iai.UserId == authuid && iai.GroupId == groupId).FirstOrDefault();
                if (incomeAccountEntity == null)
                    return this.RenderError(r => r.Message = "账户内可提现金额不足！");
                if (incomeAccountEntity.AvailableAmount < request.Amount)
                    return this.RenderError(r => r.Message = "账户内可提现金额不足!");
                var thisMonth = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01"));
                var incomeRequestHistory = Context.Set<IMS_AssociateIncomeRequestEntity>().Where(iair => iair.UserId == authuid &&
                                iair.Status != (int)AssociateIncomeRequestStatus.Failed &&
                                iair.GroupId == groupId &&
                                iair.CreateDate > thisMonth);
                var requestedAmount = incomeRequestHistory.Sum(l => (decimal?)l.Amount) ?? 0m;
                var availLimitAmount = ConfigManager.IMS_MAX_REQUEST_AMOUNT_MON - requestedAmount;
                if (request.Amount > availLimitAmount)
                    return this.RenderError(r => r.Message = string.Format("每月累计提现额度为:{0},本月还可提现:{1}",
                                    ConfigManager.IMS_MAX_REQUEST_AMOUNT_MON,
                                    availLimitAmount));
                _incomerequestRepo.Insert(new IMS_AssociateIncomeRequestEntity()
                {
                    Amount = request.Amount,
                    BankName = bankEntity.Name,
                    CreateDate = DateTime.Now,
                    BankNo = request.Bank_No,
                    BankCode = request.Bank_Code,
                    Status = (int)AssociateIncomeRequestStatus.Requesting,
                    BankAccountName = request.User_Name,
                    UpdateDate = DateTime.Now,
                    UserId = authuid,
                    TransferFee = ConfigManager.BANK_TRANSFER_FEE,
                    IDCard = request.Id_Card,
                    GroupId = groupId
                });

                incomeAccountEntity.AvailableAmount -= request.Amount;
                incomeAccountEntity.RequestAmount += request.Amount;
                incomeAccountEntity.UpdateDate = DateTime.Now;
                _incomeRepo.Update(incomeAccountEntity);

                ts.Complete();
            }

            return this.RenderSuccess<dynamic>(null);
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Order_GiftCards(PagerInfoRequest request, int authuid)
        {
            var linq = Context.Set<IMS_AssociateIncomeHistoryEntity>().
                        Where(iai => iai.AssociateUserId == authuid && iai.SourceType == (int)AssociateOrderType.GiftCard)
                        .Join(Context.Set<IMS_GiftCardOrderEntity>(), o => o.SourceNo, i => i.No, (o, i) => new
                        {
                            A = o,
                            O = i
                        });
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.A.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var response = new PagerInfoResponse<dynamic>(request.PagerRequest, totalCount)
            {
                Items = linq.ToList().Select(l => new
                {
                    no = l.O.No,
                    amount = l.O.Amount,
                    create_date = l.O.CreateDate,
                }).ToList<dynamic>()
            };

            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = response);
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Orders(PagerInfoRequest request, int authuid)
        {
            var linq = Context.Set<IMS_AssociateIncomeHistoryEntity>().
                        Where(iai => iai.AssociateUserId == authuid && iai.SourceType == (int)AssociateOrderType.Product)
                        .Join(Context.Set<OrderEntity>(), o => o.SourceNo, i => i.OrderNo, (o, i) => new
                        {
                            A = o,
                            O = i
                        });
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.A.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var response = new PagerInfoResponse<dynamic>(request.PagerRequest, totalCount)
            {
                Items = linq.ToList().Select(l => new
                {
                    order_no = l.A.SourceNo,
                    amount = l.O.TotalAmount,
                    create_date = l.O.CreateDate,
                    shipping_name = l.O.ShippingContactPerson,
                    status = ((OrderStatus)l.O.Status).ToFriendlyString()
                }).ToList<dynamic>()
            };

            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = response);

        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Update_Name(string name, int authuid)
        {
            if (string.IsNullOrEmpty(name))
                return this.RenderError(r => r.Message = "店铺名称不能为空");
            var userEntity = Context.Set<UserEntity>().Find(authuid);
            userEntity.Nickname = name;
            userEntity.UpdatedDate = DateTime.Now;
            userEntity.UpdatedUser = authuid;

            _userRepo.Update(userEntity);
            return this.RenderSuccess<dynamic>(c => c.Data = new
            {
                name = userEntity.Nickname
            });
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Update_Mobile(string mobile, int authuid)
        {
            var userEntity = Context.Set<UserEntity>().Find(authuid);
            userEntity.Mobile = mobile;
            userEntity.UpdatedDate = DateTime.Now;
            userEntity.UpdatedUser = authuid;

            _userRepo.Update(userEntity);
            return this.RenderSuccess<dynamic>(c => c.Data = new
            {
                phone = userEntity.Mobile
            });
        }



        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Update_Template(int templateId, int authuid)
        {
            var associateEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid).FirstOrDefault();
            associateEntity.TemplateId = templateId;
            _associateRepo.Update(associateEntity);
            return this.RenderSuccess<dynamic>(null);
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Feedback(string comment, int authuid)
        {
            if (string.IsNullOrEmpty(comment))
                return this.RenderError(r => r.Message = "意见不能为空");
            _feedbackRepo.Insert(new FeedbackEntity()
            {
                Content = comment,
                Contact = string.Empty,
                CreatedDate = DateTime.Now,
                CreatedUser = authuid,
                Status = (int)DataStatus.Normal,
                UpdatedDate = DateTime.Now,
                UpdatedUser = authuid,
                User_Id = authuid
            });
            return this.RenderSuccess<dynamic>(null);
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Latest_BankInfo(int authuid)
        {
            var bankInfo = Context.Set<IMS_AssociateIncomeRequestEntity>().Where(ia => ia.UserId == authuid).OrderByDescending(ia => ia.CreateDate).FirstOrDefault();
            if (bankInfo != null)
                return this.RenderSuccess<dynamic>(c => c.Data = new
                {
                    bank = bankInfo.BankName,
                    bank_code = bankInfo.BankCode,
                    bank_no = bankInfo.BankNo,
                    user_name = bankInfo.BankAccountName,
                    id_card = bankInfo.IDCard ?? string.Empty
                });
            else
                return this.RenderSuccess<dynamic>(c => c.Data = new { });
        }

    }
}
