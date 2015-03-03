using System.Web.Mvc;
using System.Linq;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.Coupon;
using Yintai.Hangzhou.Contract.DTO.Request.Coupon;
using Yintai.Hangzhou.Contract.DTO.Response.Coupon;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;
using System;
using Yintai.Hangzhou.Contract.DTO.Response.Product;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;
using Yintai.Hangzhou.Contract.DTO.Response.Store;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    [RestfulAuthorize]
    public class CouponController : RestfulController
    {
        private readonly ICouponDataService _couponDataService;
        private ICouponRepository _couponRepo;
        private IProductRepository _productRepo;
        private IPromotionRepository _promotionRepo;
        private IStoreRepository _storeRepo;
        public CouponController(ICouponDataService couponDataService,
            ICouponRepository couponRepo,
            IPromotionRepository promotionRepo,
            IProductRepository productRepo,
            IStoreRepository storeRepo)
        {
            this._couponDataService = couponDataService;
            _couponRepo = couponRepo;
            _productRepo = productRepo;
            _promotionRepo = promotionRepo;
            _storeRepo = storeRepo;
        }

        public ActionResult List(CouponInfoGetListRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            if (request == null)
                return new RestfulResult { Data = new ExecuteResult<CouponInfoResponse>(null)};
            var linq = _couponRepo.Get(c => c.User_Id == authUser.Id && c.Status != (int)CouponStatus.Deleted);
            if (request.Type.HasValue)
            {
                switch (request.Type.Value)
                { 
                    case CouponRequestType.Used:
                        linq = linq.Where(c => c.Status == (int)CouponStatus.Used);
                        break;
                    case CouponRequestType.Expired:
                        linq = linq.Where(c => c.Status != (int)CouponStatus.Used && c.ValidEndDate < DateTime.Now);
                        break;
                    case CouponRequestType.UnUsed:
                        linq = linq.Where(c => c.Status != (int)CouponStatus.Used && c.ValidEndDate >= DateTime.Now);
                        break;
                }
            }
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(c => c.CreatedDate).Skip(skipCount).Take(request.Pagesize);
            var productLinq = _productRepo.GetAll().Join(_storeRepo.GetAll(), o => o.Store_Id, i => i.Id, (o, i) => new { Pd = o, S = i });
            var promotionLinq = _promotionRepo.GetAll().Join(_storeRepo.GetAll(), o => o.Store_Id, i => i.Id, (o, i) => new { Pr = o, S = i });
            var linq2 = linq.GroupJoin(productLinq, o => o.FromProduct, i => i.Pd.Id, (o, i) => new { C=o,Pd = i.FirstOrDefault() })
                           .GroupJoin(promotionLinq, o => o.C.FromPromotion, i => i.Pr.Id, (o, i) => new { C = o.C, Pd = o.Pd, Pr = i.FirstOrDefault() });
            var responseData = from l in linq2.ToList()
                               select new CouponInfoResponse().FromEntity<CouponInfoResponse>(l.C,
                                            c => {
                                                var prod = l.Pd;
                                                if (prod != null)
                                                {
                                                    c.ProductInfoResponse = new ProductInfoResponse().FromEntity<ProductInfoResponse>(prod.Pd, p => {
                                                        p.StoreInfoResponse = new StoreInfoResponse().FromEntity<StoreInfoResponse>(prod.S);
                                                    });
                                                    c.ProductId = c.ProductInfoResponse.Id;
                                                    c.ProductName = c.ProductInfoResponse.Name;
                                                    c.ProductDescription = c.ProductInfoResponse.Description;
                                                    c.ProductType = (int)SourceType.Product;
                                                   
                                                }
                                                var pro = l.Pr;
                                                if (pro != null)
                                                {
                                                    c.PromotionInfoResponse = new PromotionInfoResponse().FromEntity<PromotionInfoResponse>(pro.Pr, p => {
                                                        p.StoreInfoResponse = new StoreInfoResponse().FromEntity<StoreInfoResponse>(pro.S);
                                                    });
                                                    c.ProductId = c.PromotionInfoResponse.Id;
                                                    c.ProductName = c.PromotionInfoResponse.Name;
                                                    c.ProductDescription = c.PromotionInfoResponse.Description;
                                                    c.ProductType = (int)SourceType.Promotion;
                                                }
                                               
                                            });
            var response = new CouponInfoCollectionResponse(request.PagerRequest,totalCount){
                                 CouponInfoResponses = responseData.ToList()
                         };
            return new RestfulResult { Data = new ExecuteResult<CouponInfoCollectionResponse>(response) };
        }

        public ActionResult Detail(CouponInfoGetRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;
            if (request == null)
                return new RestfulResult { Data = new ExecuteResult<CouponInfoResponse>(null) };
            var linq = _couponRepo.Get(c => c.User_Id == authUser.Id && c.Status != (int)CouponStatus.Deleted);
            var linq2 = linq.GroupJoin(_productRepo.GetAll(), o => o.FromProduct, i => i.Id, (o, i) => new { C = o, Pd = i })
                           .GroupJoin(_promotionRepo.GetAll(), o => o.C.FromPromotion, i => i.Id, (o, i) => new { C = o.C, Pd = o.Pd, Pr = i });
            var responseData = from l in linq2.ToList()
                               select new CouponInfoResponse().FromEntity<CouponInfoResponse>(l.C,
                                            c =>
                                            {
                                                var prod = l.Pd.FirstOrDefault();
                                                if (prod != null)
                                                {
                                                    c.ProductInfoResponse = new ProductInfoResponse().FromEntity<ProductInfoResponse>(prod);
                                                    c.ProductId = c.ProductInfoResponse.Id;
                                                    c.ProductName = c.ProductInfoResponse.Name;
                                                    c.ProductDescription = c.ProductInfoResponse.Description;
                                                }
                                                var pro = l.Pr.FirstOrDefault();
                                                if (pro != null)
                                                {
                                                    c.PromotionInfoResponse = new PromotionInfoResponse().FromEntity<PromotionInfoResponse>(pro);

                                                }
                                            });
            return new RestfulResult { Data = new ExecuteResult<CouponInfoResponse>(responseData.FirstOrDefault()) };
        }
    }
}
