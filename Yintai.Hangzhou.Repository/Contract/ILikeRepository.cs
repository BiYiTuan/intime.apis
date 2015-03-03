using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ILikeRepository : IRepository<LikeEntity, int>
    {
        /// <summary>
        /// 获取我喜欢数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetILikeCount(int userId);

        /// <summary>
        /// 获取喜欢我数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetLikeMeCount(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="likeuserid">发出喜欢(关注)人的Id</param>
        /// <param name="likeduserid">被喜欢(关注)人的Id</param>
        /// <returns></returns>
        LikeEntity GetItem(int likeuserid, int likeduserid);

        /// <summary>
        /// 获取 我喜欢(关注)列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        List<LikeEntity> GetPagedListForILike(PagerRequest request, out int totalCount, int userId, LikeSortOrder sortOrder);

        /// <summary>
        /// 获取 喜欢(关注)我列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        List<LikeEntity> GetPagedListForLikeMe(PagerRequest request, out int totalCount, int userId, LikeSortOrder sortOrder);
    }
}
