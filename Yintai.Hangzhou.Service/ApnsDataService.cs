using System;
using System.Linq;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.Apns;
using Yintai.Hangzhou.Contract.DTO.Request.Device;
using Yintai.Hangzhou.Contract.DTO.Response.Device;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class ApnsDataService : BaseService, IApnsDataService
    {
        //private readonly IDeviceTokenRepository _deviceTokenRepository;
        private readonly IDeviceLogsRepository _deviceLogsRepository;

        public ApnsDataService( IDeviceLogsRepository deviceLogsRepository)
        {
            //this._deviceTokenRepository = deviceTokenRepository;
            this._deviceLogsRepository = deviceLogsRepository;
        }

        public ExecuteResult<DeviceLogInfoResponse> Register(DeviceRegisterRequest request)
        {
            //记LOG
            DeviceLogEntity entity = null;
            bool isInsert = true;
            var userId = request.AuthUser == null || request.AuthUser.Id <= 0?0:request.AuthUser.Id;
            if (userId == 0)
            {
                if (!string.IsNullOrEmpty(request.UserId))
                    int.TryParse(request.UserId, out userId);
                else
                    int.TryParse(request.Token, out userId);
            }
            if (userId > 0)
            {
                entity = _deviceLogsRepository.Get(d => d.User_Id == userId).FirstOrDefault();
                if (entity != null)
                {
                    isInsert = false;
                    entity.Latitude = Convert.ToDecimal(request.Lat);
                    entity.Longitude = Convert.ToDecimal(request.Lng);
                    entity.DeviceUid = request.Uid;
                    entity.DeviceToken = request.DeviceToken;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedUser = userId;
                    _deviceLogsRepository.Update(entity);
                }
 
            }
            if (isInsert)
            {
                entity = this._deviceLogsRepository.Insert(new DeviceLogEntity
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUser = userId,
                        DeviceToken = request.DeviceToken,
                        DeviceUid = request.Uid,
                        Latitude = Convert.ToDecimal(request.Lat),
                        Longitude = Convert.ToDecimal(request.Lng),
                        Status = 1,
                        Type = 1,
                        UpdatedDate = DateTime.Now,
                        UpdatedUser =userId,
                        User_Id = userId
                    });

            }

           

            return new ExecuteResult<DeviceLogInfoResponse> { Data = MappingManager.DeviceLogInfoResponseMapping(entity) };
        }
    }
}
