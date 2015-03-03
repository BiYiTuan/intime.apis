using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Product;
using Yintai.Hangzhou.Contract.DTO.Response.Product;

namespace Yintai.Hangzhou.Contract.Product
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Hangzhou.Contract.Product
    /// FileName: IProductService
    ///
    /// Created at 11/12/2012 2:17:07 PM
    /// Description: 
    /// </summary>
    public interface IProductDataService
    {
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductCollectionResponse> GetProductList(GetProductListRequest request);

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> GetProductInfo(GetProductInfoRequest request);

        /// <summary>
        /// 创建一个新产品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> CreateProduct(CreateProductRequest request);

        /// <summary>
        /// 修改一个产品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> UpdateProduct(UpdateProductRequest request);

        /// <summary>
        /// 删除一个产品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> DestroyProduct(DestroyProductRequest request);

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> CreateResourceProduct(CreateResourceProductRequest request);

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> DestroyResourceProduct(DestroyResourceProductRequest request);

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductCollectionResponse> RefreshProduct(GetProductRefreshRequest request);

        /// <summary>
        /// 创建分享
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> CreateShare(CreateShareProductRequest request);

        /// <summary>
        /// 创建收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> CreateFavor(CreateFavorProductRequest request);

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> DestroyFavor(DestroyFavorProductRequest request);

        /// <summary>
        /// 创建优惠卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductInfoResponse> CreateCoupon(CreateCouponProductRequest request);

        /// <summary>
        /// 创建优惠卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ProductCollectionResponse> Search(SearchProductRequest request);
    }
}
