using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request.Share;
using Yintai.Hangzhou.Contract.Share;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    [RestfulAuthorize]
    public class ShareController : RestfulController
    {
        private readonly IShareDataService _shareDataService;

        public ShareController(IShareDataService shareDataService)
        {
            this._shareDataService = shareDataService;
        }

        public RestfulResult Create(ShareCreateRequest request, int? authuid)
        {
            request.AuthUid = authuid.Value;
            return new RestfulResult { Data = this._shareDataService.Create(request) };
        }
    }
}