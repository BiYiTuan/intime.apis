using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;

namespace com.intime.o2o.data.exchange.Ims.Domain
{
    [DataContract]
    public class LogisticStatus
    {
        [DataMember(Name = "orderno")]
        public string OrderNo { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "isSplited")]
        public bool IsOrderSplited { get; set; }

        [DataMember(Name = "itemStatus")]
        public IEnumerable<Logistic> Logistics { get; set; }
    }

    [DataContract]
    public class Logistic
    {
        [DataMember(Name = "productId")]
        public int ProductId { get; set; }

        [DataMember(Name = "stockId")]
        public int StockId { get; set; }

        [DataMember(Name = "expressId")]
        public int CompanyId { get; set; }

        [DataMember(Name = "express")]
        public string CompanyName { get; set; }

        [DataMember(Name = "shippno")]
        public string LogisticCode { get; set; }

        [DataMember(Name = "store")]
        public string Store { get; set; }

        [DataMember(Name = "storeId")]
        public int StoreId { get; set; }
    }
}
