using System;
using System.Security.Cryptography;
using System.Text;

namespace Yintai.Architecture.Framework
{
    public interface IGenerateUniqueString
    {
        /// <summary>
        /// 生成4组key
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        string[] Generate(string keyWord);

        /// <summary>
        /// 生成唯一KEY
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        string GenerateUnique(string keyWord);
    }

    /// <summary>
    /// CLR Version: 4.0.30319.239
    /// NameSpace: 
    /// FileName: GenerateUnique
    ///
    /// Created at 2/3/2012 2:41:09 PM
    /// Description: 
    /// </summary>
    public class GenerateUniqueString : IGenerateUniqueString
    {
        #region fields

        /// <summary>
        /// 要使用生成 的字符
        /// </summary>
        private static readonly string[] _chars = new[]{
                "a","b","c","d","e","f","g","h",
                "i","j","k","l","m","n","o","p",
                "q","r","s","t","u","v","w","x",
                "y","z","0","1","2","3","4","5",
                "6","7","8","9","A","B","C","D",
                "E","F","G","H","I","J","K","L",
                "M","N","O","P","Q","R","S","T",
                "U","V","W","X","Y","Z"
              };
        /// <summary>
        /// 混淆用的KEY
        /// </summary>
        private const string ConfuseKey = "1234567890qwertyuiopasdfghjklzxcvbnm@yintai.com";

        #endregion

        #region .ctor

        #endregion

        #region properties

        #endregion

        #region methods

        /// <summary>
        /// 生成唯一KEY
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public static string GetKey(string keyWord)
        {
            //可以自定义生成MD5加密字符传前的混合KEY

            //对传入网址进行MD5加密
            MD5 md5 = new MD5CryptoServiceProvider();

            var data = md5.ComputeHash(Encoding.UTF8.GetBytes(ConfuseKey + keyWord));
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            var hex = sBuilder.ToString();
            // var resUrl = new string[4];

            //for (int i = 0; i < 4; i++)
            //{
            //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
            var hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(0, 8), 16);
            var outChars = String.Empty;
            for (var j = 0; j < 6; j++)
            {
                //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                var index = 0x0000003D & hexint;
                //把取得的字符相加
                outChars += _chars[index];
                //每次循环按位右移5位
                hexint = hexint >> 5;
            }
            //把字符串存入对应索引的输出数组
            return outChars;
            //}

            //return resUrl;
        }

        /// <summary>
        /// 对MD5编码分解为4份
        /// </summary>
        /// <param name="keyWord">需要散列的STR</param>
        /// <returns></returns>
        public static string[] Generate(string keyWord)
        {
            //可以自定义生成MD5加密字符传前的混合KEY

            //对传入网址进行MD5加密
            MD5 md5 = new MD5CryptoServiceProvider();

            var data = md5.ComputeHash(Encoding.UTF8.GetBytes(ConfuseKey + keyWord));
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            var hex = sBuilder.ToString();
            var resUrl = new string[4];

            for (int i = 0; i < 4; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                var hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                var outChars = String.Empty;
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加
                    outChars += _chars[index];
                    //每次循环按位右移5位
                    hexint = hexint >> 5;
                }
                //把字符串存入对应索引的输出数组
                resUrl[i] = outChars;
            }

            return resUrl;
        }

        #endregion

        #region Implementation of IGenerateUniqueString

        string[] IGenerateUniqueString.Generate(string keyWord)
        {
            return Generate(keyWord);
        }

        /// <summary>
        /// 生成唯一KEY
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        string IGenerateUniqueString.GenerateUnique(string keyWord)
        {
            return GetKey(keyWord);
        }

        #endregion
    }
}
