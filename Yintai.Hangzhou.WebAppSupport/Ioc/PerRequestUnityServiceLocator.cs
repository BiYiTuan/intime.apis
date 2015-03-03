using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Framework.ServiceLocation;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;

namespace Yintai.Hangzhou.WebSupport.Ioc
{
    public class PerRequestUnityServiceLocator : ServiceLocatorBase
    {
        private const string HttpContextKey = "perRequestContainer";

        private readonly IUnityContainer _container;

        public PerRequestUnityServiceLocator()
        {
            _container = new UnityContainer();

            if (ConfigurationManager.GetSection(UnityConfigurationSection.SectionName) != null)
            {
                try
                {
                    var configuration = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
                    configuration.Configure(_container, "defaultContainer");
                }
                catch
                {
                    throw;
                }
            }
        }

        public object GetService(Type serviceType)
        {

            return IsRegistered(serviceType) ? ChildContainer.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (IsRegistered(serviceType))
            {
                yield return ChildContainer.Resolve(serviceType);
            }

            foreach (var service in ChildContainer.ResolveAll(serviceType))
            {
                yield return service;
            }
        }

        protected IUnityContainer ChildContainer
        {
            get
            {
                IUnityContainer childContainer = null;
                if (HttpContext.Current != null)
                {
                    var cachedContainer = HttpContext.Current.Items[HttpContextKey];

                    if (cachedContainer is IUnityContainer)
                    {
                        childContainer = cachedContainer as IUnityContainer;
                    }
                    else
                    {
                        HttpContext.Current.Items[HttpContextKey] = childContainer = _container.CreateChildContainer();
                    }
                }
               
                if (childContainer == null)
                    childContainer = _container ;
                

                return childContainer;
            }
        }

        public static void DisposeOfChildContainer()
        {
            var childContainer = HttpContext.Current.Items[HttpContextKey] as IUnityContainer;

            if (childContainer != null)
            {
                childContainer.Dispose();
            }
        }

        private bool IsRegistered(Type typeToCheck)
        {
            var isRegistered = true;

            if (typeToCheck.IsInterface || typeToCheck.IsAbstract)
            {
                isRegistered = ChildContainer.IsRegistered(typeToCheck);

                if (!isRegistered && typeToCheck.IsGenericType)
                {
                    var openGenericType = typeToCheck.GetGenericTypeDefinition();

                    isRegistered = ChildContainer.IsRegistered(openGenericType);
                }
            }

            return isRegistered;
        }

        protected override void DoRegister<TService, TType>(string key)
        {
            _container.RegisterType<TService, TType>();
        }

        protected override void DoRegisterSingleton<TService, TType>(string key)
        {
            _container.RegisterType<TService, TType>(new ContainerControlledLifetimeManager());
        }

        protected override object DoResolve(Type type)
        {
            return GetService(type);
        }

        protected override TService DoResolve<TService>(string key)
        {
            return (TService)GetService(typeof(TService));
        }

        protected override IEnumerable<object> DoResolveAll(Type type)
        {
            return GetServices(type);
        }

        public override string Name
        {
            get { return "PerRequestUnity"; }
        }
    }
}