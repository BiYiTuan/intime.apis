using System.Collections.Generic;
using System.Linq;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IFavoriteRepository : IRepository<FavoriteEntity, int>
    {
        IQueryable<FavoriteEntity> Get(int userid, PagerRequest pagerRequest, out int totalCount,
                                       FavoriteSortOrder sortOrder, SourceType sourceType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        int GetUserFavorCount(int userId, SourceType? sourceType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceid"></param>
        /// <returns></returns>
        FavoriteEntity GetItem(int userid, SourceType sourceType, int sourceid);

        /// <summary>
        /// ·ÖÒ³
        /// </summary>
        /// <param name="userid"> </param>
        /// <param name="pagerRequest"></param>
        /// <param name="totalCount"></param>
        /// <param name="sortOrder"> </param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        List<FavoriteEntity> GetPagedList(int userid, PagerRequest pagerRequest, out int totalCount, FavoriteSortOrder sortOrder, SourceType sourceType);
    }
}