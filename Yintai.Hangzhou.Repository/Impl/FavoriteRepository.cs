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
    public class FavoriteRepository : RepositoryBase<FavoriteEntity, int>, IFavoriteRepository
    {
        private static Expression<Func<FavoriteEntity, bool>> GetFiller(int? userId, SourceType? sourceType, DataStatus? dataStatus)
        {
            var filter = PredicateBuilder.True<FavoriteEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            if (sourceType != null && sourceType.Value != SourceType.Default)
            {
                filter = filter.And(v => v.FavoriteSourceType == (int)sourceType);
            }

            if (userId != null)
            {
                filter = filter.And(v => v.User_Id == userId.Value);
            }

            return filter;
        }

        private static Func<IQueryable<FavoriteEntity>, IOrderedQueryable<FavoriteEntity>> GetSort(FavoriteSortOrder sortOrder)
        {
            Func<IQueryable<FavoriteEntity>, IOrderedQueryable<FavoriteEntity>> orderBy = null;

            switch (sortOrder)
            {
                default:
                    orderBy = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return orderBy;
        }

        #region Overrides of RepositoryBase<FavoriteEntity,int>

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override FavoriteEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public IQueryable<FavoriteEntity> Get(int userid, PagerRequest pagerRequest, out int totalCount, FavoriteSortOrder sortOrder,
                              SourceType sourceType)
        {
            return base.Get(GetFiller(userid, sourceType, DataStatus.Normal), out totalCount, pagerRequest.PageIndex,
                            pagerRequest.PageSize,
                            GetSort(sortOrder));
        }

        public int GetUserFavorCount(int userId, SourceType? sourceType)
        {
            return base.Get(GetFiller(userId, sourceType, DataStatus.Normal)).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceid"></param>
        /// <returns></returns>
        public FavoriteEntity GetItem(int userid, SourceType sourceType, int sourceid)
        {
            return
                Get(
                    v =>
                    v.User_Id == userid && v.FavoriteSourceType == (int)sourceType && v.FavoriteSourceId == sourceid).
                    FirstOrDefault();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pagerRequest"></param>
        /// <param name="totalCount"></param>
        /// <param name="sortOrder"> </param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public List<FavoriteEntity> GetPagedList(int userid, PagerRequest pagerRequest, out int totalCount, FavoriteSortOrder sortOrder, SourceType sourceType)
        {
            return Get(userid, pagerRequest, out totalCount, sortOrder, sourceType).ToList();
        }

        #endregion
    }
}
