using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace com.intime.fashion.data.sync
{
    /// <summary>
    /// 网络工具类。
    /// </summary>
    public sealed class WebUtils
    {

        public static Regex REG_URL_ENCODING = new Regex(@"%[a-f0-9]{2}");

        private int _timeout = 100000;

        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest req = null;

            req = GetWebRequest(url, "POST");
            byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters));
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(postData, 0, postData.Length);
            }
            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = rsp.CharacterSet == null ? Encoding.UTF8 : Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }

            HttpWebRequest req = GetWebRequest(url, "GET");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = rsp.CharacterSet == null ? Encoding.UTF8 : Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行带文件上传的HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="fileParams">请求文件参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> textParams, IDictionary<string, FileItem> fileParams)
        {
            // 如果没有文件参数，则走普通POST请求
            if (fileParams == null || fileParams.Count == 0)
            {
                return DoPost(url, textParams);
            }

            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线

            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;

            Stream reqStream = req.GetRequestStream();
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            // 组装文本请求参数
            const string textTemplate = "Content-Disposition:form-data;name=\"{0}\"\r\nContent-Type:text/plain\r\n\r\n{1}";
            IEnumerator<KeyValuePair<string, string>> textEnum = textParams.GetEnumerator();
            while (textEnum.MoveNext())
            {
                string name = textEnum.Current.Key;
                string value = textEnum.Current.Value;
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(value))
                {
                    string textEntry = String.Format(textTemplate, textEnum.Current.Key, textEnum.Current.Value);
                    byte[] itemBytes = Encoding.UTF8.GetBytes(textEntry);
                    reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                    reqStream.Write(itemBytes, 0, itemBytes.Length);
                }
            }

            // 组装文件请求参数
            const string fileTemplate = "Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
            var fileEnum = fileParams.GetEnumerator();
            while (fileEnum.MoveNext())
            {
                string key = fileEnum.Current.Key;
                FileItem fileItem = fileEnum.Current.Value;
                string fileEntry = String.Format(fileTemplate, key, fileItem.GetFileName(), fileItem.GetMimeType());
                byte[] itemBytes = Encoding.UTF8.GetBytes(fileEntry);
                reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                reqStream.Write(itemBytes, 0, itemBytes.Length);

                byte[] fileBytes = fileItem.GetContent();
                reqStream.Write(fileBytes, 0, fileBytes.Length);
            }

            reqStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            reqStream.Close();

            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = rsp.CharacterSet == null ? Encoding.UTF8 : Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        { //直接确认，否则打不开
            return true;
        }

        private HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }

            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "crawler.intime.com.cn";
            req.Timeout = this._timeout;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            using (rsp)
            using (var stream = rsp.GetResponseStream())
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <param name="encodeUrl"></param>
        /// <returns>URL编码后的请求数据</returns>
        public string BuildQuery(IDictionary<string, string> parameters, bool encodeUrl=true)
        {
            var requestUrl = new StringBuilder();

            return parameters.Keys.Aggregate(requestUrl,
                (s, e) => s.AppendFormat("{0}={1}&", e, encodeUrl?UrlEncode(parameters[e],Encoding.UTF8):parameters[e]), s => s.ToString().TrimEnd('&'));
        }

        /**
        * C#的URL encoding有两个问题：
        * 1.左右括号没有转移（Java的URLEncoder.encode有）
        * 2.转移符合都是小写的，Java是大写的
        */
        public string UrlEncode(string str, Encoding e = null)
        {
            if (str == null)
            {
                return null;
            }

            string encodedStr = e != null ? HttpUtility.UrlEncode(str, e) : HttpUtility.UrlEncode(str);
            if (String.IsNullOrEmpty(encodedStr))
            {
                return encodedStr;
            }
            String stringToEncode = encodedStr
                .Replace("+", "%20")
                .Replace("*", "%2A")
                .Replace("(", "%28")
                .Replace(")", "%29");
            return REG_URL_ENCODING.Replace(stringToEncode, m => m.Value.ToUpperInvariant());
        }
    }
}
