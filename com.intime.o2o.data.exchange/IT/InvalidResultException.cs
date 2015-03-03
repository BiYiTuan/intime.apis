using System;

namespace com.intime.o2o.data.exchange.IT
{
    public class InvalidResultException:Exception
    {
        public InvalidResultException(string message) : base(message)
        {
        }
    }
}
