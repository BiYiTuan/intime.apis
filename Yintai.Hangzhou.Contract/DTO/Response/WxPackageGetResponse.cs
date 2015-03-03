using com.intime.fashion.common.Wxpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [XmlRoot(ElementName = "xml")]
    public class WxPackageGetResponse
    {
        [XmlElement(ElementName = "AppId")]
        public XmlCDataSection AppIdStr
        {
            get
            {

                return new XmlDocument().CreateCDataSection(AppId);
            }
            set { }
        }
        [XmlElement(ElementName = "Package")]
        public XmlCDataSection PackageStr
        {
            get
            {
                if (Package == null)
                    return new XmlDocument().CreateCDataSection(string.Empty);
                return new XmlDocument().CreateCDataSection(Package.Encode());
            }
            set { }
        }

        public long TimeStamp { get; set; }
        [XmlElement(ElementName = "NonceStr")]
        public XmlCDataSection NonceStr2
        {
            get
            {

                return new XmlDocument().CreateCDataSection(NonceStr);
            }
            set { }
        }

        public int RetCode { get; set; }
        [XmlElement(ElementName = "RetErrMsg")]
        public XmlCDataSection RetErrMsg2
        {
            get
            {

                return new XmlDocument().CreateCDataSection(RetErrMsg);
            }
            set { }
        }

        [XmlElement(ElementName = "AppSignature")]
        public string AppSignature
        {
            get
            {
                var preSigned = new Dictionary<string, dynamic>();
                preSigned.Add("appid", AppId);
                preSigned.Add("appkey", WxPayConfig.PARTER_SIGN_KEY);
                if (Package != null)
                    preSigned.Add("package", Package == null ? string.Empty : Package.Encode());
                preSigned.Add("timestamp", TimeStamp);
                preSigned.Add("noncestr", NonceStr);
                preSigned.Add("retcode", RetCode);
                if (!string.IsNullOrEmpty(RetErrMsg))
                    preSigned.Add("reterrmsg", RetErrMsg);
                return Util.PaySign(preSigned);

            }
            set { }
        }
        [XmlElement(ElementName = "SignMethod")]
        public XmlCDataSection SignMethodStr
        {
            get
            {

                return new XmlDocument().CreateCDataSection(SignMethod);
            }
            set { }
        }
        [XmlIgnore]
        public string NonceStr { get; set; }
        [XmlIgnore]
        public string AppId { get; set; }

        [XmlIgnore]
        public WxPackage Package { get; set; }
        [XmlIgnore]
        public string RetErrMsg { get; set; }
        [XmlIgnore]
        public string SignMethod { get; set; }
    }
}
