using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class PromotionProductRelationRepository : RepositoryBase<Promotion2ProductEntity, int>, IPromotionProductRelationRepository
    {
        #region methods

        private static Expression<Func<Promotion2ProductEntity, bool>> Filter(DataStatus? dataStatus, int? promotionId, IEnumerable<int> productids)
        {
            var filter = PredicateBuilder.True<Promotion2ProductEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus);
            }

            if (promotionId != null)
            {
                filter = filter.And(v => v.ProId == promotionId.Value);
            }

            if (productids != null)
            {
                filter = filter.And(v => productids.Any(s => s == v.ProdId));
            }

            return filter;
        }

        #endregion

        public override Promotion2ProductEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<Promotion2ProductEntity> GetList(int promotionId)
        {
            return base.Get(Filter(DataStatus.Normal, promotionId, null)).ToList();
        }

        public IQueryable<Promotion2ProductEntity> GetListByPromotionLinq(int promotionId)
        {
            return base.Get(Filter(DataStatus.Normal, promotionId, null));
        }

        public IQueryable<Promotion2ProductEntity> Get(DataStatus status)
        {
            return base.Get(Filter(DataStatus.Normal, null, null));
        }

        public List<Promotion2ProductEntity> GetList4Product(List<int> productids)
        {
            return GetListByProduct4Linq(productids).ToList();
        }

        public IQueryable<Promotion2ProductEntity> GetListByProduct4Linq(List<int> productids)
        {
            return base.Get(Filter(DataStatus.Normal, null, productids));
        }

        public bool Exists(int promotionid, int productid)
        {
            var list = new List<int>(1) { productid };
            var entities = base.Get(Filter(DataStatus.Normal, promotionid, list)).Select(v => v.Id).Take(1);

            if (entities.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeletedByProduct(int productId)
        {
            base.Delete(Filter(DataStatus.Normal, null, new int[] { productId }));
        }
    }
}
