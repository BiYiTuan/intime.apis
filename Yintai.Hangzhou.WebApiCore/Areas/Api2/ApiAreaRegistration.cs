using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api2
{
    public class ApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Api2";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Api2_default",
                "Api2/{controller}/{action}/{method}",
                new { action = "Index", method = UrlParameter.Optional,app=UrlParameter.Optional },
                new[] { "Yintai.Hangzhou.WebApiCore.Areas.Api2.Controllers" }
            );
        }
    }
}
