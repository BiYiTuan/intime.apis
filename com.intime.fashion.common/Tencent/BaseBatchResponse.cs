using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.intime.fashion.common.Tencent
{
    public class BaseBatchResponse
    {
        [XmlElement("op_code")]
        public string OperateCode { get; set; }
        [XmlElement("op_name")]
        public string OperateName { get; set; }
        [XmlElement("op_user")]
        public string OperateUser { get; set; }
        [XmlElement("op_time")]
        public string OperateTime { get; set; }
        [XmlElement("package_id")]
        public string PackageId { get; set; }
        [XmlElement("retcode")]
        public string RetCode { get; set; }
        [XmlElement("retmsg")]
        public string RetMsg { get; set; }

        [XmlIgnore]
        public bool IsSuccess
        {
            get
            {
                return RetCode == "0" || RetCode == "00";
            }
        }
        [XmlIgnore]
        public bool IsAllFail
        {
            get {
                var failCodes = new string[] { 
                    "03020165","03020081"
                };
                return failCodes.Any<string>(code=>string.Compare(code,RetCode,true)==0);
            }
        }
    }
}
