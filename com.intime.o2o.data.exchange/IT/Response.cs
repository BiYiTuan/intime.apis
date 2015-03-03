using System.Runtime.Serialization;

namespace com.intime.o2o.data.exchange.IT
{
    /// <summary>
    /// 数据响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class Response<T>
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        [DataMember(Name = "status")]
        public bool Status { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// 返回状态码
        /// </summary>
        [DataMember(Name = "code")]
        public int Code { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        [DataMember(Name = "data")]
        public T Data { get; set; }
    }
}