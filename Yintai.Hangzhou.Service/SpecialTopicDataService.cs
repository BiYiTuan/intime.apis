using System;
using System.Globalization;
using System.Linq;
using Yintai.Architecture.Common.Caching;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.SpecialTopic;
using Yintai.Hangzhou.Contract.DTO.Response.SpecialTopic;
using Yintai.Hangzhou.Contract.SpecialTopic;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class SpecialTopicDataService : BaseService, ISpecialTopicDataService
    {
        private readonly ISpecialTopicRepository _specialTopicRepository;

        public SpecialTopicDataService(ISpecialTopicRepository specialTopicRepository)
        {
            _specialTopicRepository = specialTopicRepository;
        }

        private ExecuteResult<SpecialTopicCollectionResponse> Get(PagerRequest pagerRequest, SpecialTopicSortOrder sortOrder, Timestamp timestamp)
        {



            int totalCount;

            var data = _specialTopicRepository.GetPagedList(pagerRequest, out totalCount, sortOrder,
                                                            timestamp);

            var response = new SpecialTopicCollectionResponse(pagerRequest, totalCount)
            {
                SpecialTopics = MappingManager.SpecialTopicInfoResponseMapping(data).ToList()
            };




            var result = new ExecuteResult<SpecialTopicCollectionResponse> { Data = response };

            return result;
        }

        public ExecuteResult<SpecialTopicCollectionResponse> GetList(GetSpecialTopicListRequest request)
        {
            return Get(request.PagerRequest, request.SortOrder, request.Timestamp);
        }

        public ExecuteResult<SpecialTopicCollectionResponse> GetForRefresh(GetSpecialTopicListForRefresh request)
        {
            return Get(request.PagerRequest, request.SortOrder, request.Timestamp);
        }

        public ExecuteResult<SpecialTopicInfoResponse> GetInfo(GetSpecialTopicInfoRequest request)
        {


            var data = _specialTopicRepository.GetItem(request.TopicId);
            var response = MappingManager.SpecialTopicInfoResponseMapping(data);




            var result = new ExecuteResult<SpecialTopicInfoResponse>(response);

            return result;
        }
    }
}
