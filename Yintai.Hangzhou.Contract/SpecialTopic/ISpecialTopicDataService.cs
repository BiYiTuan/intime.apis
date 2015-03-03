using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.SpecialTopic;
using Yintai.Hangzhou.Contract.DTO.Response.SpecialTopic;

namespace Yintai.Hangzhou.Contract.SpecialTopic
{
    public interface ISpecialTopicDataService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<SpecialTopicCollectionResponse> GetList(GetSpecialTopicListRequest request);

        /// <summary>
        /// 刷新接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<SpecialTopicCollectionResponse> GetForRefresh(GetSpecialTopicListForRefresh request);

        /// <summary>
        /// 获取详情信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<SpecialTopicInfoResponse> GetInfo(GetSpecialTopicInfoRequest request);
    }
}
