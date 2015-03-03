using System.Web.Mvc;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.Card;
using Yintai.Hangzhou.Contract.DTO.Request.Card;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    [RestfulAuthorize]
    public class CardController : RestfulController
    {
        private readonly ICardDataService _cardDataService;

        public CardController(ICardDataService cardDataService)
        {
            _cardDataService = cardDataService;
        }

        [HttpPost]
        public RestfulResult Bind(FormCollection formCollection, BindingRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;
            request.AuthUid = authuid.Value;

            return new RestfulResult { Data = _cardDataService.Binding(request) };
        }

        [HttpGet]
        public RestfulResult Bind(BindingRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;
            request.AuthUid = authuid.Value;

            return new RestfulResult { Data = _cardDataService.Binding(request) };
        }

        //[HttpPost]
        //public RestfulResult UnBind(BindingRequest request, int? authuid, UserModel authUser)
        //{
        //    request.AuthUser = authUser;
        //    request.AuthUid = authuid.Value;

        //    return new RestfulResult { Data = this._cardDataService.UnBinding(request) };
        //}

        public RestfulResult Detail(GetCardInfoRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;
            request.AuthUid = authuid.Value;

            return new RestfulResult { Data = this._cardDataService.GetInfo(request) };
        }

    }
}
