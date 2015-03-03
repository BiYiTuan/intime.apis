using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.WebSupport.Mvc
{
    /// <summary>
    /// 带权限认证的 restful 权限认证通过后会添加usermodel authuser
    /// </summary>
    public class RestfulRoleAuthorizeAttribute : RestfulAuthorizeAttribute
    {
        private readonly UserRole _userRole;
        private readonly UserLevel _userLevel;
        private bool _hasRoleAuthorize;
        private bool _hasLevelAuthorize;

        /// <summary>
        /// 用户权限时，看用户等级
        /// </summary>
        /// <param name="userLevel">用户等级(如何用户为管理员也不会通过,必须是用户权限)</param>
        public RestfulRoleAuthorizeAttribute(UserLevel userLevel)
            : this(UserRole.Admin, userLevel)

        {
        }

        /// <summary>
        /// 权限认证
        /// </summary>
        /// <param name="userrole">只认用户权限</param>
        public RestfulRoleAuthorizeAttribute(UserRole userrole)
            : this(userrole, UserLevel.None)
        {
        }

        /// <summary>
        /// 用户权限认证，先对权限进行认证，当不通过时，看用户level
        /// </summary>
        /// <param name="userrole">用户权限</param>
        /// <param name="userLevel">用户等级</param>
        public RestfulRoleAuthorizeAttribute(UserRole userrole, UserLevel userLevel)
        {
            this._userRole = userrole;
            this._userLevel = userLevel;
        }

        public override void ExecActionRoleAuthorizeing(ActionExecutingContext filterContext)
        {
            if (UserIdSession == null || AuthUser == null)
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

            //权限判断
            if (this._userRole == UserRole.None)
            {
                return;
            }

            _hasRoleAuthorize = false;

            foreach (var ur in AuthUser.UserRoles)
            {
                if ((((int)_userRole & ur) != 0))
                {
                    _hasRoleAuthorize = true;
                    break;
                }
            }

            if (!_hasRoleAuthorize)
            {
                //需要认证一下当前用户的等级
                if (this._userLevel != UserLevel.None)
                {
                    if (((this._userLevel & AuthUser.Level) != 0))
                    {
                        _hasLevelAuthorize = true;
                        return;
                    }
                }

                filterContext.Result = new RestfulResult
                    {
                        Data = new ExecuteResult
                            {
                                StatusCode = StatusCode.Unauthorized,
                                Message = "您的权限认证失败."
                            }
                    };
                return;
            }
        }

        public override void ExecResultRoleAuthorized(ResultExecutedContext filterContext)
        {
            //base.ExecRoleAuthorized(filterContext);
        }
    }
}