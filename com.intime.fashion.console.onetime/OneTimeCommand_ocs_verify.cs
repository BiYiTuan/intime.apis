using Aliyun.OpenServices.OpenStorageService;
using CLAP;
using com.intime.fashion.common.config;
using com.intime.fashion.service.images;
using com.intime.fashion.service.search;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.console.onetime
{
    partial class OneTimeCommand
    {
        [Verb(IsDefault = false,Aliases="ocs", Description = "verify access to ocs with key:")]
        static void OCS_Verify(
            [Description("the key to get")]
            string key)
        {
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            var memConfig = CommonConfiguration<Cache_AuthkeyConfiguration>.Current;
            IPAddress newaddress = Dns.GetHostEntry(memConfig.Host).AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(newaddress, 11211);
            config.Servers.Add(ipEndPoint);

            config.Protocol = MemcachedProtocol.Binary;
            config.Authentication.Type = typeof(PlainTextAuthenticator);
            config.Authentication.Parameters["zone"] = string.Empty;
            config.Authentication.Parameters["userName"] = memConfig.UserName;
            config.Authentication.Parameters["password"] = memConfig.Password;
            config.SocketPool.MinPoolSize = 5;
            config.SocketPool.MaxPoolSize = 200;
            var client = new MemcachedClient(config);
            object keyValue;

            if (client.TryGet(key??string.Empty,out keyValue))
                Console.WriteLine(string.Format("get {0} with value:{1}",key,keyValue));
            else
                Console.WriteLine("get key error:"+(key??string.Empty)+(keyValue==null?"empty":keyValue.ToString()));
            

        }

    
    }
}
