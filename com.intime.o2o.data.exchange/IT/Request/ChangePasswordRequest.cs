using com.intime.o2o.data.exchange.IT.Response;

namespace com.intime.o2o.data.exchange.IT.Request
{
    public class ChangePasswordRequest : Request<dynamic, PasswordResponse>
    {
        public override string GetResourceUri()
        {
            return "precard-rs/rest/groupcard/modPassword";
        }
    }
}