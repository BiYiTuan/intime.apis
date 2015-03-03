using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace com.intime.o2o.data.exchange.Tmall.Product.Tools
{
    public class UtilTool
    {
        public IDictionary<string, string> Zip(List<string> a, ArrayList b)
        {
            if (a == null || b == null)
            {
                return new Dictionary<string, string>();
            }

            var count = Math.Min(a.Count, b.Count);

            var dict = new Dictionary<string, string>();
            for (var i = 0; i < count; i++)
            {
                if (!dict.ContainsKey(a[i].ToString(CultureInfo.InvariantCulture)))
                    dict.Add(a[i].ToString(CultureInfo.InvariantCulture), b[i].ToString());
            }

            return dict;
        }

        public string GetValue(IDictionary<string, string> dic, string key)
        {
            if (dic.ContainsKey(key)) return dic[key];
            return string.Empty;
        }

        public string Sub(string input, int count)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            if (input.Length < count)
                return input;

            return input.Substring(0, count);
        }
    }
}
