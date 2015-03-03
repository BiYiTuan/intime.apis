using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Hangzhou.WebApiCore
{
    public class Authorize2FilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            doAuthorize(filterContext);
        }

        private void doAuthorize(ActionExecutingContext filterContext)
        {
            var log = ServiceLocator.Current.Resolve<ILog>();
            var request = filterContext.HttpContext.Request;
            var nonce = request["nonce"];
            var sign = request["sign"];
            var channel = request["from"];
            var timestamp = request["ts"];
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
            long tslong;

            if (!long.TryParse(timestamp,out tslong))
                throw new ArgumentException("ts not correct format,should be seconds from 1001");
#if DEBUG
            var validDate = new DateTime(tslong * (long)Math.Pow(10,7),DateTimeKind.Utc).ToLocalTime();
            if (validDate > DateTime.Now.AddMinutes(5) || validDate < DateTime.Now.AddMinutes(-5))
                throw new ArgumentException("ts expired");
#endif
            var rawSignings = new List<string>() { nonce, channel, timestamp };
            rawSignings.Sort();
            var rawSigning = rawSignings.Aggregate(new StringBuilder(), (s, e) => s.Append(e), s => s.ToString());
            var signedValue = string.Empty;
            using (HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(privateKey)))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawSigning));
                signedValue = Convert.ToBase64String(hashValue);
            }
            if (string.Compare(sign, signedValue, false) != 0)
            {
                log.Debug(string.Format("input sign:{0} vs correct sign:{1}", sign, signedValue));
                throw new ArgumentException("sign not match");
            }
            else {
                filterContext.ActionParameters[Define.Channel] = channel;
            }

        }
    }
}
