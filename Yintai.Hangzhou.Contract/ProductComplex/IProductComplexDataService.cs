using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.ProductComplex;
using Yintai.Hangzhou.Contract.DTO.Response;

namespace Yintai.Hangzhou.Contract.ProductComplex
{
    public interface IItemsDataService
    {
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ItemsCollectionResponse> GetProductList(GetItemsListRequest request);
    }
}
