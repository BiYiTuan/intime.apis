using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using Yintai.Architecture.Common.Helper;

namespace Yintai.Architecture.Common.Caching
{
    /// <summary>
    /// Implementation of <see cref="ICache"/> which works with ASP.NET cache object.
    /// </summary>
    internal class AspNetCacheProvider : BaseCacheProvider
    {
        private const string DependentEntitySetPrefix = "dependent_entity_set_";
        private readonly HttpContext _httpContext;

        /// <summary>
        /// Initializes a new instance of the AspNetCache class.
        /// </summary>
        public AspNetCacheProvider()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AspNetCache class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public AspNetCacheProvider(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        private Cache HttpCache
        {
            get
            {
                if (_httpContext != null)
                {
                    return _httpContext.Cache;
                }

                var context = HttpContext.Current;
                if (context == null)
                {
                    throw new InvalidOperationException("Unable to determine HTTP context.");
                }

                return context.Cache;
            }
        }

        /// <summary>
        /// Tries to the get entry by key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The retrieved value.</param>
        /// <returns>
        /// A value of <c>true</c> if entry was found in the cache, <c>false</c> otherwise.
        /// </returns>
        public override bool GetItem(string key, out object value)
        {
            key = GetCacheKey(key);
            value = HttpCache.Get(key);

            return value != null;
        }

        /// <summary>
        /// Adds the specified entry to the cache.
        /// </summary>
        /// <param name="key">The entry key.</param>
        /// <param name="value">The entry value.</param>
        /// <param name="dependentEntitySets">The list of dependent entity sets.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        public override void PutItem(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTime absoluteExpiration)
        {
            PutItem(key, value, dependentEntitySets, slidingExpiration, absoluteExpiration, null);
        }

        public void PutItem(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration,
                                     DateTime absoluteExpiration, CacheCallback<string, object, object> callback)
        {
            if (callback != null)
            {
                CacheItemRemovedCallback c = (k, v, r) => callback(k, v, r);
                PutItemExec(key, value, dependentEntitySets, slidingExpiration, absoluteExpiration, c);
            }

            PutItemExec(key, value, dependentEntitySets, slidingExpiration, absoluteExpiration, null);
        }

        /// <summary>
        /// Adds the specified entry to the cache.
        /// </summary>
        /// <param name="key">The entry key.</param>
        /// <param name="value">The entry value.</param>
        /// <param name="dependentEntitySets">The list of dependent entity sets.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="callback">callback</param>
        private void PutItemExec(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTime absoluteExpiration, CacheItemRemovedCallback callback)
        {
            key = GetCacheKey(key);
            var cache = HttpCache;

            CacheDependency cd = null;
            if (dependentEntitySets != null)
            {
                var l = dependentEntitySets.ToList();
                foreach (var entitySet in l)
                {
                    EnsureEntryExists(DependentEntitySetPrefix + entitySet);
                }

                if (l.Count > 0)
                {
                    cd = new CacheDependency(new string[0], l.Select(c => DependentEntitySetPrefix + c).ToArray());
                }
            }

            try
            {
                cache.Insert(key, value, cd, absoluteExpiration, slidingExpiration, CacheItemPriority.Normal, callback);
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Logger.Error(ex);
                    ex = ex.InnerException;
                }
                // there's a possibility that one of the dependencies has been evicted by another thread
                // in this case just don't put this item in the cache
            }
        }

        /// <summary>
        /// Invalidates all cache entries which are dependent on any of the specified entity sets.
        /// </summary>
        /// <param name="entitySets">The entity sets.</param>
        public override void InvalidateSets(IEnumerable<string> entitySets)
        {
            foreach (var entitySet in entitySets)
            {
                HttpCache.Remove(DependentEntitySetPrefix + entitySet);
            }
        }

        /// <summary>
        /// Invalidates cache entry with a given key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public override void InvalidateItem(string key)
        {
            key = GetCacheKey(key);
            HttpCache.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            var cacheEnum = HttpCache.GetEnumerator();
            var al = new ArrayList();
            while (cacheEnum.MoveNext())
            {
                al.Add(cacheEnum.Key);
            }

            foreach (string key in al)
            {
                HttpCache.Remove(key);
            }
        }

        #region methods

        /// <summary>
        /// Hashes the query to produce cache key..
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Hashed query which becomes a cache key.</returns>
        private static string GetCacheKey(string query)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(query);
            string hashString = Convert.ToBase64String(MD5.Create().ComputeHash(bytes));
            return hashString;
        }

        private void EnsureEntryExists(string key)
        {
            var cache = HttpCache;

            if (cache.Get(key) != null) return;
            try
            {
                cache.Insert(key, key, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Logger.Error(ex);
                    ex = ex.InnerException;
                }
            }
        }

        #endregion
    }
}
