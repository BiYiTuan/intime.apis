using com.intime.fashion.service.contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yintai.Architecture.Common.Logger;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.Order;
using Yintai.Hangzhou.Repository.Contract;

namespace com.intime.fashion.service
{
    public class OrderService : BusinessServiceBase, IOrderService
    {
        private IProductRepository _productRepo;
        private IOrderRepository _orderRepo;
        private IOrderLogRepository _orderLogRepo;
        private IOrderItemRepository _orderItemRepo;
        private IInventoryRepository _inventoryRepo;
        private IOrder2ExRepository _order2exRepo;
        private IShippingFeeService _shippingFeeService;
        private IAssociateIncomeService _associateIncomeService;
        public OrderService(IProductRepository productRepo,
            IOrderRepository orderRepo,
            IOrderLogRepository orderLogRepo,
            IOrderItemRepository orderItemRepo,
            IInventoryRepository inventoryRepo,
            IOrder2ExRepository order2exRepo,
            IShippingFeeService shippingFeeService,
            IAssociateIncomeService associateIncomeService)
            : base()
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _orderLogRepo = orderLogRepo;
            _orderItemRepo = orderItemRepo;
            _inventoryRepo = inventoryRepo;
            _order2exRepo = order2exRepo;
            _shippingFeeService = shippingFeeService;
            _associateIncomeService = associateIncomeService;

        }
        public bool CanChangePro(OrderEntity order)
        {
            return order.Status == (int)OrderStatus.AgentConfirmed ||
                    order.Status == (int)OrderStatus.Paid;
        }

        public bool IsAssociateOrder(int authuid, OrderEntity order)
        {
            var context = GetContext();
            var associateIncome = context.Set<IMS_AssociateIncomeHistoryEntity>()
                                    .Where(iair => iair.AssociateUserId == authuid
                                            && iair.SourceType == (int)AssociateOrderType.Product
                                            && iair.SourceNo == order.OrderNo)
                                    .FirstOrDefault();
            return associateIncome != null;
        }
        public BusinessResult<OrderCreateResult> Create(OrderCreate request, UserModel authUser, bool needShippingFee = true)
        {

            decimal totalAmount = 0m;
            decimal discountAmount = 0m;
            bool isSelfOrder = false;
            foreach (var product in request.Products)
            {
                var productEntity = _productRepo.Find(product.ProductId);
                if (productEntity == null)
                    return Error<OrderCreateResult>(string.Format("{0} 不存在！", product.ProductId));
                if (!productEntity.Is4Sale.HasValue || productEntity.Is4Sale.Value == false)
                    return Error<OrderCreateResult>(string.Format("{0} 不能销售！", productEntity.Id));
                totalAmount += productEntity.Price * product.Quantity;
                if (productEntity.ProductType.HasValue && productEntity.ProductType.Value == (int)ProductType.FromSelf)
                {
                    isSelfOrder = true;
                }
            }
            if (request.ComboId.HasValue && request.ComboId > 0)
            {
                discountAmount = OrderRule.ComputeComboDiscount(request.ComboId.Value, request.Products.Select(p => p.ProductId));
                totalAmount = totalAmount - discountAmount;

                //get storeid from comboid if no storeid passed in 
                if ((request.StoreId ?? 0) == 0)
                {
                    var assocateEntity = _db.Set<IMS_AssociateItemsEntity>().Where(iai => iai.ItemType == (int)ComboType.Product
                                && iai.ItemId == request.ComboId.Value).FirstOrDefault();
                    if (assocateEntity != null)
                        request.StoreId = assocateEntity.AssociateId;
                }
            }
            if (totalAmount <= 0)
                return Error<OrderCreateResult>("商品销售价信息错误！");


            var orderNo = OrderRule.CreateCode(0);
            decimal shippingFee = 0;
            if (needShippingFee)
            {
                shippingFee = _shippingFeeService.Calculate(request.Products);
                totalAmount += shippingFee;
            }

            using (var ts = new TransactionScope())
            {
                var orderEntity = _orderRepo.Insert(new OrderEntity()
                {
                    BrandId = 0,
                    CreateDate = DateTime.Now,
                    CreateUser = authUser.Id,
                    CustomerId = authUser.Id,
                    InvoiceDetail = request.InvoiceDetail,
                    InvoiceSubject = request.InvoiceTitle,
                    NeedInvoice = request.NeedInvoice,
                    Memo = request.Memo,
                    PaymentMethodCode = request.Payment.PaymentCode,
                    PaymentMethodName = request.Payment.PaymentName,
                    ShippingAddress = request.ShippingAddress == null ? string.Empty : request.ShippingAddress.ShippingAddress,
                    ShippingContactPerson = request.ShippingAddress == null ? string.Empty : request.ShippingAddress.ShippingContactPerson,
                    ShippingContactPhone = request.ShippingAddress == null ? string.Empty : request.ShippingAddress.ShippingContactPhone,
                    ShippingFee = shippingFee,
                    ShippingZipCode = request.ShippingAddress == null ? string.Empty : request.ShippingAddress.ShippingZipCode,
                    Status = (int)OrderStatus.Create,
                    StoreId = 0,
                    UpdateDate = DateTime.Now,
                    UpdateUser = authUser.Id,
                    TotalAmount = totalAmount,
                    InvoiceAmount = totalAmount,
                    OrderNo = orderNo,
                    TotalPoints = 0,
                    OrderSource = request.Channel,
                    OrderProductType = isSelfOrder ? (int)OrderProductType.SelfProduct : (int)OrderProductType.SystemProduct,
                    DiscountAmount = discountAmount,
                    PromotionFlag = discountAmount > 0 ? true : false
                });
                foreach (var product in request.Products)
                {
                    var productEntity = _productRepo.Find(product.ProductId);
                    string salesCode = string.Empty;
                    if (productEntity.ProductType == (int)ProductType.FromSelf)
                    {
                        var productSaleCodeEntity = _db.Set<ProductCode2StoreCodeEntity>().Where(p => p.ProductId == product.ProductId && p.Status == (int)DataStatus.Normal).First();
                        salesCode = productSaleCodeEntity.StoreProductCode;
                    }
                    var inventoryEntity = _db.Set<InventoryEntity>().Where(pm => pm.ProductId == product.ProductId && pm.PColorId == product.Properties.ColorValueId && pm.PSizeId == product.Properties.SizeValueId).FirstOrDefault();
                    if (inventoryEntity == null)
                        return Error<OrderCreateResult>(string.Format("{0}库存 不存在！", productEntity.Id));
                    if (inventoryEntity.Amount < product.Quantity)
                        return Error<OrderCreateResult>(string.Format("{0}库存不足！", productEntity.Id));
                    var productSizeEntity = _db.Set<ProductPropertyValueEntity>().Where(ppv => ppv.Id == product.Properties.SizeValueId).FirstOrDefault();
                    var productColorEntity = _db.Set<ProductPropertyValueEntity>().Where(ppv => ppv.Id == product.Properties.ColorValueId).FirstOrDefault();
                    _orderItemRepo.Insert(new OrderItemEntity()
                    {
                        BrandId = productEntity.Brand_Id,
                        CreateDate = DateTime.Now,
                        CreateUser = authUser.Id,
                        ItemPrice = productEntity.Price,
                        OrderNo = orderNo,
                        ProductId = productEntity.Id,
                        ProductName = productEntity.Name,
                        Quantity = product.Quantity,
                        Status = (int)DataStatus.Normal,
                        StoreId = productEntity.Store_Id,
                        UnitPrice = productEntity.UnitPrice,
                        UpdateDate = DateTime.Now,
                        UpdateUser = authUser.Id,
                        ExtendPrice = productEntity.Price * product.Quantity,
                        ProductDesc = product.ProductDesc,
                        ColorId = productColorEntity == null ? 0 : productColorEntity.PropertyId,
                        ColorValueId = productColorEntity == null ? 0 : productColorEntity.Id,
                        SizeId = productSizeEntity == null ? 0 : productSizeEntity.PropertyId,
                        SizeValueId = productSizeEntity == null ? 0 : productSizeEntity.Id,
                        ColorValueName = productColorEntity == null ? string.Empty : productColorEntity.ValueDesc,
                        SizeValueName = productSizeEntity == null ? string.Empty : productSizeEntity.ValueDesc,
                        StoreItemNo = productEntity.SkuCode,
                        Points = 0,
                        ProductType = productEntity.ProductType ?? (int)ProductType.FromSystem,
                        StoreSalesCode = salesCode

                    });
                    inventoryEntity.Amount = inventoryEntity.Amount - product.Quantity;
                    inventoryEntity.UpdateDate = DateTime.Now;
                    _inventoryRepo.Update(inventoryEntity);
                    int? storeId = null;
                    int? saleCodeId = null;
                    if (product.StoreId.HasValue && product.StoreId != 0 && product.SectionId.HasValue && product.SectionId != 0)
                    {
                        var storeEntity = _db.Set<StoreEntity>().Find(product.StoreId.Value);
                        storeId = storeEntity.ExStoreId;
                        var sectionEntity = _db.Set<SectionEntity>().Find(product.SectionId.Value);
                        saleCodeId = sectionEntity.ChannelSectionId;
                    }

                }
                _orderLogRepo.Insert(new OrderLogEntity()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = authUser.Id,
                    CustomerId = authUser.Id,
                    Operation = string.Format("创建订单"),
                    OrderNo = orderNo,
                    Type = (int)OrderOpera.FromCustomer
                });

                _associateIncomeService.Create(orderEntity);
                _associateIncomeService.CreateSystem(orderEntity, request.ComboId);

                string exOrderNo = string.Empty;

                bool isSuccess = true;
                if (isSuccess)
                {
                    exOrderNo = orderEntity.OrderNo;
                    ts.Complete();
                    return Success<OrderCreateResult>(OrderCreateResult.FromEntity<OrderCreateResult>(orderEntity, o => o.ExOrderNo = exOrderNo));

                }
                else
                {
                    return Error<OrderCreateResult>("失败");
                }
            }
        }


        public OrderPreCalculateResult PreCalculate(OrderPreCalculate request)
        {
            decimal? amount = 9999999m;
            decimal shippingFee = 0m;
            int totalQuantity = 0;
            switch (request.CalculateType)
            {
                case OrderPreCalculateType.Product:
                    amount = request.Products.Sum(l => (decimal?)l.Price * l.Quantity);
                    shippingFee = _shippingFeeService.Calculate(request.Products.Select(p => new OrderItem()
                    {
                        ProductId = p.ProductId,
                        Quantity = p.Quantity
                    }));
                    totalQuantity = request.Products.Sum(l => l.Quantity);
                    break;
                case OrderPreCalculateType.Combo:
                    var linq = _db.Set<IMS_Combo2ProductEntity>().Where(icp => icp.ComboId == request.ComboId)
                     .Join(_db.Set<ProductEntity>().Where(p => p.Is4Sale == true && p.Status == (int)DataStatus.Normal
                                        && _db.Set<InventoryEntity>().Where(i => i.Amount > 0).Any(i => i.ProductId == p.Id)),
                             o => o.ProductId,
                             i => i.Id,
                             (o, i) => i);
                    amount = linq.Sum(l => (decimal?)l.Price) ?? 0m;
                    totalQuantity = linq.Count();
                    var discount = OrderRule.ComputeComboDiscount(request.ComboId, linq.Select(l => l.Id));
                    amount = amount - discount;
                    shippingFee = _shippingFeeService.Calculate(linq.Select(p => new OrderItem()
                    {
                        ProductId = p.Id,
                        Quantity = 1
                    }));

                    break;
                default:
                    throw new NotSupportedException();
            }
            amount += shippingFee;
            return new OrderPreCalculateResult()
            {
                TotalAmount = amount ?? 0m,
                ExtendPrice = amount ?? 0m,
                TotalFee = shippingFee,
                TotalPoints = 0,
                TotalQuantity = totalQuantity
            };
        }

    }
}
