using System;
namespace com.intime.fashion.service.contract
{
   public interface IAssociateIncomeService
    {
        bool Avail(int associateUserId, Yintai.Hangzhou.Data.Models.IMS_GiftCardOrderEntity giftOrder);
        bool Avail(Yintai.Hangzhou.Data.Models.IMS_AssociateIncomeHistoryEntity incomeHistory);
        bool Create(Yintai.Hangzhou.Data.Models.OrderEntity order);
        bool Froze(string orderNo);
        bool Void(Yintai.Hangzhou.Data.Models.IMS_AssociateIncomeHistoryEntity incomeHistory);

        /// <summary>
        /// 系统商品导购返利处理，目前返利为0，有人工进行返利的操作
        /// </summary>
        /// <param name="order"></param>
        /// <param name="comboId">组合Id</param>
        /// <returns>操作结果</returns>
        bool CreateSystem(Yintai.Hangzhou.Data.Models.OrderEntity order, int? comboId);
    }
}
