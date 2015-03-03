using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Response.Point
{
    [DataContract(Name = "point")]
    public class PointInfoResponse : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "user_id")]
        public int User_Id { get; set; }
        [IgnoreDataMember]
        public decimal Amount { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "amount")]
        public string AmountStr
        {
            get { return Amount.ToString("F2"); }
            set { }
        }

        [IgnoreDataMember]
        public int Type { get; set; }

        [IgnoreDataMember]
        public PointType PointType
        {
            get { return (PointType)Type; }
            set { Type = (int)value; }
        }

        [DataMember(Name = "description")]
        public string Description { get; set; }
        [IgnoreDataMember]
        public int Status { get; set; }

        [DataMember(Name = "createddate")]
        public string CreatedDateStr
        {
            get { return this.CreatedDate.ToString(Define.DateDefaultFormat); }
            set { }
        }

        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }

        [IgnoreDataMember]
        public int CreatedUser { get; set; }

        [IgnoreDataMember]
        public int PointSourceId { get; set; }

        [IgnoreDataMember]
        public PointSourceType PSourceType
        {
            get { return (PointSourceType)PointSourceType; }
            set { PointSourceType = (int)value; }
        }

        [IgnoreDataMember]
        public int PointSourceType { get; set; }

        [IgnoreDataMember]
        public int UpdatedUser { get; set; }

        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }
    }

    [DataContract]
    public class PointCollectionResponse : PagerInfoResponse
    {
        public PointCollectionResponse(PagerRequest request)
            : base(request)
        {
        }

        public PointCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "points")]
        public List<PointInfoResponse> PointInfoResponses { get; set; }
    }
}
