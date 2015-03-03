using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Architecture.Framework.Utility;
using Yintai.Hangzhou.Contract.Customer;
using Yintai.Hangzhou.Contract.DTO.Request.Customer;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.WebSupport.Binder;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
    /// FileName: CustomerController
    ///
    /// Created at 11/13/2012 11:18:37 AM
    /// Description: 
    /// </summary>
    public class CustomerController : RestfulController
    {
        private readonly ICustomerDataService _customerService;
        private ICustomerRepository _customerRepo;
        private IResourceRepository _resourceRepo;
        private IUserAccountRepository _useraccountRepo;
        private ILikeRepository _likeRepo;
        public CustomerController(ICustomerDataService customerService,
            ICustomerRepository customerRepo,
            IResourceRepository resourceRepo,
            IUserAccountRepository useraccountRepo,
            ILikeRepository likeRepo)
        {
            this._customerService = customerService;
            _customerRepo = customerRepo;
            _resourceRepo = resourceRepo;
            _useraccountRepo = useraccountRepo;
            _likeRepo = likeRepo;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RestfulResult OutSiteLogin(OutSiteLoginRequest request)
        {
            request.OutsiteNickname = UrlDecode(request.OutsiteNickname);

            var response = this._customerService.OutSiteLogin(request);

            return new RestfulResult { Data = response };
        }

        [RestfulAuthorize]
        public RestfulResult Detail(UpdateCustomerRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;

            if (String.Compare(request.Method, DefineRestfulMethod.Update, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //更新
                return new RestfulResult
                {
                    Data = this._customerService.Update(request)
                };


            }
           var linq = _customerRepo.Get(u => u.Id == authUser.Id)
                .GroupJoin(_resourceRepo.Get(r => r.SourceType == (int)SourceType.CustomerThumbBackground),
                        o => o.Id,
                        i => i.SourceId,
                        (o, i) => new { C = o, RB = i })
                .GroupJoin(_useraccountRepo.Get(ua => ua.Status != (int)DataStatus.Deleted),
                        o => o.C.Id,
                        i => i.User_Id,
                        (o, i) => new { C = o.C, RB = o.RB, UA = i });
           var response = from l in linq.ToList()
                          select new CustomerInfoResponse().FromEntity<CustomerInfoResponse>(l.C
                            , c => {
                                var bgThum = l.RB.FirstOrDefault();
                                if (bgThum != null)
                                {
                                    c.BackgroundLogo_r = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(bgThum);
                                }
                                c.CountsFromEntity(l.UA);
                                c.Token = SessionKeyHelper.Encrypt(authUser.Id.ToString(CultureInfo.InvariantCulture));
                               
                            }); ;

            return new RestfulResult
            {
                Data = new ExecuteResult<CustomerInfoResponse>(response.FirstOrDefault())
            };
        }

        [RestfulAuthorize]
        [HttpPost]
        public RestfulResult Portrait(PortraitRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;

            switch (request.Method.ToLower())
            {
                case DefineRestfulMethod.Create:
                    var r = new UploadLogoRequest(request) { Files = Request.Files };
                    return new RestfulResult { Data = this._customerService.UploadLogo(r) };
                case DefineRestfulMethod.Destroy:
                    var d = new DestroyLogoRequest(request);
                    return new RestfulResult { Data = this._customerService.DestroyLogo(d) };
            }

            return new RestfulResult { Data = new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "参数错误" } };
        }

        public RestfulResult Show(ShowCustomerRequest request, [FetchRestfulAuthUser(IsCanMissing = true, KeyName = Define.Token)]UserModel currentAuthUser)
        {
            return Daren(request, currentAuthUser);
        }

        public RestfulResult Daren(ShowCustomerRequest request, [FetchRestfulAuthUser(IsCanMissing = true, KeyName = Define.Token)]UserModel currentAuthUser)
        {
            request.CurrentAuthUser = currentAuthUser;

            var linq = _customerRepo.Get(u => u.Id == request.UserId)
                   .GroupJoin(_resourceRepo.Get(r => r.SourceType == (int)SourceType.CustomerThumbBackground),
                           o => o.Id,
                           i => i.SourceId,
                           (o, i) => new { C = o, RB = i })
                   .GroupJoin(_useraccountRepo.Get(ua => ua.Status != (int)DataStatus.Deleted),
                           o => o.C.Id,
                           i => i.User_Id,
                           (o, i) => new { C = o.C, RB = o.RB, UA = i });
            var response = from l in linq.ToList()
                           select new ShowCustomerInfoResponse().FromEntity<ShowCustomerInfoResponse>(l.C
                             , c =>
                             {
                                 var bgThum = l.RB.FirstOrDefault();
                                 if (bgThum != null)
                                 {
                                     c.BackgroundLogo_r = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(bgThum);
                                 }
                                 c.CountsFromEntity(l.UA);
                                 c.IsLiked = false;
                                 if (request.CurrentAuthUser != null)
                                 {
                                     var likeEntity = _likeRepo.Get(like=>like.LikeUserId==request.CurrentAuthUser.Id &&like.LikedUserId==request.UserId).FirstOrDefault();
                                     c.IsLiked = likeEntity != null;
                                 }

                             }); ;

            return new RestfulResult
            {
                Data = new ExecuteResult<ShowCustomerInfoResponse>(response.FirstOrDefault())
            };
        }

      
    }
}
