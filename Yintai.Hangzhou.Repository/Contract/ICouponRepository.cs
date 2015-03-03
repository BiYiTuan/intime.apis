using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ICouponRepository : IRepository<CouponHistoryEntity, int>
    {
        /// <summary>
        /// 得到指定用户的COUPON
        /// </summary>
        /// <param name="pagerRequest"></param>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        List<CouponHistoryEntity> GetPagedListByUserId(PagerRequest pagerRequest, out int totalCount, int userId, CouponSortOrder sortOrder);

        /// <summary>
        /// 获取用户优惠券列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sourceId">来源ID</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        List<CouponHistoryEntity> GetListByUserIdSourceId(int userId, int sourceId, SourceType? sourceType);

        /// <summary>
        /// 获取优惠券领取数
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        int Get4Count(int sourceId, SourceType sourceType);

        /// <summary>
        /// 获取用户的优惠券数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int GetUserCouponCount(int userId, CouponBusinessStatus status);
    }
}