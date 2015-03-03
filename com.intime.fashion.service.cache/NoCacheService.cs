using com.intime.fashion.common.config;
using com.intime.fashion.service.contract;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.cache
{
   public class NoCacheService:ICacheService
   {

        public IEnumerable<T> GetList<T>(string name, Func<IEnumerable<T>> refreshResult)
        {
            
             return refreshResult().ToArray();
             
        }
        public T Get<T>(string name, Func<T> refreshResult) where T: class
        {

            return refreshResult();
        }
       
       
   }
}
