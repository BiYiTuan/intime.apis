using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.Order;
using Yintai.Hangzhou.Model.Product;

namespace Yintai.Hangzhou.Model.Order
{
    public class OrderCreate:BaseModel
    {
        public IEnumerable<OrderItem> Products { get; set; }


        public OrderShippingAddress ShippingAddress { get; set; }

        public PaymentMethod Payment { get; set; }

        public bool NeedInvoice { get; set; }
        public string InvoiceTitle { get; set; }
        public string InvoiceDetail { get; set; }
        public string Memo { get; set; }
        public int? StoreId { get; set; }
        public int? ComboId { get; set; }
        public string Channel { get; set; }
        public int ShippingType
        {
            get
            {
                if (ShippingAddress == null || ShippingAddress.ShippingZipCode == null)
                    return (int)ShipType.Self;
                return (int)ShipType.TrdParty;
            }
        }
    }
  
}
