using EmitMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Architecture.Framework
{
    public static class Mapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source)
        {
            return Map<TSource, TTarget>(source, null);
        }
        public static TTarget Map<TSource, TTarget>(TSource source,Action<TTarget> postHandler)
        {
            var target = AutoMapper.Mapper.DynamicMap<TTarget>(source);
            if (postHandler != null)
                postHandler(target);
            return target;
        }
        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TTarget>().Map(source, target);
        }
        public static IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> sourceCollection)
        {
            if (sourceCollection == null || !sourceCollection.Any())
                yield break;

            foreach (var item in sourceCollection)
            {
                yield return Map<TSource, TTarget>(item);
            }
        }

    }
}
