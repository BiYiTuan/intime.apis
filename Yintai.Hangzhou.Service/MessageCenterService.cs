using com.intime.fashion.common.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Hangzhou.Service
{
    public static class MessageCenterService
    {
        public static bool SafeNotify(Func<BaseMessage> messageCompose)
        {
            var messageProvider = ServiceLocator.Current.Resolve<IMessageCenterProvider>();
            if (messageProvider != null)
            {
               return  messageProvider.GetSender().SendMessageReliable(messageCompose());
            }
            return true;
        }
        public static void SafeNotifyAsync(Func<BaseMessage> messageCompose)
        {
            Task.Factory.StartNew(() =>
            {
                SafeNotify(messageCompose);
            });
        }
    }
}
