using System.Collections.Generic;
using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class DeviceTokenRepository : RepositoryBase<DeviceTokenEntity, int>, IDeviceTokenRepository
    {
        public override DeviceTokenEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public DeviceTokenEntity GetItemByUserIdToken(int userId, string deviceToken)
        {
            return base.Get(v => v.User_Id == userId && v.Token == deviceToken).SingleOrDefault();
        }

        public List<DeviceTokenEntity> GetItemByUserId(int userId)
        {
            return base.Get(v => v.User_Id == userId).ToList();
        }

        public List<DeviceTokenEntity> GetItemByToken(string deviceToken)
        {
            return base.Get(v => v.Token == deviceToken).ToList();
        }
    }
}
