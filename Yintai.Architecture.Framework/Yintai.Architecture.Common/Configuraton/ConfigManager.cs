using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Yintai.Architecture.Common.Configuraton
{
    public class ConfigManager
    {
        private const string CachingProvider = "CacheProvider";

        public static bool IsCloseService
        {
            get { return Boolean.Parse(GetAppkey(Define.IsCloseService)); }
        }

        public static bool IsEnableSign
        {
            get { return Boolean.Parse(GetAppkey(Define.IsEnableSign)); }
        }

        /// <summary>
        /// 获取应用程序key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppkey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 获取cacheprovider
        /// </summary>
        /// <returns></returns>
        public static string GetCacheProvider()
        {
            return GetAppkey(CachingProvider);
        }
    }
}
