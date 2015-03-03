using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Like;
using Yintai.Hangzhou.Contract.DTO.Response.Like;

namespace Yintai.Hangzhou.Contract.Like
{
    public interface ILikeDataService
    {
        /// <summary>
        /// 喜欢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<LikeCoutomerResponse> Like(LikeCreateRequest request);

        /// <summary>
        /// 获取我喜欢的列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<LikeCoutomerCollectionResponse> GetILikeList(GetLikeListRequest request);

        /// <summary>
        /// 获取喜欢我的列表（被喜欢）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<LikeCoutomerCollectionResponse> GetLikeMeList(GetLikeListRequest request);

        /// <summary>
        /// 删除喜欢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<LikeCoutomerResponse> Destroy(LikeDestroyRequest request);
    }
}
