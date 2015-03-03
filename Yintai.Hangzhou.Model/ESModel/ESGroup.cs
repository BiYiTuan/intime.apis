
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public int UpdatedUser { get; set; }
        public int Status { get; set; }
    }
}
