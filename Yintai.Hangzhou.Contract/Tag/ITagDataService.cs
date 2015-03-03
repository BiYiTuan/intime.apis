using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Tag;
using Yintai.Hangzhou.Contract.DTO.Response.Tag;

namespace Yintai.Hangzhou.Contract.Tag
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Hangzhou.Contract.Tag
    /// FileName: ITagService
    ///
    /// Created at 11/12/2012 3:01:29 PM
    /// Description: 
    /// </summary>
    public interface ITagDataService
    {
        /// <summary>
        /// 获取Tag信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<TagInfoResponse> Get(TagGetRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ExecuteResult<List<TagInfoResponse>> GetAll(TagGetAllRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ExecuteResult<List<TagInfoResponse>> GetRefresh(TagGetRefreshRequest request);

        /// <summary>
        /// 创建Tag信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<TagInfoResponse> Create(TagCreateRequest request);

        /// <summary>
        /// 修改Tag信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<TagInfoResponse> Update(TagUpdateRequest request);

        /// <summary>
        /// 删除Tag信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<TagInfoResponse> Destroy(TagDestroyRequest request);
    }
}
