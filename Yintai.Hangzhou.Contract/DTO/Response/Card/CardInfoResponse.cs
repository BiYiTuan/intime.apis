using System;
using System.Runtime.Serialization;
using Yintai.Architecture.Common;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.Card
{
    [DataContract]
    public class CardInfoResponse : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "cardno")]
        public string CardNo { get; set; }

        [DataMember(Name = "amount")]
        public int Amount
        {
            get { return Point == null ? 0 : (int)Point.Value; }
            set { }
        }

        [IgnoreDataMember]
        public decimal? Point { get; set; }

        [DataMember(Name = "lastdate")]
        public string LastDateStr
        {
            get { return LastDate == null ? String.Empty : LastDate.Value.ToString(Define.DateDefaultFormat); }
            set { }
        }

        [IgnoreDataMember]
        public DateTime? LastDate { get; set; }

        [DataMember(Name = "lvl")]
        public string CardLvl { get; set; }

        [DataMember(Name = "type")]
        public string CardType { get; set; }

        [DataMember(Name = "userid")]
        public int User_Id { get; set; }

        [IgnoreDataMember]
        public int Status { get; set; }

        [IgnoreDataMember]
        public DateTime CreatedDate { get; set; }

        [IgnoreDataMember]
        public int CreatedUser { get; set; }

        [IgnoreDataMember]
        public DateTime UpdatedDate { get; set; }

        [IgnoreDataMember]
        public int UpdatedUser { get; set; }
    }
}
