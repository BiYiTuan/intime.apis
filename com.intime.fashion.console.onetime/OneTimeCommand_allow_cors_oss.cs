using Aliyun.OpenServices.OpenStorageService;
using CLAP;
using com.intime.fashion.common.config;
using com.intime.fashion.service.images;
using com.intime.fashion.service.search;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.console.onetime
{
    partial class OneTimeCommand
    {
        [Verb(IsDefault = false, Description = "enable cors for oss")]
        static void Cors()
        {
            var config = CommonConfiguration<AliYunConfiguration>.Current;
            var ossClient = new OssClient(new Uri(config.END_POINT), config.ACCESS_ID, config.ACCESS_KEY);
            var request = new SetBucketCorsRequest("irss");
            var rule = new CORSRule();
            rule.AddAllowedOrigin("*");
            rule.AddAllowedMethod("GET");
            rule.AddAllowedMethod("HEAD");
            request.AddCORSRule(rule);
            ossClient.SetBucketCors(request);

            var rules = ossClient.GetBucketCors("irss");
            foreach (var existRule in rules)
            { 
                Console.WriteLine(string.Format("exist rule:{0}",existRule.AllowedOrigins.Aggregate(new StringBuilder(),(s,v)=>s.Append(v),s=>s.ToString())));
            }
        }

    
    }
}
