using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.Store
{
    [DataContract]
    public class StoreInfoResponse : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "tel")]
        public string Tel { get; set; }

        [IgnoreDataMember]
        public int CreatedUser { get; set; }

        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }

        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }

        [IgnoreDataMember]
        public int UpdatedUser { get; set; }

        [DataMember(Name = "lng")]
        public decimal Longitude { get; set; }

        [DataMember(Name = "lat")]
        public decimal Latitude { get; set; }

        [IgnoreDataMember]
        public int Group_Id { get; set; }

        [IgnoreDataMember]
        public int Status { get; set; }

        [IgnoreDataMember]
        public int Region_Id { get; set; }

        [IgnoreDataMember]
        public int StoreLevel { get; set; }

        /// <summary>
        /// 距离 单位 米
        /// </summary>
        [DataMember(Name = "distance")]
        public decimal Distance { get; set; }

        [DataMember(Name = "resources")]
        public List<ResourceInfoResponse> ResourceInfoResponses { get; set; }

        [IgnoreDataMember]
        public Nullable<decimal> GpsLat { get; set; }

        [IgnoreDataMember]
        public Nullable<decimal> GpsLng { get; set; }

        [IgnoreDataMember]
        public Nullable<decimal> GpsAlt { get; set; }
    }
}
