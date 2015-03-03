using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.tmall.Models
{
    public partial class JDP_TB_ITEM
    {
        public long num_iid { get; set; }
        public string nick { get; set; }
        public string approve_status { get; set; }
        public string cid { get; set; }
        public string has_showcase { get; set; }
        public string has_discount { get; set; }
        public System.DateTime created { get; set; }
        public System.DateTime modified { get; set; }
        public System.DateTime jdp_created { get; set; }
        public System.DateTime jdp_modified { get; set; }
        public int jdp_delete { get; set; }
        public string jdp_hashcode { get; set; }
        public string jdp_response { get; set; }
    }
}
