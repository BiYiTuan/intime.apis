using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service.Impl
{
    public class RemindService : BaseService, IRemindService
    {
        private readonly IRemindRepository _remindRepository;

        public RemindService(IRemindRepository remindRepository)
        {
            this._remindRepository = remindRepository;
        }

        public RemindEntity Insert(RemindEntity entity)
        {
            return _remindRepository.Insert(entity);
        }
    }
}
