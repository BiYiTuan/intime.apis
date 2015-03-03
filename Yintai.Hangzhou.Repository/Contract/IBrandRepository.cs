using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IBrandRepository : IRepository<BrandEntity, int>
    {
        /// <summary>
        /// 获取全部品牌
        /// </summary>
        /// <returns></returns>
        List<BrandEntity> GetListForAll();

        /// <summary>
        /// 获取刷新
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        List<BrandEntity> GetListForRefresh(Timestamp timestamp);

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<BrandEntity> GetListByIds(List<int> ids);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns></returns>
        List<BrandEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, BrandSortOrder sortOrder);

        IEnumerable<BrandEntity> Get(DataStatus? status);
    }
}
