using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class TagModelBinder : ModelBinderBase
    {
        private readonly ITagRepository _tagRepository;

        public TagModelBinder(ITagRepository service)
        {
            this._tagRepository = service;
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
            return this._tagRepository.GetItem(Int32.Parse(modelId));
        }

        #endregion
    }

    public class FetchTagAttribute : UseBinderAttribute
    {
        public FetchTagAttribute()
            : base(typeof(TagModelBinder))
        {
        }
    }
}
