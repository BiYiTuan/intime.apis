using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace com.intime.fashion.common.Wxpay
{
    public static class Util
    {
        public static string Nonce()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 32);
        }


        public static string PaySign(Dictionary<string, dynamic> kvalues)
        {
            var toSignStr = kvalues.OrderBy(b => b.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, b.Value), s => s.ToString());
            return SHA(toSignStr.TrimEnd('&'));
        }


        public static string SHA(string value)
       {
         //  CommonUtil.Log.Debug(string.Format("sha1:{0}", value));
           byte[] hashData = SHA1.Create().ComputeHash((Encoding.UTF8.GetBytes(value)));
           var hashText = new StringBuilder();
           foreach (byte b in hashData)
           {
               hashText.Append(b.ToString("x2"));
              
           }
           return hashText.ToString();
       }

        public static string MD5_Encode(string value)
        {

            byte[] hashData = MD5.Create().ComputeHash((Encoding.UTF8.GetBytes(value)));
            var hashText = new StringBuilder();
            foreach (byte b in hashData)
            {
                hashText.Append(b.ToString("x2"));
            }
            return hashText.ToString();
        }

        public static long Feng4Decimal(decimal value)
        {
            return (long)decimal.Multiply(value, 100);
        }

        public static string NotifySign(Dictionary<string, string> sPara)
        {
            var signingStr = sPara.OrderBy(s=>s.Key).Aggregate(new StringBuilder(),(s,b)=>s.AppendFormat("{0}={1}&",b.Key,b.Value),s=>s.ToString().TrimEnd('&'));
            signingStr = string.Format("{0}&key={1}",signingStr,WxPayConfig.PARTER_KEY);
            return MD5_Encode(signingStr).ToUpper();
        }

        public static string NotifySignApp(Dictionary<string, string> sPara) {
            var signingStr = sPara.OrderBy(s => s.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, b.Value), s => s.ToString().TrimEnd('&'));
            signingStr = string.Format("{0}&key={1}", signingStr, WxPayConfig.APP_PARTER_KEY);
            return MD5_Encode(signingStr).ToUpper();
        }

        public static string NotifySignHtml(Dictionary<string, string> sPara)
        {
            var signingStr = sPara.OrderBy(s => s.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, b.Value), s => s.ToString().TrimEnd('&'));
            signingStr = string.Format("{0}&key={1}", signingStr, WxPayConfig.HTML_PARTER_KEY);
            return MD5_Encode(signingStr).ToUpper();
        }

        public static string NotifySignIMS(Dictionary<string, string> sPara)
        {
            var signingStr = sPara.OrderBy(s => s.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, b.Value), s => s.ToString().TrimEnd('&'));
            signingStr = string.Format("{0}&key={1}", signingStr, WxPayConfig.IMS_PARTER_KEY);
            return MD5_Encode(signingStr).ToUpper();
        }
        public static bool CheckNotifySign(Dictionary<string, string> sPara, string parterKey, string signedKey)
        {
            var signingStr = sPara.OrderBy(s => s.Key).Aggregate(new StringBuilder(), (s, b) => s.AppendFormat("{0}={1}&", b.Key, b.Value), s => s.ToString().TrimEnd('&'));
            signingStr = string.Format("{0}&key={1}", signingStr, parterKey);
            var signedKeyNow = MD5_Encode(signingStr).ToUpper();
            return string.Compare(signedKey, signedKeyNow, true) == 0;
        }
        public static string UrlEncode(string value)
        {
            if (value == null)
                return null;
            return HttpUtility.UrlEncode(value).Replace("+", "%20");
        }

        public static string ClientIp(HttpRequestBase request)
        { 
            string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (ipAddress == null || ipAddress.ToLower() == "unknown")
                ipAddress = request.ServerVariables["REMOTE_ADDR"];

            return ipAddress;
        }

    }


}
