using System;
using Yintai.Architecture.Common.Configuraton;

namespace Yintai.Architecture.Common.Caching
{
    public interface ICachingFactory
    {
        ICache Create();
    }

    public class CachingFactory : ICachingFactory
    {
        private ICache _caches;
        private readonly object _syncObj = new object();

        public virtual ICache Create()
        {
            if (_caches == null)
            {
                lock (_syncObj)
                {
                    if (_caches == null)
                    {
                        var assemblyName = ConfigManager.GetCacheProvider();
                        if (String.IsNullOrWhiteSpace(assemblyName))
                        {
                            _caches = new NoCacheProvider();
                        }
                        else
                        {
                            var assemblys = assemblyName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            var handler = Activator.CreateInstance(assemblys[0], assemblys[1]);
                            var obj = handler.Unwrap();
                            _caches = obj as ICache;
                        }

                    }
                }
            }

            return _caches;
        }
    }
}
