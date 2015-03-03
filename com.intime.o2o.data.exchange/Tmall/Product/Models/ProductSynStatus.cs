
namespace com.intime.o2o.data.exchange.Tmall.Product.Models
{
    /// <summary>
    /// 商品池商品状态
    /// </summary>
    public enum ProductPoolStatus
    {
        PendingProduct = 100,
        AddProudctFinished = 200,
        ElasticSearchNotFound = 404,
        AddProudctError = 500,
        AddItemError = 700,
        AddItemFinished = 800,
        DownLoadPictureError = 600
    }
}
