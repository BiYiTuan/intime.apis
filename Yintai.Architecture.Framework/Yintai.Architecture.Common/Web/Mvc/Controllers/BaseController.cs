using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Web.Mvc.Attributes;
using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Architecture.Common.Web.Mvc.Controllers
{
    /// <summary>
    /// 所有的controller都必须继承BaseController
    /// </summary>
    [DefaultHandleError]
    public abstract class BaseController : System.Web.Mvc.Controller
    {
        protected BaseController()
        {
            Logger = LoggerManager.Current();
        }

        #region properties

        public ILog Logger { get; private set; }

        #endregion

        #region methods

        /// <summary>
        /// 提供获取Service的方法
        /// </summary>
        /// <typeparam name="TService">Service类型</typeparam>
        /// <returns>获取到的实例</returns>
        protected static TService GetService<TService>()
        {
            return ServiceLocator.Current.Resolve<TService>();
        }

        /// <summary>
        /// 提供获取Service的方法，根据键值
        /// </summary>
        /// <typeparam name="TService">Service类型</typeparam>
        /// <param name="key">指定的键值</param>
        /// <returns>获取到的实例</returns>
        protected static TService GetService<TService>(string key)
        {
            return ServiceLocator.Current.Resolve<TService>(key);
        }

        #endregion
    }
}