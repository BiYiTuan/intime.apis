using System.Collections.Generic;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IDeviceTokenRepository : IRepository<DeviceTokenEntity, int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DeviceTokenEntity GetItemByUserIdToken(int userId, string deviceToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<DeviceTokenEntity> GetItemByUserId(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceToken"></param>
        /// <returns></returns>
        List<DeviceTokenEntity> GetItemByToken(string deviceToken);
    }
}
