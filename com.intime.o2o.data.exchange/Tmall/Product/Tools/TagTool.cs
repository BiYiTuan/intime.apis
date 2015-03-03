using System.Linq;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.o2o.data.exchange.Tmall.Product.Tools
{
    /// <summary>
    /// IMS标签服务
    /// </summary>
    public class TagTool
    {
        /// <summary>
        /// 根据商品Id获取商品的IMSTag标签
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns>ImsTag</returns>
        public string GetImsTag(int productId)
        {
            using (var db = new YintaiHangzhouContext())
            {
                var query = from product2Imstag in db.Product2IMSTag
                            from imstag in db.IMS_Tag
                            where product2Imstag.IMSTagId == imstag.Id
                            && product2Imstag.ProductId == productId
                            && imstag.Only4Tmall == true
                            && imstag.Status == 1
                            select imstag.Name;
                return query.FirstOrDefault();
            }
        }
    }
}
