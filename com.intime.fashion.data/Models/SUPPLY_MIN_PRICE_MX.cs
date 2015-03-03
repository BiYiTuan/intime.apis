using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.erp.Models
{
    public partial class SUPPLY_MIN_PRICE_MX
    {
        public decimal SID { get; set; }
        public decimal SUPPLY_MIN_PRICE_SID { get; set; }
        public decimal PRODUCT_SID { get; set; }
        public decimal PRO_DETAIL_SID { get; set; }
        public Nullable<decimal> PRO_STOCK_SUM { get; set; }
        public Nullable<short> PRO_ACTIVE_BIT { get; set; }
        public Nullable<decimal> VERSION { get; set; }
        public string PRO_COLOR { get; set; }
        public string PRO_STAN_NAME { get; set; }
        public Nullable<decimal> PRO_COLOR_SID { get; set; }
        public Nullable<decimal> PRO_STAN_SID { get; set; }
        public Nullable<decimal> OPT_USER_SID { get; set; }
        public string OPT_REAL_NAME { get; set; }
        public Nullable<System.DateTime> OPT_UPDATE_TIME { get; set; }
        public virtual SUPPLY_MIN_PRICE SUPPLY_MIN_PRICE { get; set; }
    }
}
