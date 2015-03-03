using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Extension
{
    public static class DictionaryExtension
    {
        public static string SafeGet(this Dictionary<string, string> dic,string key,string value="")
        {
            if (string.IsNullOrEmpty(key))
                return value;
            if (dic.ContainsKey(key))
                return dic[key];
            return value;
        }
    }
}
