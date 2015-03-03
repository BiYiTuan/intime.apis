using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
    public class Alipay_IMSConfiguration:CommonConfigurationBase
    {
        protected override string SectionName
        {
            get { return "alipay_ims"; }
        }
        public string Md5Key { get { return GetItem("md5_key"); } }
        public string Partner { get { return GetItem("partner"); } }
        public string PrivateKey { get { return GetItem("private_key"); } }
        public string PublicKey { get { return GetItem("public_key"); } }
        public string PaymentCode { get { return GetItem("payment_code"); } }
        public string PaymentCodeGiftCard { get { return GetItem("payment_code_giftcard"); } }

    }
}
