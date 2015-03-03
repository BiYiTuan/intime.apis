using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request.Feedback;
using Yintai.Hangzhou.Contract.Feedback;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class FeedbackController : RestfulController
    {
        private readonly IFeedbackDataService _feedbackDataService;

        public FeedbackController(IFeedbackDataService feedbackDataService)
        {
            this._feedbackDataService = feedbackDataService;
        }

        [RestfulAuthorize]
        public RestfulResult Create(FeedbackCreateRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            request.Contact = UrlDecode(request.Contact);
            request.Content = UrlDecode(request.Content);

            return new RestfulResult { Data = this._feedbackDataService.Create(request) };
        }
    }
}
