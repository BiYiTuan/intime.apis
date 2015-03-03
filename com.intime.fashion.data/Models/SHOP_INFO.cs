using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.erp.Models
{
    public partial class SHOP_INFO
    {
        public SHOP_INFO()
        {
            this.SALE_CODE = new List<SALE_CODE>();
        }

        public decimal SID { get; set; }
        public string SHOP_NAME { get; set; }
        public string SHOP_LINKER { get; set; }
        public string LINKER_PHONE { get; set; }
        public string SHOP_NUM { get; set; }
        public short NET_BIT { get; set; }
        public string SHOP_ADDR { get; set; }
        public Nullable<int> POSTCODE { get; set; }
        public string REFUND_LINKER { get; set; }
        public string REFUND_TEL { get; set; }
        public string SPELL { get; set; }
        public Nullable<decimal> OPT_USER_SID { get; set; }
        public string OPT_REAL_NAME { get; set; }
        public Nullable<System.DateTime> OPT_UPDATE_TIME { get; set; }
        public Nullable<decimal> POSSHOPCODE { get; set; }
        public Nullable<decimal> PRO_CLASS_DICT_SID { get; set; }
        public Nullable<short> PRICE_UP_BIT { get; set; }
        public Nullable<short> PRINT_BARGAIN { get; set; }
        public string WEB_SITE { get; set; }
        public virtual ICollection<SALE_CODE> SALE_CODE { get; set; }
    }
}
