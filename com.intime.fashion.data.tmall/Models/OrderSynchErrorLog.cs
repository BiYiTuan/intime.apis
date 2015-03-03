using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.tmall.Models
{
    public partial class OrderSynchErrorLog
    {
        public int Id { get; set; }
        public long TmallOrderId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
        public int Type { get; set; }
        public int FailedCount { get; set; }
    }
}
