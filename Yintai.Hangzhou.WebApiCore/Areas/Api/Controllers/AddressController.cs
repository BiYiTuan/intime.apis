using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class AddressController : RestfulController
    {
        private ICustomerRepository _customerRepo;
        private IShippingAddressRepository _shippingRepo;
        public AddressController(ICustomerRepository customerRepo,
            IShippingAddressRepository shippingRepo)
        {
            _customerRepo = customerRepo;
            _shippingRepo = shippingRepo;
        }
        /// <summary>
        /// return current user's shipping address, limit to first 10
        /// </summary>
        /// <param name="authUser"></param>
        /// <returns></returns>
        [RestfulAuthorize]
        public RestfulResult My(UserModel authUser)
        {
            var linq = _customerRepo.Context.Set<ShippingAddressEntity>()
                 .Where(s => s.Status != (int)DataStatus.Deleted && s.UserId == authUser.Id)
                 .OrderByDescending(s => s.UpdateDate)
                 .Take(10).ToList()
                 .Select(s => new SelfAddressResponse().FromEntity<SelfAddressResponse>(s));
            var response = new PagerInfoResponse<SelfAddressResponse>(new PagerRequest(), linq.Count())
            {
                Items = linq.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<SelfAddressResponse>>(response) };

        }

        [RestfulAuthorize]
        public ActionResult Details(GetAddressDetailRequest request, UserModel authUser)
        {
            var linq = Context.Set<ShippingAddressEntity>()
                    .Where(s => s.Id == request.Id && s.UserId == authUser.Id).FirstOrDefault();
            if (linq == null)
                return this.RenderError(m => m.Message = "地址错误！");

            var result = new SelfAddressResponse().FromEntity<SelfAddressResponse>(linq);
            return new RestfulResult()
            {
                Data = new ExecuteResult<SelfAddressResponse>(result)
            };

        }
        [RestfulAuthorize]
        public ActionResult Create(CreateAddressRequest request, UserModel authUser)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var existAddressCount = Context.Set<ShippingAddressEntity>().Where(s => s.UserId == authUser.Id && s.Status == (int)DataStatus.Normal).Count();
            if (existAddressCount>= 8)
                return this.RenderError(r => r.Message ="您最多可维护8个地址！");
            var inserted = _shippingRepo.Insert(new ShippingAddressEntity()
            {
                ShippingAddress1 = request.ShippingAddress,
                ShippingCity = request.ShippingCity,
                ShippingCityId = request.ShippingCityId,
                ShippingContactPerson = request.ShippingContactPerson,
                ShippingContactPhone = request.ShippingContactPhone,
                ShippingProvince = request.ShippingProvince,
                ShippingProvinceId = request.ShippingProvinceId,
                ShippingZipCode = request.ShippingZipCode,
                ShippingDistrictId = request.ShippingDistrictId,
                ShippingDistrictName = request.ShippingDistrict,
                Status = (int)DataStatus.Normal,
                UpdateDate = DateTime.Now,
                UpdateUser = authUser.Id,
                UserId = authUser.Id
            });
            return this.RenderSuccess<SelfAddressResponse>(R=>R.Data=new SelfAddressResponse().FromEntity<SelfAddressResponse>(inserted));
            
        }

        [RestfulAuthorize]
        public ActionResult Delete(DeleteAddressRequest request, UserModel authUser)
        {
            var addressEntity = _shippingRepo.Get(s => s.Id == request.Id && s.UserId == authUser.Id).FirstOrDefault();
            if (addressEntity == null)
            {
                return this.RenderError(result =>
                {
                    result.Message = "地址不存在！";
                });
            }
            _shippingRepo.Delete(addressEntity);

            return this.RenderSuccess<SelfAddressResponse>(null);
        }

        [RestfulAuthorize]
        [HttpPost]
        public ActionResult Edit(CreateAddressRequest request, UserModel authUser)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var linq = Context.Set<ShippingAddressEntity>()
                    .Where(s => s.Id == request.Id && s.UserId == authUser.Id).FirstOrDefault();
            if (linq == null)
                return this.RenderError(m => m.Message = "地址错误！");
            linq.ShippingProvinceId = request.ShippingProvinceId;
            linq.ShippingProvince = request.ShippingProvince;
            linq.ShippingZipCode = request.ShippingZipCode;
            linq.ShippingContactPhone = request.ShippingContactPhone;
            linq.ShippingContactPerson = request.ShippingContactPerson;
            linq.ShippingCityId = request.ShippingCityId;
            linq.ShippingCity = request.ShippingCity;
            linq.ShippingAddress1 = request.ShippingAddress;
            linq.ShippingDistrictId = request.ShippingDistrictId;
            linq.ShippingDistrictName = request.ShippingDistrict;
            _shippingRepo.Update(linq);

            var result = new SelfAddressResponse().FromEntity<SelfAddressResponse>(linq);
            return this.RenderSuccess<SelfAddressResponse>(R => R.Data = result);
            
         
        }
    }
}
