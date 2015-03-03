using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Order
{
    public class OrderShippingAddress:BaseModel
    {

        public string ShippingContactPerson { get; set; }

        public string ShippingContactPhone { get; set; }

        public string ShippingZipCode { get; set; }

        public string ShippingAddress { get; set; }
    }
}
