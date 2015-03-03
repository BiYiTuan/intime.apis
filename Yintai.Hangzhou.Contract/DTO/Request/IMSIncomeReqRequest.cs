using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    [DataContract]
    public class IMSIncomeReqRequest:BaseRequest
    {
        public string Bank { get; set; }
        public string Bank_Code { get; set; }
        public string Bank_No { get; set; }
        public string User_Name { get; set; }
        public decimal Amount { get; set; }
        public string Id_Card { get; set; }
    }
}
