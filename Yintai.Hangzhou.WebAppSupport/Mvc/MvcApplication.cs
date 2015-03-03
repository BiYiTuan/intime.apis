using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.Routes;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.WebSupport.Binder;
using Yintai.Hangzhou.WebSupport.Ioc;

namespace Yintai.Hangzhou.WebSupport.Mvc
{
   
    public class WebApiTestApplication : MvcApplication
    {
        public WebApiTestApplication()
            : base("Yintai.Hangzhou.Tools.ApiTest.Controllers")
        {
        }
    }

    public abstract class MvcApplication : System.Web.HttpApplication
    {
        private readonly string _controller;

        protected MvcApplication()
            : this(String.Empty)
        {

        }

     
        protected MvcApplication(string defaultControllerNamespace)
        {
            _controller = defaultControllerNamespace;
        }

        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*alljs}", new { alljs = @".*\.js(/.*)?" });
            routes.IgnoreRoute("{*allcss}", new { allcss = @".*\.css(/.*)?" });
            routes.IgnoreRoute("{*alljpg}", new { alljpg = @".*\.jpg(/.*)?" });
            routes.IgnoreRoute("{*allgif}", new { allgif = @".*\.gif(/.*)?" });
            routes.IgnoreRoute("{*allpng}", new { allpng = @".*\.png(/.*)?" });

            routes.IgnoreRoute("{*allxls}", new { allxls = @".*\.xls(/.*)?" });
            //routes.IgnoreRoute("{*PRO_UPLOAD_TMPxls}", new { PRO_UPLOAD_TMPxls = @"(.*/)?PRO_UPLOAD_TMP.xls(/.*)?" });

            routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.RouteExistingFiles = false;

            RegisterRoutesC(routes);

            routes.MapLowerCaseUrlRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }// Parameter defaults
                , new[] { _controller }
            );
        }

        protected virtual void RegisterRoutesC(RouteCollection routes)
        {
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ServiceLocatorInit();

            CApplication_Start();
        }

        protected virtual void CApplication_Start()
        {
        }

        /// <summary>
        /// 服务定位 初始化
        /// </summary>
        protected void ServiceLocatorInit()
        {
            //Register

            IocRegisterRun.Current.Register();

            DependencyResolver.SetResolver(ServiceLocator.Current.Resolve<IDependencyResolver>());

            ModelBinders.Binders.Add(typeof(PagerRequest), ServiceLocator.Current.Resolve<PagerRequestBinder>());

            ModelBinderProviders.BinderProviders.Add(new XmlModelBinderProvider());
        }
    }
}
