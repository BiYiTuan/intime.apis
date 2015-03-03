using System.Web.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api
{
    public class ApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Api";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Api_default",
                "Api/{controller}/{action}/{method}",
                new { action = "Index", method = UrlParameter.Optional,app = UrlParameter.Optional },
                new[] { "Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers" }
            );
        }
    }
}
