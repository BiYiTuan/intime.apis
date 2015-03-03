using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace com.intime.fashion.data.sync.Wgw
{
    public class SyncClient
    {
        //private readonly string baseUrl;
        private readonly string _secretOAuthKey;
        private readonly WebUtils _utils;

        public SyncClient():this(WgwConfigHelper.WGW_API_BASE_URL,WgwConfigHelper.WGW_API_SECRET_OAUTH_KEY)
        {
            
        }

        /// <summary>
        ///     微购物开放平台基地址
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="secretOAuthKey"></param>
        public SyncClient(string baseUrl, string secretOAuthKey)
        {
            //this.baseUrl = baseUrl.TrimEnd('/', '\\');
            this._utils = new WebUtils();
            this._secretOAuthKey = string.Format("{0}&", secretOAuthKey);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public T Execute<T>(ISyncRequest request) where T:class
        {
            var response = this._utils.DoPost(request.Url, ConstructParameters(request),request.Attachments);
            return JsonConvert.DeserializeObject<T>(response);
        }

        private IDictionary<string,string> ConstructParameters(ISyncRequest request)
        {
            var encodedUri = this._utils.UrlEncode(request.Resource, Encoding.UTF8);
            var requestPair = this._utils.BuildQuery(request.RequestParams, false);
            var encodedParams = this._utils.UrlEncode(requestPair, Encoding.UTF8);

            string signedStringParameters = SignAndToBase64String(string.Format("{0}&{1}&{2}", 
                request.Method.ToUpper(),
                encodedUri,
                encodedParams)
                );

            request.Put("sign", signedStringParameters);
            return request.RequestParams;
        }

        private string SignAndToBase64String(string strToSign)
        {
            using (var hmac = new HMACSHA1(Encoding.Default.GetBytes(_secretOAuthKey)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(strToSign));
                return Convert.ToBase64String(hashValue);
            }
        }
    }
}