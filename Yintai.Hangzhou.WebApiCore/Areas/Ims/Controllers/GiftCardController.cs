using com.intime.fashion.common.Extension;
using com.intime.o2o.data.exchange.IT;
using com.intime.o2o.data.exchange.IT.Request;
using com.intime.o2o.data.exchange.IT.Request.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public class GiftCardController : RestfulController
    {
        private static string GIFTCARD_STORE_ID = ConfigurationManager.AppSettings["GIFTCARD_STORE_ID"];
        private ICustomerRepository _customerRepo;
        private IResourceRepository _resourceRepo;
        private IEFRepository<OrderTransactionEntity> _orderTranRepo;
        private IEFRepository<IMS_GiftCardEntity> _cardRepo;
        private IEFRepository<IMS_GiftCardOrderEntity> _orderRepo;
        private IEFRepository<IMS_GiftCardTransfersEntity> _transRepo;
        private IEFRepository<IMS_GiftCardUserEntity> _userRepo;
        private IEFRepository<IMS_GiftCardItemEntity> _itemRepo;
        private IEFRepository<IMS_GiftCardRechargeEntity> _rechargeRepo;
        private IApiClient _apiClient;

        private string _dateFormmat = "yyyy-MM-dd HH:mm";

        public GiftCardController(
            ICustomerRepository customerRepo,
            IResourceRepository resourceRepo,
            IEFRepository<OrderTransactionEntity> orderTranRepo,
            IEFRepository<IMS_GiftCardEntity> cardRepo,
            IEFRepository<IMS_GiftCardOrderEntity> orderRepo,
            IEFRepository<IMS_GiftCardTransfersEntity> transRepo,
            IEFRepository<IMS_GiftCardUserEntity> userRepo,
            IEFRepository<IMS_GiftCardItemEntity> itemRepo,
            IEFRepository<IMS_GiftCardRechargeEntity> rechargeRepo
            )
        {
            this._customerRepo = customerRepo;
            this._resourceRepo = resourceRepo;
            this._orderRepo = orderRepo;
            this._transRepo = transRepo;
            this._orderTranRepo = orderTranRepo;
            this._cardRepo = cardRepo;
            this._userRepo = userRepo;
            this._itemRepo = itemRepo;
            this._rechargeRepo = rechargeRepo;
            this._apiClient = new DefaultApiClient();
        }

        [RestfulAuthorize]
        public ActionResult IsBind(string phone, int authuid)
        {
            return this.RenderSuccess<dynamic>(c => c.Data = new { is_binded = IsPhoneBinded(phone) }); 
        }

        [RestfulAuthorize]
        public ActionResult Bind(string phone, int authuid)
        {
            try
            {
                var cardAccount = _userRepo.Find(x => x.GiftCardAccount == phone || x.UserId == authuid);
                if (cardAccount != null)
                {
                    if (cardAccount.UserId == authuid && cardAccount.GiftCardAccount == phone)
                    {
                        return this.RenderError(x => x.Message = "您已绑定此手机号!");
                    }
                    if (cardAccount.UserId == authuid)
                    {
                        return this.RenderError(x => x.Message = "一个账号只能绑定一个手机号!");
                    }
                    return this.RenderError(x => x.Message = "此手机号已被其他用户绑定.");
                }
                if (IsPhoneBinded(phone))
                {
                    try
                    {
                        var user = _customerRepo.Find(x => x.Id == authuid);
                        
                        _userRepo.Insert(new IMS_GiftCardUserEntity()
                        {
                            UserId = authuid,
                            CreateDate = DateTime.Now,
                            GiftCardAccount = phone,
                            Name = user.Nickname
                        });
                        var balance = GetUserAccountBalance(phone);
                        return this.RenderSuccess<dynamic>(c => c.Data = new
                        {
                            amount = balance,
                            phone
                        });
                    }
                    catch (GetUserBalanceExcepiton ex)
                    {
                        Logger.Error(ex);
                        return this.RenderError(r => r.Message = ex.Message);
                    }
                }
                return this.RenderError(r => r.Message = "绑定失败 ，没有团购账户信息.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return this.RenderError(r => r.Message = "绑定用户手机失败，请联系管理员!");
            }
        }

        [RestfulAuthorize]
        public ActionResult My(int authuid)
        {
            var cardAccount = _userRepo.Get(t => t.UserId == authuid).FirstOrDefault();

            if (cardAccount == null)
            {
                return this.RenderError(r => r.Message = "未绑定充值卡账号，请先绑定!");
            }

            try
            {
                var balance = GetUserAccountBalance(cardAccount.GiftCardAccount);
                return this.RenderSuccess<dynamic>(c => c.Data = new
                {
                    is_binded = true,
                    amount = balance,
                    phone = cardAccount.GiftCardAccount
                });

            }
            catch (GetUserBalanceExcepiton ex)
            {
                Logger.Error(ex);
                return this.RenderError(r => r.Message = ex.Message);
            }

        }

        [RestfulAuthorize]
        public ActionResult Create(string phone, string pwd, string charge_no, string identity_no, int authuid)
        {
            //if (IsPhoneBinded(phone))
            //{
            //    return this.RenderError(r => r.Message = "账号已创建，不能重复创建，您可以直接充值");
            //}

            IMS_GiftCardOrderEntity giftCardOrder = _orderRepo.Find(t => t.No == charge_no);
            if (giftCardOrder == null)
            {
                return this.RenderError(r => r.Message = "不存在该礼品卡!");
            }

            if (giftCardOrder.Status == (int)GiftCardOrderStatus.Recharge)
            {
                return this.RenderError(r => r.Message = "该礼品卡已充值!");
            }

            var orderItem = _itemRepo.Find(giftCardOrder.GiftCardItemId);

            if (orderItem == null)
            {
                return this.RenderError(r => r.Message = "不存在该礼品卡!");
            }

            OrderTransactionEntity orderTran = _orderTranRepo.Find(t => t.OrderNo == giftCardOrder.No);

            if (orderTran == null)
            {
                return this.RenderError(r => r.Message = "礼品卡没有支付信息!");
            }

            try
            {
                var rechargeRequest = new RechargeRequest()
                {
                    Data = new RechargeEntity()
                    {
                        Phone = phone,
                        Amount = Convert.ToInt32(orderItem.UnitPrice * 100),
                        Discount = Convert.ToInt32((orderItem.UnitPrice - orderTran.Amount) * 100),
                        IdCard = identity_no,
                        Password = pwd,
                        StoreCode = GIFTCARD_STORE_ID,
                        TotalPay = Convert.ToInt32(orderTran.Amount * 100),
                        TransCode = orderTran.TransNo
                    }
                };
                var rsp = _apiClient.Post(rechargeRequest);
                if (!rsp.Status)
                {
                    return this.RenderError(r => r.Message = rsp.Message);
                }

                //允许多次赠送会有bug
                var transfer = _transRepo.Find(x => x.OrderNo == charge_no && x.IsActive == 0 && x.IsDecline == 0);

                using (var ts = new TransactionScope())
                {
                    giftCardOrder.Status = (int)GiftCardOrderStatus.Recharge;
                    giftCardOrder.OwnerUserId = authuid;
                    _orderRepo.Update(giftCardOrder);

                    var user = _customerRepo.Find(x => x.Id == authuid);

                    _userRepo.Insert(new IMS_GiftCardUserEntity()
                    {
                        UserId = authuid,
                        CreateDate = DateTime.Now,
                        GiftCardAccount = phone,
                        Name = user.Nickname
                    });
                    _rechargeRepo.Insert(new IMS_GiftCardRechargeEntity()
                    {
                        OrderNo = giftCardOrder.No,
                        ChargePhone = phone,
                        ChargeUserId = authuid,
                        CreateDate = DateTime.Now,
                        CreateUser = authuid
                    });

                    if (transfer != null)
                    {
                        transfer.ToUserId = authuid;
                        transfer.OperateDate = DateTime.Now;
                        transfer.OperateUser = authuid;
                        transfer.IsActive = 1;
                        _transRepo.Update(transfer);
                    }

                    ts.Complete();
                }

                return this.RenderSuccess<dynamic>(null);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Recharge eorror, card no :({0}), phone ({1})", charge_no, phone));
                Logger.Error(ex);
                return this.RenderError(r => r.Message = ex.Message);
            }
        }

        [RestfulAuthorize]
        public ActionResult Recharge(string charge_no, int authuid)
        {
            Logger.Error(string.Format("Recharge orderno ({0})", charge_no));
            var cardUser = _userRepo.Find(x => x.UserId == authuid);

            if (cardUser == null)
            {
                return this.RenderError(r => r.Message = "用户未绑定手机账号!");
            }

            IMS_GiftCardOrderEntity order = _orderRepo.Find(x => x.No == charge_no);
            if (order == null)
            {
                return this.RenderError(r => r.Message = string.Format("无效的充值卡编码： ({0})", charge_no));
            }
            if (order.Status == (int)GiftCardOrderStatus.Recharge)
            {
                return this.RenderError(r => r.Message = "礼品卡已经充值!");
            }

            var orderItem = _itemRepo.Find(order.GiftCardItemId);

            if (orderItem == null)
            {
                return this.RenderError(r => r.Message = "不存在的礼品卡!");
            }
            OrderTransactionEntity orderTran = _orderTranRepo.Find(x => x.OrderNo == order.No);
            if (orderTran == null)
            {
                return this.RenderError(r => r.Message = "礼品卡无支付信息!");
            }
            try
            {
                var rechargeRequest = new RechargeRequest
                {
                    Data = new RechargeEntity()
                    {
                        Phone = cardUser.GiftCardAccount,
                        Amount = Convert.ToInt32(orderItem.UnitPrice * 100),
                        Discount = Convert.ToInt32((orderItem.UnitPrice - orderTran.Amount) * 100),
                        StoreCode = GIFTCARD_STORE_ID,
                        TotalPay = Convert.ToInt32(orderTran.Amount * 100),
                        TransCode = orderTran.TransNo
                    }
                };


                var transfer = _transRepo.Find(x => x.IsDecline == 0 && x.IsActive == 0 && x.OrderNo == charge_no);

                using (var ts = new TransactionScope())
                {
                    order.Status = (int)GiftCardOrderStatus.Recharge;
                    order.OwnerUserId = authuid;
                    _orderRepo.Update(order);
                    _rechargeRepo.Insert(new IMS_GiftCardRechargeEntity()
                    {
                        OrderNo = order.No,
                        ChargePhone = cardUser.GiftCardAccount,
                        ChargeUserId = authuid,
                        CreateDate = DateTime.Now,
                        CreateUser = authuid
                    });

                    if (transfer != null)
                    {
                        transfer.IsActive = 1;
                        transfer.OperateDate = DateTime.Now;
                        transfer.OperateUser = authuid;
                        transfer.ToUserId = authuid;
                        _transRepo.Update(transfer);
                    }
                    try
                    {
                        var rsp = _apiClient.Post(rechargeRequest);

                        if (!rsp.Status)
                        {
                            Logger.Error("充值失败：信息部返回错误：");
                            Logger.Error(rsp.ErrorMessage);
                            return this.RenderError(r => r.Message = rsp.ErrorMessage);
                        }
                        Logger.Error(string.Format("充值成功，充值卡余额:{0}", rsp.Balance));
                        ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(string.Format("调用信息部接口充值时抛出异常: 卡号:({0})", order.No));
                        Logger.Error(ex);
                        return this.RenderError(r => r.Message = ex.Message);
                    }

                }

                return this.RenderSuccess<dynamic>(null);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Recharge eorror, card no :({0})", charge_no));
                Logger.Error(ex);
                return this.RenderError(r => r.Message = ex.Message);
            }
        }

        [RestfulAuthorize]
        public ActionResult Detail(string charge_no, int authuid)
        {
            var user = _userRepo.Find(x => x.UserId == authuid);
            //if (user == null)
            //{
            //    return this.RenderError(r => r.Message = "未绑定账号！请先绑定！");
            //}
            var order = _orderRepo.Find(x => x.No == charge_no);
            if (order == null)
            {
                return this.RenderError(r => r.Message = "无效的礼品卡编码");
            }
            if (order.OwnerUserId != authuid && order.PurchaseUserId != authuid)
            {
                return this.RenderError(r => r.Message = "您无权查看别人的礼品卡!");
            }

            var transfer = _transRepo.Find(x => x.OrderNo == charge_no && x.ToUserId == authuid);

            return
                this.RenderSuccess<dynamic>(
                    c =>
                        c.Data =
                            new
                            {
                                sender = transfer != null ? transfer.Phone : null,
                                trans_id = transfer != null ? transfer.Id : 0,
                                from = transfer != null ? transfer.FromNickName : null,
                                phone = user == null ? string.Empty : user.GiftCardAccount,
                                amount = order.Amount
                            });
        }

        [RestfulAuthorize]
        public ActionResult Transfer_Detail(string charge_no, int authuid)
        {
            var trans =
                _transRepo.Get(x => x.OrderNo == charge_no)
                    .OrderByDescending(t => t.CreateDate)
                    .FirstOrDefault();
            if (trans == null)
            {
                return this.RenderError(r => r.Message = "礼品卡没有赠送信息！");
            }

            var order = _orderRepo.Find(x => x.No == charge_no);
            if (order == null)
            {
                return this.RenderError(r => r.Message = "无效的礼品卡");
            }

            return
                this.RenderSuccess<dynamic>(
                    c =>
                        c.Data = TransDetail(order, trans, _rechargeRepo.Find(x => x.OrderNo == order.No)));
        }

        [RestfulAuthorize]
        public ActionResult Items(int id, int authuid)
        {
            var card = _cardRepo.Find(id);
            if (card == null)
            {
                return this.RenderError(r => r.Message = "该礼品卡不存在");
            }
            var items = _itemRepo.Get(x => x.GiftCardId == id);

            var data = new List<dynamic>();
            foreach (var item in items)
            {
                data.Add(new { id = item.Id, unit_price = item.UnitPrice, price = item.Price });
            }

            var resource = _resourceRepo.Find(x => x.SourceType == (int)SourceType.GiftCard && x.SourceId == card.Id);

            return
                this.RenderSuccess<dynamic>(
                    c =>
                        c.Data =
                            new
                            {
                                id,
                                image = resource == null ? "" : resource.Name.Image320Url(),
                                desc = card.Name,
                                items = data
                            });
        }
        [RestfulAuthorize]
        public ActionResult List(IMSGiftcardListRequest request, int type, int authuid)
        {
            PagerInfoResponse<dynamic> rsp;
            rsp = type == 1 ? ListItemsBuyed(request, authuid) : ListItemsReceived(request, authuid);
            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = rsp);
        }

        [RestfulAuthorize]
        public ActionResult SendEx(string comment, string from, string phone, string charge_no, int? trans_id, int authuid)
        {
            var order = _orderRepo.Find(x => x.No == charge_no);
            if (order == null)
            {
                return this.RenderError(r => r.Message = "无效的礼品卡!");
            }

            if (_transRepo.Get(x => x.FromUserId == authuid && x.IsActive != 1 && x.IsDecline != 1 && x.OrderNo == charge_no).Any())
            {
                return this.RenderError(r => r.Message = "礼品卡已经赠送，不能重复赠送");
            }

            if (order.Status == (int)GiftCardOrderStatus.Recharge)
            {
                return this.RenderError(r => r.Message = "该礼品卡已经充值了，不能赠送");
            }

            IMS_GiftCardTransfersEntity preTrans = _transRepo.Find(trans_id.HasValue ? trans_id.Value : -1);
            var newTrans = new IMS_GiftCardTransfersEntity()
            {
                Phone = string.IsNullOrEmpty(phone) ? string.Empty : phone,
                Comment = comment,
                CreateDate = DateTime.Now,
                CreateUser = authuid,
                FromUserId = authuid,
                OrderNo = order.No,
                IsActive = 0,
                IsDecline = 0,
                PreTransferId = preTrans == null ? 0 : preTrans.Id,
                OperateDate = DateTime.Now,
                OperateUser = authuid,
                FromNickName = from,
                FromPhone = from,
            };
            using (var ts = new TransactionScope())
            {
                if (preTrans != null)
                {
                    preTrans.IsActive = 1;
                    preTrans.ToUserId = authuid;
                    preTrans.OperateDate = DateTime.Now;
                    preTrans.OperateUser = authuid;
                    _transRepo.Update(preTrans);
                }
                newTrans = _transRepo.Insert(newTrans);
                ts.Complete();
            }

            return this.RenderSuccess<dynamic>(r => r.Data = new { trans_id = newTrans.Id });
        }

        [RestfulAuthorize]
        public ActionResult Change_Pwd(string pwd_old, string pwd_new, int authuid)
        {
            var cardAccount = _userRepo.Find(x => x.UserId == authuid);
            if (cardAccount == null)
            {
                return this.RenderError(r => r.Message = "该用户未绑定储值卡.");
            }

            var changePwdRequest = new ChangePasswordRequest() { Data = new { oldpassword = pwd_old, newpassword = pwd_new, phone = cardAccount.GiftCardAccount } };

            var result = _apiClient.Post(changePwdRequest);
            if (result.Status)
            {
                return this.RenderSuccess<dynamic>(null);
            }
            return this.RenderError(r => r.Message = result.ErrorMessage);
        }

        [RestfulAuthorize]
        public ActionResult Reset_Pwd(string pwd_new, int authuid)
        {
            var cardAccount = _userRepo.Find(x => x.UserId == authuid);
            if (cardAccount == null)
            {
                return this.RenderError(r => r.Message = "该用户未绑定储值卡!");
            }

            var resetPwdRequest = new ResetPasswordRequest()
            {
                Data = new
                    {
                        phone = cardAccount.GiftCardAccount,
                        newpassword = pwd_new
                    }
            };

            var rsp = _apiClient.Post(resetPwdRequest);
            if (!rsp.Status)
            {
                return this.RenderError(r => r.Message = rsp.ErrorMessage);
            }

            return this.RenderSuccess<dynamic>(null);
        }

        [RestfulAuthorize]
        public ActionResult Trans_Detail2(int trans_id, int authuid)
        {
            var trans = _transRepo.Find(trans_id);
            if (trans == null)
            {
                this.RenderError(r => r.Message = "不存在的赠送记录");
            }

            var order = _orderRepo.Find(x => x.No == trans.OrderNo);
            var recharge = _rechargeRepo.Find(x => x.OrderNo == trans.OrderNo);

            return this.RenderSuccess<dynamic>(r => r.Data = this.TransDetail(order, trans, recharge));
        }

        [RestfulAuthorize]
        public ActionResult RefuseByTransId(int trans_id, int authuid)
        {
            var trans = _transRepo.Find(trans_id);
            if (trans == null)
            {
                return this.RenderError(x => x.Message = "没有赠送信息！");
            }
            using (var ts = new TransactionScope())
            {
                trans.IsDecline = 1;
                trans.ToUserId = authuid;
                trans.OperateDate = DateTime.Now.AddMilliseconds(-10);
                //trans.OperateUser = authuid;
                _transRepo.Update(trans);

                var preTrans = _transRepo.Find(x => x.Id == trans.PreTransferId);
                if (preTrans != null)
                {
                    preTrans.IsActive = 0;
                    preTrans.OperateDate = DateTime.Now;
                    preTrans.OperateUser = authuid;
                    //preTrans.ToUserId = null;
                    _transRepo.Update(preTrans);
                }
                ts.Complete();
                return this.RenderSuccess<dynamic>(null);
            }
        }

        [Obsolete("请使用refuse(int trans_id, int authuid)")]
        [RestfulAuthorize]
        public ActionResult Refuse(string charge_no, int authuid)
        {
            var trans = _transRepo.Find(x => x.OrderNo == charge_no && x.IsDecline == 0 && x.IsActive == 0);
            if (trans != null)
            {
                using (var ts = new TransactionScope())
                {
                    trans.IsDecline = 1;
                    trans.ToUserId = authuid;
                    trans.OperateDate = DateTime.Now;
                    trans.OperateUser = authuid;
                    _transRepo.Update(trans);

                    var preTrans = _transRepo.Find(x => x.Id == trans.PreTransferId);
                    if (preTrans != null)
                    {
                        preTrans.IsActive = 0;
                        preTrans.OperateDate = DateTime.Now;
                        preTrans.OperateUser = authuid;
                        //preTrans.ToUserId = null;
                        _transRepo.Update(preTrans);
                    }
                    ts.Complete();
                    return this.RenderSuccess<dynamic>(null);
                }
            }
            return this.RenderError(x => x.Message = "没有赠送信息！");
        }

        [RestfulAuthorize]
        public ActionResult Receive(string charge_no, int authuid)
        {
            var trans = _transRepo.Get(x => x.OrderNo == charge_no);
            if (trans == null || !trans.Any())
            {
                return this.RenderError(r => r.Message = "该礼品未赠送!");
            }

            var tran = trans.FirstOrDefault(x => x.IsActive == 0 && x.IsDecline == 0);
            if (tran == null)
            {
                return this.RenderError(r => r.Message = "抱歉，您来晚了，该卡已被别人捷足先登了!");
            }
            tran.IsActive = 1;
            tran.OperateDate = DateTime.Now;
            tran.OperateUser = authuid;
            tran.ToUserId = authuid;
            _transRepo.Update(tran);
            return this.RenderSuccess<dynamic>(r => r.Data = new { trans_id = tran.Id, });
        }

        private bool IsPhoneBinded(string phone)
        {
            return _apiClient.Post(new ValidatePhoneRequest() { Data = new { phone } }).Status;
        }

        private PagerInfoResponse<dynamic> ListItemsReceived(IMSGiftcardListRequest request, int authuid)
        {
            var count = _transRepo.Get(x => x.ToUserId == authuid).Count();

            var orders =
                _transRepo.Get(x => x.ToUserId == authuid)
                    .OrderByDescending(t => t.OperateDate)
                    .Skip((request.Page - 1) * request.Pagesize)
                    .Take(request.Pagesize)
                    .Join(_orderRepo.Get(o => o.Status != (int)GiftCardOrderStatus.Void), x => x.OrderNo, o => o.No,
                        (x, o) => new { transfer = x, order = o })
                    .GroupJoin(_rechargeRepo.GetAll(), x => x.order.No, r => r.OrderNo,
                        (x, r) => new { x.transfer, x.order, recharge = r.FirstOrDefault() });
            dynamic items = new List<dynamic>();
            foreach (var o in orders)
            {
                items.Add(new
                {
                    recharged = o.order.Status == (int)GiftCardOrderStatus.Recharge,
                    trans_id = o.transfer != null ? o.transfer.Id : 0,
                    from = o.transfer.FromNickName,
                    from_phone = o.transfer.FromPhone,
                    verify_phone = o.transfer != null ? o.transfer.Phone : "null",
                    card_no = o.order.No,
                    amount = o.order.Amount,
                    status_i = SetStatus4Receiver(o.order, o.transfer, o.recharge),
                    send_date = o.transfer != null ? o.transfer.CreateDate.ToString(_dateFormmat) : "null",
                    receive_date = o.transfer.OperateDate.ToString(_dateFormmat)
                });
            }
            return new PagerInfoResponse<dynamic>(request.PagerRequest, count) { Items = items };

        }

        private GiftCardListItemStatus SetStatus4Receiver(IMS_GiftCardOrderEntity order, IMS_GiftCardTransfersEntity transfers, IMS_GiftCardRechargeEntity recharge)
        {
            if (transfers.IsDecline == 1)
            {
                return GiftCardListItemStatus.Refuse;
            }

            if (recharge != null)
            {
                if (recharge.ChargeUserId == transfers.ToUserId)
                {
                    return GiftCardListItemStatus.Received;
                }
            }

            var trans = _transRepo.Get(x => x.PreTransferId == transfers.Id);
            if (trans.Any(x => x.IsActive == 1))
            {
                return GiftCardListItemStatus.ReTransfer;
            }

            if (trans.Any(x => x.IsDecline == 1))
            {
                return GiftCardListItemStatus.Refused;
            }

            if (trans.Any())
            {
                return GiftCardListItemStatus.Waiting4Receive;
            }

            if (transfers.IsActive == 1)
            {
                return GiftCardListItemStatus.ReceivedButNoOperation;
            }

            return GiftCardListItemStatus.None;
        }

        private PagerInfoResponse<dynamic> ListItemsBuyed(IMSGiftcardListRequest request, int authuid)
        {
            int page = request.Page;
            int pageSize = request.Pagesize;
            var count = _orderRepo.Get(x => x.PurchaseUserId == authuid).Count();
            var orders =
                _orderRepo.Get(x => x.PurchaseUserId == authuid)
                    .OrderByDescending(t => t.CreateDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .GroupJoin(_transRepo.Get(x => x.FromUserId == authuid), o => o.No, x => x.OrderNo,
                        (o, trans) =>
                            new { order = o, transfer = trans.OrderByDescending(x => x.CreateDate).FirstOrDefault() })
                    .GroupJoin(_rechargeRepo.Get(r => r.ChargeUserId == authuid), ot => ot.order.No, r => r.OrderNo,
                        (ot, r) => new { order = ot.order, transfer = ot.transfer, recharge = r.FirstOrDefault() });


            var items = new List<dynamic>();
            foreach (var o in orders)
            {
                items.Add(new
                {
                    recharged = o.order.Status == (int)GiftCardOrderStatus.Recharge,
                    trans_id = 0,//自己购买的卡无需
                    card_no = o.order.No,
                    amount = o.order.Amount,
                    purchase_date = o.order.CreateDate.ToString(_dateFormmat),
                    charge_date = o.recharge != null ? o.recharge.CreateDate.ToString(_dateFormmat) : "null",
                    status_i = SetStatus4Buyer(o.order, o.recharge),
                    verify_phone = o.transfer != null ? o.transfer.Phone : "null",
                    send_date = o.transfer != null ? o.transfer.CreateDate.ToString(_dateFormmat) : "null",
                    receive_date = o.transfer != null && o.transfer.IsActive == 1 ? o.transfer.CreateDate.ToString(_dateFormmat) : "null"
                });
            }
            return new PagerInfoResponse<dynamic>(request.PagerRequest, count) { Items = items };
        }

        private GiftCardListItemStatus SetStatus4Buyer(IMS_GiftCardOrderEntity order, IMS_GiftCardRechargeEntity recharge)
        {

            if (order.Status == (int)GiftCardOrderStatus.Recharge && recharge != null)
            {
                if (recharge.ChargeUserId != order.PurchaseUserId)
                {
                    return GiftCardListItemStatus.Sent;
                }
                return GiftCardListItemStatus.Recharged;
            }

            var trans = _transRepo.Get(x => x.OrderNo == order.No);
            if (!trans.Any())
            {
                return GiftCardListItemStatus.None;
            }

            if (trans.All(x => x.IsDecline == 1))
            {
                return GiftCardListItemStatus.Refused;
            }
            if (trans.Any(x => x.FromUserId == order.PurchaseUserId && !x.ToUserId.HasValue))
            {
                return GiftCardListItemStatus.Waiting4Receive;
            }
            return GiftCardListItemStatus.Sent;
        }

        private dynamic GetUserAccountBalance(string phone)
        {
            var balanceRequest = new QueryAccountBalanceRequest() { Data = new { phone } };
            var result = _apiClient.Post(balanceRequest);
            if (result.Status)
            {
                return result.Balance;
            }
            throw new GetUserBalanceExcepiton(result.ErrorMessage);
        }

        /// <summary>
        /// 礼品卡接受页面展示内容
        /// </summary>
        /// <param name="order"></param>
        /// <param name="trans"></param>
        /// <param name="recharge"></param>
        /// <returns></returns>
        private dynamic TransDetail(IMS_GiftCardOrderEntity order, IMS_GiftCardTransfersEntity trans, IMS_GiftCardRechargeEntity recharge)
        {
            return new
            {
                comment = trans.Comment,
                sender = trans.FromNickName,
                from_phone = trans.FromPhone,
                phone = trans.Phone,
                amount = order.Amount,
                trans_id = trans.Id,
                status = this.SetStatus4Receiver(order, trans, recharge),
            };
        }
    }

    public class GetUserBalanceExcepiton : Exception
    {
        public GetUserBalanceExcepiton(string message)
            : base(message)
        {
        }
    }

    public enum GiftCardListItemStatus
    {
        /// <summary>
        /// 未操作
        /// </summary>
        None = 0,

        /// <summary>
        /// 已领取
        /// </summary>
        Received = 1,

        /// <summary>
        /// 已拒收
        /// </summary>
        Refuse = 2,

        /// <summary>
        /// 已转赠
        /// </summary>
        ReTransfer = 3,

        /// <summary>
        /// 已充值
        /// </summary>
        Recharged = 4,

        /// <summary>
        /// 已赠送
        /// </summary>
        Sent = 5,

        /// <summary>
        /// 被拒绝
        /// </summary>
        Refused = 6,

        /// <summary>
        /// 已赠送但未被领取
        /// </summary>
        Waiting4Receive = 7,

        /// <summary>
        /// 已接受但是未操作状态
        /// </summary>
        ReceivedButNoOperation = 8,
    }

    public enum GiftCardOrderStatus
    {
        /// <summary>
        /// 取消
        /// </summary>
        Void = -10,

        /// <summary>
        /// 创建
        /// </summary>
        Create = 0,

        /// <summary>
        /// 已支付
        /// </summary>
        Paid = 1,

        /// <summary>
        /// 充值
        /// </summary>
        Recharge = 10,
    }
}
