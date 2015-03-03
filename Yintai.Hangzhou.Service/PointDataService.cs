using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Point;
using Yintai.Hangzhou.Contract.DTO.Response.Point;
using Yintai.Hangzhou.Contract.Point;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class PointDataService : BaseService, IPointDataService
    {
        private readonly IPointRepository _pointRepository;

        public PointDataService(IPointRepository pointRepository)
        {
            this._pointRepository = pointRepository;
        }

        public ExecuteResult<PointInfoResponse> Get(GetPointInfoRequest request)
        {
            var pointEntity = this._pointRepository.GetItem(request.PointId);

            if (pointEntity == null)
            {
                return new ExecuteResult<PointInfoResponse>(null);
            }

            if (pointEntity.User_Id != request.AuthUid)
            {
                return new ExecuteResult<PointInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您不能使用其他用户的积点" };
            }

            return new ExecuteResult<PointInfoResponse>(MappingManager.PointInfoResponseMapping(pointEntity));
        }

        public ExecuteResult<PointCollectionResponse> GetList(GetListPointCollectionRequest request)
        {
            int totalCount;

            var data = this._pointRepository.GetPagedList(request.PagerRequest, out totalCount, request.AuthUid, request.SortOrder);

            var response = new PointCollectionResponse(request.PagerRequest, totalCount)
                {
                    PointInfoResponses = MappingManager.PointInfoResponseMapping(data)
                };

            return new ExecuteResult<PointCollectionResponse>(response);
        }
    }
}
