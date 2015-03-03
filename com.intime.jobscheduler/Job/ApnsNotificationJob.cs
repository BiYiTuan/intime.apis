using Common.Logging;
using Newtonsoft.Json;
using PushSharp;
using PushSharp.Apple;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    class ApnsNotificationJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap data = context.JobDetail.JobDataMap;
            var certFile = data.GetString("p12");
            var certPwd = data.GetString("password");
            //Create our push services broker
            var push = new PushBroker();

            //Wire up the events for all the services that the broker registers
            push.OnNotificationSent += NotificationSent;
            push.OnChannelException += ChannelException;
            push.OnServiceException += ServiceException;
            push.OnNotificationFailed += NotificationFailed;

            //configure certificate
            var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certFile));
            push.RegisterAppleService(new ApplePushChannelSettings(appleCert, certPwd));
           
            QueueNotification(push,context);
            push.StopAllServices();
        }

        protected virtual void QueueNotification(PushBroker push,IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(typeof(ApnsNotificationJob));
            int cursor = 0;
           int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var prods = (from p in db.Promotions
                            where (p.StartDate >= startDate && p.StartDate < endDate)
                            && p.Status == 1
                             select p).OrderByDescending(p=>p.IsTop).ThenByDescending(p=>p.CreatedDate).FirstOrDefault();
                if (prods != null)
                {
                    var devices = (from d in db.DeviceLogs
                                   where d.Status == 1

                                   select new {DeviceToken = d.DeviceToken }).Distinct();

                    int totalCount = devices.Count();
                    while (cursor < totalCount)
                    {
                        var pageDevices = devices.OrderBy(o=>o.DeviceToken).Skip(cursor).Take(size);
                        foreach (var device in pageDevices)
                        {
                            push.QueueNotification(new AppleNotification()
                                               .ForDeviceToken(device.DeviceToken)
                                               .WithAlert(prods.Name)
                                               .WithBadge(1)
                                               .WithCustomItem("from",JsonConvert.SerializeObject(new {targettype=(int)PushSourceType.Promotion,targetvalue=prods.Id.ToString()}))
                                               .WithSound("sound.caf"));
                            successCount++;
                        }

                        cursor += size;
                    }
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} notifications in {1} => {2} notis/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }

       

        private void NotificationFailed(object sender, PushSharp.Core.INotification notification, Exception error)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            log.Error(error);
        }

        private void ServiceException(object sender, Exception error)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            log.Error(error);
        }

        private void ChannelException(object sender, PushSharp.Core.IPushChannel pushChannel, Exception error)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            log.Error(error);
        }

        private void NotificationSent(object sender, PushSharp.Core.INotification notification)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            log.Info("sent:"+notification);
        }
    }
}
