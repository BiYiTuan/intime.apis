using System;
using System.Collections.Generic;

namespace Yintai.Architecture.Framework.ServiceLocation.Adapter
{
    /// <summary>
    /// 提供对Castle中的Windsor框架适配器类
    /// </summary>
    /// <remarks>Castle官方主页：http://www.castleproject.org/</remarks>
    public class WindsorServiceLocator : ServiceLocatorBase
    {
        public override string Name
        {
            get
            {
                return "Windsor";
            }
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
        /// <exception>在获取实例过程中抛出异常
        ///   <cref>ActivationException</cref>
        /// </exception>
        protected override void DoRegister<TService, TType>(string key)
        {
            throw new NotImplementedException();
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
        protected override TService DoResolve<TService>(string key)
        {
            throw new NotImplementedException();
        }

        protected override object DoResolve(Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        protected override IEnumerable<object> DoResolveAll(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
