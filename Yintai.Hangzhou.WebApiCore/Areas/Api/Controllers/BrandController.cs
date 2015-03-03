using System;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.Brand;
using Yintai.Hangzhou.Contract.DTO.Request.Brand;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class BrandController : RestfulController
    {
        private readonly IBrandDataService _brandDataService;

        public BrandController(IBrandDataService brandDataService)
        {
            _brandDataService = brandDataService;
        }

        public RestfulResult Detail(BrandDetailRequest request)
        {
            return new RestfulResult { Data = _brandDataService.GetBrand(request) };
        }

        public RestfulResult All(BrandAllRequest request)
        {
            if (String.IsNullOrEmpty(request.Type))
            {
                return new RestfulResult { Data = _brandDataService.GetAll(request) };
            }

            if (request.Type.ToLower() == "refresh")
            {
                return Refresh(new BrandRefreshRequest
                    {
                        Refreshts = request.Refreshts
                    });
            }

            return new RestfulResult { Data = _brandDataService.GetAll(request) };
        }

        public RestfulResult Refresh(BrandRefreshRequest request)
        {
            return new RestfulResult { Data = _brandDataService.GetRefresh(request) };
        }

        public RestfulResult GroupAll(BrandAllRequest request)
        {
            if (String.IsNullOrEmpty(request.Type))
            {
                return new RestfulResult { Data = _brandDataService.GetAll4Group(request) };
            }

            if (request.Type.ToLower() == "refresh")
            {
                return GroupRefresh(new BrandRefreshRequest
                {
                    Refreshts = request.Refreshts
                });
            }

            return new RestfulResult { Data = _brandDataService.GetAll(request) };
        }

        public RestfulResult GroupRefresh(BrandRefreshRequest request)
        {
            return new RestfulResult { Data = _brandDataService.GetRefresh4Group(request) };
        }
    }
}