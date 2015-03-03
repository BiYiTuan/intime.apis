using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Promotion;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;
using Yintai.Hangzhou.Contract.Response.Promotion;

namespace Yintai.Hangzhou.Contract.Promotion
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Hangzhou.Contract.Promotion
    /// FileName: IPromotionDataService
    ///
    /// Created at 11/12/2012 2:55:40 PM
    /// Description: 
    /// </summary>
    public interface IPromotionDataService
    {
        /// <summary>
        /// 获取促销列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionCollectionResponse> GetPromotionForBanner(GetPromotionBannerListRequest request);

        /// <summary>
        /// 获取促销列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionCollectionResponse> GetPromotionList(GetPromotionListRequest request);

        /// <summary>
        /// 刷新接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionCollectionResponse> GetPromotionListForRefresh(GetPromotionListForRefresh request);

        /// <summary>
        /// 获取促销详情信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> GetPromotionInfo(GetPromotionInfoRequest request);

        /// <summary>
        /// 创建活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> CreatePromotion(CreatePromotionRequest request);

        /// <summary>
        /// 修改活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> UpdatePromotion(UpdatePromotionRequest request);

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> DestroyPromotion(DestroyPromotionRequest request);

        /// <summary>
        /// 活动添加资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> CreateResourcePromotion(CreateResourcePromotionRequest request);

        /// <summary>
        /// 活动删除资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> DestroyResourcePromotion(DestroyResourcePromotionRequest request);

        /// <summary>
        /// 创建分享
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> CreateShare(PromotionShareCreateRequest request);

        /// <summary>
        /// 创建收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> CreateFavor(PromotionFavorCreateRequest request);

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> DestroyFavor(PromotionFavorDestroyRequest request);

        /// <summary>
        /// 创建优惠卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PromotionInfoResponse> CreateCoupon(PromotionCouponCreateRequest request);


    }
}
