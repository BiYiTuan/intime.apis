using System;
using System.Configuration;
using System.Linq;

namespace Yintai.Hangzhou.WebSupport.Configuration
{
    public class ConfigManager
    {
        private static readonly ConfigManager _configManager;

        /// <summary>
        /// 配置管理
        /// </summary>
        public static ConfigManager Instance
        {
            get { return _configManager; }
        }

        static ConfigManager()
        {
            _configManager = new ConfigManager();
        }

        private ConfigManager()
        {
        }

        public static string GetParamsValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static object GetParamsObjectValue(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return GetParamsValue(key);
            }

            return null;
        }

        public static T GetParamsValueOrDefault<T>(string key, T defaultValue)
        {
            var t = GetParamsObjectValue(key);

            if (t == null)
            {
                return defaultValue;
            }

            return (T)t;
        }

        public static int AppleAppid
        {
            get
            {
                var t = com.intime.fashion.common.ConfigManager.GetAppleAppId();

                int i;
                Int32.TryParse(t, out i);

                return i;
            }
        }
    }
}
