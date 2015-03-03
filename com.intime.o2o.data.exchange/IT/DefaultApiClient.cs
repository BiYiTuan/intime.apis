using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;


namespace com.intime.o2o.data.exchange.IT
{
    /// <summary>
    /// 默认接口实现
    /// </summary>
    public class DefaultApiClient : IApiClient
    {
        private readonly Uri _baseAddress;
        private readonly string _privateKey;
        private readonly string _from;
        private readonly Random _random = new Random();
        private readonly JsonMediaTypeFormatter _mediaTypeFormatter;


        public DefaultApiClient()
            : this(ConfigurationManager.AppSettings["intime.o2o.api.url"], ConfigurationManager.AppSettings["intime.o2o.api.key"], ConfigurationManager.AppSettings["intime.o2o.api.from"])
        {

        }

        public DefaultApiClient(string baseAddress, string privateKey, string from)
        {
            _privateKey = privateKey;
            _baseAddress = new Uri(baseAddress);
            _from = from;
            _mediaTypeFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }
            };
        }


        public TResponse Post<TRequest, TResponse>(Request<TRequest, TResponse> request)
        {
            var resourceUri = request.GetResourceUri();

            using (var client = GetHttpClient())
            {
                request.From = _from;
                request.Nonce = _random.Next(0, 99);
                request.Timestamp = DateTime.Now.ToString("s");

                //计算签名
                request.Sign = Sign(request);
                var result = client.PostAsync(resourceUri, request, _mediaTypeFormatter).Result;

                if (result.IsSuccessStatusCode)
                {
                    var rst = result.Content.ReadAsStringAsync().Result;
                    return result.Content.ReadAsAsync<TResponse>().Result;
                }
            }

            return default(TResponse);
        }

        private HttpClient GetHttpClient()
        {
            return new HttpClient { BaseAddress = _baseAddress };
        }

        /// <summary>
        /// 计算签名
        /// </summary>
        /// <typeparam name="TRequest">请求对象</typeparam>
        /// <typeparam name="TResponse">响应对象</typeparam>
        /// <param name="request">请求实体</param>
        /// <returns>具体的签名</returns>
        private string Sign<TRequest, TResponse>(Request<TRequest, TResponse> request)
        {
            // 参数列表
            var list = new List<string>(3)
            {
                request.From,
                request.Nonce.ToString(CultureInfo.InvariantCulture),
                request.Timestamp
            };

            // 参数排序
            list.Sort();

            var builder = new StringBuilder();
            builder.Append(string.Join("", list));

            // 进行数据的串行化
            if (request.Data != null)
            {
                var data = GetDataString(request.Data);
                builder.Append(data);
            }

            return ComputeHash(_privateKey, builder.ToString());
        }

        private string GetDataString<T>(T data)
        {
            using (var content = new ObjectContent(typeof(T), data, _mediaTypeFormatter))
            {
                return content.ReadAsStringAsync().Result;
            }
        }

        private string ComputeHash(string privateKey, string message)
        {
            var key = Encoding.UTF8.GetBytes(privateKey);

            using (var hmac = new HMACSHA1(key))
            {
                hmac.Initialize();
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return Convert.ToBase64String(hash);
            }
        }
    }
}