namespace com.intime.o2o.data.exchange.Tmall.Product.Models
{
    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="T">结果类型</typeparam>
    public class ResultInfo<T>
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary
        public string ErrMsg { get; set; }

        /// <summary>
        /// 是否包含错误
        /// </summary>
        public bool IsError
        {
            get { return !string.IsNullOrEmpty(ErrMsg); }
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }
}
