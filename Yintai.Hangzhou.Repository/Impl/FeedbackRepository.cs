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
    public class FeedbackRepository : RepositoryBase<FeedbackEntity, int>, IFeedbackRepository
    {
        private static Expression<Func<FeedbackEntity, bool>> Filler(DataStatus? dataStatus)
        {
            var filter = PredicateBuilder.True<FeedbackEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            return filter;
        }

        private static Func<IQueryable<FeedbackEntity>, IOrderedQueryable<FeedbackEntity>> OrderBy(FeedbackSortOrder sortOrder)
        {
            Func<IQueryable<FeedbackEntity>, IOrderedQueryable<FeedbackEntity>> orderBy = null;

            switch (sortOrder)
            {
                default:
                    orderBy = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return orderBy;
        }

        public override FeedbackEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<FeedbackEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, FeedbackSortOrder sortOrder)
        {
            return
    base.Get(Filler(DataStatus.Normal), out totalCount, pagerRequest.PageIndex,
             pagerRequest.PageSize, OrderBy(sortOrder)).ToList();
        }
    }
}
