using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Hangzhou.WebApiCore
{
    public class XmlResult : ActionResult
    {
        private object _objectToSerialize;
        private XmlAttributeOverrides _xmlAttribueOverrides;

        /// <summary>
        /// Creates a new instance of the XmlResult class.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize to XML.</param>
        public XmlResult(object objectToSerialize)
        {
            _objectToSerialize = objectToSerialize;
        }

        /// <summary>
        /// Creates a new instance of the XMLResult class.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize to XML.</param>
        /// <param name="xmlAttributeOverrides"></param>
        public XmlResult(object objectToSerialize, XmlAttributeOverrides xmlAttributeOverrides)
        {
            _objectToSerialize = objectToSerialize;
            _xmlAttribueOverrides = xmlAttributeOverrides;
        }

        /// <summary>
        /// The object to be serialized to XML.
        /// </summary>
        public object ObjectToSerialize
        {
            get { return _objectToSerialize; }
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (_objectToSerialize != null)
            {

                var xs = (_xmlAttribueOverrides == null) ?
                    new XmlSerializer(_objectToSerialize.GetType()) :
                    new XmlSerializer(_objectToSerialize.GetType(), _xmlAttribueOverrides);
              
                StringBuilder builder = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = Encoding.UTF8;
                     
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                context.HttpContext.Response.ContentType = "text/xml";

                var sb = new StringBuilder();
                using (var stringWriter = XmlWriter.Create(sb, settings))
                {
                    xs.Serialize(stringWriter, _objectToSerialize, ns);
                    Log.Debug(sb.ToString());
                }
              
                using (var stringWriter = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
                {
                    xs.Serialize(stringWriter, _objectToSerialize,ns);
                }
              
               
            }
        }
        private ILog Log
        {
            get {
                return ServiceLocator.Current.Resolve<ILog>();
            }
        }
    }
}
