using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    class FreeMemoryJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            GC.Collect();
        }
    }
}
