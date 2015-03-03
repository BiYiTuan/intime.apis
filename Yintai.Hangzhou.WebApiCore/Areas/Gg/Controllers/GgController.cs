using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Hangzhou.WebApiCore.Areas.Gg.Controllers
{
    public abstract class GgController : BaseController
    {
        protected ActionResult RestfulResult(ExecuteResult data)
        {
            return new RestfulResult
            {
                Data = data
            };
        }

        /// <summary>
        /// url 解码
        /// </summary>
        /// <param name="encodeString"></param>
        /// <returns></returns>
        public static string UrlDecode(string encodeString)
        {
            return String.IsNullOrWhiteSpace(encodeString) ? String.Empty : HttpUtility.UrlDecode(encodeString);
        }
        public DbContext Context
        {
            get
            {
                return ServiceLocator.Current.Resolve<DbContext>();
            }
        }
    }
}
