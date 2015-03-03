using com.intime.o2o.data.exchange.IT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace com.intime.o2o.data.exchange.Ims
{
    public class ImsApiClient : IApiClient
    {
        private string _serviceUrl;
        private string _appKey;
        private string _appsecret;
        public ImsApiClient(string serviceUrl, string appkey, string appsecret)
        {
            _serviceUrl = serviceUrl;
            _appKey = appkey;
            _appsecret = appsecret;
        }

        public TResponse Post<TRequest, TResponse>(Request<TRequest, TResponse> request)
        {
            var url = string.Format("{0}/{1}", _serviceUrl, request.GetResourceUri());
            var requestbody = JsonConvert.SerializeObject(request.Data);
            string requestUrl = ConstructHttpRequestUrl(url, _appKey, _appsecret, requestbody);
            var client = WebRequest.CreateHttp(requestUrl);
            client.ContentType = "application/json";
            client.Method = "Post";
            using (var req = client.GetRequestStream())
            using (var streamWriter = new StreamWriter(req))
            {
                streamWriter.Write(requestbody);
            }
            var sb = new StringBuilder();
            using (var response = client.GetResponse())
            {
                var body = response.GetResponseStream();
                using (var streamReader = new StreamReader(body, Encoding.UTF8))
                {

                    sb.Append(streamReader.ReadToEnd());

                }
            }

            return JsonConvert.DeserializeObject<TResponse>(sb.ToString());
        }

        private static string ConstructHttpRequestUrl(string host, string publickey, string privatekey, string data)
        {
            var query = new Dictionary<string, string>
            {
                {"from", publickey},
                {"nonce", new Random(1000).Next().ToString()},
                {"ts", DateTime.Now.Ticks.ToString()},
                {"data", data}
            };
            var signingValue = new StringBuilder();
            string signedValue;

            var valuesToSign = query.Values.ToList();
            valuesToSign.Sort(StringComparer.Ordinal);

            foreach (var s in valuesToSign)
                signingValue.Append(s);
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(privatekey)))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(signingValue.ToString()));
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
