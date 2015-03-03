using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common;

namespace Yintai.Hangzhou.WebSupport.Filters
{
    public class VersionFilterAttribute:ActionFilterAttribute
    {
        public string ShouldGreater { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var clientVersion = filterContext.RequestContext.HttpContext.Request[Define.ClientVersion];
            if (string.IsNullOrEmpty(ShouldGreater))
                return;
            if (Version2Int(clientVersion) < Version2Int(ShouldGreater))
                throw new ArgumentException("version not support");
            
        }
        private int Version2Int(string version)
        {
            var versions = version.Split('.').Take(3).ToArray();
            int intVersion = 0;
            for (int i = 0; i < versions.Count(); i++)
            {
                intVersion += int.Parse(versions[i]) * (10 ^ (2-i));
            }
            return intVersion;

        }
    }
}
