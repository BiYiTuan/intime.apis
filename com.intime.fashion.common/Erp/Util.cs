using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Erp
{
    public static class DateTimeExtension
    {
        public static long SecondsNow(this DateTime input)
        {
            return input.Ticks / TimeSpan.TicksPerSecond;
        }
    }
}
