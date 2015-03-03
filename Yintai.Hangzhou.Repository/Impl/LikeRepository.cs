using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using System.Linq;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class LikeRepository : RepositoryBase<LikeEntity, int>, ILikeRepository
    {
        #region methods

        /// <summary>
        /// filter
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="likeType"></param>
        /// <returns></returns>
        private static Expression<Func<LikeEntity, bool>> GetFilter(int? userId, LikeType likeType)
        {
            Expression<Func<LikeEntity, bool>> filter = null;

            if (userId == null)
            {
                return filter;
            }
            else
            {
                if (likeType == LikeType.ILike)
                {
                    filter = v => v.LikeUserId == userId;
                }
                else
                    if (likeType == LikeType.LikeMe)
                    {
                        filter = v => v.LikedUserId == userId;
                    }
            }

            return filter;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        private static Func<IQueryable<LikeEntity>, IOrderedQueryable<LikeEntity>> GetOrder(LikeSortOrder sort)
        {
            Func<IQueryable<LikeEntity>, IOrderedQueryable<LikeEntity>> order = null;
            switch (sort)
            {
                case LikeSortOrder.Default:
                default:
                    order = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return order;
        }

        #endregion

        public override LikeEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public int GetILikeCount(int userId)
        {
            return base.Get(v => v.LikeUserId == userId).Count();
        }

        public int GetLikeMeCount(int userId)
        {
            return base.Get(v => v.LikedUserId == userId).Count();
        }

        public LikeEntity GetItem(int likeuserid, int likeduserid)
        {
            return base.Get(v => v.LikeUserId == likeuserid && v.LikedUserId == likeduserid).FirstOrDefault();
        }

        public List<LikeEntity> GetPagedListForILike(PagerRequest request, out int totalCount, int userId, LikeSortOrder sortOrder)
        {
            return
                base.Get(GetFilter(userId, LikeType.ILike), out totalCount, request.PageIndex, request.PageSize, GetOrder(sortOrder))
                    .ToList();
        }

        public List<LikeEntity> GetPagedListForLikeMe(PagerRequest request, out int totalCount, int userId, LikeSortOrder sortOrder)
        {
            return
    base.Get(GetFilter(userId, LikeType.LikeMe), out totalCount, request.PageIndex, request.PageSize, GetOrder(sortOrder))
        .ToList();
        }
    }
}
