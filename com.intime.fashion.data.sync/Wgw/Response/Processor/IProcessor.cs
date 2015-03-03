namespace com.intime.fashion.data.sync.Wgw.Response.Processor
{
    public interface IProcessor
    {
        bool Process(dynamic response, dynamic otherInfo);

        /// <summary>
        ///处理失败或返回失败的错误信息
        /// </summary>
        string ErrorMessage { get; }
    }
}
