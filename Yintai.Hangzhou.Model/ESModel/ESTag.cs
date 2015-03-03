using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int SortOrder { get; set; }
        public int SizeType { get; set; }
        public IEnumerable<ESSize> Sizes { get; set; }
    }
}
