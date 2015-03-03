using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class RemindRepository : RepositoryBase<RemindEntity, int>, IRemindRepository
    {
        public override RemindEntity GetItem(int key)
        {
            return base.Find(key);
        }
    }
}
