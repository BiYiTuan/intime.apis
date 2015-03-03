using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.Group;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract.Apis;
using com.intime.fashion.service;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class StorePromotionController:RestfulController
    {
          private ICardRepository _cardRepo;
        private IGroupCardService _groupData;
        private IStoreCouponsRepository _storecouponRepo;
        private IStorePromotionRepository _storeproRepo;
        public StorePromotionController(ICardRepository cardRepo,
            IGroupCardService groupData,
            IStoreCouponsRepository storecouponRepo,
            IStorePromotionRepository storeproRepo)
        {
            _cardRepo = cardRepo;
            _groupData = groupData;
            _storecouponRepo = storecouponRepo;
            _storeproRepo = storeproRepo;
        }

        [RestfulAuthorize]
        public ActionResult Amount(ExchangeStoreCouponRuleRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;
            var storepromotion = _storeproRepo.Get(sp => sp.Id == request.StorePromotionId && sp.ActiveStartDate <= DateTime.Now && sp.ActiveEndDate >= DateTime.Now).FirstOrDefault();
            if (storepromotion == null)
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "促销活动无效!" }

                };
            if (storepromotion.MinPoints > request.Points)
            {
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "兑换积点需大于最小积点限制!" }

                }; 
            }

            return new RestfulResult { Data = new ExecuteResult<ExchangeStoreCouponRuleResponse>(new ExchangeStoreCouponRuleResponse() {
                Amount = StorePromotionRule.AmountFromPoints(request.StorePromotionId,request.Points)
            }) };
        }
        
    }
}
