using System;
using System.Collections.Generic;

namespace Yintai.Architecture.Framework.ServiceLocation
{
    /// <summary>
    /// 通用的Service Locator基类，所有的Adapter都应该从此类派生
    /// </summary>
    public abstract class ServiceLocatorBase : IServiceLocator
    {
        #region IServiceLocator Members

        /// <summary>
        /// 获取当前所使用的容器名称
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 向容器中注册 单例服务类型
        /// </summary>
        /// <typeparam name="TService">要注册的服务类型</typeparam>
        /// <typeparam name="TType">要注册的组件</typeparam>
        /// <exception>在获取实例过程中抛出异常
        ///   <cref>ActivationException</cref>
        /// </exception>
        public void RegisterSingleton<TService, TType>(string key) where TType : TService
        {
            OnRegistering(new ServiceEventArgs(key));
            this.DoRegisterSingleton<TService, TType>(key);
            OnRegistered(new ServiceEventArgs(key));
        }

        /// <summary>
        /// 向容器中注册 单例服务类型
        /// </summary>
        /// <typeparam name="TService">要注册的服务类型</typeparam>
        /// <typeparam name="TType">要注册的组件</typeparam>
        /// <exception>在获取实例过程中抛出异常
        ///   <cref>ActivationException</cref>
        /// </exception>
        public void RegisterSingleton<TService, TType>() where TType : TService
        {
            RegisterSingleton<TService, TType>(null);
        }

        /// <summary>
        /// 向容器中注册服务类型
        /// </summary>
        /// <typeparam name="TService">要注册的服务类型</typeparam>
        /// <typeparam name="TType">要注册的组件</typeparam>
        /// <exception>在获取实例过程中抛出异常
        ///   <cref>ActivationException</cref>
        /// </exception>
        public virtual void Register<TService, TType>() where TType : TService
        {
            this.Register<TService, TType>(null);
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
        public virtual void Register<TService, TType>(string key) where TType : TService
        {
            OnRegistering(new ServiceEventArgs(key));
            this.DoRegister<TService, TType>(key);
            OnRegistered(new ServiceEventArgs(key));
        }

        /// <summary>
        /// 通过<typeparamref name="TService"/>指定的服务类型获取服务实例
        /// </summary>
        /// <typeparam name="TService">要获取的对象服务类型</typeparam>
        /// <exception>在获取实例过程中抛出异常
        ///   <cref>ActivationException</cref>
        /// </exception>
        /// <returns>请求的服务实例</returns>
        public virtual TService Resolve<TService>()
        {
            return this.Resolve<TService>(null);
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
        public virtual TService Resolve<TService>(string key)
        {
            OnResolving(new ServiceEventArgs(key));
            var instance = this.DoResolve<TService>(key);
            OnResolved(new ServiceEventArgs(key));

            return instance;
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            OnResolving(new ServiceEventArgs(type.ToString()));
            var instance = this.DoResolve(type);
            OnResolved(new ServiceEventArgs(type.ToString()));

            return instance;
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            OnResolving(new ServiceEventArgs(type.ToString()));
            var instances = this.DoResolveAll(type);
            OnResolved(new ServiceEventArgs(type.ToString()));

            return instances;
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// 通过<paramref name="key"/>作为键值向容器中注册 单例服务类型
        /// </summary>
        /// <typeparam name="TService">要注册的服务类型</typeparam>
        /// <typeparam name="TType">要注册的组件</typeparam>
        /// <param name="key">所指定的键值</param>
        /// <remarks>派生于该类的子类，必须实现该方法</remarks>
        protected abstract void DoRegisterSingleton<TService, TType>(string key) where TType : TService;

        /// <summary>
        /// 通过<paramref name="key"/>作为键值向容器中注册服务类型
        /// </summary>
        /// <typeparam name="TService">要注册的服务类型</typeparam>
        /// <typeparam name="TType">要注册的组件</typeparam>
        /// <param name="key">所指定的键值</param>
        /// <remarks>派生于该类的子类，必须实现该方法</remarks>
        protected abstract void DoRegister<TService, TType>(string key) where TType : TService;

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
        protected abstract TService DoResolve<TService>(string key);

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        protected abstract object DoResolve(Type type);

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        protected abstract IEnumerable<object> DoResolveAll(Type type);

        #endregion

        #region Events

        /// <summary>
        /// 向容器中注册类型之前触发该事件
        /// </summary>
        public event EventHandler<ServiceEventArgs> Registering;

        /// <summary>
        /// 向容器中注册类型之后触发该事件
        /// </summary>
        public event EventHandler<ServiceEventArgs> Registered;

        /// <summary>
        /// 向容器中获取类型之前触发该事件
        /// </summary>
        public event EventHandler<ServiceEventArgs> Resolving;

        /// <summary>
        /// 向容器中获取类型之后触发该事件
        /// </summary>
        public event EventHandler<ServiceEventArgs> Resolved;

        #endregion

        #region Event Handlers

        /// <summary>
        /// 触发ServiceLocator的Registering事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnRegistering(ServiceEventArgs e)
        {
            EventHandler<ServiceEventArgs> handler = Registering;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// 触发ServiceLocator的Registered事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnRegistered(ServiceEventArgs e)
        {
            EventHandler<ServiceEventArgs> handler = Registered;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// 触发ServiceLocator的Resolving事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnResolving(ServiceEventArgs e)
        {
            EventHandler<ServiceEventArgs> handler = Resolving;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// 触发ServiceLocator的Resolved事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnResolved(ServiceEventArgs e)
        {
            EventHandler<ServiceEventArgs> handler = Resolved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
