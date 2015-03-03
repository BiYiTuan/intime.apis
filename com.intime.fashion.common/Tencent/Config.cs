using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Tencent
{
    public static class Config
    {
        public static readonly string OP_USERID = ConfigurationManager.AppSettings["TECENT_OP_USERID"];
        public static readonly string OP_USERPWD = ConfigurationManager.AppSettings["TECENT_OP_USERPWD"];
        public static readonly string PARTER_ID = ConfigurationManager.AppSettings["TECENT_PARTER_ID"];
        public static readonly string PARTER_KEY = ConfigurationManager.AppSettings["TECENT_PARTER_KEY"];
        public static readonly string CA_FILE_PATH = ConfigurationManager.AppSettings["TECENT_CA_FILE"];
        public static readonly string CERT_FILE_PATH = ConfigurationManager.AppSettings["TECENT_CERT_FILE"];
        public static readonly string CERT_FILE_PWD = ConfigurationManager.AppSettings["TECENT_CERT_PWD"];
        public static readonly string STATIC_PUBLIC_IP = ConfigurationManager.AppSettings["STATIC_PUBLIC_IP"];
        public static readonly string SERVICE_URI_BATCH_TRANSFER=ConfigurationManager.AppSettings["SERVICE_URI_BATCH"];
        public static readonly Encoding DEFAULT_ENCODE = Encoding.GetEncoding("GBK");
    }
}
