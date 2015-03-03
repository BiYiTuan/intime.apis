using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace com.intime.fashion.common.Tencent
{
    public class BaseBatchRequest
    {
        [XmlIgnore]
        public string EncodedFormat
        {
            get
            {
                var xs = new XmlSerializer(this.GetType());
                StringBuilder builder = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = Encoding.UTF8;
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                using (var stringWriter = XmlWriter.Create(builder, settings))
                {
                    xs.Serialize(stringWriter, this, ns);
                }
                var baseEncodedXml = Convert.ToBase64String(Config.DEFAULT_ENCODE.GetBytes(builder.ToString()));
                var md5Xml = CommonUtil.MD5_Encode(baseEncodedXml, Config.DEFAULT_ENCODE).ToLower();
                var signedXml = CommonUtil.MD5_Encode(string.Concat(md5Xml, Config.PARTER_KEY), Config.DEFAULT_ENCODE).ToLower();
                return string.Format("content={0}&abstract={1}",HttpUtility.UrlEncode(baseEncodedXml,Config.DEFAULT_ENCODE), HttpUtility.UrlEncode(signedXml,Config.DEFAULT_ENCODE));
            }
        }
        [XmlIgnore]
        public int? GroupId { get; set; }
    }
}
