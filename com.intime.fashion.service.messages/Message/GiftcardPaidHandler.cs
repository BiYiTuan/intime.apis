using com.intime.fashion.common;
using com.intime.fashion.common.config;
using com.intime.fashion.common.message;
using com.intime.fashion.common.message.Messages;
using com.intime.fashion.common.Wxpay;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.service.messages.Message
{
   public class GiftcardPaidHandler : MessageHandler
    {
        private DbContext _db = null;
        public GiftcardPaidHandler()
        {
            _db = ServiceLocator.Current.Resolve<DbContext>();
        }
        public override int SourceType
        {
            get { return (int)MessageSourceType.GiftCard; }
        }

        public override int ActionType
        {
            get { return (int)(MessageAction.Paid); }
        }

        public override bool Work(BaseMessage message)
        {
                var paidMessage = message;
                if (paidMessage == null)
                    return false;
                var userEntity = _db.Set<OrderEntity>().Where(o => o.OrderNo == paidMessage.SourceNo)
                               .Join(_db.Set<OutsiteUserEntity>().Where(ou => ou.OutsiteType == (int)OutsiteType.WX), o => o.CustomerId, i => i.AssociateUserId, (o, i) => i)
                               .FirstOrDefault();
                var order_type = "礼品卡";
                var imsConfiguration = CommonConfiguration<Weixin_IMSConfiguration>.Current;
                if (userEntity != null)
                {
                    //step1: send paid message to user

                    var url_path = "ims/card_orders/list";
                    WxServiceHelper.SendMessage(new
                    {
                        touser = userEntity.OutsiteUserId,
                        template_id = imsConfiguration.Paid4User_Template_Id,
                        url = string.Format("{0}{1}", ConfigManager.AwsHost, url_path),
                        topcolor = "#FF0000",
                        data = new
                        {
                            title = new { value = "您的订单已支付", color = "#000000" },
                            storeName = new { value = "银泰商业", color = "#173177" },
                            orderId = new { value = paidMessage.SourceNo, color = "#173177" },
                            orderType = new { value = order_type, color = "#173177" },
                            remark = new { value = "感谢支持，点击进入查看订单详情", color = "#173177" }
                        }
                    }, AccessTokenType.MiniYin, null, null);
                }
                //step2: send income message to dagou
                var associateOrder = _db.Set<IMS_AssociateIncomeHistoryEntity>().Where(iai => iai.SourceNo == paidMessage.SourceNo 
                    && iai.SourceType == (int)AssociateOrderType.GiftCard).FirstOrDefault();
                if (associateOrder != null)
                {
                    var daogouEntity = _db.Set<OutsiteUserEntity>().Where(o => o.AssociateUserId == associateOrder.AssociateUserId)
                        .FirstOrDefault();
                    if (daogouEntity == null)
                        return true;
                    var daogou_url_path = "ims/store/stores/records";
                    WxServiceHelper.SendMessage(new
                    {
                        touser = daogouEntity.OutsiteUserId,
                        template_id = imsConfiguration.Paid4DaoGou_Template_Id,
                        url = string.Format("{0}{1}", ConfigManager.AwsHost, daogou_url_path),
                        topcolor = "#FF0000",
                        data = new
                        {
                            title = new { value = "您的店铺有订单已支付", color = "#000000" },
                            storeName = new { value = "银泰商业", color = "#173177" },
                            orderId = new { value = paidMessage.SourceNo, color = "#173177" },
                            orderType = new { value = order_type, color = "#173177" },
                            remark = new { value = "点击进入查看交易记录", color = "#173177" }
                        }
                    }, AccessTokenType.MiniYin, null, null);
                }
                return true;

        }
    }
}
