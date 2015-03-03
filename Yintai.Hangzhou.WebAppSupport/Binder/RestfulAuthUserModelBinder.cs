using System;
using System.Web;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Architecture.Framework.Utility;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class RestfulAuthUserModelBinder : ModelBinderBase
    {
        private readonly IUserService _userService;

        public RestfulAuthUserModelBinder(IUserService userService)
        {
            this._userService = userService;
        }

        protected override object GetModelInstance(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var userSessionData = SessionKeyHelper.Decrypt(token);
            if (userSessionData == null)
            {
                return null;
            }
#if !DEBUG
            if (userSessionData.Expired)
            {
                return null;
            }
#endif
            try
            {
                return this._userService.Get(Int32.Parse(userSessionData.UserId));
            }
            catch (Exception ex)
            {

                if (HttpContext.Current != null)
                {
                    Logger.Error("httpurl:" + HttpContext.Current.Request.Url.ToString());
                }
                while (ex != null)
                {
                    Logger.Error(ex);

                    ex = ex.InnerException;
                }

                throw new ArgumentException("在binder时获取用户信息失败");
            }
        }
    }

    public class FetchRestfulAuthUserAttribute : UseBinderAttribute
    {
        public FetchRestfulAuthUserAttribute()
            : base(typeof(RestfulAuthUserModelBinder))
        {
        }
    }
}