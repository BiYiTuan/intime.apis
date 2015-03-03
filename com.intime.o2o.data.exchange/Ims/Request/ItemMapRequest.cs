
namespace com.intime.o2o.data.exchange.Ims.Request
{
    public class ItemMapRequest: ImsRequest<dynamic, dynamic>
    {
        public override string GetResourceUri()
        {
            return "gg/stock/map";
        }
    }
}
