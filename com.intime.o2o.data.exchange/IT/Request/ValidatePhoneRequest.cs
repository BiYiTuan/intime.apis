
namespace com.intime.o2o.data.exchange.IT.Request
{
    public class ValidatePhoneRequest : Request<dynamic, Response<dynamic>>
    {
        public override string GetResourceUri()
        {
            return "precard-rs/rest/groupcard/checkPhone";
        }
    }
}
