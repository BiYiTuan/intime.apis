using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api2
{
    public class ImsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ims";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("weixin_complain",
                    "ims/complain",
                    new { controller = "WeiXin", action = "Complain", method = UrlParameter.Optional },
                    new[] { "Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers" }
                    );
            context.MapRoute("weixin_monitor",
                    "ims/monitor",
                    new { controller = "WeiXin", action = "Monitor", method = UrlParameter.Optional },
                    new[] { "Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers" }
                    );
            
            context.MapRoute(
                "Ims_default",
                "ims/{controller}/{action}/{method}",
                new { action = "Index", method = UrlParameter.Optional,app=UrlParameter.Optional },
                new[] { "Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers" }
            );
        }
    }
}
