using System.Collections.Generic;
using System.Linq;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ICommentRepository : IRepository<CommentEntity, int>
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns></returns>
        List<CommentEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, CommentSortOrder sortOrder);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="sourceId"></param>
        /// <param name="sourceType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<CommentEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, CommentSortOrder sortOrder, int? sourceId, SourceType sourceType, int? userId);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sort"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        List<CommentEntity> GetPagedList(int pageIndex, int pageSize, out int totalCount, CommentSortOrder sort, Timestamp timestamp);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sort"></param>
        /// <param name="timestamp"></param>
        /// <param name="sourceId">来源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        List<CommentEntity> GetPagedList(int pageIndex, int pageSize, out int totalCount, CommentSortOrder sort, Timestamp timestamp, int sourceId, SourceType sourceType);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="timestamp">时间戳 </param>
        /// <returns></returns>
        List<CommentEntity> GetList(int pageSize, CommentSortOrder sort, Timestamp timestamp);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="timestamp">时间戳 </param>
        /// <param name="sourceId">来源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        List<CommentEntity> GetList(int pageSize, CommentSortOrder sort, Timestamp timestamp, int sourceId, SourceType sourceType);

        /// <summary>
        /// 逻辑删
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        CommentEntity LogicallyDeleted(int commentId, int updateUser);


        IQueryable<CommentEntity> Search(int pageIndex, int pageSize, out int totalCount, Model.Filters.CommentSearchOption search);
    }
}
