using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Product
{
   public class ProductPropertyValue:BaseModel
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int? SizeValueId { get; set; }
        public string SizeValueName { get; set; }

        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int? ColorValueId { get; set; }
        public string ColorValueName { get; set; }
    }
}
