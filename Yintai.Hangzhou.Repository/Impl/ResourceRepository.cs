using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class ResourceRepository : RepositoryBase<ResourceEntity, int>, IResourceRepository
    {
        #region methods

        private static Expression<Func<ResourceEntity, bool>> Filter(DataStatus? dataStatus, SourceType? sourceType, IEnumerable<int> sourceidList)
        {
            var filter = PredicateBuilder.True<ResourceEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            if (sourceType != null)
            {
                filter = filter.And(v => v.SourceType == (int)sourceType);
            }

            if (sourceidList != null)
            {
                filter = filter.And(v => sourceidList.Any(s => v.SourceId == s));
            }

            return filter;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        private static Func<IQueryable<ResourceEntity>, IOrderedQueryable<ResourceEntity>> GetOrder(ResourceSortOrder sort)
        {
            Func<IQueryable<ResourceEntity>, IOrderedQueryable<ResourceEntity>> order = null;
            switch (sort)
            {
                case ResourceSortOrder.CreateDate:
                    order = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
                case ResourceSortOrder.Default:
                default:
                    order = v => v.OrderBy(s => s.SourceType).ThenBy(s => s.SourceId).ThenByDescending(s => s.SortOrder);
                    break;
            }

            return order;
        }

        #endregion

        #region Overrides of RepositoryBase<ResourceEntity,int>

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override ResourceEntity GetItem(int key)
        {
            return base.Find(key);
        }

        #endregion

        #region Implementation of IResourceRepository

        public IQueryable<ResourceEntity> Get(DataStatus? dataStatus, SourceType? sourceType)
        {
            return base.Get(Filter(dataStatus, sourceType, null), GetOrder(ResourceSortOrder.Default));
        }

        /// <summary>
        /// 根据id查找相应的资源
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<ResourceEntity> GetList(List<int> ids)
        {
            return base.Get(v => ids.Any(s => s == v.Id), GetOrder(ResourceSortOrder.Default)).ToList();
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="sourceid"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public List<ResourceEntity> GetList(int sourceType, int sourceid)
        {
            return
                base.Get(v => v.Status == 1 && v.SourceId == sourceid && v.SourceType == sourceType, GetOrder(ResourceSortOrder.Default)).
                    ToList();
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="sourceIds"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public List<ResourceEntity> GetList(int sourceType, List<int> sourceIds)
        {
            return base.Get(v => v.Status == 1 && v.SourceType == sourceType && sourceIds.Any(s => s == v.SourceId), GetOrder(ResourceSortOrder.Default)).ToList();
        }

        public List<ResourceEntity> Insert(List<ResourceEntity> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            return base.BatchInsert(entities.ToArray()).ToList();
        }

        public ResourceEntity SetOrder(ResourceEntity entity, int sortOrder)
        {
            if (entity == null)
            {
                return null;
            }

            var parames = new SqlParameter[2];
            parames[0] = new SqlParameter("@sort", sortOrder);
            parames[1] = new SqlParameter("@Id", entity.Id);

            var sql = "UPDATE [YintaiHzhou].[dbo].[Resources] SET [SortOrder] = @sort WHERE [Id] = @Id";
            /*
            var sql =
                "UPDATE [dbo].[UserAccount] SET    [Amount] = [Amount] + 1,[UpdatedUser] = 1,[UpdatedDate] = GETDATE() WHERE  [User_Id] = 1 AND [AccountType] = 7;";
            //*/
            //return base.ExecuteSqlCommand(sql, param);

            var i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sql, parames);

            if (i > 0)
            {
                return GetItem(entity.Id);
            }
            else
            {
                return null;
            }

        }

        public List<ResourceEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, ResourceSortOrder sortOrder)
        {
            return base.Get(Filter(DataStatus.Normal, null, null), out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize, GetOrder(sortOrder)).ToList();
        }

        public List<ResourceEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, ResourceSortOrder sortOrder, SourceType? sourceType,
                                 int? sourceId)
        {
            List<int> list = null;
            if (sourceId != null)
            {
                list = new List<int>(1) { sourceId.Value };
            }

            return base.Get(Filter(DataStatus.Normal, sourceType, list), out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize, GetOrder(sortOrder)).ToList();
        }

        #endregion
    }
}
