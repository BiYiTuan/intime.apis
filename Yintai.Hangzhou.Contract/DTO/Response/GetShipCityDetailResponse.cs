using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class GetShipCityDetailResponse:BaseResponse
    {
        [DataMember(Name="provinceid")]
        public int Id { get; set; }
        [DataMember(Name="provincename")]
        public string ProvinceName { get; set; }
        [DataMember(Name="items")]
        public IEnumerable<ShipCityModel> Cities {get;set;}
    }
    [DataContract]
    public class ShipCityModel
    {
        [DataMember(Name="cityid")]
        public int Id { get; set; }
        [DataMember(Name="cityname")]
        public string CityName { get; set; }
        [DataMember(Name="items")]
        public IEnumerable<ShipDistrictModel> Districts { get; set; }
    }
    [DataContract]
    public class ShipDistrictModel
    {
        [DataMember(Name = "districtid")]
        public int Id { get; set; }
        [DataMember(Name = "districtname")]
        public string DistrictName { get; set; }
        [DataMember(Name="zipcode")]
        public string ZipCode { get; set; }
    }
}
