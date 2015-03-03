using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.tmall.Models
{
    public partial class SubOrder
    {
        public int Id { get; set; }
        public long TmallOrderId { get; set; }
        public long TmallSubOrderId { get; set; }
        public bool LogisticsSynced { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public bool IsForceSynced { get; set; }
        public string Store { get; set; }
        public int ImsInventoryId { get; set; }
    }
}
