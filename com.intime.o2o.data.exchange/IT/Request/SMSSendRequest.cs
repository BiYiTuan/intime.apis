namespace com.intime.o2o.data.exchange.IT.Request
{
    public class SMSSendRequest : Request<dynamic, Response<dynamic>>
    {
        public override string GetResourceUri()
        {
            return "intimers/api/sendsms/notice";
        }
    }
}