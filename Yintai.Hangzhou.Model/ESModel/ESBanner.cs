
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
   
    public class ESBanner
    {
        public int Status { get; set; }
        public int Id { get; set; }
        public int SourceType { get; set; }
        public int SortOrder { get; set; }
        public ESPromotion Promotion { get; set; }
        public IEnumerable<ESResource> Resource { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
