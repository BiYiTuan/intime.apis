using com.intime.fashion.service.images;
using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.jobscheduler.Job.Aliyun
{
    [DisallowConcurrentExecution]
    public class TransferRSS2AliyunJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(this.GetType());

            JobDataMap data = context.JobDetail.JobDataMap;
            var path = data.GetString("path");
            var pattern = data.GetString("pattern");
            var prefixRemove = data.GetString("keyunprefix");

            int successCount = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var file in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories))
            {
               bool success = AliyunUtil.Transfer2Aliyun(file, file.Replace(prefixRemove,""),(object o)=>log.Error(o));
               if (success)
                   successCount++;
            }
            sw.Stop();
            log.Info(string.Format("{0} resources for aliyun in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }
    }
}
