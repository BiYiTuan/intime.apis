using System;
using System.Web;
using System.Web.Security;
using Yintai.Architecture.Framework.Extension;
using Yintai.Architecture.Framework.Utility;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service.Impl
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
       
        public void Signout(HttpContextBase httpContext)
        {
            FormsAuthentication.SignOut();
        }

        public void SetAuthorize(HttpContextBase httpContext, WebSiteUser webSiteUser)
        {
            if (webSiteUser == null)
            {
                throw new ArgumentNullException("webSiteUser");
            }

            FormsAuthentication.SetAuthCookie(webSiteUser.ToJson(), false);
        }

        public WebSiteUser GetCurrentUser(HttpContextBase httpContext)
        {
            if (Islogged(httpContext))
            {
                var siteUser = httpContext.User.Identity.Name.FromJson<WebSiteUser>();
                if (siteUser == null)
                {
                    Logger.Error(String.Format("WebSiteUser反序列化失败str:{0}", httpContext.User.Identity.Name));
                }

                return siteUser;
            }

            return null;
        }
        public WebSiteUser CurrentUserFromHttpContext(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;
            return httpContext.User.Identity.Name.FromJson<WebSiteUser>();
        }

        public bool Islogged(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }

            return false;
        }

        public string GetLoginUrl(HttpContextBase httpContext)
        {
            return FormsAuthentication.LoginUrl;
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserIdSessionData Decrypt(string token)
        {
            //验证用户信息
            UserIdSessionData sessionData = null;
            try
            {
                sessionData = SessionKeyHelper.Decrypt(token);
            }
            catch
            {
                Logger.Error(String.Format("{0}解密失败 ", token));
            }

            return sessionData;
        }

        /// <summary>
        /// 加码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(UserIdSessionData data)
        {
            return SessionKeyHelper.Encrypt(data);
        }

        public string Encrypt(string userid)
        {
            return SessionKeyHelper.Encrypt(userid);
        }
    }
}
