
namespace com.intime.fashion.data.sync.Wgw.Request.Order
{
    public class OrderStatusConst
    {
        /******************** 微购订单状态(代码引用) ******************************/
        //微购在线支付订单状态
        public static string STATE_WG_WAIT_PAY = "60"; //微购订单等待买家付款 
        public static string STATE_WG_PAY_OK = "61"; //微购订单买家已付款，等待卖家发货 
        public static string STATE_WG_SHIPPING_OK = "62";//微购订单卖家已发货，等待买家确认收货 
        public static string STATE_WG_END = "63"; //微购订单交易成功 
        public static string STATE_WG_CANCLE = "64"; //微购订单交易关闭 
        //微购COD订单状态 
        public static string STATE_WG_COD_WAIT_SHIP = "70"; //微购COD订单等待卖家发货 
        public static string STATE_WG_COD_SHIPPING_OK = "71"; //微购COD订单卖家已发货,等待确认收货 
        public static string STATE_WG_COD_END = "72"; //微购COD订单交易成功 
        public static string STATE_WG_COD_CANCLE = "73"; //微购COD订单交易关闭(订单关闭 or 发货后拒签)
        //订单复合状态常量定义 
        public static string STATE_WG_COMPLEX_WAIT_PAY = "801"; //待付款(60/) 
        public static string STATE_WG_COMPLEX_WAIT_SHIP = "802"; //待发货 (61/70) 
        public static string STATE_WG_COMPLEX_WAIT_RECV = "803"; //待收货(62/71) 
        public static string STATE_WG_COMPLEX_WAIT_END = "804"; //已结束(63/64/72/73)
    }
}
