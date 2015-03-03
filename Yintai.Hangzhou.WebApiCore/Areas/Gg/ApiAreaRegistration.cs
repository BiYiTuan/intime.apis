using System.Web.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Gg
{
    public class ApiAreaRegistration : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Gg_default",
                "Gg/{controller}/{action}/{method}",
                new { action = "Index", method = UrlParameter.Optional, app = UrlParameter.Optional },
                new[] { "Yintai.Hangzhou.WebApiCore.Areas.Gg.Controllers" }
            );
        }

        public override string AreaName
        {
            get { return "Gg"; }
        }
    }
}
