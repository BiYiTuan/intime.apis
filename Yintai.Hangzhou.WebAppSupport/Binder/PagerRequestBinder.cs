using System;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using System.Linq;
using Yintai.Hangzhou.WebSupport.Configuration;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Architecture.Common.Logger;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    /// <summary>
    /// 分页请求 绑定器
    /// </summary>
    public class PagerRequestBinder : IModelBinder
    {
        /// <summary>
        /// pagesize 在配置文件中的命名模板
        /// </summary>
        private const string PageSizeKeyInConfigTemplate = "pagesize_controller_action|{0}_{1}";


        /// <summary>
        /// 1、从url获取pagnumber 
        /// 2、配置文件加载对应的pagesize
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="bindingContext">
        /// The binding context.
        /// </param>
        /// <returns>
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // get pagenumber from form when httpost else from routedata

            var area = String.Empty;
            if (controllerContext.RouteData.DataTokens.Keys.Contains("area"))
            {
                area = controllerContext.RouteData.DataTokens["area"].ToString().ToLower();
            }

            string pageNumberRaw;

            if (controllerContext.HttpContext.Request.Params["page"] == null &&
                controllerContext.RouteData.Values["page"] == null)
            {
                pageNumberRaw = "1";
            }
            else
            {
                if (controllerContext.HttpContext.Request.Params["page"] !=null) 
                    pageNumberRaw = controllerContext.HttpContext.Request.Params["page"];
                else
                    pageNumberRaw = controllerContext.RouteData.Values["page"].ToString();
            }


            int pageNumber;
            int.TryParse(pageNumberRaw, out pageNumber);

            // get pagesize from config file
            var pageSizeKeyInConfig = String.Format(
                PageSizeKeyInConfigTemplate,
                String.IsNullOrEmpty(area) ? controllerContext.RouteData.Values["controller"] : String.Concat(area, "_", controllerContext.RouteData.Values["controller"]),
                controllerContext.RouteData.Values["action"]).ToLower();
            var pageSizeValueInConfig = ConfigManager.GetParamsValueOrDefault(pageSizeKeyInConfig, String.Empty);
            int pageSize ;
            if (!Int32.TryParse(pageSizeValueInConfig, out pageSize))
            {
                ServiceLocator.Current.Resolve<ILog>().Warn(
                    String.Format("miss config item of pagesize ,name:{0}", pageSizeKeyInConfig));
                pageSize = 10;
            }

            return new PagerRequest(pageNumber, pageSize);
        }
    }
}
