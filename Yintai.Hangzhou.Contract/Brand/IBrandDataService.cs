using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Brand;
using Yintai.Hangzhou.Contract.DTO.Response.Brand;

namespace Yintai.Hangzhou.Contract.Brand
{
    public interface IBrandDataService
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<BrandInfoResponse> GetBrand(BrandDetailRequest request);

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<List<BrandInfoResponse>> GetAll(BrandAllRequest request);

        /// <summary>
        /// 获取刷新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<List<BrandInfoResponse>> GetRefresh(BrandRefreshRequest request);

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<GroupStructInfoResponse<BrandInfoResponse>> GetAll4Group(BrandAllRequest request);

        /// <summary>
        /// 获取刷新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<GroupStructInfoResponse<BrandInfoResponse>> GetRefresh4Group(BrandRefreshRequest request);

        /// <summary>
        /// 创建一个品牌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<BrandInfoResponse> Create(BrandCreateRequest request);

        /// <summary>
        /// 修改一个品牌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<BrandInfoResponse> Update(BrandUpdateRequest request);

        /// <summary>
        /// 给品牌添加图片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<BrandInfoResponse> AddLogo(BrandLogoAddRequest request);

        /// <summary>
        /// 删除品牌的图片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<BrandInfoResponse> DestroyLogo(BrandLogoDestroyRequest request);
    }
}
