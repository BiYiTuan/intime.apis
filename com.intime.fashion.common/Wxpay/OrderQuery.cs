using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Wxpay
{
    public class OrderQuery
    {
        [JsonProperty("appid")]
        public string AppId { get; set; }
         [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }
         [JsonProperty("sign_method")]
        public string SignMethod { get; set; }
        [JsonProperty("package")]
         public string PackageStr { get { return Package.Encode; } }
        [JsonIgnore]
        public OrderQueryPackage Package { get; set; }
        [JsonProperty("app_signature")]
        public string AppSignature { get {
            var dic = new Dictionary<string, dynamic>();
            dic.Add("appid", AppId);
            dic.Add("appkey", WxPayConfig.PARTER_SIGN_KEY);
            dic.Add("package", Package.Encode);
            dic.Add("timestamp", TimeStamp);
            return Util.PaySign(dic);
        } }
    }
    public class OrderQueryPackage
    {
        public string TradeNo { get; set; }
        public string Encode { get {
            var values = new Dictionary<string, string>();

            values.Add("partner", WxPayConfig.PARTER_ID);
            values.Add("out_trade_no", TradeNo);
           
            var signingStr = values.OrderBy(b => b.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, b.Value), s => s.ToString()).TrimEnd('&');
            signingStr = string.Format("{0}&key={1}", signingStr, WxPayConfig.PARTER_KEY);
            var signedStr = Util.MD5_Encode(signingStr).ToUpper();
            var resultStr = values.OrderBy(b => b.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key,b.Value), s => s.ToString()).TrimEnd('&');
            return string.Format("{0}&sign={1}", resultStr, signedStr);

        } }
    }
}
