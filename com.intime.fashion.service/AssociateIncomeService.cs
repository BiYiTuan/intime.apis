using com.intime.fashion.common;
using com.intime.fashion.service.contract;
using com.intime.fashion.service.IncomeRule;
using com.intime.fashion.service.PromotionRule;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.service
{
    public class AssociateIncomeService : BusinessServiceBase, IAssociateIncomeService
    {
        private IEFRepository<IMS_AssociateIncomeHistoryEntity> _incomeHistoryRepo;
        private IEFRepository<IMS_AssociateIncomeEntity> _incomeAccountRepo;

        private static string _promotionTimeRange =
            ConfigurationManager.AppSettings["intime.associate.promotion.timerange"];

        public AssociateIncomeService(IEFRepository<IMS_AssociateIncomeHistoryEntity> incomeHistoryRepo
            , IEFRepository<IMS_AssociateIncomeEntity> incomeAccountRepo
            )
            : base()
        {
            _incomeHistoryRepo = incomeHistoryRepo;
            _incomeAccountRepo = incomeAccountRepo;
        }
        public bool Create(OrderEntity order)
        {
            foreach (var orderItem in _db.Set<OrderItemEntity>().Where(oi => oi.OrderNo == order.OrderNo
                                        && oi.ProductType == (int)ProductType.FromSelf
                                        && oi.Status == (int)DataStatus.Normal))
            {
                var _associateUserEntity = _db.Set<ProductEntity>().Find(orderItem.ProductId);
                var _storeEntity = _db.Set<StoreEntity>().Find(orderItem.StoreId);

                _incomeHistoryRepo.Insert(new IMS_AssociateIncomeHistoryEntity()
                {
                    CreateDate = DateTime.Now,
                    SourceNo = order.OrderNo,
                    SourceType = (int)AssociateOrderType.Product,
                    Status = (int)AssociateIncomeStatus.Create,
                    UpdateDate = DateTime.Now,
                    AssociateIncome = ComputeIncome(order, orderItem),
                    AssociateUserId = _associateUserEntity.CreatedUser,
                    GroupId = _storeEntity.Group_Id
                });
            }

            return true;
        }


        /// <summary>
        /// 系统商品导购返利处理，目前返利为0，有人工进行返利的操作
        /// </summary>
        /// <param name="order"></param>
        /// <param name="comboId">组合Id</param>
        /// <returns>操作结果</returns>
        public bool CreateSystem(OrderEntity order, int? comboId)
        {
            if (!comboId.HasValue)
            {
                return false;
            }

            foreach (var orderItem in _db.Set<OrderItemEntity>().Where(oi => oi.OrderNo == order.OrderNo
                                        && oi.ProductType == (int)ProductType.FromSystem
                                        && oi.Status == (int)DataStatus.Normal))
            {

                //根据comboId,获取组合用户
                var comboEntity = _db.Set<IMS_ComboEntity>().Find(comboId);
                if (comboEntity == null)
                {
                    return false;
                }

                var storeEntity = _db.Set<StoreEntity>().Find(orderItem.StoreId);

                _incomeHistoryRepo.Insert(new IMS_AssociateIncomeHistoryEntity()
                {
                    CreateDate = DateTime.Now,
                    SourceNo = order.OrderNo,
                    SourceType = (int)AssociateOrderType.Product,
                    Status = (int)AssociateIncomeStatus.Create,
                    UpdateDate = DateTime.Now,
                    AssociateIncome = 0,
                    AssociateUserId = comboEntity.CreateUser,
                    GroupId = storeEntity != null ? storeEntity.Group_Id : 1
                });
            }

            return true;
        }

        public bool Froze(string orderNo)
        {
            orderNo = orderNo.Trim();
            IncomePromotionCalculator calculator = new IncomePromotionCalculator();
            foreach (var item in _db.Set<IMS_AssociateIncomeHistoryEntity>()
                        .Where(iai => iai.SourceType == (int)AssociateOrderType.Product && iai.SourceNo == orderNo)
                        .ToList())
            {
                item.Status = (int)AssociateIncomeStatus.Frozen;
                item.UpdateDate = DateTime.Now;
                item.AssociateIncome = calculator.Calculate(item, _db);
                _incomeHistoryRepo.Update(item);

                AssociateIncomeAccount.Froze(item.AssociateUserId, item.AssociateIncome);
            }
            return true;
        }
        public bool Avail(int associateUserId, IMS_GiftCardOrderEntity giftOrder)
        {
            var groupid = GetGroupFromGiftCard(giftOrder);
            var thisIncome = ComputeIncome(giftOrder, groupid);
            _incomeHistoryRepo.Insert(new IMS_AssociateIncomeHistoryEntity()
            {
                CreateDate = DateTime.Now,
                SourceNo = giftOrder.No,
                SourceType = (int)AssociateOrderType.GiftCard,
                Status = (int)AssociateIncomeStatus.Avail,
                UpdateDate = DateTime.Now,
                AssociateIncome = thisIncome,
                AssociateUserId = associateUserId,
                GroupId = groupid
            });

            AssociateIncomeAccount.AvailGift(associateUserId, thisIncome);

            return true;
        }
        public bool Avail(IMS_AssociateIncomeHistoryEntity incomeHistory)
        {

            incomeHistory.Status = (int)AssociateIncomeStatus.Avail;
            incomeHistory.UpdateDate = DateTime.Now;
            _incomeHistoryRepo.Update(incomeHistory);
            AssociateIncomeAccount.Avail(incomeHistory.AssociateUserId, incomeHistory.AssociateIncome);
            return true;
        }
        public bool Void(IMS_AssociateIncomeHistoryEntity incomeHistory)
        {

            incomeHistory.Status = (int)AssociateIncomeStatus.Void;
            incomeHistory.UpdateDate = DateTime.Now;
            _incomeHistoryRepo.Update(incomeHistory);
            AssociateIncomeAccount.Void(incomeHistory.AssociateUserId, incomeHistory.AssociateIncome);
            return true;
        }


        private decimal ComputeIncome(IMS_GiftCardOrderEntity giftcardOrder, int? groupId)
        {
            if (giftcardOrder == null)
                return 0m;

            return DoInternalCompute(giftcardOrder.Price.Value, ConfigManager.IMS_GIFTCARD_CAT_ID, 1, groupId);
        }
        private int? GetGroupFromGiftCard(IMS_GiftCardOrderEntity giftcardOrder)
        {
            var group = _db.Set<IMS_GiftCardItemEntity>().Where(igi => igi.GiftCardId == giftcardOrder.GiftCardItemId)
                         .Join(_db.Set<IMS_GiftCardEntity>(), o => o.GiftCardId, i => i.Id, (o, i) => i)
                         .FirstOrDefault();
            return group == null ? null : group.GroupId;
        }
        private decimal ComputeIncome(OrderEntity order, OrderItemEntity orderItem)
        {
            if (order == null)
                return 0m;
            var incomeSum = 0m;
            var hasPromotion = order.PromotionFlag ?? false;
            IPromotionSharePolicy promotionSharedPolicy = null;
            if (hasPromotion)
            {
                promotionSharedPolicy = new PromotionService().GetDefaultSharePolicy();
                promotionSharedPolicy.SourceOrder = order;
            }

            var product = _db.Set<ProductEntity>().Find(orderItem.ProductId);
            var incomePrice = orderItem.ItemPrice;
            if (hasPromotion)
                incomePrice = promotionSharedPolicy.ComputeActualPrice(orderItem);
            var groupId = _db.Set<StoreEntity>().Find(orderItem.StoreId).Group_Id;

            incomeSum = DoInternalCompute(incomePrice, product.Tag_Id, orderItem.Quantity, groupId);
            return incomeSum;
        }
        private decimal DoInternalCompute(decimal price, int categoryId, int quantity, int? groupId = null)
        {
            var incomeRuleEntity = _db.Set<IMS_AssociateIncomeRuleEntity>().Where(iar => iar.Status == (int)DataStatus.Normal &&
                            iar.FromDate <= DateTime.Now && iar.EndDate > DateTime.Now
                            && iar.CategoryId == categoryId
                            && (groupId == null || iar.GroupId == groupId)).FirstOrDefault();
            if (incomeRuleEntity == null)
                return 0m;

            IIncomeRule rule = CreateRule(incomeRuleEntity.RuleType);
            if (rule == null)
                return 0m;
            var income = rule.Compute(incomeRuleEntity.Id, price, quantity);
            return income;

        }

        private IIncomeRule CreateRule(int ruleType)
        {
            switch (ruleType)
            {
                case (int)IncomeRuleType.Fix:
                    return new IncomeRuleFix();
                case (int)IncomeRuleType.Flat:
                    return new IncomeRuleFlatten();
                case (int)IncomeRuleType.Flex:
                    return new IncomeRuleFlex();
                default:
                    return null;
            }
        }

        internal class IncomePromotionCalculator
        {
            private static string _promotionTimeRange =
                        ConfigurationManager.AppSettings["intime.associate.promotion.timerange"];

            private static int _beginnerOrderCountBenchmark =
                string.IsNullOrEmpty(ConfigurationManager.AppSettings["intime.associate.promotion.benchmark.x"])
                    ? 3
                    : int.Parse(ConfigurationManager.AppSettings["intime.associate.promotion.benchmark.x"]);

            private static string _experiencedAssociateBenchmarkY =
                ConfigurationManager.AppSettings["intime.associate.promotion.benchmark.y"];

            internal IncomePromotionCalculator()
            {
                setIsInPromotionRange();
                setIsMatchPromotionCondition();
            }

            private void setIsMatchPromotionCondition()
            {
                orderAmountBenchmark4Experienced = decimal.MaxValue;
                orderCountBenchmark4Experienced = int.MaxValue;
                if (string.IsNullOrEmpty(_experiencedAssociateBenchmarkY)) return;
                var slices = _experiencedAssociateBenchmarkY.Split('|');
                if (slices.Count() != 2) return;
                int cnt;
                decimal amount;
                if (!int.TryParse(slices[0], out cnt) || !decimal.TryParse(slices[1], out amount)) return;
                orderAmountBenchmark4Experienced = amount;
                orderCountBenchmark4Experienced = cnt;
            }

            private static decimal orderAmountBenchmark4Experienced;
            private static int orderCountBenchmark4Experienced;
            private static DateTime promotionBegin;
            private static DateTime promotionEnd;
            private static void setIsInPromotionRange()
            {
                var timeRange = _promotionTimeRange;

                if (String.IsNullOrEmpty(timeRange))
                {
                    promotionBegin = DateTime.Now.AddYears(-10);
                    promotionEnd = DateTime.Now.AddYears(-10);
                    return;
                }

                var slices = timeRange.Split('|');
                if (slices.Length != 2)
                {
                    return;
                }

                if (!DateTime.TryParse(slices[0], out promotionBegin))
                {
                    promotionBegin = DateTime.Now.AddDays(1);
                }

                if (!DateTime.TryParse(slices[1], out promotionEnd))
                {
                    promotionEnd = DateTime.Now.AddDays(-1);
                }

            }

            internal decimal Calculate(IMS_AssociateIncomeHistoryEntity item, DbContext db)
            {
                var income = item.AssociateIncome;
                if (!isPromoting())
                {
                    return income;
                }

                if (!isMatchPromotionCondition(item, db))
                {
                    return income;
                }

                return income * 2;
            }

            /// <summary>
            /// 是否满足佣金翻倍条件。
            /// 活动之前没有销售经验的导购前3单和10单后佣金翻倍，有经验的超过10单的部分翻倍
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="db"></param>
            /// <returns></returns>
            private bool isMatchPromotionCondition(IMS_AssociateIncomeHistoryEntity item, DbContext db)
            {
                int userId = item.AssociateUserId;
                if (isExperiencedAssociate(db, userId))
                {
                    return isSatisfyOrderCountWithAmountBenchmark(db, userId);
                }

                var cnt =
                    db.Set<IMS_AssociateIncomeHistoryEntity>()
                        .Where(
                            t =>
                                t.AssociateUserId == userId &&
                                t.SourceType == (int)AssociateOrderType.Product &&
                                t.CreateDate >= promotionBegin &&
                                t.CreateDate < promotionEnd &&
                                db.Set<OrderTransactionEntity>().Any(x => x.OrderNo == t.SourceNo))
                        .Select(x => x.SourceNo)
                        .Distinct()
                        .Count();

                if (cnt < _beginnerOrderCountBenchmark)
                {
                    return true;
                }
                return isSatisfyOrderCountWithAmountBenchmark(db, userId);
            }

            /// <summary>
            /// 有付款订单则认为是有经验的导购
            /// </summary>
            /// <param name="db"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            private bool isExperiencedAssociate(DbContext db, int userId)
            {
                return db.Set<IMS_AssociateIncomeHistoryEntity>()
                                   .Where(t => t.AssociateUserId == userId &&
                                               t.SourceType == (int)AssociateOrderType.Product &&
                                               t.CreateDate < promotionBegin).Join(db.Set<OrderTransactionEntity>(), h => h.SourceNo, ot => ot.OrderNo, (h, o) => h
                                   ).Any();
            }

            /// <summary>
            /// 订单金额>基准金额 且 订单数量 > 基准数量
            /// </summary>
            /// <param name="db"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            internal bool isSatisfyOrderCountWithAmountBenchmark(DbContext db, int userId)
            {
                var cnt = db.Set<IMS_AssociateIncomeHistoryEntity>()
                        .Where(
                            x =>
                                x.SourceType == (int)AssociateOrderType.Product &&
                                x.AssociateUserId == userId &&
                                x.CreateDate >= promotionBegin &&
                                x.CreateDate < promotionEnd &&
                                db.Set<OrderEntity>().Any(t => t.OrderNo == x.SourceNo && t.TotalAmount >= orderAmountBenchmark4Experienced) &&
                                db.Set<OrderTransactionEntity>().Any(t => t.OrderNo == x.SourceNo))
                        .Select(x => x.SourceNo)
                        .Distinct()
                        .Count();
                return cnt >= orderCountBenchmark4Experienced;
            }

            internal bool isPromoting()
            {
                return DateTime.Now >= promotionBegin && DateTime.Now < promotionEnd;
            }
        }
    }
}
