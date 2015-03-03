using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class PointRepository : RepositoryBase<PointHistoryEntity, int>, IPointRepository
    {
        #region methods

        /// <summary>
        /// 过滤
        /// </summary>
        /// <returns></returns>
        private static Expression<Func<PointHistoryEntity, bool>> GetFilter(int? userId, DataStatus? dataStatus, List<PointType> pointTypes = null)
        {
            var filter = PredicateBuilder.True<PointHistoryEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            if (userId != null)
            {
                filter = filter.And(v => v.User_Id == userId.Value);
            }

            if (pointTypes != null && pointTypes.Count > 0)
            {
                var ids = new List<int>(pointTypes.Count);
                ids.AddRange(pointTypes.Select(v => (int)v));

                filter = filter.And(v => ids.Any(s => s == v.Type));
            }

            return filter;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        private static Func<IQueryable<PointHistoryEntity>, IOrderedQueryable<PointHistoryEntity>> GetOrder(PointSortOrder sort)
        {
            Func<IQueryable<PointHistoryEntity>, IOrderedQueryable<PointHistoryEntity>> order = null;
            switch (sort)
            {
                case PointSortOrder.CreatedDateDesc:
                case PointSortOrder.Default:
                default:
                    order = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return order;
        }

        #endregion

        public override PointHistoryEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<PointHistoryEntity> GetPagedList(PagerRequest request, out int totalCount, int userId, PointSortOrder sortOrder)
        {
            return
                base.Get(GetFilter(userId, DataStatus.Normal), out totalCount, request.PageIndex, request.PageSize, GetOrder(sortOrder))
                    .ToList();
        }

        public List<PointHistoryEntity> GetPagedList(PagerRequest request, out int totalCount, PointSortOrder sortOrder)
        {
            return
                base.Get(GetFilter(null, DataStatus.Normal), out totalCount, request.PageIndex, request.PageSize, GetOrder(sortOrder))
                    .ToList();
        }

        public int GetUserPointSum(int userId, List<PointType> pointType)
        {
            var t = base.Get(GetFilter(userId, DataStatus.Normal, pointType)).Sum(v => (decimal?)v.Amount);

            return t == null ? 0 : (int) t;
        }
    }
}
