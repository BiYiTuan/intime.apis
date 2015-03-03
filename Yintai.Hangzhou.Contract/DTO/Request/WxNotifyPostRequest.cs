using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    [XmlRoot(ElementName = "xml")]
   public class WxNotifyPostRequest
    {

       [XmlElement(ElementName = "AppId")]
       public string AppId { get; set; }
       [XmlElement(ElementName = "OpenId")]
       public string OpenId { get; set; }
       [XmlElement(ElementName = "IsSubscribe")]
       public int IsSubscribe { get; set; }
       [XmlElement(ElementName = "TimeStamp")]
       public string TimeStamp { get; set; }
       [XmlElement(ElementName = "NonceStr")]
       public string NonceStr { get; set; }
       [XmlElement(ElementName = "AppSignature")]
       public string AppSignature { get; set; }
       [XmlElement(ElementName = "SignMethod")]
       public string SignMethod { get; set; }
    }
}
