using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Wxpay
{
    public class WxHtmlPayUrl
    {
        public string OrderNo { get; set; }
        public decimal TotalFee { get; set; }
        public string ClientIp { get; set; }
        public string ReturnUrl { get; set; }

        public string PayUrl
        {
            get
            {
                var dic = new Dictionary<string, dynamic>();
                dic.Add("input_charset", "UTF-8");
                dic.Add("bank_type", "WX");
                dic.Add("body", "银泰微信支付");
                dic.Add("return_url", ReturnUrl);
                dic.Add("notify_url", WxPayConfig.PAY4HTML_NOTIFY_URL);
                dic.Add("partner", WxPayConfig.HTML_PARTER_ID);
                dic.Add("out_trade_no", OrderNo);
                dic.Add("total_fee", Util.Feng4Decimal(TotalFee));
                dic.Add("fee_type", 1);
                dic.Add("spbill_create_ip", ClientIp);

                var signingStr = dic.OrderBy(b => b.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, b.Value), s => s.ToString()).TrimEnd('&');
                signingStr = string.Format("{0}&key={1}", signingStr, WxPayConfig.HTML_PARTER_KEY);
                var signedStr = Util.MD5_Encode(signingStr).ToUpper();
                var resultStr = dic.OrderBy(b => b.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, Util.UrlEncode(b.Value.ToString())), s => s.ToString()).TrimEnd('&');
                return string.Format("{0}?{1}&sign={2}", WxPayConfig.PAY4HTML_TOKEN_URL, resultStr, signedStr);
            }
        }
    }
}
