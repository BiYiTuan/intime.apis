using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IPointRepository : IRepository<PointHistoryEntity, int>
    {
        /// <summary>
        /// 积点分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        List<PointHistoryEntity> GetPagedList(PagerRequest request, out int totalCount, int userId,
                                              PointSortOrder sortOrder);

        /// <summary>
        /// 积点分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        List<PointHistoryEntity> GetPagedList(PagerRequest request, out int totalCount, PointSortOrder sortOrder);

        /// <summary>
        /// 积点类型
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pointType"></param>
        /// <returns></returns>
        int GetUserPointSum(int userId, List<PointType> pointType);
    }
}
