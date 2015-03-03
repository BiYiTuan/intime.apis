using com.intime.fashion.common.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.analysis.DTO
{
   abstract class SummaryRequestBase:AnalysisRequestBase
    {
        
        protected override string MetricType
        {
            get { return "appMetrics"; }
        }

        public override string Url
        {
            get {
                var config = CommonConfiguration<AnalysisConfiguration>.Current;
                return string.Format("{0}/{1}/{2}?apiAccessCode={3}&apiKey={4}&startDate={5}&endDate={6}",
                    config.ServiceBaseUri,
                    MetricType,
                    MetricName,
                    config.ApiAccessCode,
                    config.ApiKey,
                    StartDate.ToString("yyyy-MM-dd"),
                    EndDate.ToString("yyyy-MM-dd")
                    );
            }
        }
    }
}
