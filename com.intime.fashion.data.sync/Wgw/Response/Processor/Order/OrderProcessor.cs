using System;
using System.Globalization;
using System.Linq;
using System.Transactions;
using com.intime.fashion.data.sync.Wgw.Request.Order;
using com.intime.jobscheduler.Job.Wgw;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using com.intime.fashion.service;

namespace com.intime.fashion.data.sync.Wgw.Response.Processor.Order
{
    /// <summary>
    /// 处理已经支付的订单
    /// </summary>
    public abstract class OrderProcessor : IProcessor
    {
        protected OrderProcessor()
        {
            this.ErrorMessage = string.Empty;
        }

        /// <summary>
        /// 微购物上已经支付的订单同步处理
        /// </summary>
        /// <param name="orderInfo"><see cref="http://open.weigou.qq.com/api/wgwdeal/wgQueryDealList"/> wgQueryDealList返回的信息</param>
        /// <param name="orderDetail"><see cref="http://open.weigou.qq.com/api/wgwdeal/wgQueryDealDetail"/>wgQueryDealDetail返回的信息</param>
        /// <returns></returns>
        public bool Process(dynamic orderInfo, dynamic orderDetail)
        {
            if (orderDetail.errorCode != 0)
            {
                ErrorMessage = orderDetail.errorMessage;
                return false;
            }
            try
            {
                ProcessOrder(orderInfo, orderDetail.dealDetail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }


        /// <summary>
        /// 已同步过的不再处理
        /// </summary>
        /// <param name="orderInfo">订单列表查询wgQueryDealList返回的dealList.dealListVo</param>
        /// <param name="dealDetail">订单详情wgQueryDealDetail返回的Json对象</param>
        protected abstract void ProcessOrder(dynamic orderInfo, dynamic dealDetail);

        protected void CreateOrder(dynamic orderInfo, dynamic dealDetail)
        {
            UserEntity customer = FindCustomer(orderInfo);
            using (var ts = new TransactionScope())
            {
                using (var db = DbContextHelper.GetDbContext())
                {
                    OrderEntity order = CreateOrder(dealDetail);
                    order.CustomerId = customer.Id;
                    db.Orders.Add(order);
                    foreach (var item in dealDetail.trades)
                    {
                        int stockId = this.GetInventoryId(item);

                        var inventory =
                            db.Inventories.FirstOrDefault(x => x.Id == stockId);
                        if (inventory == null)
                        {
                            throw new WgwSyncException(string.Format("Invalid inventory ID ({0})", stockId));
                        }

                        var product = db.Products.FirstOrDefault(t => t.Id == inventory.ProductId);
                        if (product == null)
                        {
                            throw new WgwSyncException(string.Format("Order from wgw contains invalid product ({1}) ({0})", item.tilte, dealDetail.dealCode));
                        }

                        int buyNum = int.Parse(item.buyNum.ToString());

                        //扣减库存

                        if (!CheckStocks(inventory, buyNum))
                        {
                            throw new StockInsufficientException(dealDetail.dealCode, OrderStatusConst.STATE_WG_PAY_OK);
                        }

                        var colorEntity = db.ProductPropertyValues.FirstOrDefault(ppv => ppv.Id == inventory.PColorId);
                        var sizeEntity = db.ProductPropertyValues.FirstOrDefault(ppv => ppv.Id == inventory.PSizeId);

                        db.OrderItems.Add(new OrderItemEntity()
                        {
                            OrderNo = order.OrderNo,
                            ProductId = product.Id,
                            BrandId = product.Brand_Id,
                            StoreId = product.Store_Id,
                            CreateUser = ConstValue.WGW_OPERATOR_USER,
                            CreateDate = DateTime.Now,
                            ItemPrice = ((decimal)item.price) / 100,
                            Quantity = item.buyNum,
                            ProductName = product.Name,
                            Status = (int)DataStatus.Normal,
                            UpdateDate = DateTime.Now,
                            UpdateUser = ConstValue.WGW_OPERATOR_USER,
                            ExtendPrice = ((decimal)item.disTotal) / 100,
                            ProductDesc = item.attr,//string.Empty,
                            ColorId = colorEntity == null ? 0 : colorEntity.PropertyId,
                            ColorValueId = colorEntity == null ? 0 : colorEntity.Id,
                            SizeId = sizeEntity == null ? 0 : sizeEntity.PropertyId,
                            SizeValueId = sizeEntity == null ? 0 : sizeEntity.Id,
                            ColorValueName = colorEntity == null ? string.Empty : colorEntity.ValueDesc,
                            SizeValueName = sizeEntity == null ? string.Empty : sizeEntity.ValueDesc,
                            StoreItemNo = product.SkuCode,
                            Points = 0,
                            UnitPrice = product.UnitPrice
                        });
                    }
                    db.Map4Order.Add(new Map4OrderEntity
                    {
                        ChannelOrderCode = dealDetail.dealCode,
                        OrderNo = order.OrderNo,
                        Channel = ConstValue.WGW_CHANNEL_NAME,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        SyncStatus = (int)OrderOpera.FromCustomer,
                    });
                    db.OrderLogs.Add(new OrderLogEntity()
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = ConstValue.WGW_OPERATOR_USER,
                        CustomerId = customer.Id,
                        Operation = dealDetail.dealState.ToString() == OrderStatusConst.STATE_WG_WAIT_PAY ? string.Format("创建订单") : dealDetail.dealState.ToString() == OrderStatusConst.STATE_WG_PAY_OK ? string.Format("支付订单") : string.Format("取消订单"),
                        OrderNo = order.OrderNo,
                        Type = (int)OrderOpera.FromOperator
                    });

                    db.OrderTransactions.Add(CreateOrderTransaction(order, dealDetail));

                    db.SaveChanges();
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// Create order transaction
        /// </summary>
        /// <param name="order"></param>
        /// <param name="dealDetail"></param>
        /// <returns></returns>
        protected virtual OrderTransactionEntity CreateOrderTransaction(OrderEntity order, dynamic dealDetail)
        {
            return new OrderTransactionEntity()
            {
                OrderNo = order.OrderNo,
                PaymentCode = order.PaymentMethodCode,
                Amount = order.TotalAmount,
                CreateDate = DateTime.Now,
                TransNo = dealDetail.cftPayId, //财付通订单号
                IsSynced = false,
                CanSync = -1,
                OutsiteUId = dealDetail.buyerOpenid,
                OrderType = (int)PaidOrderType.Self
            };
        }

        /// <summary>
        /// 扣减库存
        /// </summary>
        /// <param name="inventory">库存ID</param>
        /// <param name="buyNum">库存数量</param>
        protected abstract bool CheckStocks(InventoryEntity inventory, int buyNum);

        /// <summary>
        /// 订单是否已经同步
        /// </summary>
        /// <param name="dealCode">微购物订单编码</param>
        /// <returns></returns>
        protected bool IsOrderExists(string dealCode)
        {
            using (var db = GetDbContext())
            {
                return
                    db.Orders.Where(o => o.OrderSource == ConstValue.WGW_CHANNEL_NAME)
                        .Join(
                            db.Map4Order.Where(m => m.Channel == ConstValue.WGW_CHANNEL_NAME && m.ChannelOrderCode == dealCode),
                            o => o.OrderNo, m => m.OrderNo, (o, m) => o).Any();
            }
        }

        /// <summary>
        /// 通过openid查找用户，如果找不到则创建一个联合登录用户
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        private UserEntity FindCustomer(dynamic orderInfo)
        {
            string openId = orderInfo.buyerOpenid;
            OutsiteUserEntity outsiteUser = null;
            using (var db = DbContextHelper.GetDbContext())
            {
                outsiteUser =
                    db.OutsiteUsers.FirstOrDefault(
                        o => o.OutsiteUserId == openId && o.OutsiteType == (int)OutsiteType.WX);
            }
            if (outsiteUser == null)
            {
                return CreateNewUser(orderInfo);
            }
            using (var db = DbContextHelper.GetDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Id == outsiteUser.AssociateUserId);
            }
        }

        /// <summary>
        /// 通过订单相关信息创建外站用户和用户
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        private UserEntity CreateNewUser(dynamic orderInfo)
        {
            using (var db = GetDbContext())
            {
                var name = String.Format("__{0}{1}",
                    ((int)OutsiteType.WX).ToString(CultureInfo.InvariantCulture), orderInfo.buyerOpenid);
                var user = db.Set<UserEntity>().Add(new UserEntity()
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUser = ConstValue.WGW_OPERATOR_USER,
                    UpdatedUser = ConstValue.WGW_OPERATOR_USER,
                    EMail = String.Empty,
                    LastLoginDate = DateTime.Now,
                    Logo = String.Empty,
                    Nickname = orderInfo.buyerNickName,
                    Name = name,
                    Password = Guid.NewGuid().ToString(),
                    Status = (int)OrderStatus.Paid,
                    Store_Id = 0,
                    Region_Id = 0,
                    IsCardBinded = false,
                    UserLevel = (int)UserLevel.User,
                    Description = String.Empty,
                    Mobile = string.Empty,
                    Gender = (int)GenderType.Default
                });

                db.SaveChanges();

                db.OutsiteUsers.Add(new OutsiteUserEntity()
                {
                    CreatedDate = DateTime.Now,
                    CreatedUser = user.Id,
                    Description = String.Empty,
                    LastLoginDate = DateTime.Now,
                    AssociateUserId = user.Id,
                    OutsiteType = (int)OutsiteType.WX,
                    Status = (int)DataStatus.Normal,
                    OutsiteUserId = orderInfo.buyerOpenid
                });

                db.SaveChanges();
                return user;
            }
        }

        /// <summary>
        /// 创建订单编号，参考OrderRule
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private string CreateOrderCode(int storeId)
        {
            return OrderRule.CreateCode(storeId);
        }

        protected int ExtractOrderStatus(dynamic dealState)
        {
            if (dealState.ToString() == OrderStatusConst.STATE_WG_WAIT_PAY)
            {
                return (int)OrderStatus.Create;
            }
            if (dealState.ToString() == OrderStatusConst.STATE_WG_PAY_OK)
            {
                return (int)OrderStatus.Paid;
            }
            if (dealState.ToString() == OrderStatusConst.STATE_WG_END)
            {
                return (int)OrderStatus.Complete;
            }
            if (dealState.ToString() == OrderStatusConst.STATE_WG_CANCLE)
            {
                return (int)OrderStatus.Void;
            }
            if (dealState.ToString() == OrderStatusConst.STATE_WG_SHIPPING_OK)
            {
                return (int)OrderStatus.Shipped;
            }
            throw new WgwSyncException(string.Format("Unsupported order status {0}", dealState));
        }

        /// <summary>
        /// 创建订单实体对象
        /// </summary>
        /// <param name="dealDetail"></param>
        /// <returns></returns>
        private OrderEntity CreateOrder(dynamic dealDetail)
        {
            int orderStatus = ExtractOrderStatus(dealDetail.dealState);
            decimal totalAmount = decimal.Parse(dealDetail.disTotalPay.ToString()) / 100;
            return new OrderEntity()
            {
                BrandId = 0,
                StoreId = 0,
                CreateDate = DateTime.Now,
                CreateUser = ConstValue.WGW_OPERATOR_USER,
                OrderNo = CreateOrderCode(0),
                InvoiceDetail = string.Empty,
                InvoiceSubject = string.Empty,
                NeedInvoice = false,
                Memo = dealDetail.noteContent,
                ShippingAddress = dealDetail.recv.recvInfo,
                ShippingContactPerson = dealDetail.recv.name,
                ShippingContactPhone = dealDetail.recv.mobile ?? dealDetail.recv.phone,
                ShippingZipCode = dealDetail.recv.postCode,
                ShippingFee = dealDetail.shipFee,
                Status = orderStatus,
                UpdateDate = DateTime.Now,
                UpdateUser = ConstValue.WGW_OPERATOR_USER,
                TotalAmount = totalAmount,
                PaymentMethodCode = dealDetail.bankType == 1 ? WgwConfigHelper.WGW_WX_PAYMENTCODE : WgwConfigHelper.WGW_CFT_PAYMENTCODE,
                PaymentMethodName = dealDetail.bankType == 1 ? WgwConfigHelper.WGW_WX_PAYMENTNAME : WgwConfigHelper.WGW_CFT_PAYMENTNAME,
                InvoiceAmount = totalAmount,
                TotalPoints = 0,
                OrderSource = ConstValue.WGW_CHANNEL_NAME
            };
        }

        protected int GetInventoryId(dynamic trade)
        {
            int stockId;
            using (var db = GetDbContext())
            {
                if (!int.TryParse(trade.stockLoCode.ToString(), out stockId))
                {
                    Int64 skuid;
                    if (!Int64.TryParse(trade.skuId.ToString(), out skuid))
                    {
                        return GetInventoryByItemId(db, trade.itemCode.ToString());
                    }
                    var map =
                        db.Map4Inventory.FirstOrDefault(
                            m => m.Channel == ConstValue.WGW_CHANNEL_NAME && m.skuId == skuid);
                    if (map == null)
                    {
                        return GetInventoryByItemId(db, trade.itemCode.ToString());
                    }
                    stockId = (int)map.InventoryId;
                }
            }
            return stockId;
        }

        private int GetInventoryByItemId(YintaiHangzhouContext context, string itemId)
        {
            string snapshotId = SnapShotId2ItemId(itemId);
            var product =
                context.Set<Map4ProductEntity>().Where(m => m.Channel == ConstValue.WGW_CHANNEL_NAME && (m.ChannelProductId == itemId || m.ChannelProductId == snapshotId))
                    .Join(context.Set<ProductEntity>(), m => m.ProductId, p => p.Id, (m, p) => p)
                    .FirstOrDefault();
            if (product == null)
            {
                throw new WgwSyncException(string.Format("Can't find product accordding itemId ({0})", itemId));
            }
            int productId = product.Id;
            var cnt = context.Inventories.Count(i => i.ProductId == productId);

            if (cnt == 0)
            {
                throw new WgwSyncException(string.Format("Product ({0}) has no stock", productId));
            }
            if (cnt > 1)
            {
                throw new WgwSyncException(string.Format("Product ({0}) is multi stocks , can't determine which is the correct inventory", productId));
            }
            return context.Inventories.First(i => i.ProductId == productId).Id;
        }

        private string SnapShotId2ItemId(string snapshotId)
        {
            if (string.IsNullOrEmpty(snapshotId))
            {
                throw new ArgumentNullException("snapshotId");
            }
            if (snapshotId.Length < 16)
            {
                throw new WgwSyncException(string.Format("Invalid snapshot itemId {0}", snapshotId));
            }
            var arr = snapshotId.ToArray();
            for (int i = 12; i < 16; i++)
            {
                arr[i] = '0';
            }
            return string.Join("", arr);
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        protected YintaiHangzhouContext GetDbContext()
        {
            return DbContextHelper.GetDbContext();
        }
    }
}
