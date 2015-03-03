using com.intime.fashion.common.Wxpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    [XmlRoot(ElementName="xml")]
    public class WxPackageGetRequest
    {
        [XmlElement(ElementName="AppId")]
        public string AppId { get; set; }
        [XmlElement(ElementName = "OpenId")]
        public string OpenId { get; set; }
        [XmlElement(ElementName = "IsSubscribe")]
        public int IsSubscribe { get; set; }
        [XmlElement(ElementName = "ProductId")]
        public string ProductId { get; set; }
        [XmlElement(ElementName = "TimeStamp")]
        public long TimeStamp { get; set; }
        [XmlElement(ElementName = "NonceStr")]
        public string NonceStr { get; set; }
        [XmlElement(ElementName = "AppSignature")]
        public string AppSignature { get; set; }
        [XmlElement(ElementName = "SignMethod")]
        public string SignMethod { get; set; }

        public string ValidSign { get {
            var preSigned = new Dictionary<string, dynamic>();
            preSigned.Add("appid", AppId);
            preSigned.Add("appkey", WxPayConfig.PARTER_SIGN_KEY);
            preSigned.Add("productid", ProductId);
            preSigned.Add("timestamp", TimeStamp);
            preSigned.Add("noncestr", NonceStr);
            preSigned.Add("openid", OpenId);
            preSigned.Add("issubscribe", IsSubscribe);
          return  Util.PaySign(preSigned);
           
        } }
    }
}
