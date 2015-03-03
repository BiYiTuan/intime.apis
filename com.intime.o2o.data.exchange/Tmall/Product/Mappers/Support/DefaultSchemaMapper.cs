using System;
using System.Collections;
using System.IO;

using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

namespace com.intime.o2o.data.exchange.Tmall.Product.Mappers.Support
{
    /// <summary>
    /// 默认Schema转化器
    /// </summary>
    public class DefaultSchemaMapper : ISchemaMapper
    {
        private readonly VelocityEngine _velocity;
        private readonly string _tplPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration\\Tmall\\Templates");
        private const string Ext = ".xml";

        public DefaultSchemaMapper()
        {
            var props = new ExtendedProperties();
            props.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, _tplPath);
            props.SetProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.SetProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            props.SetProperty(RuntimeConstants.COUNTER_INITIAL_VALUE, 0);



            _velocity = new VelocityEngine(props);
        }

        public bool ExistsTemplate(string templateName)
        {
            return File.Exists(Path.Combine(_tplPath, templateName + Ext));
        }

        /// <summary>
        /// 映射模版数据
        /// </summary>
        /// <param name="schemaName">模版名称</param>
        /// <param name="context">模版数据</param>
        /// <returns>合并后的Xml数据</returns>
        public string Map(string schemaName, Hashtable context)
        {
            var template = _velocity.GetTemplate(string.Format("{0}{1}", schemaName, Ext));

            var velocityContext = new VelocityContext(context);
            var writer = new StringWriter();
            template.Merge(velocityContext, writer);

            return writer.GetStringBuilder().ToString();
        }
    }
}
