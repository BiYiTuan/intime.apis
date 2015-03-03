using System.Collections.Generic;

namespace com.intime.fashion.data.sync
{
    public interface ISyncRequest
    {
        /// <summary>
        /// 请求方法，支持GET和POST
        /// </summary>
        string Method { get; }

        /// <summary>
        /// 请求资源相对地址
        /// </summary>
        string Resource { get; }

        string BaseUrl { get; }

        string Url { get; }

        /// <summary>
        /// 请求所需参数，包括系统参数及应用级别参数
        /// </summary>
        IDictionary<string, string> RequestParams { get; }

        IDictionary<string,FileItem> Attachments { get; }

        void Put(string key, object value);

        /// <summary>
        /// remove parameter
        /// </summary>
        /// <param name="key"></param>
        object Remove(string key);
    }
}
