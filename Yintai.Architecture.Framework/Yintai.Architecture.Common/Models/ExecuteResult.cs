using System.Runtime.Serialization;

namespace Yintai.Architecture.Common.Models
{
    /// <summary>
    /// 执行结果
    /// </summary>
    [DataContract(Name = "result")]
    public class ExecuteResult
    {
        #region .ctor

        public ExecuteResult()
        {
            this.StatusCode = StatusCode.Success;
        }

        #endregion

        #region properties

        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember(Name = "statusCode", Order = 2)]
        public StatusCode StatusCode { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember(Name = "isSuccessful", Order = 1)]
        public bool IsSuccess
        {
            get { return StatusCode == StatusCode.Success; }
            set { }
        }

        /// <summary>
        ///  信息
        /// </summary>
        [DataMember(Name = "message", Order = 3)]
        public string Message { get; set; }

        #endregion
    }

    /// <summary>
    /// 泛型的执行结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract(Name = "result")]
    public class ExecuteResult<T> : ExecuteResult
    {
        #region fields

        #endregion

        #region .ctor

        public ExecuteResult(T data)
        {
            Data = data;
        }

        public ExecuteResult()
            : this(default(T))
        {
        }

        #endregion

        #region properties

        /// <summary>
        /// 返回数据
        /// </summary>
        [DataMember(Name = "data")]
        public T Data { get; set; }

        #endregion

        #region methods

        #endregion
    }
}