using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace com.intime.fashion.common.Tencent
{
    [XmlRoot("root")]
    public class BatchTransferRequest:BaseBatchRequest
    {
        [XmlElement("op_code", Order = 2)]
        public string OperateCode { get {
            return "1013";
        }
            set { }
        }
        [XmlElement("op_name", Order = 3)]
        public string OperateName { get {
            return "batch_draw";
        }
            set { }
        }
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
        [XmlElement("total_num", Order = 11)]
        public int TotalNum { get; set; }
        [XmlElement("total_amt", Order = 10)]
        public int TotalAmountOfFen { get; set; }
        [XmlElement("client_ip",Order=1)]
        public string ClientIp { get; set; }
        [XmlArray("record_set", Order = 8)]
        [XmlArrayItem("record")]
        public BatchTransferItem[] Records { get; set; }
        [XmlIgnore]
        public string OperatePwd { get; set; }
       
    }
    public class BatchTransferItem
    {
        [XmlElement("serial")]
        public string Serial { get; set; }
        [XmlElement("rec_bankacc")]
        public string BankNo { get; set; }
        [XmlElement("bank_type")]
        public string BankCode { get; set; }
        [XmlElement("rec_name")]
        public string AccountName { get; set; }
        [XmlElement("pay_amt")]
        public int AmountOfFen { get; set; }
        [XmlElement("acc_type")]
        public int AccountType { get; set; }
        [XmlElement("desc")]
        public string Desc { get; set; }
        [XmlElement("recv_mobile")]
        public string NotifyMobile { get; set; }
    }
}
