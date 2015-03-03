using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Wxpay
{
   public class WxAppPayTokenResponse
    {

       public string partnerid { get {
           return WxPayConfig.APP_PARTER_ID;
       } }

        public string prepayid { get; set; }

        public string noncestr { get; set; }

        public long timestamp { get; set; }

        public string package { get; set; }

        public string sign { get {
            var preSigned = new Dictionary<string, dynamic>();
            preSigned.Add("appid", WxPayConfig.APP_APPID);
            preSigned.Add("appkey", WxPayConfig.APP_PARTER_SIGN_KEY);
            preSigned.Add("noncestr", noncestr);
            preSigned.Add("package", package);
            preSigned.Add("partnerid", partnerid);
            preSigned.Add("prepayid", prepayid);
            preSigned.Add("timestamp", timestamp);
            return Util.PaySign(preSigned);
            
        } }
    }
}
