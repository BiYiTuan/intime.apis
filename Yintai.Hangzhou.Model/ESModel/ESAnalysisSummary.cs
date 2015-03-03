using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.ESModel
{
    public class ESAnalysisSummary
    {
        public int NewUsers { get; set; }

        public int ActiveUsers { get; set; }

        public int Sessions { get; set; }

        public DateTime StarDate { get; set; }

        public DateTime EndDate { get; set; }

        public string App { get; set; }
    }
}
