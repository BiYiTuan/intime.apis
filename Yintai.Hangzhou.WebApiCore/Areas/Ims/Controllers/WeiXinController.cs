using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Web.Mvc.Controllers;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public class WeiXinController:BaseController
    {
        [HttpPost]
        public ActionResult Complain()
        {
            using(var sr = new StreamReader(Request.InputStream))
            {
                Logger.Info("weixin complain request:");
                Logger.Info(sr.ReadToEnd());
            }
            return Content("ok");
        }
        [HttpPost]
        public ActionResult Monitor()
        {
            using (var sr = new StreamReader(Request.InputStream))
            {
                Logger.Info("weixin complain request:");
                Logger.Info(sr.ReadToEnd());
            }
           return Content("ok");
        }
    }
}
