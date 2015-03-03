using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESStore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public int Status { get; set; }
        public Location Location { get; set; }
        public Nullable<decimal> GpsLat { get; set; }
        public Nullable<decimal> GpsLng { get; set; }
        public Nullable<decimal> GpsAlt { get; set; }
        public IEnumerable<ESResource> Resource { get; set; }
        public IEnumerable<ESDepartment> Departments { get; set; }
        public int? GroupId { get; set; }
    }

    public class Location
    {
        public decimal Lat { get; set; }  
        public decimal Lon { get; set; }

    }
}
