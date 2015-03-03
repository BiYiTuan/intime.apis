using System.Collections.Generic;
using System.Linq;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IResourceRepository : IRepository<ResourceEntity, int>
    {
        IQueryable<ResourceEntity> Get(DataStatus? dataStatus, SourceType? sourceType);

        /// <summary>
        /// 根据id查找相应的资源
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<ResourceEntity> GetList(List<int> ids);

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="sourceid"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        List<ResourceEntity> GetList(int sourceType, int sourceid);

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="sourceIds"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        List<ResourceEntity> GetList(int sourceType, List<int> sourceIds);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<ResourceEntity> Insert(List<ResourceEntity> entities);

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        ResourceEntity SetOrder(ResourceEntity entity, int sortOrder);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns></returns>
        List<ResourceEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, ResourceSortOrder sortOrder);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns></returns>
        List<ResourceEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, ResourceSortOrder sortOrder, SourceType? sourceType, int? sourceId);
    }
}
