using com.intime.fashion.common.Weigou;
using com.intime.fashion.common.Wxpay;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common
{
    public static class WgServiceHelper
    {
        private static AccessToken WxToken;

        public static AccessToken Token
        {
            get {
                if (WxToken == null ||
                    WxToken.IsExpired)
                {
                    WxToken = new AccessToken(AccessTokenType.XihuanYintai).Token();
                };
                return WxToken;
            }
        }

        public static AccessToken TokenMini
        {
            get
            {

               return new AccessToken(AccessTokenType.MiniYin).Token();
            }
        }
       
    }
}
