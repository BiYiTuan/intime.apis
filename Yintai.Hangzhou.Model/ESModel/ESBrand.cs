using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESBrand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Group { get; set; }
        public IEnumerable<ESStore> Stores { get; set; }
    }
}
