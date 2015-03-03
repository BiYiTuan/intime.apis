using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.intime.fashion.common.Tencent
{
    [XmlRoot("root")]
    public class BatchTransferResponse : BaseBatchResponse
    {
        [XmlElement("charset")]
        public string CharSet { get; set; }

    }
}
