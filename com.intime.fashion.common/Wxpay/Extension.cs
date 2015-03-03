using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Wxpay
{
   public static class DateTimeExtension
    {
       public static long TicksOfWx(this DateTime date)
       {
          return (long)(date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
       }
    }
  
}