using System;
using System.ComponentModel;
using System.Globalization;

namespace Yintai.Architecture.Framework.Extension
{
    /// <summary>
    /// CLR Version: 4.0.30319.239
    /// NameSpace: Yintai.Architecture.Framework.Extension
    /// FileName: EnumExtension
    ///
    /// Created at 1/12/2012 2:01:49 PM
    /// </summary>
    public static class EnumExtension
    {
        #region fields

        #endregion

        #region .ctor

        #endregion

        #region properties

        #endregion

        #region methods

        /// <summary>
        /// 获取 枚举 上的Description 特性的值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum source)
        {
            var enumInfo = source.GetType().GetField(source.ToString(CultureInfo.InvariantCulture));

            var enumAttributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return enumAttributes.Length > 0 ? enumAttributes[0].Description : source.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetEnumName(Type enumType, Object value)
        {
            return Enum.GetName(enumType, value);
        }

        public static string[] GetNames(Type enumType)
        {
            return Enum.GetNames(enumType);
        }

        public static T Parser<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static T Parser<T>(int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static bool IsDefined(Type enumType, object value)
        {
            return Enum.IsDefined(enumType, value);
        }

        #endregion
    }
}
