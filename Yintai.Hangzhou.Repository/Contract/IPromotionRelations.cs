using System.Collections.Generic;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IPromotionBrandRelationRepository : IRepository<PromotionBrandRelationEntity, int>
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="promotionId"></param>
        /// <param name="brandId"></param>
        void Del(int promotionId, int brandId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="promotionId"></param>
        void Del(int promotionId);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<PromotionBrandRelationEntity> BatchInsert(List<PromotionBrandRelationEntity> entities);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        List<PromotionBrandRelationEntity> GetList(int promotionId);


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        List<PromotionBrandRelationEntity> GetList(List<int> promotionId);
    }
}
