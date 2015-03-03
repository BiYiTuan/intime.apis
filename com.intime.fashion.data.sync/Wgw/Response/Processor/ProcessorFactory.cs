using System;
using com.intime.fashion.data.sync.Wgw.Request.Order;
using com.intime.fashion.data.sync.Wgw.Response.Processor.Order;

namespace com.intime.fashion.data.sync.Wgw.Response.Processor
{
    public class ProcessorFactory
    {
        public static IProcessor CreateProcessor<T>() where T:class,new()
        {
            return new T() as IProcessor;
        }
    }

    public class OrderPrcossorFactory
    {
        public static IProcessor CreateProcessor(string dealState)
        {
            if (dealState == OrderStatusConst.STATE_WG_PAY_OK)
            {
                return new PaidOrderProcessor();
            }

            if (dealState == OrderStatusConst.STATE_WG_WAIT_PAY)
            {
                return new CreatedOrderProcessor();
            }

            if (dealState == OrderStatusConst.STATE_WG_CANCLE)
            {
                return new CancelOrderProcessor();
            }
            throw new NotSupportedException(string.Format("Order status does'nt supported {0}",dealState));
        }
    }
}
