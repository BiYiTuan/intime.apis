using System;
using Yintai.Hangzhou.Data.Models;
namespace com.intime.fashion.service.contract
{
   public interface IComboService
    {
        IMS_ComboEntity CreateComboFromProduct(ProductEntity productEntity, IMS_AssociateEntity associateEntity);
        bool IfCanOnline(int userId);
        void OfflineComboOne(int authuid);
        void RefreshPrice(int productId);
        void RefreshPrice(IMS_ComboEntity combo);
    }
}
