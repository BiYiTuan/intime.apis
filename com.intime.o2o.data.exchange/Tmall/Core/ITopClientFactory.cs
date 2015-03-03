using Top.Api;

namespace com.intime.o2o.data.exchange.Tmall.Core
{
    /// <summary>
    /// 淘宝Client客户端工厂
    /// </summary>
    public interface ITopClientFactory
    {
        ITopClient Get(string consumerKey);

        string GetSessionKey(string consumerKey);
    }
}
