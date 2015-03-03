using com.intime.fashion.common.Weigou;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request
{
    public abstract class RequestBase : ISyncRequest
    {
        private const string Param_uin = "uin";
        private const string Param_accessToken = "accessToken";
        private const string Param_appOAuthID = "appOAuthID";
        private const string Param_timeStamp = "timeStamp";
        private const string Param_randomValue = "randomValue";
        private const string Param_format = "format";
        //private const string Param_version = "version";
        private const string Param_sellerUin = "sellerUin";
        private const string Param_subUin = "subUin";

        protected readonly IDictionary<string, string> requestParamDictionary;
        protected readonly IDictionary<string,FileItem> files;

        protected RequestBase()
        {
            files = new Dictionary<string,FileItem>();
            requestParamDictionary = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                {Param_timeStamp, GetTimeStamp()},
                {Param_randomValue, new Random(1000).Next().ToString("D")},
                {Param_uin, WgwConfigHelper.WGW_API_UIN},
                {Param_accessToken, WgwConfigHelper.WGW_API_ACCESS_TOKEN},
                {Param_appOAuthID, WgwConfigHelper.WGW_API_APPOAUTH_ID},
                {Param_format, WgwConfigHelper.WGW_API_FORMAT},
                //{Param_version, WeigouConfig.WGW_API_VERSION},
                {Param_sellerUin, WgwConfigHelper.WGW_API_SELLER_UIN},
                {Param_subUin, WgwConfigHelper.WGW_API_SUB_UIN},

            };
        }

        public virtual string Method
        {
            get { return "POST"; }
        }

        /// <summary>
        ///     请求资源相对地址
        /// </summary>
        public abstract string Resource { get; }

        public virtual string BaseUrl
        {
            get { return "http://api.weigou.qq.com"; }
        }

        /// <summary>
        /// Url
        /// </summary>
        public virtual string Url
        {
            get { return string.Format("{0}{1}", this.BaseUrl, this.Resource); }
        }

        /// <summary>
        ///     请求所需参数，包括系统参数及应用级别参数
        /// </summary>
        public IDictionary<string, string> RequestParams
        {
            get { return requestParamDictionary; }
        }

        /// <summary>
        ///     添加请求参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        private void AddRequestParameters(string paramName, string paramValue)
        {
            if (string.IsNullOrEmpty(paramName) || string.IsNullOrEmpty(paramValue))
            {
                return;
            }
            if (requestParamDictionary.ContainsKey(paramName))
            {
                requestParamDictionary[paramName] = paramValue;
            }
            else
            {
                requestParamDictionary.Add(paramName, paramValue);
            }
        }

        /// <summary>
        /// 附件等图片文件上传
        /// </summary>
        public IDictionary<string,FileItem> Attachments
        {
            get; protected set;
        }

        /// <summary>
        /// 添加请求参数,覆盖已有参数,值类型和字符串类型直接加入,其他类型json序列化后加入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(string key, object value)
        {
            if (value is string || value is ValueType)
            {
                this.AddRequestParameters(key, value.ToString());
                return;
            }
            this.AddRequestParameters(key, JsonConvert.SerializeObject(value));
        }

        public object Remove(string key)
        {
            if (!requestParamDictionary.ContainsKey(key)) return null;
            var obj = Get(key);
            requestParamDictionary.Remove(key);
            return obj;
        }

        private object Get(string key)
        {
            if (requestParamDictionary.ContainsKey(key))
            {
                return requestParamDictionary[key];
            }
            return null;
        }

        private string GetTimeStamp()
        {
            Int64 retval = 0;
            var st = new DateTime(1970, 1, 1);
            var t = (DateTime.Now.ToUniversalTime() - st);
            retval = (Int64) (t.TotalMilliseconds + 0.5);
            return retval.ToString();
        }
    }
}