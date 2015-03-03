using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JdSoft.Apple.Apns.Notifications;
namespace Yintai.Hangzhou.Apns.FSNotificationService
{
    class FSNotificationManager
    {
        private static FSNotificationManager _instance;
        private static object lockObject = new object();

        private NotificationService _apnService;
        private bool _inRunning;
        public static FSNotificationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject) { 
                        _instance =_instance??new FSNotificationManager();
                    }
                }
                return _instance;
            }
        }
        private FSNotificationManager()
        {
            _apnService = new NotificationService(NotificationSetting.CurrentElement.IsSandBox
                ,NotificationSetting.CurrentElement.P12File
                ,NotificationSetting.CurrentElement.P12Pass
                ,NotificationSetting.Current.Concurrent);
        }
        internal void Start()
        {
            if (_inRunning)
                return;
            _inRunning = true;
            doStart();
        }

        private void doStart()
        {
            FSNotificationPolicyBuilder builder = new FSNotificationPolicyBuilder(_apnService);
            foreach (INotificationPolicy policy in builder.Build())
            {
                policy.Execute();
            }
        }

        internal void Close()
        {
            if (!_inRunning)
                return;
            _apnService.Close();
        }

        internal void Stop()
        {
            throw new NotImplementedException();
        }

        internal void Resume()
        {
            throw new NotImplementedException();
        }

    }
}
