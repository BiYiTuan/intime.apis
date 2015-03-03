using System;
using System.Linq.Expressions;

namespace Yintai.Architecture.Framework.Mapping
{
    public class ResolutionContext
    {
        private readonly Type _sourceType;
        private readonly Type _targetType;
        private readonly ParameterExpression _parameter;

        public ResolutionContext(Type sourceType, Type targetType, ParameterExpression parameter)
        {
            this._sourceType = sourceType;
            this._targetType = targetType;
            this._parameter = parameter;
        }

        public Type SourceType
        {
            get { return this._sourceType; }
        }

        public Type TargetType
        {
            get { return this._targetType; }
        }

        public ParameterExpression Parameter
        {
            get { return this._parameter; }
        }
    }
}
