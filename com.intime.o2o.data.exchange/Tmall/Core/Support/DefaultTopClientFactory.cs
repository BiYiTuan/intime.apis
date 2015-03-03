using System.Configuration;
using Top.Api;

namespace com.intime.o2o.data.exchange.Tmall.Core.Support
{
    /// <summary>
    /// 默认TopClient工厂实现
    /// </summary>
    public class DefaultTopClientFactory : ITopClientFactory
    {
        public ITopClient Get(string consumerKey)
        {
            var serverUrl = GetConfig("serverURL", consumerKey);
            var appKey = GetConfig("appKey", consumerKey);
            var appSecure = GetConfig("appSecure", consumerKey);

            return new DefaultTopClient(serverUrl, appKey, appSecure);
        }

        /// <summary>
        /// 获取授权的SessionKey
        /// </summary>
        /// <param name="consumerKey">消费者</param>
        /// <returns></returns>
        public string GetSessionKey(string consumerKey)
        {
            return GetConfig("sessionKey", consumerKey);
        }

        private string GetConfig(string key, string consumerKey)
        {
            var configKey = string.Format("{0}.tmall.{1}", consumerKey, key);
            return ConfigurationManager.AppSettings[configKey] ?? string.Empty;
        }
    }
}
