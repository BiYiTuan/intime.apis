using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Device;
using Yintai.Hangzhou.Contract.DTO.Response.Device;

namespace Yintai.Hangzhou.Contract.Apns
{
    public interface IApnsDataService
    {
        ///// <summary>
        ///// 注册 devicetoken 必须是 登录用户
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        
        //ExecuteResult<DeviceInfoResponse> Register(DeviceRegisterRequest request);

        /// <summary>
        /// 注册 devicetoken 必须是 登录用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<DeviceLogInfoResponse> Register(DeviceRegisterRequest request);
    }
}
