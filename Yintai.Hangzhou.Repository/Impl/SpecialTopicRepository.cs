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
    public class SpecialTopicRepository : RepositoryBase<SpecialTopicEntity, int>, ISpecialTopicRepository
    {
        #region methods

        private static Expression<Func<SpecialTopicEntity, bool>> Filler(DataStatus? dataStatus, Timestamp timestamp)
        {
            var filter = PredicateBuilder.True<SpecialTopicEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            if (timestamp != null)
            {
                switch (timestamp.TsType)
                {
                    case TimestampType.New:
                        filter = filter.And(v => v.UpdatedDate > timestamp.Ts);
                        break;
                    case TimestampType.Old:
                    default:
                        filter = filter.And(v => v.UpdatedDate <= timestamp.Ts);
                        break;
                }
            }

            return filter;
        }

        private static Func<IQueryable<SpecialTopicEntity>, IOrderedQueryable<SpecialTopicEntity>> OrderBy(SpecialTopicSortOrder sortOrder)
        {
            Func<IQueryable<SpecialTopicEntity>, IOrderedQueryable<SpecialTopicEntity>> orderBy = null;

            switch (sortOrder)
            {
                default:
                    orderBy = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return orderBy;
        }

        #endregion

        public override SpecialTopicEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<SpecialTopicEntity> GetListByIds(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<SpecialTopicEntity>(0);
            }

            return base.Get(v => ids.Any(s => s == v.Id)).ToList();
        }

        public List<SpecialTopicEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, SpecialTopicSortOrder sortOrder, Timestamp timestamp)
        {
            return
    base.Get(Filler(DataStatus.Normal, timestamp), out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize, OrderBy(sortOrder))
        .ToList();
        }
    }
}