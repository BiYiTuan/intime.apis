using System;
using System.Collections.Generic;
using Yintai.Architecture.Framework.Mapping;

namespace Yintai.Architecture.Framework.Mapping
{
    public interface IConfigurationProvider
    {
        List<IMappingRunner> AllMappingRunners { get; }
        IMappingRunner FindRunner(Type type);
    }
}
