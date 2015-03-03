using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class DeviceLogsRepository : RepositoryBase<DeviceLogEntity, int>, IDeviceLogsRepository
    {
        public override DeviceLogEntity GetItem(int key)
        {
            return base.Find(key);
        }
    }
}