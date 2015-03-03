using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface ILikeService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="likeUserId">喜欢</param>
        /// <param name="likedUserId">被喜欢</param>
        /// <returns></returns>
        LikeEntity Get(int likeUserId, int likedUserId);
    }
}
