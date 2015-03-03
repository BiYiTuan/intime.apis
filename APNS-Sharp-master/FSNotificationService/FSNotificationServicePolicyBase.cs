using JdSoft.Apple.Apns.Notifications;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Apns.FSNotificationService
{
    abstract class FSNotificationServicePolicyBase : INotificationPolicy
    {
        private Timer _runner;
        private NotificationService _apnsService;
        protected YintaiHangzhouContext _dbContext;
        protected abstract TimeSpan ScheduleSpan { get; }
        public NotificationService ApnsService
        {
            get
            {
                return _apnsService;
            }
            set
            {
                _apnsService = value;
                if (_apnsService != null)
                    bindServiceHandler();
            }
        }

        public FSNotificationServicePolicyBase():this(null) { }
        public FSNotificationServicePolicyBase(NotificationService apnsService)
        {
            if (apnsService!= null)
                ApnsService = apnsService;
            _dbContext = new YintaiHangzhouContext();
        }
        private void bindServiceHandler()
        { 
            ApnsService.Error += new NotificationService.OnError(service_Error);
            ApnsService.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

            ApnsService.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
            ApnsService.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
            ApnsService.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
            ApnsService.Connecting += new NotificationService.OnConnecting(service_Connecting);
            ApnsService.Connected += new NotificationService.OnConnected(service_Connected);
            ApnsService.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);
        }
        public void Execute() {
            ensureRunner();
            if (ScheduleSpan == TimeSpan.MinValue)
                return;
            _runner.Change(TimeSpan.Zero, ScheduleSpan);
        }

        private void ensureRunner()
        {
            if (_runner == null)
                _runner = new Timer((object o) => { ((FSNotificationServicePolicyBase)o).InternalExecute();}
                    ,this, Timeout.Infinite, Timeout.Infinite);
            else
                _runner.Change(Timeout.Infinite, Timeout.Infinite);
        }

       protected abstract void InternalExecute();

        public void Dispose()
        {
            ApnsService.Dispose();
            if (_runner!= null)
             _runner.Dispose();
        }
        static void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            Console.WriteLine("Bad Device Token: {0}", ex.Message);
        }

        static void service_Disconnected(object sender)
        {
            Console.WriteLine("Disconnected...");
        }

        static void service_Connected(object sender)
        {
            Console.WriteLine("Connected...");
        }

        static void service_Connecting(object sender)
        {
            Console.WriteLine("Connecting...");
        }

        static void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            Console.WriteLine(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));
        }

       protected virtual void service_NotificationSuccess(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Success: {0}", notification.ToString()));
        }

        static void service_NotificationFailed(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Failed: {0}", notification.ToString()));
        }

        static void service_Error(object sender, Exception ex)
        {
            Console.WriteLine(string.Format("Error: {0}", ex.Message));
        }
    }
}
