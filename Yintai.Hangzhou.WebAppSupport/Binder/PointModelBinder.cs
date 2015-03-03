using System;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class PointModelBinder : ModelBinderBase
    {
        private readonly IPointRepository _pointRepository;

        public PointModelBinder(IPointRepository service)
        {
            _pointRepository = service;
        }

        #region Overrides of ModelBinderBase

        /// <summary>
        /// 根据模型标识获取模型实例
        /// </summary>
        /// <param name="modelId">
        /// 模型标识
        /// </param>
        /// <returns>
        /// 模型实例
        /// </returns>
        protected override object GetModelInstance(string modelId)
        {
            return _pointRepository.GetItem(Int32.Parse(modelId));
        }

        #endregion
    }

    public class FetchPointAttribute : UseBinderAttribute
    {
        public FetchPointAttribute()
            : base(typeof(PointModelBinder))
        {
        }
    }
}
