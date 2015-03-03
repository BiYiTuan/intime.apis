using System;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class StoreModelBinder : ModelBinderBase
    {
        private readonly IStoreRepository _storeRepository;

        public StoreModelBinder(IStoreRepository service)
        {
            _storeRepository = service;
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
            return _storeRepository.GetItem(Int32.Parse(modelId));
        }

        #endregion
    }

    public class FetchStoreAttribute : UseBinderAttribute
    {
        public FetchStoreAttribute()
            : base(typeof(StoreModelBinder))
        {
        }
    }
}
