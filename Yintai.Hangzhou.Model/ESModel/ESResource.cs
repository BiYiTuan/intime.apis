using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public int SortOrder { get; set; }
        public bool IsDefault { get; set; }
        public int Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Status { get; set; }
        public int SourceId { get; set; }
        public int SourceType { get; set; }
        public Nullable<int> ColorId { get; set; }
    }
}
