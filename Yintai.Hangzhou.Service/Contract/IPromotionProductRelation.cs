using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Contract.DTO.Request.Promotion;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface IPromotionService
    {
        /// <summary>
        /// 当前活动是否存在该商品
        /// </summary>
        /// <param name="promotionid"></param>
        /// <param name="productid"></param>
        /// <returns></returns>
        bool Exists(int promotionid, int productid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionid"></param>
        /// <returns></returns>
        PromotionEntity Get(int promotionid);

        /// <summary>
        /// 验证活动
        /// </summary>
        /// <param name="promotionEntity"></param>
        /// <returns>错误信息</returns>
        string Verification(PromotionEntity promotionEntity);

        /// <summary>
        /// 验证活动
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns>错误信息</returns>
        string Verification(int promotionId);

        /// <summary>
        /// 获取第一个 可发行的活动
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        PromotionEntity GetFristNormalForProductId(int productId);

        /// <summary>
        /// 验证活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns>错误信息</returns>
        string Verification(PromotionCouponCreateRequest request);
    }
}
