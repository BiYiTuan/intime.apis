using com.intime.fashion.service;
using com.intime.fashion.service.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class EnvironmentController : RestfulController
    {
        private IAuthKeysService _keyService;
        public EnvironmentController(IAuthKeysService keyService) {
            _keyService = keyService;
        }
        public ActionResult ServerDateTime()
        {
            return new RestfulResult
            {
                Data = new ExecuteResult<string>(DateTime.Now.ToString(Define.DateDefaultFormat))
                    {
                        StatusCode = StatusCode.Success,
                        Message = "成功"
                    }
            };
        }
        /// <summary>
        /// return all payment methods
        /// </summary>
        /// <returns></returns>
        public ActionResult Payments()
        {
            var result = Context.Set<PaymentMethodEntity>().Where(p => p.Status == (int)DataStatus.Normal)
                        .ToList()
                       .Select(p => new PaymentResponse().FromEntity<PaymentResponse>(p));
            var response = new PagerInfoResponse<PaymentResponse>(new PagerRequest(), result.Count())
            {
                Items = result.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<PaymentResponse>>(response) };
        }

        /// <summary>
        /// return all support shipping area
        /// </summary>
        /// <returns></returns>
        public ActionResult SupportShipments()
        {
            var linq = Context.Set<CityEntity>().Where(p => p.Status == (int)DataStatus.Normal && p.IsProvince == true)
                        .GroupJoin(Context.Set<CityEntity>().Where(p => p.Status == (int)DataStatus.Normal && p.IsCity.HasValue && p.IsCity.Value == true)
                                   .GroupJoin(Context.Set<CityEntity>().Where(p => p.Status == (int)DataStatus.Normal && p.IsProvince == false && p.IsCity.Value == false),
                                            o => o.Id,
                                            i => i.ParentId,
                                            (o, i) => new { C=o,D =i}),
                                o => o.Id,
                                i => i.C.ParentId,
                                (o, i) => new { P = o, C = i });
            var result = linq.ToList()
                        .Select(l => new GetShipCityDetailResponse().FromEntity<GetShipCityDetailResponse>(l.P, r => {
                            r.ProvinceName = l.P.Name;
                            r.Cities = l.C.Select(c => new ShipCityModel() { 
                                 Id = c.C.Id,
                                 CityName = c.C.Name,
                                 Districts = c.D.Select(d=>new ShipDistrictModel(){
                                     Id = d.Id,
                                      DistrictName = d.Name,
                                       ZipCode = d.ZipCode
                                 })
                            });
                        }));
            var response = new PagerInfoResponse<GetShipCityDetailResponse>(new PagerRequest(), result.Count())
            {
                Items = result.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<GetShipCityDetailResponse>>(response) };
        }

        /// <summary>
        /// return all support rma reasons
        /// </summary>
        /// <returns></returns>
        public ActionResult SupportRMAReasons()
        {
            var linq = Context.Set<RMAReasonEntity>().Where(p => p.Status == (int)DataStatus.Normal)
                       .Select(l => new RMAReasonResponse { 
                        Reason = l.Reason,
                        Id = l.Id
                       });
            var response = new PagerInfoResponse<RMAReasonResponse>(new PagerRequest(), linq.Count())
            {
                Items = linq.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<RMAReasonResponse>>(response) };
        }

        /// <summary>
        /// return all support ship vias
        /// </summary>
        /// <returns></returns>
        public ActionResult SupportShipvias()
        {
            
            var linq = Context.Set<ShipViaEntity>().Where(p => p.Status == (int)DataStatus.Normal)
                       .Select(l => new ShipViaResponse
                       {
                          Name = l.Name,
                           Id = l.Id,
                           IsOnline = l.IsOnline??false
                       });
            var response = new PagerInfoResponse<ShipViaResponse>(new PagerRequest(), linq.Count())
            {
                Items = linq.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<ShipViaResponse>>(response) };
        }
        /// <summary>
        /// return all preconfigured messages
        /// </summary>
        /// <returns></returns>
        public ActionResult Messages() {
            var linq = Context.Set<ConfigMsgEntity>()
                    .Where(c => c.Channel == "iphone")
                    .ToList()
                    .Select(l => new GetMessageDetailReponse() { 
                         Key =l.MKey,
                         Message = l.Message
                    });
           
            var response = new PagerInfoResponse<GetMessageDetailReponse>(new PagerRequest(), linq.Count())
            {
                Items = linq.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<GetMessageDetailReponse>>(response) };
        }

        /// <summary>
        /// return all supported invoice detail
        /// </summary>
        /// <returns></returns>
        public ActionResult SupportInvoiceDetails()
        {

            var invoices = new List<dynamic>();
            invoices.Add(new { id = 1, name="礼品"});
            invoices.Add(new { id = 2, name = "日用品" });
            invoices.Add(new { id = 3, name = "买什么开什么" });
            var response = new PagerInfoResponse<dynamic>(new PagerRequest(), invoices.Count())
            {
                Items = invoices.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<dynamic>>(response) };
        }

        public ActionResult GetAliPayKey(int groupId)
        {
            var key = _keyService.GetAlipayKey(groupId);
           return this.RenderSuccess<dynamic>(c => c.Data = new { 
                partner_id = key.ParterId,
                md5_key = key.Md5Key,
                seller_account = key.SellerAccount
            });
        }

        public ActionResult GetWeixinKey(int groupId)
        {
            var key = _keyService.GetWeixinPayKey(groupId);
            return this.RenderSuccess<dynamic>(c => c.Data = new
            {
                app_id = key.AppId,
                app_secret = key.AppSecret,
                pay_signkey = key.PaySignKey,
                parter_id = key.ParterId,
                parter_key = key.ParterKey
            });

        }
    }
}
