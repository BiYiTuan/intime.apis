using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Apns.FSNotificationService
{
    class FSNotificationPolicyBuilder
    {
        private JdSoft.Apple.Apns.Notifications.NotificationService _apnService;

        public FSNotificationPolicyBuilder(JdSoft.Apple.Apns.Notifications.NotificationService _apnService)
        {
            this._apnService = _apnService;
        }
        internal IEnumerable<INotificationPolicy> Build()
        {
            FSNotificationServicePolicyBase policy = new FSNotificationPolicyOfProNew();
            policy.ApnsService = _apnService;
            yield return policy;
        }
    }
}
