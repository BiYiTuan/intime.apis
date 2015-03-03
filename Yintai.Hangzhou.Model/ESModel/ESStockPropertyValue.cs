
using System;

namespace Yintai.Hangzhou.Model.ESModel
{
    public class ESStockPropertyValue
    {
        public int Id { get; set; }

        public int InventoryId { get; set; }

        public string PropertyData { get; set; }

        public DateTime UpdateTime { get; set; }

        public string BrandSizeCode { get; set; }

        public string BrandSizeName { get; set; }
    }
}
