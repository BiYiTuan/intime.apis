using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Apns.FSNotificationService
{
    /// <summary>
    /// this policy will behave as this:
    /// 1. retrieve the about coming promotion of tomorrow
    /// 2. compose the notification message body: promotion body and duration
    /// 3. send this message to all match device
    /// </summary>
    class FSNotificationPolicyOfProNew:FSNotificationServicePolicyBase
    {
        protected override void InternalExecute()
        {
            
            DateTime notifyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime filterDate = notifyDate.AddDays(1);
            var newPros = (from pro in _dbContext.Promotions
                       where pro.StartDate >= filterDate
                       orderby pro.UpdatedDate descending
                       select pro).Take<PromotionEntity>(1);
            if (newPros == null)
                return;
            PromotionEntity pros = newPros.FirstOrDefault();
            pros.Description = pros.Description ?? string.Empty;
            pros.Description = pros.Description.Trim().Length < 1 ? NotificationSetting.Current.DefaultMessage : pros.Description.Trim();
            var devices = (from device in _dbContext.DeviceLogs
                           where !(from nf in _dbContext.NotificationLogs
                                  where nf.NotifyDate >= notifyDate
                                  select nf.DeviceToken).Contains( device.DeviceToken)
                               &&
                            device.Status == 1
                           orderby device.CreatedDate ascending
                           select device).Take(500);
            
            //var devices = new string[] { @"d4e0f45464ec2d0f8c9f6bb28efe36e2431e01d06c53c24800d81338f5191de8",
              //  @"a10c54e8827052b77cd377db98ef739617032710b9378d95ccb847e39bf2562a" };
            foreach (var device in devices)
            {
                ApnsService.QueueNotification(new JdSoft.Apple.Apns.Notifications.Notification()
                {
                    DeviceToken = device.DeviceToken,
                    Payload = new JdSoft.Apple.Apns.Notifications.NotificationPayload
                    {
                        Alert = new JdSoft.Apple.Apns.Notifications.NotificationAlert() { 
                             Body = pros.Description
                        },
                         Sound= "default",
                         Badge=1
                    }
                });
            }

        }
        protected override void service_NotificationSuccess(object sender, JdSoft.Apple.Apns.Notifications.Notification notification)
        {
            NotificationLogEntity insert = _dbContext.NotificationLogs.Create();
            insert.DeviceToken = notification.DeviceToken;
            insert.InDate = DateTime.Now;
            insert.Message = notification.Payload.Alert.Body;
            insert.NotifyDate = DateTime.Now;
            insert.Status = 1;
            _dbContext.NotificationLogs.Add(insert);
            _dbContext.SaveChanges();
        }
        protected override TimeSpan ScheduleSpan
        {
            get
            {
                return TimeSpan.FromMinutes(30);
            }
        }
    }
}
