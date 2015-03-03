using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Wxpay
{
    public class WxNotify
    {
        public string OpenId { get; set; }
        public string TransactionId { get; set; }
        public string OutTradeNo { get; set; }
        public string DeliverTS { get; set; }
        public string DeliverStatus { get; set; }
        public string DeliverMsg { get; set; }
        public dynamic EncodedRequest {
            get
            {
                var preSigned = new Dictionary<string, dynamic>();
                preSigned.Add("appid", WxPayConfig.APP_ID);
                preSigned.Add("appkey", WxPayConfig.PARTER_SIGN_KEY);
                preSigned.Add("openid", OpenId);
                preSigned.Add("transid", TransactionId);
                preSigned.Add("out_trade_no", OutTradeNo);
                preSigned.Add("deliver_timestamp", DeliverTS);
                preSigned.Add("deliver_status", DeliverStatus);
                preSigned.Add("deliver_msg", DeliverMsg);
                return new
                {
                    appid = WxPayConfig.APP_ID,
                    openid = OpenId,
                    transid = TransactionId,
                    out_trade_no = OutTradeNo,
                    deliver_timestamp = DeliverTS,
                    deliver_status = DeliverStatus,
                    deliver_msg = DeliverMsg,
                    app_signature = Util.PaySign(preSigned),
                    sign_method = "sha1"
                };

            }
        }
    }
}
