using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Wxpay
{
    public class WxNativeUrl
    {
        public string ProductId { get; set; }
        public string Encode()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("appid", WxPayConfig.APP_ID);
            dic.Add("productid", ProductId);
            dic.Add("timestamp", DateTime.Now.TicksOfWx().ToString());
            dic.Add("noncestr", Util.Nonce());
            dic.Add("appkey", WxPayConfig.PARTER_SIGN_KEY);
            var presign = dic.OrderBy(d => d.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, b.Value), s => s.ToString().TrimEnd('&'));
            var sign = Util.SHA(presign);
            return string.Format("weixin://wxpay/bizpayurl?sign={4}&appid={0}&productid={1}&timestamp={2}&noncestr={3}"
                        ,dic["appid"]
                        ,dic["productid"]
                        ,dic["timestamp"]
                        ,dic["noncestr"]
                        ,sign
                        );
        }
    }
}
