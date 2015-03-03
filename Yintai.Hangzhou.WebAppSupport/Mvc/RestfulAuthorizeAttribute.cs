using System;
using System.Globalization;
using System.Web.Mvc;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Architecture.Framework.Utility;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.WebSupport.Mvc
{
    /// <summary>
    /// 验证用户认证，失败返回错误信息.默认终止当前action 执行,当设置为true，则action将继续进行下去
    /// </summary>
    public class RestfulAuthorizeAttribute : ActionFilterAttribute
    {
        private UserIdSessionData _sessionData;
        private static readonly ILog _log;
        private readonly bool _holdon;
        //private readonly IAuthenticationService _authorizeService;

        private UserModel _authUser;

        /// <summary>
        /// 验证用户认证，失败返回错误信息.默认终止当前action 执行
        /// </summary>

        public RestfulAuthorizeAttribute()
            : this(false)
        {
          
        }

        /// <summary>
        /// 验证用户认证，失败返回错误信息，当设置为true，则action将继续进行下去
        /// </summary>
        /// <param name="holdon">是否hold on </param>
        public RestfulAuthorizeAttribute(bool holdon)
        {
            this._holdon = holdon;
            //_authorizeService = ServiceLocator.Current.Resolve<IAuthenticationService>();
        }

        static RestfulAuthorizeAttribute()
        {
            _log = LoggerManager.Current();

        }

        protected ILog Logger
        {
            get { return _log; }
            private set { }
        }

        protected UserIdSessionData UserIdSession
        {
            get { return _sessionData; }
            private set { _sessionData = value; }
        }

        protected UserModel AuthUser
        {
            get { return _authUser; }
            private set { _authUser = value; }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ExecActionExecuting(filterContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ExecResultExecuted(filterContext);
        }

        public virtual void ExecActionExecuting(ActionExecutingContext filterContext)
        {
            //获取SessionKey
            var httpContext = filterContext.HttpContext;
            var token = httpContext.Request[Define.Token];

            if (String.IsNullOrEmpty(token) && !_holdon)
            {
                //filterContext.HttpContext.ClearError();
                filterContext.Result = new RestfulResult
                {
                    Data = new ExecuteResult
                    {
                        StatusCode = StatusCode.ClientError,
                        Message = String.Format("{0}为空", Define.Token)
                    }
                };
                return;
                //return false;
            }

            //验证用户信息
            try
            {
                _sessionData = SessionKeyHelper.Decrypt(token);
            }
            catch
            {
                _log.Error(String.Format("{0}解密失败 ", token));
            }

            // 解密失败返回结果
            if (_sessionData == null && !_holdon)
            {
                filterContext.Result = new RestfulResult
                {
                    Data = new ExecuteResult
                    {
                        StatusCode = StatusCode.Unauthorized,
                        Message = "您的身份验证失败."
                    }
                };

                return;
            }

            // Session过期
#if !DEBUG
            if (_sessionData != null && _sessionData.Expired && !_holdon)
            {
                filterContext.Result = new RestfulResult
                {
                    Data = new ExecuteResult
                    {
                        StatusCode = StatusCode.Unauthorized,
                        Message = "您已经很长时候没有使用啦,为保证你的账户安全,请重新登录."
                    }
                };

                return;
            }
#endif
            //TODO:可以通过ActionDescriptor获取参数的类型，这里约定好就可以了，没有必要去那样做


            var output = 0;

            if (_sessionData != null)
            {
                Int32.TryParse(_sessionData.UserId, out output);
            }

            if (_sessionData != null)
            {
                this._authUser = ServiceLocator.Current.Resolve<IUserService>().Get(Int32.Parse(_sessionData.UserId));
                if (this._authUser == null)
                {
                    filterContext.Result = new RestfulResult
                    {
                        Data = new ExecuteResult
                        {
                            StatusCode = StatusCode.Unauthorized,
                            Message = "您的身份验证失败."
                        }
                    };
                    return;
                }

                filterContext.ActionParameters[Define.AuthUser] = this._authUser;
                ////// 设置参数userId的值
                ////httpContext.Request.
                httpContext.Request.RequestContext.RouteData.Values.Add(Define.AuthUserId, output.ToString(CultureInfo.InvariantCulture));
                //httpContext.Request.Params.Add(Define.AuthUserId, output.ToString(CultureInfo.InvariantCulture));
                filterContext.ActionParameters[Define.AuthUserId] = output;
            }
            else
            {
                filterContext.ActionParameters[Define.AuthUser] = null;
                filterContext.ActionParameters[Define.AuthUserId] = null;
            }

            ExecActionRoleAuthorizeing(filterContext);
        }

        public virtual void ExecActionRoleAuthorizeing(ActionExecutingContext filterContext)
        {
        }

        public virtual void ExecResultRoleAuthorized(ResultExecutedContext filterContext)
        {
        }

        public virtual void ExecResultExecuted(ResultExecutedContext filterContext)
        {
            if (_sessionData == null)
            {
                return;
            }

            ExecResultRoleAuthorized(filterContext);
            //var result = filterContext.Result as RestfulResult;

            //if (result != null)
            //{
            //    var data = result.Data as ExecuteResult;

            //    // 当数据不为空，返回的SessionKey不为
            //    if (data != null && String.IsNullOrEmpty(data.))
            //    {
            //        // 用户id不为空时，进行重新返回sessionKey
            //        if (!string.IsNullOrEmpty(_sessionData.UserId))
            //        {
            //            data.UserId = SessionKeyHelper.Encrypt(_sessionData.UserId);
            //        }
            //    }
            //}
        }
    }


    //*/
}
