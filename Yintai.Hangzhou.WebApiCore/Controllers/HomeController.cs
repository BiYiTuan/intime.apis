using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.WebApiCore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           
            return Content("ok");
        }

        public ActionResult ServerT()
        {
            ViewBag.Message = "Welcome to Yintai Hangzhou";
            return View("About");
        }
    }
}
