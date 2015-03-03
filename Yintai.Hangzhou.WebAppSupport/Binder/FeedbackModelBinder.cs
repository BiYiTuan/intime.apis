using System;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class FeedbackModelBinder : ModelBinderBase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackModelBinder(IFeedbackRepository service)
        {
            _feedbackRepository = service;
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
            return _feedbackRepository.GetItem(Int32.Parse(modelId));
        }

        #endregion
    }

    public class FetchFeedbackAttribute : UseBinderAttribute
    {
        public FetchFeedbackAttribute()
            : base(typeof(FeedbackModelBinder))
        {
        }
    }
}
