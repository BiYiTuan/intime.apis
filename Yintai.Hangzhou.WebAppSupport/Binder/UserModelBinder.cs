using System;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class UserModelBinder : ModelBinderBase
    {
        private readonly IUserService _userService;

        public UserModelBinder(IUserService userService)
        {
            this._userService = userService;
        }

        protected override object GetModelInstance(string modelId)
        {
            return this._userService.Get(Int32.Parse(modelId));
        }
    }

    public class FetchUserAttribute : UseBinderAttribute
    {
        public FetchUserAttribute()
            : base(typeof(UserModelBinder))
        {
        }
    }
}
