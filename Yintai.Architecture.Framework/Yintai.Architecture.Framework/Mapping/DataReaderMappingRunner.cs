using System;
using System.Collections.Generic;
using Yintai.Architecture.Framework.Mapping;

namespace Yintai.Architecture.Framework.Mapping
{
    public class DataReaderMappingRunner : IMappingRunner
    {
        #region IMappingRunner Members

        public ResolutionResult Map(ResolutionContext context)
        {
            throw new NotImplementedException();
        }

        public Func<List<IMappingResolver>> AllResolvers
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
