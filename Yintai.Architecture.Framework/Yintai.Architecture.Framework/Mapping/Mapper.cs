using System;
using System.Collections.Generic;
using System.Linq;
using EmitMapper;

namespace Yintai.Architecture.Framework.Mapping
{
    public interface IMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        TTarget Map<TSource, TTarget>(TSource source);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        TTarget Map<TSource, TTarget>(TSource source, IMappingEngine engine);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        TTarget Map<TSource, TTarget>(TSource source, Func<TSource, TTarget> mapper);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="sourceCollection"></param>
        /// <returns></returns>
        IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> sourceCollection);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        TTarget Map<TSource, TTarget>(TSource source, TTarget target);
    }

    public sealed class Mapper : IMapper
    {
        #region fields

        private static readonly IMappingEngine _engine;
        private static readonly ObjectMapperManager _emitMapper;
        private static readonly Mapper _instance;

        #endregion

        #region .ctor

        static Mapper()
        {
            _engine = new MappingEngine();
            _emitMapper = ObjectMapperManager.DefaultInstance;
            _instance = new Mapper();
        }

        private Mapper()
        {
        }

        #endregion

        #region properties

        public static IMappingEngine Engine
        {
            get { return _engine; }
        }

        public static Mapper Instance
        {
            get { return _instance; }
        }

        #endregion

        #region methods

        #endregion

        #region public static For Map

        public static TTarget Map<TSource, TTarget>(TSource source)
        {
            return _engine.Map<TSource, TTarget>(source);
        }

        public static TTarget Map<TSource, TTarget>(TSource source, IMappingEngine engine)
        {
            return engine.Map<TSource, TTarget>(source);
        }

        public static TTarget Map<TSource, TTarget>(TSource source, Func<TSource, TTarget> mapper)
        {
            return mapper(source);
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

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            return _emitMapper.GetMapper<TSource, TTarget>().Map(source, target);
        }

        #endregion

        #region Implementation of IMapper

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        TTarget IMapper.Map<TSource, TTarget>(TSource source, IMappingEngine engine)
        {
            return Map<TSource, TTarget>(source, engine);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        TTarget IMapper.Map<TSource, TTarget>(TSource source, Func<TSource, TTarget> mapper)
        {
            return Map<TSource, TTarget>(source, mapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="sourceCollection"></param>
        /// <returns></returns>
        IEnumerable<TTarget> IMapper.Map<TSource, TTarget>(IEnumerable<TSource> sourceCollection)
        {
            return Map<TSource, TTarget>(sourceCollection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        TTarget IMapper.Map<TSource, TTarget>(TSource source, TTarget target)
        {
            return Map<TSource, TTarget>(source, target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        TTarget IMapper.Map<TSource, TTarget>(TSource source)
        {
            return Map<TSource, TTarget>(source);
        }

        #endregion
    }
}
