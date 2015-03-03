namespace Yintai.Architecture.Framework.Extension
{
    public static class ObjectExtension
    {
        #region fields

        #endregion

        #region .ctor

        #endregion

        #region properties

        #endregion

        #region methods

        #region Basic Extensions

        /// <summary>
        /// 判断指定的对象是空引用
        /// </summary>
        /// <param name="value">要测试的对象</param>
        /// <returns>如果指定的对象为空引用，返回true；否则返回false</returns>
        public static bool IsNull(this object value)
        {
            return value == null;
        }

        /// <summary>
        /// 判断指定的对象不是空引用
        /// </summary>
        /// <param name="value">要测试的对象</param>
        /// <returns>如果指定的对象不为空引用，返回true；否则返回falsee</returns>
        public static bool IsNotNull(this object value)
        {
            return !IsNull(value);
        }

        /// <summary>
        /// 转换指定对象到另外一个对象
        /// </summary>
        /// <typeparam name="T">转换的结果对象类型</typeparam>
        /// <param name="value">要被转换的对象</param>
        /// <returns>转换结果</returns>
        /// <exception cref="System.InvalidCastException">如果不能转换为指定类型，将会抛出该异常</exception>
        public static T UnsafeCast<T>(this object value)
        {
            return value.IsNull() ? default(T) : (T)value;
        }

        /// <summary>
        /// 转换指定对象到另外一个对象
        /// </summary>
        /// <typeparam name="T">转换的结果对象类型</typeparam>
        /// <param name="value">要被转换的对象</param>
        /// <returns>转换结果，如果不能转换则为Null</returns>
        public static T SafeCast<T>(this object value)
        {
            return value is T ? value.UnsafeCast<T>() : default(T);
        }

        #endregion

        #endregion
    }
}
