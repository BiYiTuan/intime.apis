using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Point;
using Yintai.Hangzhou.Contract.DTO.Response.Point;

namespace Yintai.Hangzhou.Contract.Point
{
    public interface IPointDataService
    {
        /// <summary>
        /// 获取积点详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PointInfoResponse> Get(GetPointInfoRequest request);

        /// <summary>
        /// 获取积点列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<PointCollectionResponse> GetList(GetListPointCollectionRequest request);
    }
}
