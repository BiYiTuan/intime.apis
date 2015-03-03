using System.Web.Mvc;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.Apns;
using Yintai.Hangzhou.Contract.DTO.Request.Device;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class DeviceController : RestfulController
    {
        private readonly IApnsDataService _apnsDataService;

        public DeviceController(IApnsDataService apnsDataService)
        {
            this._apnsDataService = apnsDataService;
        }

        [RestfulAuthorize(true)]
        public ActionResult Register(DeviceRegisterRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authUser == null ? 0 : authUser.Id;
            request.AuthUser = authUser == null ? null : authUser;

            return new RestfulResult { Data = this._apnsDataService.Register(request) };
        }
    }
}
