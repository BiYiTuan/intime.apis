using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class ResourceModelBinder : ModelBinderBase
    {
        private readonly IResourceRepository _ResourceRepository;

        public ResourceModelBinder(IResourceRepository service)
        {
            _ResourceRepository = service;
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
            return _ResourceRepository.GetItem(Int32.Parse(modelId));
        }

        #endregion
    }

    public class FetchResourceAttribute : UseBinderAttribute
    {
        public FetchResourceAttribute()
            : base(typeof(ResourceModelBinder))
        {
        }
    }
}
