using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.Filters;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class CommentRepository : RepositoryBase<CommentEntity, int>, ICommentRepository
    {
        #region methods

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="dataStatus"></param>
        /// <param name="timestamp"></param>
        /// <param name="sourceId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        private static Expression<Func<CommentEntity, bool>> Filter(DataStatus? dataStatus, Timestamp timestamp, int? sourceId,
                                                                       SourceType? sourceType, int? userId)
        {
            var filter = PredicateBuilder.True<CommentEntity>();

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

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus);
            }

            if (sourceId != null && sourceId > 0)
            {
                filter = filter.And(v => v.SourceId == sourceId.Value);
            }

            if (sourceType != null && sourceType != SourceType.Default)
            {
                filter = filter.And(v => v.SourceType == (int)sourceType);
            }

            if (userId != null)
            {
                filter = filter.And(v => v.User_Id == userId.Value);
            }

            return filter;
        }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="sourceId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        private static Expression<Func<CommentEntity, bool>> GetFilter(Timestamp timestamp, int sourceId, SourceType sourceType)
        {
            return Filter(DataStatus.Normal, timestamp, sourceId, sourceType, null);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        private static Func<IQueryable<CommentEntity>, IOrderedQueryable<CommentEntity>> GetOrder(CommentSortOrder sort)
        {
            Func<IQueryable<CommentEntity>, IOrderedQueryable<CommentEntity>> order = null;
            switch (sort)
            {
                case CommentSortOrder.Default:
                default:
                    order = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return order;
        }

        #endregion

        public override CommentEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<CommentEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, CommentSortOrder sortOrder)
        {
            return GetPagedList(pagerRequest.PageIndex, pagerRequest.PageSize, out totalCount, sortOrder,
                                new Timestamp());
        }

        public List<CommentEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, CommentSortOrder sortOrder, int? sourceId,
                                 SourceType sourceType, int? userId)
        {
            return
                base.Get(Filter(DataStatus.Normal, null, sourceId, sourceType, userId), out totalCount, pagerRequest.PageIndex,
                         pagerRequest.PageSize, GetOrder(sortOrder)).ToList();
        }

        public List<CommentEntity> GetPagedList(int pageIndex, int pageSize, out int totalCount, CommentSortOrder sort, Timestamp timestamp)
        {
            return base.Get(GetFilter(timestamp, 0, SourceType.Default), out totalCount, pageIndex, pageSize, GetOrder(sort)).ToList();
        }

        public List<CommentEntity> GetPagedList(int pageIndex, int pageSize, out int totalCount, CommentSortOrder sort, Timestamp timestamp,
                                 int sourceId, SourceType sourceType)
        {
            return base.Get(GetFilter(timestamp, sourceId, sourceType), out totalCount, pageIndex, pageSize, GetOrder(sort)).ToList();

        }

        public List<CommentEntity> GetList(int pageSize, CommentSortOrder sort, Timestamp timestamp)
        {
            return base.Get(GetFilter(timestamp, 0, SourceType.Default), GetOrder(sort)).Take(pageSize).ToList();
        }

        public List<CommentEntity> GetList(int pageSize, CommentSortOrder sort, Timestamp timestamp, int sourceId, SourceType sourceType)
        {
            return base.Get(GetFilter(timestamp, sourceId, sourceType), GetOrder(sort)).Take(pageSize).ToList();

        }

        public CommentEntity LogicallyDeleted(int commentId, int updateUser)
        {
            var entity = GetItem(commentId);
            if (entity == null)
            {
                return null;
            }

            entity.Status = (int)DataStatus.Deleted;
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedUser = updateUser;

            base.Update(entity);

            return entity;
        }


        public IQueryable<CommentEntity> Search(int pageIndex, int pageSize, out int totalCount, Model.Filters.CommentSearchOption search)
        {
            var linq = Context.Set<CommentEntity>().Where(p => (!search.ProductId.HasValue ||
                                                   (p.SourceId == search.ProductId.Value && p.SourceType == (int)SourceType.Product))
                                           && (!search.PromotionId.HasValue ||
                                                   (p.SourceId == search.PromotionId.Value && p.SourceType == (int)SourceType.Promotion))
                                           && p.Status != (int)DataStatus.Deleted);
            var userAuthRepo = ServiceLocator.Current.Resolve<IUserAuthRepository>();
            linq = userAuthRepo.AuthFilter(linq, search.CurrentUser, search.CurrentUserRole) as IQueryable<CommentEntity>;
            if (!string.IsNullOrEmpty(search.CommentUserName) &&
                search.CommentUserName.Trim().Length > 0)
            {
                linq = (from p in linq
                        from ps in Context.Set<UserEntity>()
                        where ps.Id == p.CreatedUser &&
                           (ps.Name.StartsWith(search.CommentUserName) || ps.Nickname.StartsWith(search.CommentUserName))
                        select p);
            }

            if (!string.IsNullOrEmpty(search.CreateUserName) &&
                 search.CreateUserName.Trim().Length > 0)
            {
                linq = from p in linq
                       let prods = (from prod in Context.Set<ProductEntity>()
                                    join c in Context.Set<UserEntity>() on prod.CreatedUser equals c.Id
                                    where c.Nickname.StartsWith(search.CreateUserName)
                                     && prod.Id == p.SourceId
                                     && p.SourceType == (int)SourceType.Product
                                    select prod)
                       let pros = (from pro in Context.Set<PromotionEntity>()
                                   join c in Context.Set<UserEntity>() on pro.CreatedUser equals c.Id
                                   where
                                    c.Nickname.StartsWith(search.CreateUserName)
                                    && pro.Id == p.SourceId
                                    && p.SourceType == (int)SourceType.Promotion
                                   select pro)
                       where prods.Any()
                              ||
                              pros.Any()
                       select p;
            }
            if (search.CreateUserId.HasValue)
            {
                linq = from p in linq
                       let prods = (from prod in Context.Set<ProductEntity>()
                                    join c in Context.Set<UserEntity>() on prod.CreatedUser equals c.Id
                                    where c.Id == search.CreateUserId.Value
                                     && prod.Id == p.SourceId
                                     && p.SourceType == (int)SourceType.Product
                                    select prod)
                       let pros = (from pro in Context.Set<PromotionEntity>()
                                   join c in Context.Set<UserEntity>() on pro.CreatedUser equals c.Id
                                   where
                                    c.Id == search.CreateUserId.Value
                                    && pro.Id == p.SourceId
                                    && p.SourceType == (int)SourceType.Promotion
                                   select pro)
                       where prods.Any()
                              ||
                              pros.Any()
                       select p;
            }

            Func<IQueryable<CommentEntity>, IOrderedQueryable<CommentEntity>> orderBy = (IQueryable<CommentEntity> e) =>
            {
                if (!search.OrderBy.HasValue)
                    return e.OrderByDescending(o => o.CreatedDate);
                else
                {
                    switch (search.OrderBy.Value)
                    {
                        case GenericOrder.OrderByCreateUser:
                            return e.OrderByDescending(o => o.CreatedUser);
                        case GenericOrder.OrderByName:
                            return e.OrderByDescending(o => o.Content);
                        default:
                            return e.OrderByDescending(o => o.CreatedDate);

                    }
                }
            };
            linq = orderBy(linq);
            totalCount = linq.Count();

            var skipCount = (pageIndex - 1) * pageSize;

            linq = skipCount == 0 ? linq.Take(pageSize) : linq.Skip(skipCount).Take(pageSize);
            return linq;

        }
    }
}
