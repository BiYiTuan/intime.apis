using System.Runtime.Serialization;

namespace com.intime.o2o.data.exchange.IT.Request.Entity
{
    [DataContract]
    public class RechargeEntity
    {
        [DataMember(Name="phone")]
        public string Phone { get; set; }

        [DataMember(Name ="idcard")]
        public string IdCard { get; set; }

        [DataMember(Name = "amount")]        
        public int Amount { get; set; }

        [DataMember(Name = "totalpay")]
        public int TotalPay { get; set; }

        [DataMember(Name = "discount")]
        public int Discount { get; set; }

        [DataMember(Name = "transcode")]
        public string TransCode { get; set; }


        [DataMember(Name = "storeid")]
        public string StoreCode { get; set; }

        [DataMember(Name="password")]
        public string Password { get; set; }
    }
}