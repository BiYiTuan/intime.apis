using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace Yintai.Architecture.Common.Caching
{
    /// <summary>
    /// 委托
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public delegate bool CacheGetter<TData>(out TData data);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TReason"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="reason"></param>
    public delegate void CacheCallback<in TKey, in TValue, in TReason>(TKey key, TValue value, TReason reason);

    /// <summary>
    /// 对Cache的封装
    /// </summary>
    public static class CachingHelper
    {
        // 以 Factor = 5 为基础的默认值
        /// <summary>
        /// 一天
        /// </summary>
        public static readonly int DayFactor = 86400;
        /// <summary>
        /// 一小时
        /// </summary>
        public static readonly int HourFactor = 3600;
        /// <summary>
        /// 1分钟
        /// </summary>
        public static readonly int MinuteFactor = 60;
        /// <summary>
        /// 1秒钟
        /// </summary>
        public static readonly double SecondFactor = 1;

        private static readonly ICache Cacher;

        private static int _factor = 1;

        /// <summary>
        /// 静态初始化可以确保我们对当前Cache类只实例化一次
        /// </summary>
        static CachingHelper()
        {
            Cacher = CachingManager.Current;
        }

        /// <summary>
        /// 重设基数
        /// </summary>
        /// <param name="cacheFactor"></param>
        public static void ReSetFactor(int cacheFactor)
        {
            _factor = cacheFactor;
        }

        /// <summary>
        /// 从Cache中移除所有项目
        /// </summary>
        public static void Clear()
        {
            Cacher.Clear();
        }

        ///// <summary>
        ///// 根据正则表达式匹配符合的Key，然后移除
        ///// </summary>
        ///// <param name="pattern"></param>
        //public static void RemoveByPattern(string pattern)
        //{
        //    var cacheEnum = Cacher.GetEnumerator();
        //    var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        //    while (cacheEnum.MoveNext())
        //    {
        //        if (regex.IsMatch(cacheEnum.Key.ToString()))
        //            Cacher.Remove(cacheEnum.Key.ToString());
        //    }
        //}

        /// <summary>
        /// 根据Key从Cache中移除项目
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            Cacher.InvalidateItem(key);
        }

        /// <summary>
        /// 插入对象到Cache中 60秒
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 60);
        }

        /// <summary>
        /// 默认一分钟
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        public static void Insert(string key, object obj, IEnumerable<string> dep)
        {
            Insert(key, obj, dep, MinuteFactor);
        }

        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, null, seconds);
        }

        public static void Insert(string key, object obj, IEnumerable<string> dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        private static void Insert(string key, object obj, IEnumerable<string> dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                Cacher.PutItem(key, obj, dep, TimeSpan.Zero, DateTime.Now.AddSeconds(_factor * seconds));
            }
        }

        /// <summary>
        /// 插入对象到Cache中，缓存时间设为最短
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="secondFactor"></param>
        public static void MicroInsert(string key, object obj, int secondFactor)
        {
            if (obj != null)
            {
                Cacher.PutItem(key, obj, null, TimeSpan.Zero, DateTime.Now.AddSeconds(_factor * secondFactor));
            }
        }

        /// <summary>
        /// 插入对象到Cache中，缓存时间设置为最大
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Max(string key, object obj)
        {
            Max(key, obj, null);
        }

        public static void Max(string key, object obj, IEnumerable<string> dep)
        {
            if (obj != null)
            {
                Cacher.PutItem(key, obj, dep, TimeSpan.Zero, DateTime.MaxValue);
            }
        }

        /// <summary>
        /// 插入对象到Cache中，缓存时间设置为最大
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Permanent(string key, object obj)
        {
            Permanent(key, obj, null);
        }

        public static void Permanent(string key, object obj, IEnumerable<string> dep)
        {
            if (obj != null)
            {
                Cacher.PutItem(key, obj, dep, TimeSpan.Zero, DateTime.MaxValue);
            }
        }

        public static object Get(string key)
        {
            object o;
            if (Cacher.GetItem(key, out o))
            {
                return o;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        public static int SecondFactorCalculate(int seconds)
        {
            return Convert.ToInt32(Math.Round(seconds * SecondFactor));
        }

        /// <summary>
        /// 缓存方法
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="cacheGetter">取缓存</param>
        /// <param name="sourceGetter">取源数据</param>
        /// <param name="cacheSetter">写缓存</param>
        /// <returns></returns>
        public static TData Get<TData>(CacheGetter<TData> cacheGetter, Func<TData> sourceGetter, Action<TData> cacheSetter)
        {
            TData data;
            //取缓存
            if (cacheGetter(out data))
            {
                return data;
            }
            //取源数据
            data = sourceGetter();
            //写缓存
            cacheSetter(data);

            return data;
        }


        #region 使用例子

        //public List<User> GetFriends(int userId)
        //{
        //    string cacheKey = "friends_of_user_" + userId;

        //    return CacheHelper.Get(
        //        delegate(out List<User> data) // cache getter
        //        {
        //            object objData = cacheManager.Get(cacheKey);
        //            data = (objData == null) ? null : (List<User>)objData;

        //            return objData != null;
        //        },
        //        () => // source getter
        //        {
        //            return new UserService().GetFriends(userId);
        //        },
        //        (data) => // cache setter
        //        {
        //            cacheManager.Set(cacheKey, data);
        //        });

        //}

        #endregion
    }
}
