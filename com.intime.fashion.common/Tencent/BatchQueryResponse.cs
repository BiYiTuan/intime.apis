using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.intime.fashion.common.Tencent
{
    [XmlRoot("root")]
    public class BatchQueryResponse:BaseBatchResponse
    {
        [XmlElement("result")]
        public BatchQueryResult Result { get; set; }
    }

    public class BatchQueryResult
    {
        [XmlElement("trade_state")]
        public int TradeState { get; set; }
        [XmlElement("total_count")]
        public int TotalCount { get; set; }
        [XmlElement("total_fee")]
        public int TotalFee { get; set; }
        [XmlElement("succ_count")]
        public int SuccessCount { get; set; }
        [XmlElement("succ_fee")]
        public int SuccessFee { get; set; }
        [XmlElement("fail_count")]
        public int FailCount { get; set; }
        [XmlElement("fail_fee")]
        public int FailFee { get; set; }
        [XmlElement("success_set")]
        public TransferSuccessResult SuccessResult { get; set; }
        [XmlElement("fail_set")]
        public TransferFailResult FailResult { get; set; }

    }

    public class TransferSuccessResult
    {
        [XmlElement("suc_total")]
        public int SuccessTotal { get; set; }
        [XmlElement("suc_rec")]
        public BatchTransferSuccessItem[] Records { get; set; }
    }
    public class TransferFailResult
    {
        [XmlAttribute("fail_total")]
        public int FailTotal { get; set; }
        [XmlElement("fail_rec")]
        public BatchTansferFailItem[] Records { get; set; }
    }

    public class BatchTransferSuccessItem : BatchTransferItem
    {
        [XmlElement("modify_time")]
        public string ModifyTime { get; set; }
    }
    public class BatchTansferFailItem : BatchTransferItem
    {
        [XmlElement("err_code")]
        public string ErrorCode { get; set; }
        [XmlElement("err_msg")]
        public string ErrorMsg { get; set; }
        [XmlElement("modify_time")]
        public string ModifyTime { get; set; }
    }
}
