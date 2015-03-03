using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Apns.FSNotificationService
{
    interface INotificationPolicy:IDisposable
    {
       void Execute();
    }
}
