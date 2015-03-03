using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Store;
using Yintai.Hangzhou.Contract.DTO.Response.Store;
using Yintai.Hangzhou.Model;

namespace Yintai.Hangzhou.Contract.Store
{
    public interface IStoreDataService
    {
        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<StoreInfoResponse> GetStore(StoreRequest request);

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        ExecuteResult<List<StoreInfoResponse>> GetAll(StoreGetAllRequest request);

        /// <summary>
        /// 刷新
        /// </summary>
        /// <returns></returns>
        ExecuteResult<List<StoreInfoResponse>> GetRefresh(StoreGetRefreshRequest request);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<StoreInfoResponse> CreateStore(StoreCreateRequest request);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<StoreInfoResponse> UpdateStore(StoreUpdateRequest request);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<StoreInfoResponse> DestroyStore(StoreDestroyRequest request);

        /// <summary>
        /// 获取店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StoreModel Get(int id);

        /// <summary>
        /// 获取店铺列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<StoreModel> GetList(List<int> ids);
    }
}
