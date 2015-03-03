using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.ESModel
{
    public class ESAnalysisEvent
    {

        public string Count { get; set; }

        public string EventId { get; set; }

        public int? SectionId { get; set; }

        public int StoreId { get; set; }

        public int AssociateId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int EventType { get; set; }
    }
}
