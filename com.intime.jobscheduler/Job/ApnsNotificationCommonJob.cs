using log4net;
using Newtonsoft.Json;
using PushSharp;
using PushSharp.Apple;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    class ApnsNotificationCommonJob:ApnsNotificationJob
    {
     
        protected override void QueueNotification(PushBroker push,IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(typeof(ApnsNotificationCommonJob));
            JobDataMap data = context.JobDetail.JobDataMap;
            var interval = data.GetIntValue("intervalofsec");
            int cursor = 0;
           int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            var benchDate = DateTime.Now.AddSeconds(-interval);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var prods = (from p in db.NotificationLogs
                             where p.CreateDate>= benchDate &&
                                    p.Status==(int)NotificationStatus.Default
                             select p);
                if (prods != null)
                {
                   

                    int totalCount = prods.Count();
                    while (cursor < totalCount)
                    {
                        var linq = prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size).ToList();
                        foreach (var l in linq)
                        {
                            try
                            {
                                if (l.SourceType.Value == (int)SourceType.PMessage)
                                {
                                    var device = db.PMessages.Where(p => p.Id == l.SourceId)
                                                .Join(db.DeviceLogs, o => o.ToUser, i => i.User_Id, (o, i) => new { 
                                                    D = i,U=o
                                                }).FirstOrDefault();
                                    if (device==null)
                                        continue;
                                        push.QueueNotification(new AppleNotification()
                                               .ForDeviceToken(device.D.DeviceToken)
                                               .WithAlert("新私信...")
                                               .WithBadge(1)
                                               .WithCustomItem("from", JsonConvert.SerializeObject(new { targettype = (int)PushSourceType.PMessage, targetvalue =device.U.FromUser}))
                                               .WithSound("sound.caf"));
                                        successCount++;
                                    
                                    l.NotifyDate = DateTime.Now;
                                    l.Status = (int)NotificationStatus.Notified;
                                    db.Entry(l).State = System.Data.EntityState.Modified;
                                    db.SaveChanges();
                                } 
                                else if (l.SourceType.Value == (int)SourceType.Comment)
                                {
                                    // if it's comment, always notify all comments owners of this item
                                    var comment = db.Comments.Find(l.SourceId);
                                    if (comment == null)
                                        continue;
                                    var relatedComments = db.Comments.Where(c => c.SourceType == comment.SourceType
                                                                    && c.SourceId == comment.SourceId
                                                                    && c.User_Id != comment.User_Id)
                                                           .Join(db.DeviceLogs.Where(d => d.User_Id > 0),
                                                                o => o.User_Id,
                                                                i => i.User_Id,
                                                                (o, i) => new { Token = i.DeviceToken }).ToList();
                                    if (comment.SourceType == (int)SourceType.Product)
                                    {
                                        var product = db.Products.Where(p => p.Id == comment.SourceId && p.RecommendUser != comment.User_Id)
                                                        .Join(db.DeviceLogs.Where(d => d.User_Id > 0),
                                                                o => o.CreatedUser,
                                                                i => i.User_Id,
                                                                (o, i) => new { Token = i.DeviceToken }).FirstOrDefault();
                                        if (product != null)
                                            relatedComments.Add(new { Token = product.Token });
                                    }
                                    else if (comment.SourceType == (int)SourceType.Promotion)
                                    {
                                        var promotion = db.Promotions.Where(p => p.Id == comment.SourceId && p.RecommendUser != comment.User_Id)
                                                        .Join(db.DeviceLogs.Where(d => d.User_Id > 0),
                                                                o => o.CreatedUser,
                                                                i => i.User_Id,
                                                                (o, i) => new { Token = i.DeviceToken }).FirstOrDefault();
                                        if (promotion != null)
                                            relatedComments.Add(new { Token = promotion.Token });
                                    }
                                    foreach (var device in relatedComments.Distinct())
                                    {
                                        push.QueueNotification(new AppleNotification()
                                               .ForDeviceToken(device.Token)
                                               .WithAlert("新评论...")
                                               .WithBadge(1)
                                               .WithCustomItem("from", JsonConvert.SerializeObject(new { targettype = (int)PushSourceType.SelfComment, targetvalue = comment.SourceId }))
                                               .WithSound("sound.caf"));
                                        successCount++;
                                    }
                                    l.NotifyDate = DateTime.Now;
                                    l.Status = (int)NotificationStatus.Notified;
                                    db.Entry(l).State = System.Data.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Info(ex);
                            }
                        }

                        cursor += size;
                    }
                  
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} notifications in {1} => {2} notis/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }
    }
}
