using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Yintai.Hangzhou.Service.Manager
{
    public partial class ConfigManager
    {
        private const string _AppleAppId = "appleappid";
        private const string _ResourceDomain = "resourcedomain";
        private const string _Domain = "domain";
        private const string _GroupCradKey = "jtapis_key";
        private const string _GroupCradInfoUrl = "jtapis_cardinfo";
        private const string _GroupCradQueryPointUrl = "jtapis_cardscore";

        private static readonly string _domainPath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _imageUpload = ConfigurationManager.AppSettings["resourceimagedomain"];
        private static readonly string _soundUpload = ConfigurationManager.AppSettings["resourcesounddomain"];
        private static readonly string _videoUpload = ConfigurationManager.AppSettings["resourcevideodomain"];
        private static readonly string _defUpload = ConfigurationManager.AppSettings["resourcedefdomain"];

        private static readonly string _resourcedomain = ConfigurationManager.AppSettings["resourcedomain"];
        private static readonly string _point2GroupRatio = ConfigurationManager.AppSettings["pointratio2group"];

        private static readonly string _awshttppublickey = ConfigurationManager.AppSettings["awshttppublickey"];
        private static readonly string _awshttpprivatekey = ConfigurationManager.AppSettings["awshttpprivatekey"];
        private static readonly string _awshttphost = ConfigurationManager.AppSettings["awshttphost"];
        private static readonly string _awshttpaction_voidcoupon = ConfigurationManager.AppSettings["awshttpaction_voidcoupon"];

        private static readonly string _grouphttppublickey = ConfigurationManager.AppSettings["grouphttppublickey"];
        private static readonly string _grouphttpprivatekey = ConfigurationManager.AppSettings["grouphttpprivatekey"];
        private static readonly string _grouphttphost = ConfigurationManager.AppSettings["grouphttphost"];
        private static readonly string _grouphttpaction_exchange = ConfigurationManager.AppSettings["grouphttpaction_exchange"];
        private static string _appStoreNoInGroup = ConfigurationManager.AppSettings["appstorenoingroup"];
        public static int GetCacheSeed()
        {
            var t = GetAppConfigParamsValueOrDefault("cacheseedfactory", "1");

            return Int32.Parse(t);
        }

        /// <summary>
        /// image
        /// </summary>
        /// <returns></returns>
        public static string GetHttpApiImagePath()
        {
            return _resourcedomain + _imageUpload;
            //return _domain + "api/" + _imageUpload;
        }

        /// <summary>
        /// sound
        /// </summary>
        /// <returns></returns>
        public static string GetHttpApiSoundPath()
        {
            return _resourcedomain + _soundUpload;
            //return _domain + "api/" + _imageUpload;
        }

        /// <summary>
        /// video
        /// </summary>
        /// <returns></returns>
        public static string GetHttpApivideoPath()
        {
            return _resourcedomain + _videoUpload;
            //return _domain + "api/" + _imageUpload;
        }

        /// <summary>
        /// def
        /// </summary>
        /// <returns></returns>
        public static string GetHttpApidefPath()
        {
            return _resourcedomain + _defUpload;
            //return _domain + "api/" + _imageUpload;
        }

        private ConfigManager()
        {
        }

        #region methods

        private static string GetAppConfigParamsValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static string GetAppConfigParamsValueOrDefault(string key, string def)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return GetAppConfigParamsValue(key);
            }

            return def;
        }

        #endregion

        /// <summary>
        /// 获取苹果 appid
        /// </summary>
        /// <returns></returns>
        public static string GetAppleAppId()
        {
            return GetAppConfigParamsValue(_AppleAppId);
        }

        /// <summary>
        /// 获取 域
        /// </summary>
        public static string Domain
        {
            get { return GetAppConfigParamsValue(_Domain); }
        }

        public static string GetGroupCardKey()
        {
            return GetAppConfigParamsValue(_GroupCradKey);
        }

        public static string GetGroupCardInfoUrl()
        {
            return GetAppConfigParamsValue(_GroupCradInfoUrl);
        }

        public static string GetGroupCardQueryScoreUrl()
        {
            return GetAppConfigParamsValue(_GroupCradQueryPointUrl);
        }
        public static decimal Point2GroupRatio
        {
            get
            {
                return decimal.Parse(_point2GroupRatio);
            }
        }
        public static string AwsHttpPublicKey
        {
            get
            {
                return _awshttppublickey;
            }
        }
        public static string AwsHttpPrivateKey
        {
            get
            {
                return _awshttpprivatekey;
            }
        }
        public static string AwsHttpUrlVoidCoupon
        {
            get
            {
                return Path.Combine(_awshttphost,_awshttpaction_voidcoupon);
            }
        }
        public static string GroupHttpPublicKey
        {
            get
            {
                return _grouphttppublickey;
            }
        }
        public static string GroupHttpPrivateKey
        {
            get
            {
                return _grouphttpprivatekey;
            }
        }
        public static string GroupHttpUrlExchange
        {
            get
            {
                return Path.Combine(_grouphttphost, _grouphttpaction_exchange);
            }
        }
        public static string AppStoreNoInGroup
        {
            get
            {
                return _appStoreNoInGroup;
            }
        }
    }
}
