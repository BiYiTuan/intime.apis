using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Yintai.Hangzhou.Contract.Response;
using com.intime.fashion.common.Extension;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class IMSIncomeDetailResponse : BaseResponse
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name="order_type")]
        public int SourceType { get; set; }
        [DataMember(Name="order_no")]
        public string SourceNo { get; set; }
        [DataMember(Name="income_amount")]
        public decimal AssociateIncome { get; set; }
        [DataMember(Name="status")]
        public int Status { get; set; }
        [DataMember(Name="amount")]
        public decimal TotalAmount { get; set; }
        [DataMember(Name = "create_date")]
        public DateTime CreateDate { get; set; }
        [DataMember(Name="status_show")]
        public string Status_Display { get {
            return ((AssociateIncomeStatus)Status).FriendString();
        } }
    }
}
