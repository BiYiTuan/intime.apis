using System;
using System.Web;
using System.Web.Mvc;

namespace Yintai.Architecture.Common.Web.Mvc.Controllers
{

    public abstract class BackAdminBaseController : BaseController
    {
    }

    /// <summary>
    /// 要求必须登录的 controller
    /// </summary>
    public abstract class CustomerBaseController : BaseController
    {
        ///// <summary>
        ///// 当前登录用户
        ///// </summary>
        //private WebSiteUser _currentUser;

        ///// <summary>
        ///// Gets or sets AuthenticationService.
        ///// </summary>
        //public IAuthenticationService AuthenticationService { get; set; }

        ///// <summary>
        ///// 获取当前登录用户
        ///// </summary>
        //public WebSiteUser CurrentUser
        //{
        //    get { return this._currentUser ?? (this._currentUser = this.AuthenticationService.GetCurrentUser(base.HttpContext)); }
        //}

        /// <summary>
        /// 禁用 Response
        /// </summary>
        [Obsolete]
        public new HttpResponseBase Response
        {
            get
            {
                throw new NotSupportedException("禁止直接使用Response");
            }
        }

        /// <summary>
        /// 禁止直接使用Request
        /// </summary>
        [Obsolete]
        public new HttpRequestBase Request
        {
            get
            {
                throw new NotSupportedException("禁止直接使用Request");
            }
        }

        /// <summary>
        /// 禁止直接使用Session
        /// </summary>
        [Obsolete]
        public new HttpSessionStateBase Session
        {
            get
            {
                throw new NotSupportedException("禁止直接使用Session");
            }
        }

        /// <summary>
        /// 禁止直接使用HttpContext
        /// </summary>
        [Obsolete]
        public new HttpContextBase HttpContext
        {
            get
            {
                throw new NotSupportedException("禁止直接使用HttpContext");
            }
        }

        /// <summary>
        /// 禁止直接使用 ControllerContext.
        /// </summary>
        [Obsolete]
        public new ControllerContext ControllerContext
        {
            get
            {
                throw new NotSupportedException("禁止直接使用ControllerContext");
            }
        }
    }
}