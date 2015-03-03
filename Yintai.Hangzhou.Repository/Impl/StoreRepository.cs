using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class StoreRepository : RepositoryBase<StoreEntity, int>, IStoreRepository
    {
        private static Expression<Func<StoreEntity, bool>> Filler(DataStatus? dataStatus)
        {
            var filter = PredicateBuilder.True<StoreEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            return filter;
        }

        private static Func<IQueryable<StoreEntity>, IOrderedQueryable<StoreEntity>> OrderBy(StoreSortOrder sortOrder)
        {
            Func<IQueryable<StoreEntity>, IOrderedQueryable<StoreEntity>> orderBy = null;

            switch (sortOrder)
            {
                default:
                    orderBy = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return orderBy;
        }

        #region Overrides of RepositoryBase<StoreEntity,int>

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override StoreEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public IQueryable<StoreEntity> Get(DataStatus? dataStatus)
        {
            return base.Get(Filler(dataStatus));
        }

        /// <summary>
        /// 获取指定ID 的 店铺
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<StoreEntity> GetListByIds(List<int> ids)
        {
            return base.Get(v => ids.Any(s => s == v.Id)).ToList();
        }

        public List<StoreEntity> GetListForAll()
        {
            return base.Get(v => v.Status == (int)DataStatus.Normal).ToList();
        }

        public List<StoreEntity> GetListForRefresh(Timestamp timestamp)
        {
            switch (timestamp.TsType)
            {
                case TimestampType.Old:
                    return base.Get(v => v.UpdatedDate <= timestamp.Ts && v.Status == (int)DataStatus.Normal).ToList();
                case TimestampType.New:
                default:
                    return base.Get(v => v.UpdatedDate > timestamp.Ts && v.Status == (int)DataStatus.Normal).ToList();

            }
        }

        public List<StoreEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, StoreSortOrder sortOrder)
        {
            return base.Get(Filler(DataStatus.Normal), out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize, OrderBy(sortOrder)).ToList();
        }

        #endregion
    }
}
