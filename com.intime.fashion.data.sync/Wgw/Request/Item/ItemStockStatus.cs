
namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public enum StockStatus
    {
        /// <summary>
        /// 仓库中
        /// </summary>
        IS_IN_STORE = 1,//仓库中

        /// <summary>
        /// 上架销售中
        /// </summary>
        IS_FOR_SALE = 2,//上架销售中

        /// <summary>
        /// 预删除
        /// </summary>
        IS_PRE_DELETE = 9,//预删除
        /// <summary>
        /// 自定义上架时间
        /// </summary>
        IS_SALE_ON_TIME = 64, //自定义时间上架

        /// <summary>
        /// 售罄
        /// </summary>
        IS_SOLD_OUT = 6,//售罄
    }
}
