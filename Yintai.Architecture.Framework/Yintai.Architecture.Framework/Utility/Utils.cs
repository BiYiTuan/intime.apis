using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using Yintai.Architecture.Framework.Security.Cryptography;

namespace Yintai.Architecture.Framework.Utility
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Architecture.Framework.Utility
    /// FileName: Utils
    ///
    /// Created at 11/9/2012 11:22:03 AM
    /// Description: 
    /// </summary>
    public class Utils
    {
        public static string DataContractToJson(object target)
        {
            var dcs = new DataContractJsonSerializer(target.GetType());
            string result;

            using (var stream = new MemoryStream())
            {
                dcs.WriteObject(stream, target);
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            return result;
        }

        public static T DeserializeDataContract<T>(string json) where T : class
        {
            var ds = new DataContractJsonSerializer(typeof(T));

            T obj = null;

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                obj = (T)ds.ReadObject(ms);
                ms.Close();
            }

            return obj;
        }

      
    }

    /// <summary>
    /// SessionKey帮助类
    /// </summary>
    public static class SessionKeyHelper
    {
        #region fields

        private const string Vector = "abcdefghijklmnop";
        private const string Hash = "SHA1";
        private const int KeySize = 256;
        private const string Key = "sessionKey";

        #endregion

        #region Encrypt

        /// <summary>
        ///  加码完成后 替换 + 为 %2B 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string UrlEncode(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Replace("+", "%2B");
        }

        private static string UrlDecode(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Replace("%2B", "+");
        }

        public static string Encrypt(UserIdSessionData data)
        {
            return UrlEncode(AESEncrypt(Utils.DataContractToJson(data)));
        }

        public static string Encrypt(string userId)
        {
            var data = new UserIdSessionData
            {
                UserId = userId,
                IssueDate = DateTime.Now
            };

            return Encrypt(data);
        }

        #endregion

        #region Decrypt

        public static UserIdSessionData Decrypt(string input)
        {
            var str = String.Empty;
            UserIdSessionData data = null;

            try
            {
                str = AESDecrypt(UrlDecode(input));
            }
            catch (Exception)
            { }

            if (!String.IsNullOrEmpty(str))
            {
                data = Utils.DeserializeDataContract<UserIdSessionData>(str);
            }

            return data;
        }

        #endregion

        #region Helper

        private static string AESEncrypt(string input)
        {
            return CryptoHelper.Base64AESEncrypt(input,
               Key,
               Key,
                Hash,
                1,
                Vector,
                KeySize
            );
        }

        private static string AESDecrypt(string input)
        {
            return CryptoHelper.Base64AESDecrypt(input,
                Key,
                Key,
                Hash,
                1,
                Vector,
                KeySize
            );
        }

        #endregion
    }

    #region Session Data

    [DataContract]
    public class UserIdSessionData
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember(Name = "u")]
        public string UserId { get; set; }

        /// <summary>
        /// 操作的时间
        /// </summary>
        [DataMember(Name = "i")]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        [IgnoreDataMember]
        public bool Expired
        {
            get
            {
                return DateTime.Now > IssueDate.AddDays(30);
            }
        }
    }

    #endregion
}
