using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Yintai.Hangzhou.Tools.ApiTest.Models
{

    [XmlRoot(ElementName = "api")]
    public class ApiViewModel
    {
        [XmlArray(ElementName = "resources")]
        public List<Method> Methods { get; set; }
    }

    [XmlType(TypeName = "resource")]
    public class Method
    {
        /// <summary>
        /// controller.acton
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "param")]
        public List<Param> Params { get; set; }

        [XmlElement(ElementName = "defaultValue")]
        public string DefaultValue { get; set; }
    }

    [XmlType(TypeName = "param")]
    public class Param
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}