using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO
{
    public static class DateTimeExtension
    {
        public static string ToClientTimeFormat(this Nullable<DateTime> date)
        {
            if (!date.HasValue)
                return string.Empty;
            return date.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToClientTimeFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
