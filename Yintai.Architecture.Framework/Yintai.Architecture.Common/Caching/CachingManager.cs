using System;
using System.Collections.Generic;

namespace Yintai.Architecture.Common.Caching
{
    public class CachingManager : ICache
    {
        private static readonly CachingManager Instance = new CachingManager();
        private readonly ICachingFactory _cachingFactory;
        private readonly ICache _cacheProvider;

        private CachingManager(ICachingFactory cachingFactory)
        {
            _cachingFactory = cachingFactory;
            _cacheProvider = _cachingFactory.Create();
        }

        private CachingManager()
            : this(new CachingFactory())
        {
        }

        public static CachingManager Current
        {
            get { return Instance; }
        }

        public bool GetItem(string key, out object value)
        {
            return _cacheProvider.GetItem(key, out value);
        }

        public void PutItem(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration,
                            DateTime absoluteExpiration)
        {
            _cacheProvider.PutItem(key, value, dependentEntitySets, slidingExpiration, absoluteExpiration);
        }

        public void InvalidateSets(IEnumerable<string> entitySets)
        {
            _cacheProvider.InvalidateSets(entitySets);
        }

        public void InvalidateItem(string key)
        {
            _cacheProvider.InvalidateItem(key);
        }

        public void Clear()
        {
            _cacheProvider.Clear();
        }
    }
}
