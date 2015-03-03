using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.tmall.Models
{
    public partial class LogisticsAddressMapping
    {
        public int Id { get; set; }
        public long TmallAddressId { get; set; }
        public int StoreId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
