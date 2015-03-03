using com.intime.o2o.data.exchange.IT;
using System;
using System.Runtime.Serialization;

namespace com.intime.o2o.data.exchange.Ims
{
    public abstract class ImsRequest<TRequest,TResponse> :Request<TRequest,TResponse>
    {
        [DataMember(Name = "ts")]
        public override string Timestamp { get; set; }
    }
}
