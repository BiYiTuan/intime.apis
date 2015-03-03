using System;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.Region;
using Yintai.Hangzhou.Contract.Request.Region;
using Yintai.Hangzhou.Contract.Response.Region;

namespace Yintai.Hangzhou.Service
{
    public class RegionService:BaseService,IRegionService
    {

        #region Implementation of IRegionService

        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<GetRegionListResponse> GetRegionList(GetRegionListRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}