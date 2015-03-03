using System;
using System.Collections.Generic;
using Yintai.Architecture.Common.Logger;

namespace Yintai.Architecture.Common.Caching
{
    internal abstract class BaseCacheProvider : ICache
    {
        protected ILog Logger;

        protected BaseCacheProvider()
            : this(LoggerManager.Current())
        {
        }

        protected BaseCacheProvider(ILog log)
        {
            Logger = log;
        }

        public abstract bool GetItem(string key, out object value);

        public abstract void PutItem(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration,
                                     DateTime absoluteExpiration);

        public abstract void InvalidateSets(IEnumerable<string> entitySets);
        public abstract void InvalidateItem(string key);
        public abstract void Clear();
    }
}