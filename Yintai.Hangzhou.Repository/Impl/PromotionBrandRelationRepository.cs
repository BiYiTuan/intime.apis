using System.Collections.Generic;
using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class PromotionBrandRelationRepository : RepositoryBase<PromotionBrandRelationEntity, int>, IPromotionBrandRelationRepository
    {
        public override PromotionBrandRelationEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public void Del(int promotionId, int brandId)
        {
            base.Delete(v => v.Promotion_Id == promotionId && v.Brand_Id == brandId);
        }

        public void Del(int promotionId)
        {
            base.Delete(v => v.Promotion_Id == promotionId);
        }

        public List<PromotionBrandRelationEntity> BatchInsert(List<PromotionBrandRelationEntity> entities)
        {
            return base.BatchInsert(entities.ToArray()).ToList();
        }

        public List<PromotionBrandRelationEntity> GetList(int promotionId)
        {
            return base.Get(v => v.Promotion_Id == promotionId).ToList();
        }

        public List<PromotionBrandRelationEntity> GetList(List<int> promotionIds)
        {
            return base.Get(v => promotionIds.Any(s => s == v.Promotion_Id)).ToList();
        }
    }
}
