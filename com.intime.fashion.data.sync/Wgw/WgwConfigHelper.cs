using System;
using System.Configuration;

namespace com.intime.fashion.data.sync.Wgw
{
    public class WgwConfigHelper
    {
        ///<summary>
        ///上架时间延时，默认是730天后，两年
        ///</summary>
        public static readonly int UpDelay = Int32.Parse(String.IsNullOrEmpty(ConfigurationManager.AppSettings["Up_Time_Delay"]) ? "730" : ConfigurationManager.AppSettings["Up_Time_Delay"]);

        /// <summary>
        /// 购买限制，默认50
        /// </summary>
        public static readonly string ArrivingDays = String.IsNullOrEmpty(ConfigurationManager.AppSettings["Arriving_Days"]) ? "4" : ConfigurationManager.AppSettings["Arriving_Days"];


        /// <summary>
        /// 购买限制，默认50
        /// </summary>
        public static readonly string BuyLimit = String.IsNullOrEmpty(ConfigurationManager.AppSettings["Buy_Limit"]) ? "50" : ConfigurationManager.AppSettings["Buy_Limit"];

        /// <summary>
        ///商品所属省份，拍拍对应浙江省为16
        /// </summary>
        public static readonly string Province =
            String.IsNullOrEmpty(ConfigurationManager.AppSettings["Belong_To_Province"]) ? "16" : ConfigurationManager.AppSettings["Belong_To_Province"];

        /// <summary>
        /// 商品所属城市，杭州在拍拍上对应1600
        /// </summary>
        public static readonly string City =
            String.IsNullOrEmpty(ConfigurationManager.AppSettings["Belong_To_City"]) ? "1600" : ConfigurationManager.AppSettings["Belong_To_City"];

        /// <summary>
        /// 商品资源图片URL基地址
        /// </summary>
        public static readonly string Image_BaseUrl =
            String.IsNullOrEmpty(ConfigurationManager.AppSettings["Product_Image_BaseUrl"]) ? "http://irss.ytrss.com" : ConfigurationManager.AppSettings["Product_Image_BaseUrl"];

        /// <summary>
        /// 微信支付编码
        /// </summary>
        public static readonly string WGW_WX_PAYMENTCODE = String.IsNullOrEmpty(ConfigurationManager.AppSettings["Wgw_Wx_PaymentCode"]) ? "27" : ConfigurationManager.AppSettings["Wgw_Wx_PaymentCode"];

        /// <summary>
        /// 微信支付
        /// </summary>
        public static readonly string WGW_WX_PAYMENTNAME = String.IsNullOrEmpty(ConfigurationManager.AppSettings["Wgw_Wx_PaymentName"]) ? "微信支付":ConfigurationManager.AppSettings["Wgw_Wx_PaymentName"];
        
        /// <summary>
        /// 财付通支付编码
        /// </summary>
        public static readonly string WGW_CFT_PAYMENTCODE = String.IsNullOrEmpty(ConfigurationManager.AppSettings["Wgw_Cft_PaymentCode"]) ? "26" : ConfigurationManager.AppSettings["Wgw_Cft_PaymentCode"];
        
        /// <summary>
        /// 财付通支付
        /// </summary>
        public static readonly string WGW_CFT_PAYMENTNAME =String.IsNullOrEmpty(ConfigurationManager.AppSettings["Wgw_Cft_PaymentName"])?"财付通支付":ConfigurationManager.AppSettings["Wgw_Cft_PaymentName"];

        public static readonly string WGW_API_SECRET_OAUTH_KEY = ConfigurationManager.AppSettings["Wgw_Api_Secret_OAuth_Key"];

        public static readonly string WGW_API_ACCESS_TOKEN = ConfigurationManager.AppSettings["Wgw_Api_Access_Token"];

        public static readonly string WGW_API_BASE_URL = ConfigurationManager.AppSettings["Wgw_Api_Base_Url"];

        public static readonly string WGW_API_UIN = ConfigurationManager.AppSettings["Wgw_Api_Uin"];

        public static readonly string WGW_API_APPOAUTH_ID = ConfigurationManager.AppSettings["Wgw_Api_OAuth_ID"];

        public static readonly string WGW_API_SELLER_UIN = ConfigurationManager.AppSettings["Wgw_Api_Seller_Uin"];

        public static readonly string WGW_API_SUB_UIN = ConfigurationManager.AppSettings["Wgw_Api_Sub_Uin"];

        public static readonly string WGW_API_FORMAT = string.IsNullOrEmpty(ConfigurationManager.AppSettings["Wgw_Api_Format"]) ? "json" : ConfigurationManager.AppSettings["Wgw_Api_Format"];
    }
}
