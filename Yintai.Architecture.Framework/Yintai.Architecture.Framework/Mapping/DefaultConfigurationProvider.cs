using System;
using System.Collections.Generic;
using System.Data;

namespace Yintai.Architecture.Framework.Mapping
{
    public class DefaultConfigurationProvider : IConfigurationProvider
    {
        #region IConfigurationProvider Members

        public List<IMappingRunner> AllMappingRunners
        {
            get { return MapperRegistry.AllMappingRunners(); }
        }

        public IMappingRunner FindRunner(Type type)
        {
            IMappingRunner runner = new PlainObjectMappingRunner();

            if (type == typeof(IDataReader))
            {
                runner = new DataReaderMappingRunner();
            }

            return runner;
        }

        #endregion

        #region Instance

        private static IConfigurationProvider provider;
        private static object sync = new object();

        public static IConfigurationProvider Current
        {
            get
            {
                if (provider == null)
                {
                    lock (sync)
                    {
                        if (provider == null)
                        {
                            provider = new DefaultConfigurationProvider();
                        }
                    }
                }

                return provider;
            }
        }

        #endregion
    }
}
