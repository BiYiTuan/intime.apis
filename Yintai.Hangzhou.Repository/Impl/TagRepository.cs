using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class TagRepository : RepositoryBase<TagEntity, int>, ITagRepository
    {
        private static Expression<Func<TagEntity, bool>> Filler(DataStatus? dataStatus)
        {
            var filter = PredicateBuilder.True<TagEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            return filter;
        }

        private static Func<IQueryable<TagEntity>, IOrderedQueryable<TagEntity>> OrderBy(TagSortOrder sortOrder)
        {
            Func<IQueryable<TagEntity>, IOrderedQueryable<TagEntity>> orderBy = null;

            switch (sortOrder)
            {
                default:
                    orderBy = v => v.OrderByDescending(s => s.SortOrder).ThenByDescending(s => s.CreatedDate);
                    break;
            }

            return orderBy;
        }


        public override TagEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<TagEntity> GetListByIds(List<int> ids)
        {
            return base.Get(v => ids.Any(s => s == v.Id)).ToList();
        }

        public List<TagEntity> GetListForAll()
        {
            return base.Get(v => v.Status == (int)DataStatus.Normal, OrderBy(TagSortOrder.Default)).ToList();
        }

        public List<TagEntity> GetListForRefresh(Timestamp timestamp)
        {
            switch (timestamp.TsType)
            {
                case TimestampType.Old:
                    return base.Get(v => v.UpdatedDate <= timestamp.Ts && v.Status == (int)DataStatus.Normal, OrderBy(TagSortOrder.Default)).ToList();
                case TimestampType.New:
                default:
                    return base.Get(v => v.UpdatedDate > timestamp.Ts && v.Status == (int)DataStatus.Normal, OrderBy(TagSortOrder.Default)).ToList();

            }
        }

        public List<TagEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, TagSortOrder sortOrder)
        {
            return
               base.Get(Filler(DataStatus.Normal), out totalCount, pagerRequest.PageIndex,
                        pagerRequest.PageSize, OrderBy(sortOrder)).ToList();
        }
    }
}
