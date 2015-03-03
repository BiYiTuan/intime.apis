using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.erp.Models
{
    public partial class SUPPLY_MIN_PRICE
    {
        public SUPPLY_MIN_PRICE()
        {
            this.SUPPLY_MIN_PRICE_MX = new List<SUPPLY_MIN_PRICE_MX>();
        }

        public decimal SID { get; set; }
        public decimal PRODUCT_SID { get; set; }
        public Nullable<System.DateTime> PRO_SELLING_TIME { get; set; }
        public decimal BRAND_SID { get; set; }
        public Nullable<decimal> PROMOTION_PRICE { get; set; }
        public Nullable<decimal> ORIGINAL_PRICE { get; set; }
        public Nullable<decimal> CURRENT_PRICE { get; set; }
        public Nullable<System.DateTime> PROMOTION_BEGIN_TIME { get; set; }
        public Nullable<System.DateTime> PROMOTION_END_TIME { get; set; }
        public Nullable<decimal> OFF_VALUE { get; set; }
        public Nullable<decimal> PRO_SUM { get; set; }
        public string PRO_SKU { get; set; }
        public string PRO_SKU2 { get; set; }
        public string PROT_COLOR2 { get; set; }
        public string PRO_PICT_NAME { get; set; }
        public string PRODUCT_NAME { get; set; }
        public Nullable<decimal> ORDER_WEIGHT { get; set; }
        public Nullable<short> IS_NEW { get; set; }
        public Nullable<short> IS_HOT { get; set; }
        public Nullable<short> IS_PREVIEW { get; set; }
        public Nullable<decimal> OPT_USER_SID { get; set; }
        public string OPT_REAL_NAME { get; set; }
        public Nullable<System.DateTime> OPT_UPDATE_TIME { get; set; }
        public Nullable<decimal> SALE_CODE_SID { get; set; }
        public Nullable<int> VERSION { get; set; }
        public Nullable<decimal> PRO_CLASS_SID { get; set; }
        public Nullable<decimal> SHOP_SID { get; set; }
        public string PRO_DESC { get; set; }
        public string BARCODE { get; set; }
        public Nullable<short> PRO_SELLING { get; set; }
        public Nullable<System.DateTime> PRO_SELLING_DOWN_TIME { get; set; }
        public virtual SALE_CODE SALE_CODE { get; set; }
        public virtual ICollection<SUPPLY_MIN_PRICE_MX> SUPPLY_MIN_PRICE_MX { get; set; }
    }
}
