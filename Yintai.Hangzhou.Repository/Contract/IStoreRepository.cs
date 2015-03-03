using System.Collections.Generic;
using System.Linq;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IStoreRepository : IRepository<StoreEntity, int>
    {
        IQueryable<StoreEntity> Get(DataStatus? dataStatus);

        /// <summary>
        /// 获取指定ID 的 店铺
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<StoreEntity> GetListByIds(List<int> ids);

        /// <summary>
        /// 获取全部店铺
        /// </summary>
        /// <returns></returns>
        List<StoreEntity> GetListForAll();

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        List<StoreEntity> GetListForRefresh(Timestamp timestamp);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns></returns>
        List<StoreEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, StoreSortOrder sortOrder);
    }
}
