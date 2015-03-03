using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.intime.fashion.service.analysis.DTO
{
    abstract class AnalysisRequestBase
    {
        protected abstract string MetricName { get; }
        protected abstract string MetricType { get; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public abstract string Url { get; }
    }
}
