using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using com.intime.fashion.common.Erp;
using Newtonsoft.Json;

namespace com.intime.fashion.common.IT
{
    public static class ITServiceHelper
    {
        public static bool SendHttpMessage(Request request, Action<dynamic> successCallback, Action<dynamic> failCallback)
        {
            string requestUrl = ConstructHttpRequestUrl(request);
            var client = WebRequest.CreateHttp(requestUrl);
            client.ContentType = "application/json";
            client.Method = "Post";

            var sb = new StringBuilder();
            using (var response = client.GetResponse())
            {
                var body = response.GetResponseStream();
                using (var streamReader = new StreamReader(body, Encoding.UTF8))
                {
                    sb.Append(streamReader.ReadToEnd());
                }
            }
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(sb.ToString());

            if (jsonResponse != null &&
                jsonResponse.error == string.Empty)
            {
                if (successCallback != null)
                    successCallback(jsonResponse);
                return true;
            }
            if (failCallback != null)
                failCallback(jsonResponse);
            return false;
        }

        private static string ConstructHttpRequestUrl(Request request)
        {
            if (null == request)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrEmpty(request.Url))
            {
                throw new ArgumentException("Invalid request, url is empty");
            }

            string signedValue;

            using (var hmac = new HMACSHA1(Encoding.ASCII.GetBytes(ErpConfig.Private_KEY)))
            {
                var hashValue = hmac.ComputeHash(Encoding.ASCII.GetBytes(request.ValueToSign));
                signedValue = Convert.ToBase64String(hashValue);
            }

            var query = new Dictionary<string, string>
            {
                {"From", request.TimeStamp},
                {"Nonce", request.Nonce},
                {"Timestamp", request.TimeStamp},
                {"Sign", signedValue},
                {"Data",request.RequestParams}
            };


            var requestUrl = new StringBuilder();
            requestUrl.Append(request.Url);
            requestUrl.Append("?");
            return query.Keys.Aggregate(requestUrl, (s, e) => s.AppendFormat("{0}={1}&", e, HttpUtility.UrlEncode(query[e])), s => s.ToString().TrimEnd('&'));

        }

    }
}
