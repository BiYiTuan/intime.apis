using com.intime.fashion.common;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.Extension;
using Yintai.Hangzhou.Contract.Customer;
using Yintai.Hangzhou.Contract.DTO.Request.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class CustomerDataService : BaseService, ICustomerDataService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOutSiteCustomerRepository _outSiteCustomerRepository;
        private readonly IVerifyCodeRepository _verifyCodeRepository;
        private readonly IResourceService _resourceService;
        private readonly ILikeService _likeService;
        private readonly IUserService _userService;
        private readonly IPointService _pointService;

        public CustomerDataService(IPointService pointService, IUserService userService, ILikeService likeService, ICustomerRepository customerRepository, IOutSiteCustomerRepository outSiteCustomerRepository, IVerifyCodeRepository verifyCodeRepository, IResourceService resourceService)
        {
            _likeService = likeService;
            _customerRepository = customerRepository;
            _outSiteCustomerRepository = outSiteCustomerRepository;
            _verifyCodeRepository = verifyCodeRepository;
            _resourceService = resourceService;
            _userService = userService;
            _pointService = pointService;
        }

        #region methods

        private static readonly Random Rnd = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// 获取随机码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string GenerateCode(int length)
        {
            const string vCode = "012356789";

            var rndStr = String.Empty;

            for (var i = 0; i < length; i++)
                rndStr += vCode[Rnd.Next(0, vCode.Length)];

            return rndStr;
        }

        private UserModel GetUser(int userId)
        {
            var entity = _customerRepository.GetItem(userId);
            if (entity == null)
            {
                return null;
            }

            var model = MappingManager.UserModelMapping(entity);

            return model;
        }

        #endregion

        #region Implementation of ICustomerService

        public ExecuteResult<ShowCustomerInfoResponse> GetShowCustomer(ShowCustomerRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var entity = _customerRepository.GetItem(request.UserId);

            if (entity == null)
            {
                return new ExecuteResult<ShowCustomerInfoResponse>(null);
            }

            var userModel = MappingManager.UserModelMapping(entity);

            var response = MappingManager.ShowCustomerInfoResponseMapping(userModel);

            if (request.CurrentAuthUser != null)
            {
                //取是否关注当前
                var likeEntity = _likeService.Get(request.CurrentAuthUser.Id, request.UserId);
                response.IsLiked = likeEntity != null;
            }
            return new ExecuteResult<ShowCustomerInfoResponse>(response);
        }

        /// <summary>
        /// 外站用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<CustomerInfoResponse> OutSiteLogin(OutSiteLoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (String.IsNullOrWhiteSpace(request.OutsiteUid))
            {
                return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            if (request.OsType == OutsiteType.None)
            {
                return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            int userId;

            var outsiteEntity = _outSiteCustomerRepository.GetItem(request.OutsiteUid, (int)request.OsType);
            if (outsiteEntity == null)
            {
                //没有这个用户
                var utmp = _customerRepository.Insert(new UserEntity
                                {
                                    CreatedDate = DateTime.Now,
                                    CreatedUser = 0,
                                    EMail = String.Empty,
                                    LastLoginDate = DateTime.Now,
                                    Logo = String.Empty,
                                    Mobile = String.Empty,
                                    Id = 0,
                                    Nickname = request.OutsiteNickname,
                                    Name =
                                        String.Format("__{0}{1}",
                                                      ((int)EnumExtension.Parser<OutsiteType>(request.OutsiteType)).
                                                          ToString(CultureInfo.InvariantCulture), request.OutsiteUid),
                                    Password = Guid.NewGuid().ToString(),
                                    UpdatedDate = DateTime.Now,
                                    Status = (int)DataStatus.Normal,
                                    //默认达人
                                    UserLevel = (int)UserLevel.User,
                                    Description = String.Empty,
                                    Gender = (int)GenderType.Default
                                });

                if (utmp == null || utmp.Id < 1)
                {
                    return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.InternalServerError, Message = "创建用户失败" };
                }

                //创建
                var tmp = _outSiteCustomerRepository.Insert(new OutsiteUserEntity
                                                           {
                                                               CreatedDate = DateTime.Now,
                                                               CreatedUser = utmp.Id,
                                                               Description = String.Empty,
                                                               LastLoginDate = DateTime.Now,
                                                               AssociateUserId = utmp.Id,
                                                               OutsiteType = (int)request.OsType,
                                                               Status = (int)DataStatus.Normal,
                                                               OutsiteUserId = request.OutsiteUid
                                                           });

                if (tmp == null)
                {
                    return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.InternalServerError, Message = "2创建用户失败" };
                }

                _pointService.Insert(new PointHistoryEntity
                    {
                        Amount = 100,
                        CreatedDate = DateTime.Now,
                        CreatedUser = request.AuthUid,
                        Description = String.Empty,
                        Name = "恭喜您注册成功获得100积点",
                        PointSourceId = 0,
                        PointSourceType = (int)PointSourceType.System,
                        Status = (int)DataStatus.Normal,
                        User_Id = tmp.AssociateUserId,
                        UpdatedDate = DateTime.Now,
                        UpdatedUser = request.AuthUid,
                        Type = (int)PointType.Register
                    });


                userId = tmp.AssociateUserId;
            }
            else
            {
                userId = outsiteEntity.AssociateUserId;

                var customerEntity = _customerRepository.Find(userId);
                customerEntity.LastLoginDate = DateTime.Now;
                _customerRepository.Update(customerEntity);
            }

            return GetUserInfo(new GetUserInfoRequest
                {
                    AuthUid = userId,
                    AuthUser = null,
                    Method = null,
                    Token = null,
                    Client_Version = request.Client_Version
                });
        }



        public UserEntity OutSiteLogin2(OutSiteLoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (String.IsNullOrWhiteSpace(request.OutsiteUid))
            {
                return null;
            }

            if (request.OsType == OutsiteType.None)
            {
                return null;
            }

            int userId;
            var outsiteEntity = _outSiteCustomerRepository.GetItem(request.OutsiteUid, (int)request.OsType);
            if (outsiteEntity == null)
            {
                using (var ts = new TransactionScope())
                {
                    var utmp = _customerRepository.Insert(new UserEntity
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUser = 0,
                        EMail = String.Empty,
                        LastLoginDate = DateTime.Now,
                        Logo = String.Empty,
                        Mobile = String.Empty,
                        Id = 0,
                        Nickname = request.OutsiteNickname,
                        Name =
                            String.Format("__{0}{1}",
                                          ((int)EnumExtension.Parser<OutsiteType>(request.OutsiteType)).
                                              ToString(CultureInfo.InvariantCulture), request.OutsiteUid),
                        Password = Guid.NewGuid().ToString(),
                        UpdatedDate = DateTime.Now,
                        Status = (int)DataStatus.Normal,
                        UserLevel = (int)UserLevel.User,
                        Description = String.Empty,
                        Gender = (int)GenderType.Default
                    });

                    _outSiteCustomerRepository.Insert(new OutsiteUserEntity
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUser = utmp.Id,
                        Description = String.Empty,
                        LastLoginDate = DateTime.Now,
                        AssociateUserId = utmp.Id,
                        OutsiteType = (int)request.OsType,
                        Status = (int)DataStatus.Normal,
                        OutsiteUserId = request.OutsiteUid
                    });
                    ts.Complete();

                    return utmp;
                }
            }
            else
            {
                userId = outsiteEntity.AssociateUserId;
                var customerEntity = _customerRepository.Find(userId);
                customerEntity.LastLoginDate = DateTime.Now;

                _customerRepository.Update(customerEntity);
                return _customerRepository.GetItem(userId);
            }

           
        }
        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<CustomerInfoResponse> GetUserInfo(GetUserInfoRequest request)
        {
            var userModel = GetUser(request.AuthUid);
            if (userModel == null)
            {
                return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "获取用户信息失败" };
            }

            var result = new ExecuteResult<CustomerInfoResponse>();
            var response = MappingManager.CustomerInfoResponseMappingForReadCount(userModel);

            //modify: plug in template if user is the daogou
            if (response.UserLevel == (int)UserLevel.DaoGou)
            {
                var associateEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == response.Id).FirstOrDefault();
                if (associateEntity != null)
                {
                    response.TemplateId = associateEntity.TemplateId;
                    response.OperateRight = associateEntity.OperateRight;
                    response.AssociateId = associateEntity.Id;
                    response.MaxComboItems = ConfigManager.MAX_COMBO_ONLINE;                    
                }
            }
            result.Data = response;

            return result;
        }

        /// <summary>
        /// 绑定手机号，发送验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult VerifyBindMobile(VerifyBindMobileRequest request)
        {
            _verifyCodeRepository.Insert(new VerifyCodeEntity
                                                  {
                                                      Code = GenerateCode(6),
                                                      CreatedDate = DateTime.Now,
                                                      CreatedUser = 0,
                                                      Status = 1,
                                                      TryCount = 0,
                                                      Type = request.Type,
                                                      User_Id = request.AuthUid,
                                                      VerifyMode = request.Mode,
                                                      VerifySource = request.Moblie
                                                  });

            return new ExecuteResult
                       {
                           Message = "Ok",
                           StatusCode = StatusCode.Success
                       };
        }

        /// <summary>
        /// 绑定手机号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult BindMobile(BindMobileRequest request)
        {
            const int tryCount = 5;

            var entity = _verifyCodeRepository.GetItem(request.AuthUid, request.Type, request.Code, request.Moblie, request.Mode);

            if (entity == null)
            {
                return new ExecuteResult
                           {
                               StatusCode = StatusCode.Unauthorized,
                               Message = "验证失败"
                           };
            }

            //超过5分钟验证失败
            var t = (DateTime.Now - entity.CreatedDate);

            if (t.TotalMinutes > 5)
            {
                return new ExecuteResult
                {
                    StatusCode = StatusCode.Unauthorized,
                    Message = "验证失败"
                };
            }
            //重试次数 这个没实现。
            if (entity.TryCount > tryCount)
            {
                return new ExecuteResult
                {
                    StatusCode = StatusCode.Unauthorized,
                    Message = "验证失败"
                };
            }

            //删除验证码
            _verifyCodeRepository.Delete(entity);

            var userEntity = _customerRepository.GetItem(request.AuthUid);
            userEntity.Mobile = request.Moblie;
            userEntity.UpdatedDate = DateTime.Now;
            userEntity.UpdatedUser = request.AuthUid;

            //修改用户信息
            _customerRepository.Update(userEntity);

            return new ExecuteResult { StatusCode = StatusCode.Success, Message = "验证成功" };

        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<CustomerInfoResponse> UploadLogo(UploadLogoRequest request)
        {
            //直接替换掉以前的头像，使用新头像

            if (request.Files.Count == 0)
            {
                return new ExecuteResult<CustomerInfoResponse>(null)
                    {
                        StatusCode = StatusCode.ClientError,
                        Message = "图像文件不能为空"
                    };
            }

            if (request.Files.Count != 1)
            {
                return new ExecuteResult<CustomerInfoResponse>(null)
                {
                    StatusCode = StatusCode.ClientError,
                    Message = "您只能上传一张头像"
                };
            }
            if (!request.Type.HasValue ||
                request.Type.Value == Hangzhou.Contract.DTO.ThumnImageType.Logo)
            {
                var oldResource = _resourceService.Get(request.AuthUid, SourceType.CustomerPortrait);

                var resourceResult = _resourceService.Save(request.Files, request.AuthUid, 0, request.AuthUid,
                                                           SourceType.CustomerPortrait);

                if (resourceResult == null || resourceResult.Count == 0)
                {
                    Logger.Error(String.Format("保存用户LOGO资源文件异常,用户ID={0}", request.AuthUid));
                    return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.InternalServerError, Message = "保存文件异常" };
                }

                if (resourceResult.Count != 1)
                {
                    Logger.Warn(String.Format("用户上传头像为1张时，返回不止一个资源.用户Id={0},资源Id={1}", request.AuthUid, String.Join(",", resourceResult.Select(v => v.Id).ToArray())));
                }

                //删除旧resource
                foreach (var item in oldResource)
                {
                    _resourceService.Del(item.Id);
                }

                //update
                var userentity = _customerRepository.GetItem(request.AuthUid);
                userentity.Logo = Path.Combine(resourceResult[0].Domain, resourceResult[0].Name);
                userentity.UpdatedDate = DateTime.Now;
                userentity.UpdatedUser = request.AuthUser.Id;
                _customerRepository.Update(userentity);
                return GetUserInfo(new GetUserInfoRequest
                {
                    AuthUid = request.AuthUid,
                    AuthUser = null,
                    Method = null,
                    Token = null,
                    Client_Version = request.Client_Version
                });
            }
            else
            {
                ResourceInfoResponse logoBG =null;
                using (var ts = new TransactionScope())
                {
                    var oldResource = _resourceService.Get(request.AuthUser.Id, SourceType.CustomerThumbBackground).FirstOrDefault();
                    if (oldResource != null)
                        _resourceService.Del(oldResource.Id);
                    var resourceResult = _resourceService.Save(request.Files, request.AuthUser.Id, 0, request.AuthUser.Id,
                                                               SourceType.CustomerThumbBackground);
                    if (resourceResult == null || resourceResult.Count == 0)
                    {
                        Logger.Error(String.Format("保存用户LOGO资源文件异常,用户ID={0}", request.AuthUser.Id));
                        return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.InternalServerError, Message = "保存文件异常" };
                    }
                    logoBG = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(resourceResult.First());
                    ts.Complete();
                }
                
                var response = new CustomerInfoResponse()
                {
                    BackgroundLogo_r = logoBG
                };
                return new ExecuteResult<CustomerInfoResponse>(response);
            }
           
        }

        public ExecuteResult<CustomerInfoResponse> DestroyLogo(DestroyLogoRequest request)
        {
            if (!request.Type.HasValue ||
                request.Type.Value == Hangzhou.Contract.DTO.ThumnImageType.Logo)
            {
                var oldResource = _resourceService.Get(request.AuthUser.Id, SourceType.CustomerPortrait);

                if (oldResource == null || oldResource.Count == 0)
                {
                    return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您还没有上传过肖像" };
                }

                foreach (var item in oldResource)
                {
                    _resourceService.Del(item.Id);
                }

                return GetUserInfo(new GetUserInfoRequest
                {
                    AuthUid = request.AuthUser.Id,
                    AuthUser = null,
                    Method = null,
                    Token = null,
                    Client_Version = request.Client_Version
                });
            }
            else
            {
                var oldResource = _resourceService.Get(request.AuthUser.Id, SourceType.CustomerThumbBackground).FirstOrDefault();
                if (oldResource != null)
                    _resourceService.Del(oldResource.Id);
                return new ExecuteResult<CustomerInfoResponse>(null);
            }
        }

        public ExecuteResult<CustomerInfoResponse> Update(UpdateCustomerRequest request)
        {
            var user = _customerRepository.GetItem(request.AuthUid);

            if (user == null)
            {
                return new ExecuteResult<CustomerInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            user.Nickname = request.Nickname ?? String.Empty;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedUser = request.AuthUid;
            user.Gender = request.Gender ?? 0;
            user.Description = request.Desc ?? String.Empty;
            user.Mobile = request.Mobile ?? String.Empty;

            _customerRepository.Update(user);

            return GetUserInfo(new GetUserInfoRequest
                {
                    AuthUid = request.AuthUid,
                    AuthUser = request.AuthUser,
                    Method = request.Method,
                    Token = request.Token,
                    Client_Version = request.Client_Version
                });
        }

        #endregion
    }
}
