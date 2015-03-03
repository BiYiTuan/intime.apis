using com.intime.fashion.common.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.analysis.DTO
{
    abstract class EventRequestBase:AnalysisRequestBase
    {


        protected override string MetricType
        {
            get { return "eventMetrics"; }
        }

        public override string Url
        {
            get
            {
                var config = CommonConfiguration<AnalysisConfiguration>.Current;
                return string.Format("{0}/{1}/Event?apiAccessCode={2}&apiKey={3}&startDate={4}&endDate={5}&eventName={6}",
                    config.ServiceBaseUri,
                    MetricType,
                    config.ApiAccessCode,
                    config.ApiKey,
                    StartDate.ToString("yyyy-MM-dd"),
                    EndDate.ToString("yyyy-MM-dd"),
                    MetricName
                    );
            }
        }
    }
}
