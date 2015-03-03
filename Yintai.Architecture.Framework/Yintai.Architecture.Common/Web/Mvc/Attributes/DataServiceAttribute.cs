using System.Web.Mvc;

namespace Yintai.Architecture.Common.Web.Mvc.Attributes
{
    public class DataServiceAttribute : ActionFilterAttribute
    {
        #region .ctor

        public DataServiceAttribute() { }

        #endregion

        #region override

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ParameterManager.Current.Validate(filterContext);
        }

        #endregion
    }
}