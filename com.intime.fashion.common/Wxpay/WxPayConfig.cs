using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Wxpay
{
    public static class WxPayConfig
    {
        public static readonly string APP_ID = ConfigurationManager.AppSettings["WX_APP_ID"];
        public static readonly string APP_SECRET = ConfigurationManager.AppSettings["WX_APP_SECRET"];
        public static readonly string PARTER_SIGN_KEY = ConfigurationManager.AppSettings["WX_SIGN_KEY"];
        public static readonly string PARTER_ID = ConfigurationManager.AppSettings["WX_PARTER_ID"];
        public static readonly string PARTER_KEY = ConfigurationManager.AppSettings["WX_PARTER_KEY"];

        public static readonly string NOTIFY_URL = ConfigurationManager.AppSettings["WX_NOTIFY_URL"];
        public static readonly string NOTIFY_ERP_URL = ConfigurationManager.AppSettings["WX_NOTIFY_ERP_URL"];
        public static readonly string NOTIFY_ERP2_URL = ConfigurationManager.AppSettings["WX_NOTIFY_ERP2_URL"];

        public static readonly string APP_APPID = ConfigurationManager.AppSettings["WX_APP_APPID"];
        public static readonly string APP_PARTER_SIGN_KEY = ConfigurationManager.AppSettings["WX_SIGN_KEY_4APP"];
        public static readonly string APP_PARTER_ID = ConfigurationManager.AppSettings["WX_PARTER_ID_4APP"];
        public static readonly string APP_PARTER_KEY = ConfigurationManager.AppSettings["WX_PARTER_KEY_4APP"];
        public static readonly string PAY4APP_TOKEN_URL = ConfigurationManager.AppSettings["WX_PAY4APP_TOKEN_URL"];
        public static readonly string PAY4APP_RETURN_URL = ConfigurationManager.AppSettings["WX_PAY4APP_RETURN_URL"];
        public static readonly string PAY4APP_NOTIFY_URL = ConfigurationManager.AppSettings["WX_PAY4APP_NOTIFY_URL"];

        public static readonly string HTML_PARTER_SIGN_KEY = ConfigurationManager.AppSettings["WX_SIGN_KEY_4HTML"];
        public static readonly string HTML_PARTER_ID = ConfigurationManager.AppSettings["WX_PARTER_ID_4HTML"];
        public static readonly string HTML_PARTER_KEY = ConfigurationManager.AppSettings["WX_PARTER_KEY_4HTML"];
        public static readonly string PAY4HTML_TOKEN_URL = ConfigurationManager.AppSettings["WX_PAY4HTML_TOKEN_URL"];
        public static readonly string PAY4HTML_NOTIFY_URL = ConfigurationManager.AppSettings["WX_PAY4HTML_NOTIFY_URL"];

        public static readonly string PAYMENT_CODE4APP = ConfigurationManager.AppSettings["WX_PAYMENT_CODE4APP"];
        public static readonly string PAYMENT_CODE4HTML = ConfigurationManager.AppSettings["WX_PAYMENT_CODE4HTML"];

        public static readonly string PAYMENT_CODE4IMS = ConfigurationManager.AppSettings["WX_PAYMENT_CODE4IMS"];
        public static readonly string PAYMENT_CODE4GIFTCARD = ConfigurationManager.AppSettings["WX_PAYMENT_CODE4GIFTCARD"];
        public static readonly string IMS_PARTER_KEY = ConfigurationManager.AppSettings["WX_PARTER_KEY_4IMS"];

        public static string PaymentCode
        {
            get
            {
                return ConfigurationManager.AppSettings["WX_paymentcode"];
            }
        }
        public static readonly string PaymentName = "微信支付";
        public static readonly string ORDER_SOURCE = "BarSale";
        public static string WEB_SERVICE_BASE = ConfigurationManager.AppSettings["wx_WEBSERVICE_BASE"];
        public static readonly string MESSAGE_TEMPLATE_ID = ConfigurationManager.AppSettings["WX_MESSAGE_TEMPLATE_ID"];
        public static readonly string MESSAGE_TEMPLATE_ID_MINI = ConfigurationManager.AppSettings["WX_MESSAGE_TEMPLATE_ID_MINI"];

    }

    public enum WxPayRetCode
    { 
        Success = 0,
        RequestError = 1
    }
    public enum WxPackageType
    { 
        Order = 1,
        Sku = 2,
        Product = 3,
        ErpOrder = 4,
        Erp2Order = 5
    }
    public static class WxPaySignMethod
    {
        public static readonly string SHA1 = "sha1";
    }
}
