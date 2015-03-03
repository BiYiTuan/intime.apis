using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Comment;
using Yintai.Hangzhou.Contract.DTO.Response.Comment;

namespace Yintai.Hangzhou.Contract.Comment
{
    public interface ICommentDataService
    {
        /// <summary>
        /// 获取 评论列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CommentCollectionResponse> GetList(CommentListRequest request);

        /// <summary>
        /// 创建评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CommentInfoResponse> Create(CommentCreateRequest request);

        /// <summary>
        /// 获取评论详细信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CommentInfoResponse> Detail(CommentDetailRequest request);

        /// <summary>
        /// 更新评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CommentInfoResponse> Update(CommentUpdateRequest request);

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CommentInfoResponse> Destroy(CommentDestroyRequest request);

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CommentCollectionResponse> GetListRefresh(CommentRefreshRequest request);
    }
}
