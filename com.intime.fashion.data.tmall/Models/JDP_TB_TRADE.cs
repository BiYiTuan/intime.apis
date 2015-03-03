using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.tmall.Models
{
    public partial class JDP_TB_TRADE
    {
        public long tid { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string seller_nick { get; set; }
        public string buyer_nick { get; set; }
        public System.DateTime created { get; set; }
        public System.DateTime modified { get; set; }
        public System.DateTime jdp_created { get; set; }
        public System.DateTime jdp_modified { get; set; }
        public string jdp_hashcode { get; set; }
        public string jdp_response { get; set; }
    }
}
