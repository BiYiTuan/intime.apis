using System.Web.Mvc;
using Yintai.Architecture.Common.Logger;

namespace Yintai.Architecture.Common.Web.Mvc.Attributes
{
    /// <summary>
    /// CLR Version: 4.0.30319.239
    /// NameSpace: Yintai.Architecture.Common.Web.Mvc.Attributes
    /// FileName: DefaultErrorAttribuage
    ///
    /// Created at 1/11/2012 6:19:41 PM
    /// </summary>
    public class DefaultHandleErrorAttribute : HandleErrorAttribute
    {
        #region fields

        #endregion

        #region .ctor

        #endregion

        #region properties

        #endregion

        #region methods

        #endregion

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var ex = filterContext.Exception;
            while (ex != null)
            {
                LoggerManager.Current().Error(ex);
                ex = ex.InnerException;
            }
        }
    }
}