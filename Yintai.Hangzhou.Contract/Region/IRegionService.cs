using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.Request.Region;
using Yintai.Hangzhou.Contract.Response.Region;

namespace Yintai.Hangzhou.Contract.Region
{
    public interface IRegionService
    {
        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<GetRegionListResponse> GetRegionList(GetRegionListRequest request);
    }
}
