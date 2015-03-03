using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Favorite;
using Yintai.Hangzhou.Contract.DTO.Response.Favorite;
using Yintai.Hangzhou.Contract.Request.Favorite;

namespace Yintai.Hangzhou.Contract.Favorite
{
    public interface IFavoriteDataService
    {
        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult Create(FavoriteCreateRequest request);

        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<FavoriteCollectionResponse> GetFavoriteList(GetFavoriteListRequest request);

        /// <summary>
        /// 获取达人收藏列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<FavoriteCollectionResponse> GetDarenFavoriteList(DarenFavoriteListRequest request);

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult Destroy(FavoriteDestroyRequest request);
    }
}
