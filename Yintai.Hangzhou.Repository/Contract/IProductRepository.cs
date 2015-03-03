using System.Collections.Generic;
using System.Linq;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.Filters;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IProductRepository : IRepository<ProductEntity, int>
    {
        ProductEntity SetCount(ProductCountType countType, int id, int count);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="timestamp">时间戳 </param>
        /// <param name="productName">产品名称</param>
        /// <param name="brandName">品牌名</param>
        /// <param name="recommendUser">推荐人</param>
        /// <param name="tagids">Tag ids</param>
        /// <param name="brandId">品牌Id</param>
        /// <param name="dataStatus"></param>
        /// <returns></returns>
        IQueryable<ProductEntity> Search(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, Timestamp timestamp, string productName, string brandName, int? recommendUser, List<int> tagids, int? brandId, DataStatus? dataStatus);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="timestamp">时间戳 </param>
        /// <param name="productName">产品名称</param>
        /// <param name="recommendUser">推荐人</param>
        /// <param name="tagids">Tag ids</param>
        /// <param name="brandId">品牌Id</param>
        /// <returns></returns>
        List<ProductEntity> GetPagedListForSearch(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, Timestamp timestamp, string productName, int? recommendUser, List<int> tagids, int? brandId);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="timestamp">时间戳 </param>
        /// <param name="tagId">tag id</param>
        /// <param name="recommendUser">推荐人</param>
        /// <param name="brandId">品牌</param>
        /// <returns></returns>
        List<ProductEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, Timestamp timestamp, int? tagId, int? recommendUser, int? brandId);

        /// <summary>
        /// list
        /// </summary>
        /// <param name="ids">product ids</param>
        /// <returns></returns>
        List<ProductEntity> GetList(List<int> ids);

        /// <summary>
        /// SetSortOrder
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sortOrder"></param>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        ProductEntity SetSortOrder(ProductEntity entity, int sortOrder, int updateUser);

        /// <summary>
        /// SetSortOrder
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="hasImage"></param>
        /// <param name="dataStatus"></param>
        /// <param name="updateUser"></param>
        /// <param name="des">说明</param>
        /// <returns></returns>
        ProductEntity SetIsHasImage(int entityId, bool hasImage, DataStatus dataStatus, int updateUser, string des);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="productFilter">过滤选项</param>
        /// <returns></returns>
        List<ProductEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, ProductFilter productFilter);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="productFilter">过滤选项</param>
        /// <returns></returns>
        IQueryable<ProductEntity> Get(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, ProductFilter productFilter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="productFilter">过滤选项</param>
        /// <returns></returns>
        IQueryable<ProductEntity> Get(ProductSortOrder? sortOrder, ProductFilter productFilter);
    }
}
