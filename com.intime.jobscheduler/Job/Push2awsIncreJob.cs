using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.intime.jobscheduler.Job
{
    class Push2awsIncreJob:Push2awsJob
    {
        protected override DateTime BenchDate(Quartz.IJobExecutionContext context)
        {
            //every 5mins, figure out new create/updated products, promotions
            var dataMap = context.JobDetail.JobDataMap;
            var interval = dataMap.ContainsKey("interval") ? dataMap.GetInt("interval") : 5;
            if (!dataMap.ContainsKey("benchtime"))
            {
                dataMap.Put("benchtime", DateTime.Now.AddMinutes(-interval));
            }
            else
            {
                dataMap["benchtime"] = dataMap.GetDateTimeValue("benchtime").AddMinutes(interval);
            }

            return dataMap.GetDateTime("benchtime");
        }
        protected override bool CascadPush
        {
            get
            {
                return true;
            }
        }
    }
}
