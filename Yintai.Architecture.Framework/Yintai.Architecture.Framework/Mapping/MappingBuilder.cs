using System;
using System.Linq.Expressions;

namespace Yintai.Architecture.Framework.Mapping
{
    internal static class MappingBuilder<TSource, TTarget>
    {
        private static readonly Func<TSource, TTarget> mapper;
        private static readonly Exception initializationException;

        internal static TTarget Map(TSource source)
        {
            if (initializationException != null)
            {
                throw initializationException;
            }

            return source == null ? default(TTarget) : mapper(source);
        }

        static MappingBuilder()
        {
            try
            {
                mapper = BuildMapper();
                initializationException = null;
            }
            catch (Exception e)
            {
                mapper = null;
                initializationException = e;
            }
        }

        private static Func<TSource, TTarget> BuildMapper()
        {
            Type sourceType = typeof(TSource);
            Type targetType = typeof(TTarget);
            ParameterExpression sourceParameter = Expression.Parameter(sourceType, "source");

            IConfigurationProvider provider = DefaultConfigurationProvider.Current;
            IMappingRunner runner = provider.FindRunner(sourceType);
            ResolutionContext context = new ResolutionContext(sourceType, targetType, sourceParameter);

            ResolutionResult result = runner.Map(context);

            Expression initializer = Expression.MemberInit(Expression.New(targetType), result.MemberBindings);
            Func<TSource, TTarget> func = Expression.Lambda<Func<TSource, TTarget>>(initializer, sourceParameter).Compile();
            
            return func;
        }

        private static Func<TSource,TTarget,TTarget> B2()
        {
            Type sourceType = typeof(TSource);
            Type targetType = typeof(TTarget);
            ParameterExpression sourceParameter = Expression.Parameter(sourceType, "source");

            IConfigurationProvider provider = DefaultConfigurationProvider.Current;
            IMappingRunner runner = provider.FindRunner(sourceType);
            ResolutionContext context = new ResolutionContext(sourceType, targetType, sourceParameter);

            ResolutionResult result = runner.Map(context);

            Expression initializer = Expression.MemberInit(Expression.New(targetType), result.MemberBindings);
            Func<TSource, TTarget, TTarget> func = Expression.Lambda<Func<TSource, TTarget, TTarget>>(initializer, sourceParameter).Compile();

            return func;
        }
    }
}
