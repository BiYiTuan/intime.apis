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
    public class BrandRepository : RepositoryBase<BrandEntity, int>, IBrandRepository
    {
        private static Expression<Func<BrandEntity, bool>> Filler(DataStatus? dataStatus)
        {
            var filter = PredicateBuilder.True<BrandEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            return filter;
        }

        private static Func<IQueryable<BrandEntity>, IOrderedQueryable<BrandEntity>> OrderBy(BrandSortOrder sortOrder)
        {
            Func<IQueryable<BrandEntity>, IOrderedQueryable<BrandEntity>> orderBy = null;

            switch (sortOrder)
            {
                default:
                    orderBy = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return orderBy;
        }

        public override BrandEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<BrandEntity> GetListForAll()
        {
            return base.Get(v => v.Status == 1).ToList();
        }

        public List<BrandEntity> GetListForRefresh(Timestamp timestamp)
        {
            switch (timestamp.TsType)
            {
                case TimestampType.Old:
                    return base.Get(v => v.UpdatedDate <= timestamp.Ts && v.Status == 1).ToList();
                case TimestampType.New:
                default:
                    return base.Get(v => v.UpdatedDate > timestamp.Ts && v.Status == 1).ToList();

            }
        }

        public List<BrandEntity> GetListByIds(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<BrandEntity>(0);
            }

            return base.Get(v => ids.Any(s => s == v.Id)).ToList();
        }

        public List<BrandEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, BrandSortOrder sortOrder)
        {
            return base.Get(Filler(DataStatus.Normal), out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize, OrderBy(sortOrder)).ToList();
        }

        public IEnumerable<BrandEntity> Get(DataStatus? status)
        {
            return base.Get(Filler(status));
        }
    }
}
