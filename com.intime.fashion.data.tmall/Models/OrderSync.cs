using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.tmall.Models
{
    public partial class OrderSync
    {
        public int Id { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string ImsOrderNo { get; set; }
        public long TmallOrderId { get; set; }
        public int Type { get; set; }
        public bool LogisticsSynced { get; set; }
    }
}
