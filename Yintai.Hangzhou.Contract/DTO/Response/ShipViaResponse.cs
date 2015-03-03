using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
   public class ShipViaResponse
    {
        [DataMember(Name="name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name="canship")]
        public bool IsOnline { get; set; }
    }
}
