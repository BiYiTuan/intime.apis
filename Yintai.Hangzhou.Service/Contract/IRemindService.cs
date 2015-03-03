using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface IRemindService
    {
        RemindEntity Insert(RemindEntity entity);
    }
}
