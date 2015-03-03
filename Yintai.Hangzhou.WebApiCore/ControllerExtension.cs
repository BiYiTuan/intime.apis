using com.intime.fashion.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;

namespace Yintai.Hangzhou.WebApiCore
{
    public static class ControllerExtension
    {
        public static ActionResult RenderError(this BaseController controller, Action<ExecuteResult> callback)
        {
            return CommonUtil.RenderError(callback);
        }
        public static ActionResult RenderSuccess<T>(this BaseController controller, Action<ExecuteResult<T>> callback)
        {
            return CommonUtil.RenderSuccess<T>(callback);
        }
    }
}
