using System.Configuration;

namespace com.intime.fashion.common.IT
{
    public class Config
    {
        public static string IT_Service_From = ConfigurationManager.AppSettings["IT_Service_From"];
        public static string IT_Service_SecretKey = ConfigurationManager.AppSettings["IT_Service_SecretKey"];
        public static string IT_Service_Host = ConfigurationManager.AppSettings["IT_Service_Host"];
        public static string IT_GiftCardStore_Id = ConfigurationManager.AppSettings["IT_GiftCardStore_Id"];
    }
}
