using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.fashion.common;
using com.intime.fashion.common.config;
using com.intime.fashion.common.message;
using com.intime.fashion.common.Wxpay;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.service.messages.Message
{
    public class DaogouApplyApprovedHandler : MessageHandler
    {
        private DbContext _db = null;
        private static readonly Weixin_IMSConfiguration imsConfiguration = CommonConfiguration<Weixin_IMSConfiguration>.Current;
        public DaogouApplyApprovedHandler()
        {
            _db = ServiceLocator.Current.Resolve<DbContext>();
        }

        public override int SourceType
        {
            get { return (int)MessageSourceType.DaogouApply; }
        }

        public override int ActionType
        {
            get { return (int)MessageAction.Approved; }
        }

        public override bool Work(BaseMessage message)
        {
            if (message == null)
            {
                return false;
            }

            var requestWithUser =
                _db.Set<IMS_InviteCodeRequestEntity>()
                    .Where(x => x.Id == message.EntityId)
                    .Join(_db.Set<IMS_AssociateEntity>(), x => x.UserId, a => a.UserId, (x, a) => new { x, a })
                    .Join(_db.Set<OutsiteUserEntity>().Where(ou => ou.OutsiteType == (int)OutsiteType.WX),
                        o => o.x.UserId, i => i.AssociateUserId,
                        (q, i) => new { Request = q.x, Associate = q.a, User = i })
                    .FirstOrDefault();

            if (requestWithUser == null)
            {
                return false;
            }

            var user = requestWithUser.User;
            var request = requestWithUser.Request;
            var associate = requestWithUser.Associate;
            var storeUrl = string.Format("{0}{1}{2}{3}", ConfigManager.AwsHost, "ims/store/stores/", associate.Id,
                "/my/");
            bool sent;
            var store = _db.Set<StoreEntity>().FirstOrDefault(x => x.Id == request.StoreId);
            var department = _db.Set<DepartmentEntity>().FirstOrDefault(x => x.Id == request.DepartmentId);

            if (store == null || department == null)
            {
                return false;
            }

            if (request.Approved.HasValue && request.Approved.Value)
            {
                sent = WxServiceHelper.SendMessage(new
                {
                    touser = user.OutsiteUserId,
                    template_id = imsConfiguration.StoreApply_Template_Id,
                    url = storeUrl,
                    topcolor = "#FF0000",
                    data = new
                    {

                        first = new { value = "您的开店申请已审核通过,20分钟后生效,请耐心等候。" },
                        cardNumber = new { value = associate.Id, color = "#173177" },
                        type = new { value = "迷你银导购", color = "#173177" },
                        address = new { value = string.Format("{0}-{1}", store.Name, department.Name), color = "#173177" },
                        VIPName = new { value = requestWithUser.Request.Name, color = "#173177" },
                        VIPPhone = new { value = requestWithUser.Request.ContactMobile, color = "#173177" },
                        expDate = new { value = "长期有效", color = "#173177" },
                        remark = new { value = "点击进入您的店铺", color = "#173177" }
                    }
                }, AccessTokenType.MiniYin, null, null);
                request.ApprovedNotificationTimes = request.ApprovedNotificationTimes.HasValue
                    ? request.ApprovedNotificationTimes.Value + 1
                    : 1;
            }
            else
            {
                sent = WxServiceHelper.SendMessage(new
                {
                    touser = user.OutsiteUserId,
                    template_id = imsConfiguration.StoreApply_Template_Id,
                    url = storeUrl,
                    topcolor = "#FF0000",
                    data = new
                    {
                        first = new { value = "您的店铺已降级为普通店铺" },
                        VIPName = new { value = requestWithUser.Request.Name, color = "#173177" },
                        VIPPhone = new { value = requestWithUser.Request.ContactMobile, color = "#173177" },
                        remark = new { value = "点击进入您的店铺", color = "#173177" }
                    }
                }, AccessTokenType.MiniYin, null, null);
                request.ApprovedNotificationTimes = request.ApprovedNotificationTimes.HasValue
                    ? request.ApprovedNotificationTimes.Value + 1
                    : 1;
                request.DemotionNotificationTimes = request.DemotionNotificationTimes.HasValue
                    ? request.DemotionNotificationTimes.Value + 1
                    : 1;
            }

            if (!sent) return false;
            _db.Entry(request).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
    }
}
