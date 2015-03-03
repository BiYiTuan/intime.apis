using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Hangzhou.WebApiCore
{
    [Authorize2Filter]
    public abstract class Rest2Controller:BaseController
    {
        public DbContext Context
        {
            get
            {
                return ServiceLocator.Current.Resolve<DbContext>();
            }
        }
    }
}
