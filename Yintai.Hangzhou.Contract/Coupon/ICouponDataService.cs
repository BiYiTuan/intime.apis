using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Coupon;
using Yintai.Hangzhou.Contract.DTO.Response.Coupon;

namespace Yintai.Hangzhou.Contract.Coupon
{
    public interface ICouponDataService
    {
        /// <summary>
        /// 创建优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CouponCodeResponse> CreateCoupon(CouponCouponRequest request);

        /// <summary>
        /// 获取 COUPON
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Obsolete("过期，建议使用 couponinforesponse")]
        ExecuteResult<CouponCodeResponse> Get(CouponGetRequest request);

        /// <summary>
        /// 获取用户的优惠券列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Obsolete("过期，建议使用 couponinforesponse")]
        ExecuteResult<CouponCodeCollectionResponse> GetList(CustomerCouponCodeGetListRequest request);

        /// <summary>
        /// 获取 COUPON
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CouponInfoResponse> Get(CouponInfoGetRequest request);

        /// <summary>
        /// 获取用户的优惠券列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        ExecuteResult<CouponInfoCollectionResponse> GetList(CouponInfoGetListRequest request);
    }
}
