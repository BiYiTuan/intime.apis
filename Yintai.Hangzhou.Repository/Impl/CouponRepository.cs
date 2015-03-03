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
    public class CouponRepository : RepositoryBase<CouponHistoryEntity, int>, ICouponRepository
    {
        #region methods

        private static Expression<Func<CouponHistoryEntity, bool>> Filter(CouponBusinessStatus? businessStatus, DataStatus? dataStatus, SourceType? sourceType, int sourceId, int? userId)
        {
            var filter = PredicateBuilder.True<CouponHistoryEntity>();

            if (businessStatus != null)
            {
                switch (businessStatus)
                {
                    case CouponBusinessStatus.All:
                        break;
                    case CouponBusinessStatus.Expired:
                        filter = filter.And(v => v.ValidEndDate > DateTime.Now);
                        break;
                    case CouponBusinessStatus.UnExpired:
                        filter = filter.And(v => v.ValidEndDate > DateTime.Now);
                        break;
                    case CouponBusinessStatus.Default:
                    case CouponBusinessStatus.Normal:
                    default:
                        filter = filter.And(v => v.ValidEndDate > DateTime.Now);
                        break;
                }
            }

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            if (sourceType != null)
            {
                switch (sourceType)
                {
                    case SourceType.Promotion:
                        filter = filter.And(v => v.FromPromotion == sourceId);
                        break;
                    case SourceType.Product:
                        filter = filter.And(v => v.FromProduct == sourceId);
                        break;
                    case SourceType.Store:
                        filter = filter.And(v => v.FromStore == sourceId);
                        break;
                    case SourceType.Customer:
                        filter = filter.And(v => v.FromUser == sourceId);
                        break;
                }
            }

            if (userId != null)
            {
                filter = filter.And(v => v.User_Id == userId.Value);
            }

            return filter;
        }

        /// <summary>
        /// ≈≈–Ú
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        private static Func<IQueryable<CouponHistoryEntity>, IOrderedQueryable<CouponHistoryEntity>> GetOrder(CouponSortOrder sort)
        {
            Func<IQueryable<CouponHistoryEntity>, IOrderedQueryable<CouponHistoryEntity>> order = null;
            switch (sort)
            {
                case CouponSortOrder.Default:
                default:
                    order = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return order;
        }

        #endregion

        #region Overrides of RepositoryBase<CouponHistoryEntity,int>

        /// <summary>
        /// ≤È’“key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override CouponHistoryEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<CouponHistoryEntity> GetPagedListByUserId(PagerRequest pagerRequest, out int totalCount, int userId, CouponSortOrder sortOrder)
        {
            return
                base.Get(Filter(CouponBusinessStatus.Default, DataStatus.Normal, null, 0, userId), out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize,
                         GetOrder(sortOrder)).ToList();
        }

        public List<CouponHistoryEntity> GetListByUserIdSourceId(int userId, int sourceId, SourceType? sourceType)
        {

            return
                base.Get(Filter(CouponBusinessStatus.Default, DataStatus.Normal, sourceType, sourceId, userId)).ToList();

            //Expression<Func<CouponHistoryEntity, bool>> filter = null;
            //if (sourceType == null)
            //{
            //    filter = v => v.User_Id == userId && v.Status == (int)DataStatus.Normal;
            //}
            //else
            //{
            //    switch (sourceType)
            //    {
            //        case SourceType.Promotion:
            //            filter = v => v.User_Id == userId && v.FromPromotion == sourceId && v.Status == (int)DataStatus.Normal;
            //            break;
            //        case SourceType.Product:
            //            filter = v => v.User_Id == userId && v.FromProduct == sourceId && v.Status == (int)DataStatus.Normal;
            //            break;
            //    }

            //}

            //return base.Get(filter).ToList();
        }

        public int Get4Count(int sourceId, SourceType sourceType)
        {
            return base.Get(Filter(null, DataStatus.Normal, sourceType, sourceId, null)).Count();
        }

        public int GetUserCouponCount(int userId, CouponBusinessStatus status)
        {
            return base.Get(Filter(status, null, null, 0, userId)).Count();
        }

        #endregion
    }
}