using System;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class CommentModelBinder : ModelBinderBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentModelBinder(ICommentRepository service)
        {
            _commentRepository = service;
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
            return _commentRepository.GetItem(Int32.Parse(modelId));
        }

        #endregion
    }

    public class FetchCommentAttribute : UseBinderAttribute
    {
        public FetchCommentAttribute()
            : base(typeof(CommentModelBinder))
        {
        }
    }
}
