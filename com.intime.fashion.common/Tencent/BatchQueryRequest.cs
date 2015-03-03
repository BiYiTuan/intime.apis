using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.intime.fashion.common.Tencent
{
    [XmlRoot("root")]
    public class BatchQueryRequest:BaseBatchRequest
    {
        [XmlElement("op_code", Order = 2)]
        public string OperateCode { get {
            return "1014";
        }
            set { }
        }
        [XmlElement("op_name", Order = 3)]
        public string OperateName { get {
            return "batch_draw_query";
        }
            set { }
        }
        [XmlElement("service_version", Order = 8)]
        public string ServiceVersion { get; set; }
        [XmlElement("op_user", Order = 6)]
        public string OperateUser { get; set; }
        [XmlElement("op_passwd", Order = 4)]
        public string OperatePwdMd5 { get; set; }
        [XmlElement("op_time", Order = 5)]
        public string OperateTime { get; set; }
        [XmlElement("sp_id", Order = 9)]
        public string SPId { get; set; }
        [XmlElement("package_id", Order = 7)]
        public string PackageId { get; set; }
        [XmlElement("client_ip",Order=1)]
        public string ClientIp { get; set; }
        [XmlIgnore]
        public string OperatePwd { get; set; }
    }
}
