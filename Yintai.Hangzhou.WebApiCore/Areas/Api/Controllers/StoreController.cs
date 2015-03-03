using System;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request.Store;
using Yintai.Hangzhou.Contract.Store;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class StoreController : RestfulController
    {
        private readonly IStoreDataService _storeDataService;

        public StoreController(IStoreDataService storeDataService)
        {
            this._storeDataService = storeDataService;
        }

        public RestfulResult Detail(StoreRequest request)
        {
            return new RestfulResult { Data = this._storeDataService.GetStore(request) };
        }

        public RestfulResult All(StoreGetAllRequest request)
        {
            if (String.IsNullOrEmpty(request.Type))
            {
                return new RestfulResult { Data = this._storeDataService.GetAll(request) };
            }

            if (request.Type.ToLower() == "refresh")
            {
                return Refresh(new StoreGetRefreshRequest
                {
                    Refreshts = request.Refreshts
                });
            }

            return new RestfulResult { Data = this._storeDataService.GetAll(request) };
        }

        public RestfulResult Refresh(StoreGetRefreshRequest request)
        {
            return new RestfulResult { Data = this._storeDataService.GetRefresh(request) };
        }
    }
}