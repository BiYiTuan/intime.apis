using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.tmall.Models
{
    public partial class ShipViaMapping
    {
        public int Id { get; set; }
        public int ImsShipViaId { get; set; }
        public string CompanyCode { get; set; }
        public string Channel { get; set; }
        public string CompanyName { get; set; }
    }
}
