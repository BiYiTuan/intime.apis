using System;
using System.Collections.Generic;

namespace Yintai.Architecture.Framework.Mapping
{
    public static class MapperRegistry
    {
        public static Func<List<IMappingRunner>> AllMappingRunners = () =>
        {
            return new List<IMappingRunner>
			{
				new PlainObjectMappingRunner(),
				new DataReaderMappingRunner()
			};
        };
    }
}
