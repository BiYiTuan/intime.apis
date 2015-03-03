using com.intime.fashion.common.Wxpay;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;

namespace com.intime.fashion.common
{
    public static class WxServiceHelper
    {
       
        public static bool Notify(dynamic requestData, Action<dynamic> successCallback, Action<dynamic> failCallback)
        {
            var client = new RestClient(WxPayConfig.WEB_SERVICE_BASE);
            var token = WgServiceHelper.Token;
            if (token == null)
            {
                CommonUtil.Log.Info("token is empty");
                return false;
            }
            var notifyRequest = new RestRequest("pay/delivernotify?access_token={access_token}", Method.POST);
            notifyRequest.RequestFormat = DataFormat.Json;
            notifyRequest.AddUrlSegment("access_token", token.access_token);
            notifyRequest.AddBody(requestData);
            var response = client.Execute(notifyRequest).Content;
            var notifyResponse = JsonConvert.DeserializeObject<dynamic>(response);

            if (notifyResponse.errcode == 0)
            {
                if (successCallback != null)
                    successCallback(notifyResponse);
                return true;
            }
            else
            {
                Logger.Debug(response);

                if (failCallback != null)
                    failCallback(notifyResponse);
                return false;
            }


        }

        public static bool Query(dynamic requestData, Action<dynamic> successCallback, Action<dynamic> failCallback)
        {
            var client = new RestClient(WxPayConfig.WEB_SERVICE_BASE);
            var token = WgServiceHelper.Token;
            if (token == null)
            {
                CommonUtil.Log.Info("token is empty");
                return false;
            }
            var notifyRequest = new RestRequest("pay/orderquery?access_token={access_token}", Method.POST);
            notifyRequest.RequestFormat = DataFormat.Json;
            notifyRequest.AddUrlSegment("access_token", token.access_token);
            notifyRequest.AddBody(requestData);
            var notifyResponse =JsonConvert.DeserializeObject<dynamic>(client.Execute(notifyRequest).Content);

            if (notifyResponse.errcode == 0)
            {
                if (successCallback != null)
                    successCallback(notifyResponse);
                return true;
            }
            else
            {
                Logger.Debug(notifyResponse);

                if (failCallback != null)
                    failCallback(notifyResponse);
                return false;
            }


        }

        public static bool SendMessage(dynamic requestData, Action<dynamic> successCallback, Action<dynamic> failCallback)
        {
            return SendMessage(requestData, AccessTokenType.XihuanYintai, successCallback, failCallback);
        }
        public static bool SendMessage(dynamic requestData,AccessTokenType tokenType, Action<dynamic> successCallback, Action<dynamic> failCallback)
        {
            var client = new RestClient(WxPayConfig.WEB_SERVICE_BASE);
            AccessToken token = null;
            switch(tokenType)
            {
                case AccessTokenType.MiniYin:
                        token = WgServiceHelper.TokenMini;
                    break;
                case AccessTokenType.XihuanYintai:
                    token = WgServiceHelper.Token;
                    break;
                default:
                    throw new ArgumentException("tokenType mismatch");
            }
            if (token == null)
            {
                CommonUtil.Log.Info("token is empty");
                return false;
            }

            var notifyRequest = new RestRequest("cgi-bin/message/template/send?access_token={access_token}", Method.POST);
            notifyRequest.RequestFormat = DataFormat.Json;
            notifyRequest.AddUrlSegment("access_token", HttpUtility.UrlEncode(token.access_token));
            notifyRequest.AddBody(requestData);
            var content = client.Execute(notifyRequest).Content;

            var notifyResponse = JsonConvert.DeserializeObject<dynamic>(content);

            if (notifyResponse!=null && notifyResponse.errcode == 0)
            {
                if (successCallback != null)
                    successCallback(notifyResponse);
                return true;
            }
            else
            {
                token.Renew();
                Logger.Debug(notifyResponse.ToString());

                if (failCallback != null)
                    failCallback(notifyResponse);
                return false;
            }


        }

        public static bool RefreshMenu(WxMenu requestData, Action<dynamic> successCallback, Action<dynamic> failCallback)
        {
            var client = new RestClient(WxPayConfig.WEB_SERVICE_BASE);
            var token = new AccessToken(AccessTokenType.MiniYin);
            if (token == null)
            {
                CommonUtil.Log.Info("token is empty");
                return false;
            }
            //delete menu first
            var request = new RestRequest("cgi-bin/menu/delete?access_token={access_token}", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("access_token", token.access_token);
            var notifyResponse = JsonConvert.DeserializeObject<dynamic>(client.Execute(request).Content);

            if (notifyResponse.errcode == 0)
            {
                request = new RestRequest("cgi-bin/menu/create?access_token={access_token}", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddUrlSegment("access_token", token.access_token);
                request.AddBody(requestData);
                var response = JsonConvert.DeserializeObject<dynamic>(client.Execute(request).Content);
                if (response.errcode == 0)
                {
                    if (successCallback != null)
                        successCallback(response);
                    return true; 
                }
                else
                {
                    Logger.Debug(response);
                    if (failCallback != null)
                        failCallback(response);
                    return false;
                }
                
            }
            else
            {
                Logger.Debug(notifyResponse);

                if (failCallback != null)
                    failCallback(notifyResponse);
                return false;
            }
           
        }

        public static WxAppPayTokenResponse GetAppPayToken(string orderNo, decimal totalAmount, string clientIp)
        {
            var client = WebRequest.CreateHttp(new WxAppPayToken() { 
                     OrderNo = orderNo,
                      TotalFee = totalAmount,
                      ClientIp = clientIp
            }.TokenUrl);
            StringBuilder sb = new StringBuilder();
            using (var response = client.GetResponse())
            {
                var body = response.GetResponseStream();
                using (var streamReader = new StreamReader(body, Encoding.UTF8))
                {
                    sb.Append(streamReader.ReadToEnd());
                }
            }
            dynamic xmlResponse = null;
            try
            {
                Logger.Debug(sb);
                xmlResponse = DynamicXml.Parse(sb.ToString());
            }
            catch (Exception ex)
            {

                Logger.Error(sb.ToString());
                throw ex;
            }

            if (xmlResponse == null ||
                xmlResponse.retcode != "0")
            {
                Logger.Error(sb);
                throw new ApplicationException("获取支付token失败");
            }
            return new WxAppPayTokenResponse() { 
                 noncestr = Wxpay.Util.Nonce(),
                 timestamp = DateTime.Now.TicksOfWx(),
                  package = string.Format("Sign={0}",xmlResponse.tenpay_sign),
                 prepayid = xmlResponse.trade_token
            };
            
        }

        private static ILog Logger
        {
            get
            {
                return ServiceLocator.Current.Resolve<ILog>();
            }
        }


        public static string GetHtmlPayUrl(string orderNo, decimal totalAmount, string clientIp, string returnUrl)
        {
            return new WxHtmlPayUrl()
            {
                OrderNo = orderNo,
                TotalFee = totalAmount,
                ClientIp = clientIp,
                ReturnUrl = returnUrl
            }.PayUrl;
        }
    }
}
