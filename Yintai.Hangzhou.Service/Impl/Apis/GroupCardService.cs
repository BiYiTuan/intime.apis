using com.intime.fashion.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Yintai.Architecture.Framework.Extension;
using Yintai.Hangzhou.Service.Contract.Apis;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service.Impl.Apis
{
    public class GroupCardService : BaseService, IGroupCardService
    {
        private readonly byte[] _key = Encoding.UTF8.GetBytes(ConfigManager.GetGroupCardKey());

        #region methods

        private string GetGroupCardEncryptString(string input)
        {
            return GetBase64String(DesEncrypt(input, _key, CipherMode.ECB));
        }

        private static string GetBase64String(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        private static byte[] DesEncrypt(string input, byte[] key, CipherMode cipherMode)
        {
            var inputByteArray = Encoding.UTF8.GetBytes(input);
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.Mode = cipherMode;
                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cStream.FlushFinalBlock();

                        return mStream.ToArray();
                    }
                }
            }
        }

        private static string OpenUrl(string url, string method, string ua, string ct, Dictionary<string, string> queryParams, string postdata, string enc)
        {
            HttpWebResponse response;

            if (queryParams != null)
            {
                if (url.IndexOf("?", StringComparison.Ordinal) == -1)
                {
                    url = url + "?";
                }

                url = url + EQueryParams(queryParams);
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            if (!String.IsNullOrEmpty(ua))
            {
                request.UserAgent = ua;
            }

            request.Method = method;
            if (!String.IsNullOrEmpty(ct))
            {
                request.ContentType = ct;
            }

  
            try
            {
                if (method.Equals("POST"))
                {
                    if (postdata != null)
                    {
                        //var bytes = Encoding.ASCII.GetBytes(postdata);
                       // request.ContentLength = bytes.Length;

                        using (var objRequestStream = request.GetRequestStream())
                        using (var streamWriter = new StreamWriter(objRequestStream))
                        {
                            
                            streamWriter.Write(postdata);
                        }
                    }
                }
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Error(string.Format("exception postdata:{0}",postdata));

                throw;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var body = response.GetResponseStream();

                return Read4Buffer(body, String.IsNullOrEmpty(enc) ? Encoding.UTF8 : Encoding.GetEncoding(enc));
            }

            return null;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="encoding"> </param>
        /// <returns></returns>
        private static string Read4Buffer(Stream obj, Encoding encoding)
        {
            var reader = new StreamReader(obj, encoding);

            var readbuffer = new char[256];
            var n = reader.Read(readbuffer, 0, 256);
            var sb = new StringBuilder();

            while (n > 0)
            {
                var str = new String(readbuffer, 0, n);
                sb.Append(str);
                n = reader.Read(readbuffer, 0, 256);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="encoding"> </param>
        /// <returns></returns>
        private static string ReadToEnd(Stream obj, Encoding encoding)
        {
            var reader = new StreamReader(obj, encoding);

            var str = reader.ReadToEnd();

            return str;
        }

        private static string EQueryParams(Dictionary<string, string> queryParams)
        {
            if (queryParams == null)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();

            foreach (var key in queryParams.Keys)
            {
                sb.AppendFormat("{0}={1}&", key.ToLower(), queryParams[key]);
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            else
            {
                return String.Empty;
            }

            return sb.ToString();
        }

        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        static string GetMd5Hash(string input)
        {
            //return input;

            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            md5Hasher.Clear();
            md5Hasher.Dispose();

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(input);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        public GroupCardPointResult GetPoint(GroupCardPointRequest request)
        {
            var root = new XElement("vipCard",
                        new XElement("cardno", GetGroupCardEncryptString(request.CardNo)));
            var postData = root.ToString(SaveOptions.DisableFormatting);

            var body = OpenUrl(ConfigManager.GetGroupCardQueryScoreUrl(), "POST", null, "application/xml", null, postData, null);

            var result = JsonExtension.FromJson<GroupCardPointResult>(body);

            return result;
        }

        public GroupCardInfoResult GetInfo(GroupCardInfoRequest request)
        {
            var root = new XElement("vipCard",
                                    new XElement("cardno", GetGroupCardEncryptString(request.CardNo)),
                                    new XElement("passwd", GetMd5Hash(request.Passwd).ToUpper()));
            var postData = root.ToString(SaveOptions.DisableFormatting);

            var body = OpenUrl(ConfigManager.GetGroupCardInfoUrl(), "POST", null, "application/xml", null, postData, null);

            var result = JsonExtension.FromJson<GroupCardInfoResult>(body);

            return result;
        }


        public GroupExchangeResult Exchange(GroupExchangeRequest request)
        {
            //todo: add implementation here
            return new GroupExchangeResult() {
                 Success = true,
                 
            };
        }
    }
}
