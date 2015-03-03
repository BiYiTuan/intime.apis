using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Model;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class GetProductInfo4PRequest : BaseRequest
    {
        [Required(ErrorMessage="产品编码不存在")]
        public int ProductId { get; set; }

        [DataMember(Name = "lng")]
        public double? Lng { get; set; }

        [DataMember(Name = "lat")]
        public double? Lat { get; set; }

       

        /// <summary>
        /// 当前请求的用户，可以是匿名的
        /// </summary>
        public UserModel CurrentAuthUser { get; set; }

      
    }
}
