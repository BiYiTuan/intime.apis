using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.Extension;

namespace Yintai.Architecture.Common.Web.HttpModules
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Architecture.Framework.Web.HttpModules
    /// FileName: Class1
    ///
    /// Created at 11/8/2012 7:50:21 PM
    /// Description: 
    /// </summary>
    /// <summary>
    /// 请求记录日志
    /// </summary>
    public class RequestLogginHttpModule : IHttpModule
    {
        #region fileds

        private static readonly ILog _log = LoggerManager.Current();

        #endregion

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, System.EventArgs e)
        {
            var context = sender as HttpApplication;

            if (context == null)
            {
                _log.Error("RequestLogginHttpModule Parameter context is null");
                return;
            }
            var unvalid_request = context.Context.Request.Unvalidated;
            
            string form = unvalid_request.Form.ToJson();
            string query = unvalid_request.QueryString.ToJson();
            
            //logging the Request
            var sb = new StringBuilder();

            sb.AppendFormat("{0} [ {1} ] ", unvalid_request.Url, DateTime.Now);
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine("[ Headers ]");
            sb.AppendLine();

            foreach (string key in unvalid_request.Headers)
            {
                sb.AppendFormat("{0} : {1}", key, unvalid_request.Headers[key]);
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("[ QueryString ]");
            sb.AppendLine();

            foreach (string key in unvalid_request.QueryString)
            {
                sb.AppendFormat("{0} : {1}", key, unvalid_request.QueryString[key]);
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("[ Form ]");
            sb.AppendLine();

            foreach (string key in unvalid_request.Form)
            {
                sb.AppendFormat("{0} : {1}", key, unvalid_request.Form[key]);
                sb.AppendLine();
            }
            if (unvalid_request.Headers["Content-Type"] != null && unvalid_request.Headers["Content-Type"].IndexOf("multipart/form-data") < 0)
            {
                var sr = new StreamReader(context.Context.Request.InputStream);
                var rawBody = sr.ReadToEnd();
                context.Context.Request.InputStream.Position = 0;
                sb.AppendLine("[ rawbody ]");
                sb.AppendLine(rawBody);
            }
            _log.Debug(sb.ToString());
        }

        #region helper

        public Dictionary<string, string> NameValueCollectionToDictionary(NameValueCollection collection)
        {
            var dict = new Dictionary<string, string>();
            foreach (string key in collection.Keys)
            {
                dict.Add(key, collection[key]);
            }

            return dict;
        }

        #endregion

        #endregion
    }
}
