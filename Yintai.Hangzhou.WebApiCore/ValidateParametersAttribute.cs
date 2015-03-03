using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Hangzhou.WebApiCore
{
    public class ValidateParametersAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DoValidate(filterContext);
        }

        private void DoValidate(ActionExecutingContext filterContext)
        {
            var log = ServiceLocator.Current.Resolve<ILog>();
            var request = filterContext.HttpContext.Request;
            var nonce = request["nonce"];
            var sign = request["sign"];
            var channel = request["from"];
            var timestamp = request["ts"];
            var data = request["data"];

            if (string.IsNullOrEmpty(nonce))
                throw new ArgumentException("nonce not correct");
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentException("from not correct");
            if (string.IsNullOrEmpty(timestamp))
                throw new ArgumentException("ts not correct");
            if (string.IsNullOrEmpty(sign))
                throw new ArgumentException("sign not correct");
            var privateKey = ParameterManager.FindKeyByChannel(channel);
            if (string.IsNullOrEmpty(privateKey))
                throw new ArgumentException("from no private key");
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("no data parameter");
            }
            long tslong;

            if (!long.TryParse(timestamp, out tslong))
                throw new ArgumentException("ts not correct format,should be seconds from 1001");
//#if DEBUG
//            var validDate = new DateTime(tslong * (long)Math.Pow(10, 7), DateTimeKind.Utc).ToLocalTime();
//            if (validDate > DateTime.Now.AddMinutes(5) || validDate < DateTime.Now.AddMinutes(-5))
//                throw new ArgumentException("ts expired");
//#endif
            var rawSignings = new List<string> { nonce, channel, timestamp, data };
            rawSignings.Sort(StringComparer.Ordinal);
            var rawSigning = rawSignings.Aggregate(new StringBuilder(), (s, e) => s.Append(e), s => s.ToString());
            string signedValue;
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(privateKey)))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawSigning));
                signedValue = Convert.ToBase64String(hashValue);
            }
            if (String.CompareOrdinal(sign, signedValue) != 0)
            {
                log.Debug(string.Format("input sign:{0} vs correct sign:{1}", sign, signedValue));
                throw new ArgumentException("sign not match");
            }
            try
            {
                filterContext.ActionParameters["request"] = JsonConvert.DeserializeObject<dynamic>(data);
            }
            catch (Exception)
            {
                throw new ArgumentException("data format must be json,please check it ");
            }
            filterContext.ActionParameters[Define.Channel] = channel;
        }
    }
}
