using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface IFavoriteService
    {
        /// <summary>
        /// 创建一个收藏
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        FavoriteEntity Create(FavoriteEntity entity);

        /// <summary>
        /// 获取一个收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="soureceId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        FavoriteEntity Get(int userId, int soureceId, SourceType sourceType);

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="entity"></param>
        void Del(FavoriteEntity entity);
    }
}