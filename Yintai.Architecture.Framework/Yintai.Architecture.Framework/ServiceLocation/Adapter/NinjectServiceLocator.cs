using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Yintai.Architecture.Framework.ServiceLocation.Adapter
{
    /// <summary>
    /// CLR Version: 4.0.30319.239
    /// NameSpace: Yintai.Architecture.Framework.ServiceLocation.Adapter
    /// FileName: NinjectServiceLocator
    ///
    /// Created at 1/17/2012 3:37:25 PM
    /// 
    /// http://http://ninject.org/
    /// </summary>
    public class NinjectServiceLocator : ServiceLocatorBase
    {
        #region fields

        private readonly IKernel _kernel;

        #endregion

        #region .ctor

        /// <summary>
        /// 配置
        /// </summary>
        private NinjectServiceLocator()
        {
            this._kernel = new StandardKernel();
        }

        #endregion

        #region properties

        #endregion

        #region methods

        #endregion

        #region Overrides of ServiceLocatorBase

        /// <summary>
        /// 获取当前所使用的容器名称
        /// </summary>
        public override string Name
        {
            get { return "Ninject"; }
        }

        /// <summary>
        /// 通过<paramref name="key"/>作为键值向容器中注册 单例服务类型
        /// </summary>
        /// <typeparam name="TService">要注册的服务类型</typeparam>
        /// <typeparam name="TType">要注册的组件</typeparam>
        /// <param name="key">所指定的键值</param>
        /// <remarks>派生于该类的子类，必须实现该方法</remarks>
        protected override void DoRegisterSingleton<TService, TType>(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 通过<paramref name="key"/>作为键值向容器中注册服务类型
        /// </summary>
        /// <typeparam name="TService">要注册的服务类型</typeparam>
        /// <typeparam name="TType">要注册的组件</typeparam>
        /// <param name="key">所指定的键值</param>
        /// <remarks>派生于该类的子类，必须实现该方法</remarks>
        protected override void DoRegister<TService, TType>(string key)
        {
            this._kernel.Bind<TService>().To<TType>();
        }

        /// <summary>
        /// 通过<typeparamref>
        ///     <name>key</name>
        ///   </typeparamref> 给定的名称获取服务实例
        /// </summary>
        /// <typeparam name="TService">要获取的对象服务类型</typeparam>
        /// <param name="key">对象在容器中注册的名称</param>
        /// <exception>在获取实例过程中抛出异常
        ///   <cref>ActivationException</cref>
        /// </exception>
        /// <returns>请求的服务实例</returns>
        /// <remarks>派生于该类的子类，必须实现该方法</remarks>
        protected override TService DoResolve<TService>(string key)
        {
            return this._kernel.Get<TService>(key);
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        protected override object DoResolve(Type type)
        {
            return this._kernel.Get(type);
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        protected override IEnumerable<object> DoResolveAll(Type type)
        {
            return _kernel.GetAll(type);
        }

        #endregion
    }
}
