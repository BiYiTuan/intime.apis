using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.erp.Models
{
    public partial class PRO_PICTURE
    {
        public decimal SID { get; set; }
        public Nullable<decimal> PRODUCT_SID { get; set; }
        public string PRO_PICT_NAME { get; set; }
        public string PRO_PICT_DIR { get; set; }
        public Nullable<decimal> PRO_PICT_ORDER { get; set; }
        public short PICTURE_MODEL_BIT { get; set; }
        public short PICTURE_MAST_BIT { get; set; }
        public Nullable<decimal> PRO_COLOR_SID { get; set; }
        public Nullable<decimal> PRO_PICTURE_SIZE_SID { get; set; }
        public Nullable<System.DateTime> PRO_WRI_TIME { get; set; }
        public Nullable<decimal> OPT_USER_SID { get; set; }
        public string OPT_REAL_NAME { get; set; }
        public Nullable<System.DateTime> OPT_UPDATE_TIME { get; set; }
        public Nullable<int> VERSION { get; set; }
        public Nullable<short> MAIN_BIT { get; set; }
        public Nullable<short> UPLOAD_BIT { get; set; }
        public Nullable<decimal> CRC { get; set; }
        public Nullable<short> DELETE_BIT { get; set; }
    }
}
