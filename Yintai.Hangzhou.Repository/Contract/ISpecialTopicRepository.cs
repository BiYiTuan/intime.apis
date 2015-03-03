using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ISpecialTopicRepository : IRepository<SpecialTopicEntity, int>
    {
        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<SpecialTopicEntity> GetListByIds(List<int> ids);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        List<SpecialTopicEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, SpecialTopicSortOrder sortOrder, Timestamp timestamp);
    }

    public interface ISpecialTopicProductRelationRepository : IRepository<SpecialTopicProductRelationEntity, int>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="specialTopicId">id</param>
        /// <returns></returns>
        List<SpecialTopicProductRelationEntity> GetList(int specialTopicId);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="specialTopicIds">ids</param>
        /// <returns></returns>
        List<SpecialTopicProductRelationEntity> GetList(List<int> specialTopicIds);


        IQueryable<SpecialTopicProductRelationEntity> GetList4Linq(List<int> ids);

        IQueryable<SpecialTopicProductRelationEntity> GetListByProduct4Linq(List<int> productIds);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="productId">productId</param>
        /// <returns></returns>
        List<SpecialTopicProductRelationEntity> GetListByProduct(int productId);

        /// <summary>
        /// del
        /// </summary>
        /// <param name="productId"></param>
        void DeleteByProductId(int productId);
    }
}
