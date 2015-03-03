using System.Runtime.Serialization;

namespace Yintai.Architecture.Common.Models
{
    /// <summary>
    /// 系统状态码
    /// </summary>
    [DataContract(Name = "statusCode")]
    public enum StatusCode
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        [EnumMember]
        UnKnow = 0,

        /// <summary>
        /// 操作成功
        /// </summary>
        [EnumMember]
        Success = 200,

        /// <summary>
        /// 客户端错误
        /// </summary>
        [EnumMember]
        ClientError = 400,

        /// <summary>
        /// 未通过验证的
        /// </summary>
        [EnumMember]
        Unauthorized = 401,

        /// <summary>
        /// 请求超时
        /// </summary>
        [EnumMember]
        RequestTimeout = 408,

        /// <summary>
        /// 系统内部错误
        /// </summary>
        [EnumMember]
        InternalServerError = 500,

        /// <summary>
        /// 服务暂不可用
        /// </summary>
        [EnumMember]
        ServiceUnavailable = 503
    }
}