using System;

namespace com.intime.o2o.data.exchange.Tmall.Core
{
    public static class ConvertionExtensions
    {
        public static T? ConvertTo<T>(this IConvertible convertible) where T : struct
        {
            if (null == convertible)
            {
                return null;
            }

            return (T?)Convert.ChangeType(convertible, typeof(T));
        }
    }
}
