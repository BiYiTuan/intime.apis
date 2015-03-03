using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ITagRepository : IRepository<TagEntity, int>
    {
        /// <summary>
        /// 获取 Tag
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<TagEntity> GetListByIds(List<int> ids);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<TagEntity> GetListForAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        List<TagEntity> GetListForRefresh(Timestamp timestamp);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns></returns>
        List<TagEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, TagSortOrder sortOrder);
    }
}
