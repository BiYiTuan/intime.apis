namespace com.intime.o2o.data.exchange.IT
{
    /// <summary>
    /// Api接口类
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="TRequest">请求对象类型</typeparam>
        /// <typeparam name="TResponse">响应对象类型</typeparam>
        /// <param name="request">请求对象</param>
        /// <returns>响应结果</returns>
        TResponse Post<TRequest, TResponse>(Request<TRequest, TResponse> request);
    }
}