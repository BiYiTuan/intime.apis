using System;
using System.Linq;
using System.Reflection;

namespace Yintai.Architecture.Framework.Extension
{
    public static class MemberInfoExtension
    {
        #region fields

        #endregion

        #region .ctor

        #endregion

        #region properties

        #endregion

        #region methods

        /// <summary>
        /// 为指定的成员元数据获取自定义的Attribute
        /// </summary>
        /// <typeparam name="T">自定义的Attribute类型</typeparam>
        /// <param name="member">成员元数据</param>
        /// <returns>自定义的Attribute实例集合</returns>
        public static T[] GetCustomAttributes<T>(this MemberInfo member)
            where T : Attribute
        {
            var queryResult = from attr in Attribute.GetCustomAttributes(member, typeof(T))
                              select attr.SafeCast<T>();
            return queryResult.IsNull() ? null : queryResult.ToArray();
        }

        /// <summary>
        /// 检测指定的成员元数据是否存在特定的Attribute
        /// </summary>
        /// <param name="member">目标成员元数据</param>
        public static bool IsDefined<T>(this MemberInfo member)
            where T : Attribute
        {
            ExceptionUtils.CheckOnNull(member, "member");
            return Attribute.IsDefined(member, typeof(T));
        }

        /// <summary>
        /// 获取指定成员元数据的Attribute实例
        /// </summary>
        /// <typeparam name="T">自定义的Attribute类型</typeparam>
        /// <param name="target">目标元数据成员</param>
        /// <returns>特定的Attribute实例</returns>
        public static T GetCustomAttribute<T>(this MemberInfo target)
            where T : Attribute
        {
            return Attribute.GetCustomAttribute(target, typeof(T)).SafeCast<T>();
        }

        #endregion
    }
}