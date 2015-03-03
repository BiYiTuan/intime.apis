using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class ExchangeStoreCouponResponse:BaseResponse
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [IgnoreDataMember]
        public System.DateTime ValidStartDate { get; set; }
        [IgnoreDataMember]
        public System.DateTime ValidEndDate { get; set; }
        [DataMember(Name = "storename")]
        public string StoreName { get; set; }
        [DataMember(Name = "exclude")]
        public string Exclude { get; set; }
        [DataMember(Name = "points")]
        public int Points { get; set; }
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }
        [IgnoreDataMember]
        public DateTime CreateDate { get; set; }

        [DataMember(Name = "validstartdate")]
        public string ValidStartDate_S
        {
            get
            {
                return ValidStartDate.ToClientTimeFormat();
            }
            set { }
        }
        [DataMember(Name = "validenddate")]
        public string ValidEndDate_S
        {
            get
            {
                return ValidEndDate.ToClientTimeFormat();
            }
            set { }
        }
        [DataMember(Name = "createddate")]
        public string CreateDate_S
        {
            get
            {
                return CreateDate.ToClientTimeFormat();
            }
            set { }
        }

    }
}
