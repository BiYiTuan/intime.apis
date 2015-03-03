using Aliyun.OpenServices.OpenStorageService;
using com.intime.fashion.common.config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.images
{
    public static class AliyunUtil
    {
        public static void Transfer2AliyunAsync(string local, string key,Action<object> traceCallBack)
        {
            Task.Factory.StartNew(() =>
            {
                Transfer2Aliyun(local, key,traceCallBack);
            });
        }
        public static bool Transfer2Aliyun(string local, string key,Action<object> traceCallBack )
        {
                try
                {
                    var config = CommonConfiguration<AliYunConfiguration>.Current;
                    var client = GetOssClient();
                    using (var fs = File.Open(local, FileMode.Open))
                    {
                        client.PutObject(config.RSS_BUCKET_NAME, key.Replace("\\","/"), fs, new ObjectMetadata());
                    }
                    try
                    {
                        File.Delete(local);
                    }
                    catch (Exception ex)
                    { 
                       //swallow exception here 
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    traceCallBack(ex);
                    traceCallBack(string.Format("transfer file:{0} error, need redo it", local));
                    return false;
                }
           
        }

        private static OssClient GetOssClient()
        {
            var config = CommonConfiguration<AliYunConfiguration>.Current;
            return new OssClient(new Uri(config.END_POINT),config.ACCESS_ID, config.ACCESS_KEY);
        }
    }
}
