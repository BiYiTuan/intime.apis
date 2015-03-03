using System;
using System.Globalization;
using System.Linq;
using Yintai.Architecture.Common.Helper;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.Coupon;
using Yintai.Hangzhou.Contract.DTO.Request.Coupon;
using Yintai.Hangzhou.Contract.DTO.Response.Coupon;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service
{
    /// <summary>
    /// CLR Version: 4.0.30319.296
    /// NameSpace: Yintai.Hangzhou.Service
    /// FileName: CouponDataService
    ///
    /// Created at 11/27/2012 11:03:30 AM
    /// Description: 
    /// </summary>
    public class CouponDataService : BaseService, ICouponDataService
    {
        #region fields

        private readonly ITimeSeedRepository _timeSeedRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly ISeedRepository _seedRepository;
        private readonly IUserService _userService;
        private readonly IProductRepository _productRepository;

        #endregion

        #region .ctor

        public CouponDataService(IProductRepository productRepository, ITimeSeedRepository timeSeedRepository, ISeedRepository seedRepository, IUserService userService, ICouponRepository couponRepository, IPromotionRepository promotionRepository)
        {
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _promotionRepository = promotionRepository;
            _userService = userService;
            _seedRepository = seedRepository;
            _timeSeedRepository = timeSeedRepository;
        }

        #endregion

        #region properties

        #endregion

        #region methods

        private void Get(CouponCouponRequest request, out int storeId, out int recommendedUserId)
        {
            switch (request.SType)
            {
                case SourceType.Promotion:
                    GetPromotion(request.PromotionId, out storeId, out recommendedUserId);
                    break;
                case SourceType.Product:
                    GetProduct(request.ProductId, out storeId, out recommendedUserId);
                    break;
                default:
                    storeId = 0;
                    recommendedUserId = 0;
                    break;
            }
        }

        private ProductEntity GetProduct(int pid, out int storeId, out int recommendedUserId)
        {
            var entity = _productRepository.GetItem(pid);
            if (entity == null)
            {
                storeId = 0;
                recommendedUserId = 0;

                return null;
            }

            storeId = entity.Store_Id;
            recommendedUserId = entity.RecommendUser;

            return entity;
        }

        private PromotionEntity GetPromotion(int pid, out int storeId, out int recommendedUserId)
        {
            var entity = _promotionRepository.GetItem(pid);
            if (entity == null)
            {
                storeId = 0;
                recommendedUserId = 0;

                return null;
            }

            storeId = entity.Store_Id;
            recommendedUserId = entity.RecommendUser;

            return entity;
        }

        #endregion

        #region Implementation of ICouponDataService

        /// <summary>
        /// 创建优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<CouponCodeResponse> CreateCoupon(CouponCouponRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<CouponCodeResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }


            /*
             * 8位, 前三位+5位流水
             * 前三位 当天距离2012/12/31的天数，不足补0
             * 后5位为自增的流水码
             */
            return CreateCoupon4PromotionId(request);
        }


        private ExecuteResult<CouponCodeResponse> CreateCoupon4PromotionId(CouponCouponRequest request)
        {
            //            优惠码新规则如下：
            //1.	全数字组成
            //2.	优惠码由两部分组成{promotionid info}+{流水号}
            //3.	{promotionid info} = X x x … 
            //其中第一位数字为promotionid的长度
            //后面的数字为promotionid
            //4.	{流水号}  为5位数字，即每个促销活动最大只能够有9999个优惠码

            //示例如下：
            //一个促销活动的promotionid为2，它的优惠码之一为：
            //         1200001
            //一个促销活动的promotionid为100，它的优惠码之一为：
            //         310000001


            if (request.SType != SourceType.Promotion)
            {
                return new ExecuteResult<CouponCodeResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var kp = request.PromotionId.ToString(CultureInfo.InvariantCulture);
            var keyPre = kp.Length.ToString(CultureInfo.InvariantCulture) + kp;

            var seed = _seedRepository.Generate("yt.hz.promotion", 9999, request.PromotionId);
            if (seed == -2)
            {
                return new ExecuteResult<CouponCodeResponse>(null) { StatusCode = StatusCode.ClientError, Message = "已经超出优惠券领取最大值" };
            }

            //前补零
            var keySuf = UtilHelper.PreFilled(seed, 5, '0');
            var couponid = keyPre + keySuf;

            var storeId = 0;
            var recommendedUserId = 0;

            var p = GetPromotion(request.PromotionId, out storeId, out recommendedUserId);

            //create coupon

            var coupon = this._couponRepository.Insert(new CouponHistoryEntity
            {
                CouponId = couponid,
                CreatedDate = DateTime.Now,
                CreatedUser = request.AuthUid,
                FromProduct =
                   request.ProductId,
                FromPromotion =
                    request.PromotionId,
                FromStore = storeId,
                FromUser = recommendedUserId,
                Id = 0,
                Status = (int)DataStatus.Normal,
                User_Id = request.AuthUid,

                ValidStartDate = p.StartDate,
                ValidEndDate = p.EndDate,
                IsLimitOnce = p.IsCodeUseLimit
            });

            

            return new ExecuteResult<CouponCodeResponse>(MappingManager.CouponCodeResponseMapping(coupon));
        }

       
        /// <summary>
        /// 获取 COUPON
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<CouponCodeResponse> Get(CouponGetRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<CouponCodeResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = this._couponRepository.GetItem(request.CouponId);

            if (entity.User_Id == request.AuthUid && entity.CouponId == request.CouponCode)
            {
                return new ExecuteResult<CouponCodeResponse>(MappingManager.CouponCodeResponseMapping(entity));
            }

            return new ExecuteResult<CouponCodeResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您不能领取他人的优惠券" };
        }

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<CouponCodeCollectionResponse> GetList(CustomerCouponCodeGetListRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<CouponCodeCollectionResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            int totalCount;
            var datas = this._couponRepository.GetPagedListByUserId(request.PagerRequest, out totalCount, request.AuthUid,
                                                        request.CouponSortOrder);
            var response = new CouponCodeCollectionResponse(request.PagerRequest, totalCount)
                {
                    CouponCodeResponses = MappingManager.CouponCodeResponseMapping(datas)
                };

            var result = new ExecuteResult<CouponCodeCollectionResponse>(response);

            return result;
        }

        public ExecuteResult<CouponInfoResponse> Get(CouponInfoGetRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<CouponInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = this._couponRepository.GetItem(request.CouponId);

            if (entity.User_Id == request.AuthUid && entity.CouponId == request.CouponCode)
            {
                return new ExecuteResult<CouponInfoResponse>(MappingManager.CouponInfoResponseMapping(entity));
            }

            return new ExecuteResult<CouponInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您不能领取他人的优惠券" };
        }

        public ExecuteResult<CouponInfoCollectionResponse> GetList(CouponInfoGetListRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<CouponInfoCollectionResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            int totalCount;
            var datas = this._couponRepository.GetPagedListByUserId(request.PagerRequest, out totalCount, request.AuthUid,
                                                        request.CouponSortOrder);
            var response = new CouponInfoCollectionResponse(request.PagerRequest, totalCount)
            {
                CouponInfoResponses = MappingManager.CouponInfoResponseMapping(datas).ToList()
            };

            var result = new ExecuteResult<CouponInfoCollectionResponse>(response);

            return result;
        }

        #endregion
    }
}
