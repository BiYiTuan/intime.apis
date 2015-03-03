using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Transactions;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.Extension;
using Yintai.Hangzhou.Contract.Card;
using Yintai.Hangzhou.Contract.DTO.Request.Card;
using Yintai.Hangzhou.Contract.DTO.Response.Card;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.Service.Contract.Apis;

namespace Yintai.Hangzhou.Service
{
    public class CardDataService : BaseService, ICardDataService
    {
        [DataContract]
        private class CardProfile
        {
            [DataMember(Name = "l")]
            public string Lvl { get; set; }

            [DataMember(Name = "t")]
            public string Type { get; set; }
        }

        private readonly IGroupCardService _groupCardService;
        private readonly ICardRepository _cardRepository;
        private readonly IUserService _userService;

        public CardDataService(IGroupCardService groupCardService, ICardRepository cardRepository, IUserService userService)
        {
            _groupCardService = groupCardService;
            _cardRepository = cardRepository;
            _userService = userService;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<CardInfoResponse> Binding(BindingRequest request)
        {
            //1.当前用户是否绑定过该卡
            //2.该卡是否被绑定过
            //3.验证是否可以绑定
            // 3.1 集团验证
            //4.入库
            //5.查询积点

            if (request == null || request.AuthUser == null)
            {
                return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = _cardRepository.GetItemByCard(request.CardNo, CardType.YintaiMemberCard, DataStatus.Normal);
            if (entity != null)
            {
                if (entity.User_Id != request.AuthUser.Id)
                {
                    return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "该会员卡已经被绑定" };
                }

                return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您已经绑定过该会员卡" };
            }

            var result = _groupCardService.GetInfo(new GroupCardInfoRequest
                {
                    CardNo = request.CardNo,
                    Passwd = request.Password
                });

            if (result.RetType == GroupCardRetType.Ok)
            {
                using (var ts = new TransactionScope())
                {
                    //入库
                    var e = _cardRepository.Insert(new CardEntity
                        {
                            CardNo = request.CardNo,
                            CardProfile = JsonExtension.ToJson_(new CardProfile
                                {
                                    Lvl = result.Lvl,
                                    Type = result.Type
                                }),
                            CreatedDate = DateTime.Now,
                            CreatedUser = request.AuthUser.Id,
                            Status = (int)DataStatus.Normal,
                            Type = (int)CardType.YintaiMemberCard,
                            UpdatedDate = DateTime.Now,
                            UpdatedUser = request.AuthUser.Id,
                            User_Id = request.AuthUser.Id
                        });
                    //成功后
                    _userService.SetCardBinder(e.User_Id, true);

                    ts.Complete();
                }

                return GetInfo(new GetCardInfoRequest
                    {
                        AuthUser = request.AuthUser,
                        CardNo = request.CardNo,
                        Client_Version = request.Client_Version,
                        Method = request.Method,
                        Token = request.Token
                    }, "恭喜您绑定会员卡成功");
            }

            return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "会员卡号或密码错" };
        }

        public ExecuteResult<CardInfoResponse> UnBinding(BindingRequest request)
        {

            throw new NotSupportedException("不支持该方法");

            if (request == null || request.AuthUser == null)
            {
                return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entities = _cardRepository.GetListForUserId(request.AuthUser.Id, CardType.YintaiMemberCard, DataStatus.Normal).ToList();

            if (entities.Count == 0)
            {
                return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "你还没有绑定银泰卡，请您先绑定" };
            }

            if (entities.Count > 1)
            {
                return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.InternalServerError, Message = "服务器开小差了，请您等等再试" };
            }

            var entity = entities[0];

            using (var ts = new TransactionScope())
            {
                var delEntity = _cardRepository.GetItem(entity.Id);
                delEntity.UpdatedDate = DateTime.Now;
                delEntity.UpdatedUser = request.AuthUser.Id;
                delEntity.Status = (int)DataStatus.Deleted;

                _cardRepository.Delete(delEntity);

                _userService.SetCardBinder(entity.User_Id, false);

                ts.Complete();
            }

            return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.Success, Message = "解除绑定成功" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="okMessage"></param>
        /// <returns></returns>
        private ExecuteResult<CardInfoResponse> GetInfo(GetCardInfoRequest request, string okMessage)
        {
            if (request == null || request.AuthUser == null)
            {
                return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entities = _cardRepository.GetListForUserId(request.AuthUser.Id, CardType.YintaiMemberCard, DataStatus.Normal).ToList();

            if (entities.Count == 0)
            {
                return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "你还没有绑定会员卡，请您先绑定" };
            }

            if (entities.Count > 1)
            {
                return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.InternalServerError, Message = "服务器开小差了，请您等等再试" };
            }

            var entity = entities[0];

            var pointResult = _groupCardService.GetPoint(new GroupCardPointRequest
            {
                CardNo = entity.CardNo
            });

            var cardProfile = JsonExtension.FromJson_<CardProfile>(entity.CardProfile);

            if (pointResult.Success)
            {
                var response = new CardInfoResponse
                {
                    Point = pointResult.Point,
                    CardLvl = cardProfile.Lvl,
                    CardNo = entity.CardNo,
                    CardType = cardProfile.Type,
                    User_Id = entity.User_Id,
                    LastDate = DateTime.Now,
                    Id = entity.Id
                };

                var result = new ExecuteResult<CardInfoResponse>(response);

                if (!String.IsNullOrEmpty(okMessage))
                {
                    result.Message = okMessage;
                }

                return result;
            }

            return new ExecuteResult<CardInfoResponse>(null) { StatusCode = StatusCode.InternalServerError, Message = String.Concat("查询积点失败,", pointResult.Desc) };
        }

        public ExecuteResult<CardInfoResponse> GetInfo(GetCardInfoRequest request)
        {
            return GetInfo(request, null);
        }
    }
}
