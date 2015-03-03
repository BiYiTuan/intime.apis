
using System.Collections;

namespace com.intime.o2o.data.exchange.Tmall.Product.Mappers
{
    /// <summary>
    /// 商品模型架构转化器
    /// </summary>
    public interface ISchemaMapper
    {
        /// <summary>
        /// 转化商品模型
        /// </summary>
        /// <param name="schemaName">架构模型名称</param>
        /// <param name="context">转化的上下文</param>
        /// <returns>转化后的数据</returns>
        string Map(string schemaName, Hashtable context);

        bool ExistsTemplate(string templateName);
    }
}
