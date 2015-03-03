using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class SpecialTopicProductRelationRepository : RepositoryBase<SpecialTopicProductRelationEntity, int>, ISpecialTopicProductRelationRepository
    {
        #region

        private static Expression<Func<SpecialTopicProductRelationEntity, bool>> Filter(DataStatus? dataStatus, List<int> productIds)
        {
            var filter = PredicateBuilder.True<SpecialTopicProductRelationEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus);
            }

            if (productIds != null)
            {
                filter = filter.And(v => productIds.Any(s => s == v.Product_Id));
            }

            return filter;
        }

        #endregion

        public IQueryable<SpecialTopicProductRelationEntity> GetList4Linq(List<int> ids)
        {
            return base.Get(v => ids.Any(s => s == v.SpecialTopic_Id) && v.Status == (int)DataStatus.Normal);
        }

        public IQueryable<SpecialTopicProductRelationEntity> GetListByProduct4Linq(List<int> productIds)
        {
            return base.Get(v => productIds.Any(s => s == v.Product_Id) && v.Status == (int)DataStatus.Normal);
        }

        public override SpecialTopicProductRelationEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<SpecialTopicProductRelationEntity> GetList(int specialTopicId)
        {
            return base.Get(v => v.SpecialTopic_Id == specialTopicId && v.Status == (int)DataStatus.Normal).ToList();
        }

        public List<SpecialTopicProductRelationEntity> GetList(List<int> ids)
        {
            return GetList4Linq(ids).ToList();
        }

        public List<SpecialTopicProductRelationEntity> GetListByProduct(int productId)
        {
            return base.Get(v => v.Product_Id == productId && v.Status == (int)DataStatus.Normal).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            base.Delete(Filter(DataStatus.Normal, new List<int>(1) { productId }));
        }
    }
}
