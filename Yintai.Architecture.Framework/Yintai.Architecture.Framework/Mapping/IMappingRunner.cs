using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Architecture.Framework.Mapping
{
    public interface IMappingRunner
    {
        ResolutionResult Map(ResolutionContext context);

        Func<List<IMappingResolver>> AllResolvers { get; }
    }
}
