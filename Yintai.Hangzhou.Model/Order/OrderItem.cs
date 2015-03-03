using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Model.Product;

namespace Yintai.Hangzhou.Model.Order
{
   public class OrderItem:BaseModel
    {
        public int ProductId { get; set; }

        public string Desc { get; set; }

        public int Quantity { get; set; }

        public int? StoreId { get; set; }

        public int? SectionId { get; set; }

        public ProductPropertyValue Properties { get; set; }

        public string ProductDesc
        {
            get
            {
                if (Properties == null)
                    return string.Empty;
                var description = string.Format("{0}:{1},{2}:{3}", "颜色", Properties.ColorValueName, "尺码", Properties.SizeValueName)
                   ;
                if (description.Length > 0)
                    description = description.TrimEnd(',');
                return description;

            }
        }
    }
}
