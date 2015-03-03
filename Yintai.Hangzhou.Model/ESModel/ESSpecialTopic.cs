using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESSpecialTopic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreateUser { get; set; }
        public IEnumerable<ESResource> Resource { get; set; }
        public int Type { get; set; }
        public string TargetValue { get; set; }
    }
}
