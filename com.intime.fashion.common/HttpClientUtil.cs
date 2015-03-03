using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.intime.fashion.common
{
    public static class HttpClientUtil
    {
        public static bool SendHttpMessage(string url, object requestData, string publickey, string privatekey, Action<dynamic> successCallback, Action<dynamic> failCallback)
        {

            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("url is empty");
            if (string.IsNullOrEmpty(publickey) || string.IsNullOrEmpty(privatekey))
                throw new ArgumentException("private/public key is empty");

            var requestbody = JsonConvert.SerializeObject(new
            {
                data = requestData
            });
            string requestUrl = ConstructHttpRequestUrl(url, publickey, privatekey);
            var client = WebRequest.CreateHttp(requestUrl);
            client.ContentType = "application/json";
            client.Method = "Post";
            using (var request = client.GetRequestStream())
            using (var streamWriter = new StreamWriter(request))
            {
                streamWriter.Write(requestbody);
            }
            StringBuilder sb = new StringBuilder();
            using (var response = client.GetResponse())
            {
                var body = response.GetResponseStream();
                using (var streamReader = new StreamReader(body, Encoding.UTF8))
                {

                    sb.Append(streamReader.ReadToEnd());

                }
            }
            dynamic jsonResponse = JsonConvert.DeserializeObject(sb.ToString());

            if (jsonResponse != null &&
                (jsonResponse.isSuccessful == null || jsonResponse.isSuccessful == true))
            {
                if (successCallback != null)
                    successCallback(jsonResponse);
                return true;
            }
            else
            {
                if (failCallback != null)
                    failCallback(jsonResponse);
                return false;
            }


        }
        private static string ConstructHttpRequestUrl(string host, string publickey, string privatekey)
        {

            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("key", publickey);
            query.Add("nonce", new Random(1000).Next().ToString());
            query.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ"));
            var signingValue = new StringBuilder();
            var signedValue = string.Empty;
            foreach (var s in query.Values.ToArray().OrderBy(s => s))
                signingValue.Append(s);
            using (HMACSHA1 hmac = new HMACSHA1(Encoding.ASCII.GetBytes(privatekey)))
            {
                var hashValue = hmac.ComputeHash(Encoding.ASCII.GetBytes(signingValue.ToString()));
                signedValue = Convert.ToBase64String(hashValue);

            }
            query.Add("sign", signedValue);
            var requestUrl = new StringBuilder();
            requestUrl.Append(host);
            requestUrl.Append("?");
            return query.Keys.Aggregate(requestUrl, (s, e) => s.AppendFormat("&{0}={1}", e, HttpUtility.UrlEncode(query[e])), s => s.ToString());

        }
    }
}
