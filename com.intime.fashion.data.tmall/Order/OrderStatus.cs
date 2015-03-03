
namespace com.intime.fashion.data.tmall.Order
{
    public class OrderStatus
    {
        /// <summary>
        /// 没有创建支付宝交易
        /// </summary>
        public const string TRADE_NO_CREATE_PAY = "TRADE_NO_CREATE_PAY";

        /// <summary>
        /// 等待买家付款
        /// </summary>
        public const string WAIT_BUYER_PAY = "WAIT_BUYER_PAY";

        /// <summary>
        /// 卖家部分发货
        /// </summary>
        public const string SELLER_CONSIGNED_PART = "SELLER_CONSIGNED_PART";

        /// <summary>
        /// 等待卖家发货
        /// </summary>
        public const string WAIT_SELLER_SEND_GOODS = "WAIT_SELLER_SEND_GOODS";

        /// <summary>
        /// 等待买家确认收货
        /// </summary>
        public const string WAIT_BUYER_CONFIRM_GOODS = "WAIT_BUYER_CONFIRM_GOODS";

        /// <summary>
        /// 买家已签收,货到付款专用
        /// </summary>
        public const string TRADE_BUYER_SIGNED = "TRADE_BUYER_SIGNED";

        /// <summary>
        /// 交易成功
        /// </summary>
        public const string TRADE_FINISHED = "TRADE_FINISHED";

        /// <summary>
        /// 付款以后用户退款成功，交易自动关闭
        /// </summary>
        public const string TRADE_CLOSED = "TRADE_CLOSED";

        /// <summary>
        /// 付款以前，卖家或买家主动关闭交易
        /// </summary>
        public const string TRADE_CLOSED_BY_TAOBAO = "TRADE_CLOSED_BY_TAOBAO";

        /// <summary>
        /// 国际信用卡支付付款确认中
        /// </summary>
        public const string PAY_PENDING = "PAY_PENDING";

        /// <summary>
        /// 0元购合约中
        /// </summary>
        public const string AIT_PRE_AUTH_CONFIRM = "AIT_PRE_AUTH_CONFIRM";
    }
}
