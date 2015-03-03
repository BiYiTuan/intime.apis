using com.intime.o2o.data.exchange.IT.Response;

namespace com.intime.o2o.data.exchange.IT.Request.Entity
{
    public class RechargeRequest : Request<RechargeEntity, RechargeResponse>
    {
        public override string GetResourceUri()
        {
            return "precard-rs/rest/groupcard/recharge";
        }
    }
}
