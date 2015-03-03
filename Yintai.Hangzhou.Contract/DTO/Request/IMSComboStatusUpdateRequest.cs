using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    [DataContract]
    public class IMSComboStatusUpdateRequest:BaseRequest
    {
        public int Item_Type { get; set; }
        public int Item_Id { get; set; }
        public bool Is_Online { get; set; }
    }
}
