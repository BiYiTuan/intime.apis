using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.Brand;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Request.Product;
using Yintai.Hangzhou.Contract.Product;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.WebSupport.Binder;
using Yintai.Hangzhou.WebSupport.Mvc;

using Yintai.Hangzhou.WebApiCore;
using System.Transactions;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Contract.DTO.Response;
using com.intime.fashion.service;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using com.intime.fashion.common;
using com.intime.fashion.service.contract;
using Yintai.Hangzhou.Model.Order;
using Yintai.Hangzhou.Model.Product;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class ProductController : RestfulController
    {
        private readonly IProductDataService _productDataService;

        private readonly PassHelper _passHelper;
        private IOrderRepository _orderRepo;
        private IOrderItemRepository _orderItemRepo;
        private IOrderLogRepository _orderLogRepo;
        private IProductRepository _productRepo;
        private IFavoriteRepository _favoriteRepo;
        private IPromotionRepository _promotionRepo;
        private IOrder2ExRepository _orderexRepo;
        private IRMA2ExRepository _rmaexRepo;
        private IInventoryRepository _inventoryRepo;
        private IOrderService _orderService;

        public ProductController(IProductDataService productDataService,
            IBrandDataService brandDataService,
            IOrderRepository orderRepo,
            IOrderItemRepository orderItemRepo,
            IOrderLogRepository orderLogRepo,
            IProductRepository productRepo,
            IFavoriteRepository favoriteRepo,
            IPromotionRepository promotionRepo,
            IOrder2ExRepository orderexRepo,
            IRMA2ExRepository rmaexRepo,
           IInventoryRepository inventoryRepo,
           IOrderService orderService)
        {
            _productDataService = productDataService;
            _passHelper = new PassHelper(brandDataService);
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _orderLogRepo = orderLogRepo;
            _productRepo = productRepo;
            _favoriteRepo = favoriteRepo;
            _promotionRepo = promotionRepo;
            _orderexRepo = orderexRepo;
            _rmaexRepo = rmaexRepo;
            _inventoryRepo = inventoryRepo;
            _orderService = orderService;

        }

        public ActionResult List(GetProductListRequest request)
        {
            request.Type = UrlDecode(request.Type);

            if (!String.IsNullOrEmpty(request.Type))
            {
                if (request.Type.ToLower() == "refresh")
                {
                    return new RestfulResult
                    {
                        Data = _productDataService.RefreshProduct(new GetProductRefreshRequest
                            {
                                Page = request.Page,
                                Pagesize = request.Pagesize,
                                RefreshTs = request.RefreshTs,
                                Sort = request.Sort,
                                BrandId = request.BrandId,
                                Lat = request.Lat,
                                Lng = request.Lng,
                                TagId = request.TagId,
                                TopicId = request.TopicId,
                                PromotionId = request.PromotionId
                            })
                    };
                }
            }
            else
            {
                return new RestfulResult { Data = this._productDataService.GetProductList(request) };
            }

            return new RestfulResult();
        }

        public ActionResult Detail(GetProductInfoRequest request, [FetchRestfulAuthUserAttribute(IsCanMissing = true, KeyName = Define.Token)]UserModel currentAuthUser)
        {
            request.CurrentAuthUser = currentAuthUser;

            return new RestfulResult { Data = this._productDataService.GetProductInfo(request) };
        }

        [RestfulRoleAuthorize(UserRole.Admin, UserLevel.Daren)]
        [HttpPost]
        public ActionResult Create(CreateProductRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            request.Description = UrlDecode(request.Description);
            request.Favorable = UrlDecode(request.Favorable);
            request.Name = UrlDecode(request.Name);
            request.RecommendedReason = UrlDecode(request.RecommendedReason);
            request.RecommendUser = request.AuthUid;
            request.Files = Request.Files;

            return new RestfulResult { Data = this._productDataService.CreateProduct(request) };
        }

        [RestfulAuthorize]
        [HttpPost]
        [Obsolete]
        public ActionResult Update(UpdateProductRequest request, int? authuid)
        {
            request.AuthUid = authuid.Value;
            request.Description = UrlDecode(request.Description);
            request.Favorable = UrlDecode(request.Favorable);
            request.Name = UrlDecode(request.Name);
            request.RecommendedReason = UrlDecode(request.RecommendedReason);

            return new RestfulResult { Data = this._productDataService.UpdateProduct(request) };
        }

        [HttpPost]
        [RestfulAuthorize]
        public ActionResult Favor(CreateFavorProductRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;

            if (System.String.Compare(request.Method, DefineRestfulMethod.Destroy, System.StringComparison.OrdinalIgnoreCase) == 0)
            {
                //d
                return new RestfulResult { Data = this._productDataService.DestroyFavor(new DestroyFavorProductRequest(request)) };
            }

            if (String.IsNullOrWhiteSpace(request.Method))
            {
                return new RestfulResult { Data = this._productDataService.CreateFavor(new CreateFavorProductRequest(request)) };
            }

            return new RestfulResult { Data = new ExecuteResult() { StatusCode = StatusCode.ClientError, Message = "参数错误" } };
        }

        [RestfulAuthorize]
        [HttpPost]
        public ActionResult Share(CreateShareProductRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            return new RestfulResult { Data = this._productDataService.CreateShare(request) };
        }

        [RestfulAuthorize]
        [HttpPost]
        public ActionResult Coupon(CreateCouponProductRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            if (System.String.Compare(request.Method, DefineRestfulMethod.Create, System.StringComparison.OrdinalIgnoreCase) == 0)
            {
                var result = this._productDataService.CreateCoupon(request);
                /*
                if (request.IsPass == 1 && result.Data != null && result.Data.CouponCodeResponse != null)
                {
                    var code = result.Data.CouponCodeResponse;
                    
                    result.Data.CouponCodeResponse.Pass = _passHelper.GetPass(ControllerContext.HttpContext, code.Id,
                                                                              code.CouponId, code.User_Id);
                   
                }
                */
                return new RestfulResult
                {
                    Data = result
                };
            }

            return new RestfulResult { Data = new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "参数错误" } };
        }

        public ActionResult Daren(GetProductListRequest request, [FetchUser(KeyName = "userid")]UserModel showUser)
        {
            if (System.String.Compare(request.Method, DefineRestfulMethod.List, System.StringComparison.OrdinalIgnoreCase) != 0)
            {
                return new RestfulResult { Data = new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "方法错误" } };
            }

            request.UserModel = showUser;

            return new RestfulResult { Data = this._productDataService.GetProductList(request) };
        }

        [RestfulRoleAuthorize(UserRole.Admin, UserLevel.Daren)]
        [HttpPost]
        public ActionResult Destroy(FormCollection formCollection, DestroyProductRequest request, int? authuid, UserModel authUser, [FetchProduct(KeyName = "productid", IsCanMissing = true)]ProductEntity entity)
        {
            if (entity == null || authUser == null)
            {
                return new RestfulResult() { Data = new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "参数错误" } };
            }

            //达人只能删除自己的商品，
            //店长 可以删除自己店铺下的商品
            //运营 管理员权限的用户才可以删除他人的商品
            var t = false;
            //3
            if (((authUser.UserRole & (int)UserRole.Admin) != 0))
            {
                t = true;
            }
            else
            {
                if (authUser.Id == entity.RecommendUser && (authUser.Level & UserLevel.Daren) != 0)
                {
                    t = true;
                }
            }
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;

            return t ? new RestfulResult { Data = this._productDataService.DestroyProduct(request) } : new RestfulResult { Data = new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "您没有权限操作他人的商品" } };
        }


        [RestfulAuthorize]
        [HttpPost]
        public ActionResult Order(OrderRequest request, UserModel authUser, string channel)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var createRequest = OrderCreate.FromEntity<OrderCreate>(request.OrderModel, o =>
            {
                o.Channel = channel;
                o.Payment = PaymentMethod.FromEntity<PaymentMethod>(request.OrderModel.Payment);
                o.Products = request.OrderModel.Products.Select(p => OrderItem.FromEntity<OrderItem>(p, pp => {
                    pp.Properties = ProductPropertyValue.FromEntity<ProductPropertyValue>(p.Properties);
                }));
                o.ShippingAddress = OrderShippingAddress.FromEntity<OrderShippingAddress>(request.OrderModel.ShippingAddress);

            });


            var result = _orderService.Create(createRequest, authUser);
            if (result.IsSuccess)
                return this.RenderSuccess<OrderResponse>(r => r.Data = new OrderResponse().FromEntity<OrderResponse>(result.Result));
            else
                return this.RenderError(r => r.Message = result.Error.Message);

        }



        /// <summary>
        /// get the purchase detail product info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentAuthUser"></param>
        /// <returns></returns>
        public ActionResult Detail4P(GetProductInfo4PRequest request, string channel)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var context = _productRepo.Context;
            var linq = _productRepo.Get(p => p.Id == request.ProductId && p.Is4Sale.HasValue && p.Is4Sale.Value)
                       .GroupJoin(context.Set<BrandEntity>(), o => o.Brand_Id, i => i.Id, (o, i) => new { P = o, B = i.FirstOrDefault() })
                     .FirstOrDefault();

            if (linq == null)
                return this.RenderError(r => r.Message = "产品不存在!");

            var data = new GetProductInfo4PResponse().FromEntity<GetProductInfo4PResponse>(linq.P, res =>
            {
                var dimensionEntity = Context.Set<ResourceEntity>().Where(r => r.Status == (int)DataStatus.Normal &&
                                    r.IsDimension.HasValue &&
                                    r.IsDimension.Value == true &&
                                    r.SourceId == linq.P.Id).FirstOrDefault();
                if (dimensionEntity != null)
                    res.DimensionResource = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(dimensionEntity);
                var rmaMsg = Context.Set<ConfigMsgEntity>().Where(c => c.MKey == "O_C_RMAPolicy").FirstOrDefault();
                res.RMAPolicy = rmaMsg == null ? string.Empty : rmaMsg.Message;
                var channelEntity = Context.Set<ChannelEntity>().Where(c => c.Name == channel).FirstOrDefault();
                if (channelEntity != null)
                {
                    res.SupportPayments = context.Set<PaymentMethodEntity>().Where(p => p.Status == (int)DataStatus.Normal)
                                    .ToList()
                                    .Where(p => (!p.AvailChannels.HasValue) || ((p.AvailChannels | channelEntity.BusinessId) == channelEntity.BusinessId))
                                    .Select(p => new PaymentResponse().FromEntity<PaymentResponse>(p));
                }
                res.SaleColors = Context.Set<InventoryEntity>().Where(pi => pi.ProductId == linq.P.Id && pi.Amount > 0).GroupBy(pi => pi.PColorId)
                                .Select(pi => pi.Key)
                                .Join(Context.Set<ProductPropertyValueEntity>(), o => o, i => i.Id, (o, i) => i)
                                .GroupJoin(Context.Set<ResourceEntity>().Where(pr => pr.SourceType == (int)SourceType.Product && pr.Type == (int)ResourceType.Image && pr.SourceId == linq.P.Id), o => o.Id, i => i.ColorId, (o, i) => new { C = o, CR = i.FirstOrDefault() })
                                .ToList()
                                .Select(color => new SaleColorPropertyResponse()
                                {
                                    ColorId = color.C.Id,
                                    ColorName = color.C.ValueDesc,
                                    Resource = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(color.CR),
                                    Sizes = Context.Set<InventoryEntity>().Where(pi => pi.ProductId == linq.P.Id && pi.PColorId == color.C.Id)
                                            .Join(Context.Set<ProductPropertyValueEntity>().Where(ppv => ppv.Status == (int)DataStatus.Normal), o => o.PSizeId, i => i.Id, (o, i) => new { PI = o, PPV = i }).ToList()
                                            .Where(pi => pi.PI.Amount > 0)
                                            .Select(pi => new SaleSizePropertyResponse()
                                            {
                                                Is4Sale = pi.PI.Amount > 0,
                                                SizeId = pi.PI.PSizeId,
                                                SizeName = pi.PPV.ValueDesc
                                            })

                                });
                if (linq.B != null)
                {
                    res.BrandId = linq.B.Id;
                    res.BrandName = linq.B.Name;
                    res.Brand2Name = linq.B.EnglishName;
                }
            });
            return this.RenderSuccess<GetProductInfo4PResponse>(m => m.Data = data);
        }

        /// <summary>
        /// return the compute order amount
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currentAuthUser"></param>
        /// <returns></returns>
        public ActionResult ComputeAmount(ComputeAmountRequest request)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var linq = Context.Set<ProductEntity>().Where(p => p.Id == request.ProductId).FirstOrDefault();
            if (linq == null)
                return this.RenderError(m => m.Message = "商品不存在");
            var items = new List<OrderPreCalculateItem>();
            items.Add(new OrderPreCalculateItem()
            {
                ProductId = linq.Id,
                Quantity = request.Quantity,
                Price = linq.Price
            });
            OrderPreCalculateResult model = _orderService.PreCalculate(new OrderPreCalculate()
            {
                CalculateType = OrderPreCalculateType.Product,
                Products = items
            });

            return this.RenderSuccess<dynamic>(m => m.Data = new
            {
                totalquantity = model.TotalQuantity,
                totalfee = model.TotalFee,
                totalpoints = model.TotalPoints,
                totalamount = model.TotalAmount,
                extendprice = model.ExtendPrice
            });

        }

        /// <summary>
        /// get the operations available to current user for this product.
        /// operations includes:
        /// 1. if can get coupon
        /// 2. if favored
        /// </summary>
        /// <param name="authUser"></param>
        /// <returns></returns>
        public ActionResult AvailOperations(GetProductInfoRequest request, [FetchRestfulAuthUserAttribute(IsCanMissing = true, KeyName = Define.Token)] UserModel authUser)
        {
            //是否收藏
            bool isFavored = false;
            bool ifCanCoupon = false;
            var withUserId = (authUser != null && authUser.Id > 0);
            if (withUserId)
            {
                isFavored = _favoriteRepo.Get(f => f.User_Id == authUser.Id && f.FavoriteSourceType == (int)SourceType.Product && f.FavoriteSourceId == request.ProductId && f.Status != (int)DataStatus.Deleted).Any();
            }

            var dbContext = Context;
            var linq = dbContext.Set<Promotion2ProductEntity>().Where(pp => pp.ProdId == request.ProductId && pp.Status != (int)DataStatus.Deleted)
                .Join(dbContext.Set<PromotionEntity>().Where(p => p.Status == (int)DataStatus.Normal && p.EndDate > DateTime.Now),
                        o => o.ProId,
                        i => i.Id,
                        (o, i) => new { Pro = i })
                 .ToList();
            ifCanCoupon = linq.Any(l =>
            {
                bool hadGetCoupon = false;
                if (withUserId)
                {
                    hadGetCoupon = dbContext.Set<CouponHistoryEntity>().Where(c => c.User_Id == authUser.Id && c.FromPromotion == l.Pro.Id).Any();
                }
                if (l.Pro.PublicationLimit == null || l.Pro.PublicationLimit == -1)
                {
                    return (!hadGetCoupon) ||
                                   (hadGetCoupon && (!l.Pro.IsLimitPerUser.HasValue || l.Pro.IsLimitPerUser.Value == false));
                }
                else
                {
                    return l.Pro.InvolvedCount < l.Pro.PublicationLimit &&
                                 (!hadGetCoupon || (hadGetCoupon && (!l.Pro.IsLimitPerUser.HasValue || l.Pro.IsLimitPerUser.Value == false)));
                }

            });
            var productEntity = dbContext.Set<ProductEntity>().Where(p => p.Id == request.ProductId &&
                                                 p.Status == (int)DataStatus.Normal && (p.Is4Sale ?? false) == true)
                                    .Join(dbContext.Set<InventoryEntity>().Where(inv => inv.Amount > 0),
                                        o => o.Id,
                                        i => i.ProductId,
                                        (o, i) => o).FirstOrDefault();
            return this.RenderSuccess<GetAvailOperationsResponse>(m => m.Data = new GetAvailOperationsResponse()
            {
                IsFavored = isFavored,
                IfCanCoupon = ifCanCoupon,
                IfCanTalk = false,
                Is4Sale = productEntity != null
            });

        }



    }
}
