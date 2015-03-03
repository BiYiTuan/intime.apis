using System;
using System.Globalization;
using System.Web.Mvc;

namespace Yintai.Architecture.Common.Web.Mvc.Binders
{
    /// <summary>
    /// 使用模型绑定器 Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class UseBinderAttribute : CustomModelBinderAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UseBinderAttribute"/> class.
        /// </summary>
        /// <param name="binderType">
        /// The binder type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 参数Null引用
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 自定义绑定类型未实现接口IModelBinder
        /// </exception>
        public UseBinderAttribute(Type binderType)
        {
            if (binderType == null)
            {
                throw new ArgumentNullException("binderType");
            }

            if (!typeof(IModelBinder).IsAssignableFrom(binderType))
            {
                var message = String.Format(CultureInfo.CurrentCulture, "自定义绑定类型{0}未实现接口IModelBinder", binderType.FullName);
                throw new ArgumentException(message, "binderType");
            }

            this.BinderType = binderType;
        }

        /// <summary>
        /// Gets BinderType.
        /// </summary>
        public Type BinderType { get; private set; }

        /// <summary>
        /// 标识名。form querystring etc..
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool IsCanMissing { get; set; }

        /// <summary>
        /// Retrieves the associated model binder.
        /// </summary>
        /// <returns>
        /// A reference to an object that implements the <see cref="T:System.Web.Mvc.IModelBinder"/> interface.
        /// </returns>
        public override IModelBinder GetBinder()
        {
            try
            {
                var modelBinder = (IModelBinder)DependencyResolver.Current.GetService(this.BinderType);
                var asModelBinderBase = modelBinder as ModelBinderBase;
                if (asModelBinderBase == null)
                {
                    return modelBinder;
                }

                asModelBinderBase.KeyName = this.KeyName;
                asModelBinderBase.IsCanMissing = this.IsCanMissing;

                return asModelBinderBase;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "获取模型绑定器{0}失败", this.BinderType.FullName), ex);
            }
        }
    }
}