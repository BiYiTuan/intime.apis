using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.erp.Models
{
    public partial class SALE_CODE
    {
        public SALE_CODE()
        {
            this.SUPPLY_MIN_PRICE = new List<SUPPLY_MIN_PRICE>();
        }

        public decimal SID { get; set; }
        public Nullable<decimal> SUPPLY_INFO_SID { get; set; }
        public Nullable<decimal> SUPPLY_CLASS_SID { get; set; }
        public Nullable<decimal> SHOP_SID { get; set; }
        public string SALE_CODE1 { get; set; }
        public short SUPPLY_SALING_BIT { get; set; }
        public Nullable<decimal> AREA_SID { get; set; }
        public short NET_SALE_BIT { get; set; }
        public Nullable<decimal> OPT_USER_SID { get; set; }
        public string OPT_REAL_NAME { get; set; }
        public Nullable<System.DateTime> OPT_UPDATE_TIME { get; set; }
        public string MEMO { get; set; }
        public Nullable<System.DateTime> SUPPLY_SALING_TIME { get; set; }
        public string SUPPLY_SHOP_CODE { get; set; }
        public string COUNTER_TEL { get; set; }
        public Nullable<short> ACTIVE_BIT { get; set; }
        public string HOURSE_NUMBER { get; set; }
        public Nullable<short> DISCOUNT_RATE_TYPE { get; set; }
        public string IMPORT_GYS_SHOP_NO { get; set; }
        public string SALE_CODE_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string IMAGE_NAME { get; set; }
        public Nullable<decimal> DISPLAY_NUM { get; set; }
        public string DESCRIPT { get; set; }
        public Nullable<short> DISCOUNT_RATE_UNION { get; set; }
        public Nullable<decimal> PRO_RULE_MEMBER_SID { get; set; }
        public Nullable<decimal> SC_OFF_UP { get; set; }
        public Nullable<decimal> SC_OFF_DOWN { get; set; }
        public Nullable<decimal> SC_MEMBER_PRICE_OFF { get; set; }
        public Nullable<decimal> SC_NOT_MEMBER_PRICE_OFF { get; set; }
        public Nullable<decimal> PRO_RULE_MEMBER_NET_SID { get; set; }
        public Nullable<decimal> SC_NET_PRICE_OFF { get; set; }
        public Nullable<decimal> SC_NET_OFF_UP { get; set; }
        public Nullable<decimal> SC_NET_OFF_DOWN { get; set; }
        public virtual SHOP_INFO SHOP_INFO { get; set; }
        public virtual ICollection<SUPPLY_MIN_PRICE> SUPPLY_MIN_PRICE { get; set; }
    }
}
