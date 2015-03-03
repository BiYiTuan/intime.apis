using System;
using System.Configuration;
using com.intime.o2o.data.exchange.IT;
using com.intime.o2o.data.exchange.IT.Request;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public class SMSController:RestfulController
    {
        private string IT_SMS_BASE_URL = ConfigurationManager.AppSettings["IT_SMS_BASE_URL"];
        private string IT_SMS_SECRET_KEY = ConfigurationManager.AppSettings["IT_SMS_SECRET_KEY"];
        private string IT_SMS_FROM = ConfigurationManager.AppSettings["IT_SMS_FROM"];
        private IApiClient _client ;
        public SMSController()
        {
            this._client = new DefaultApiClient(IT_SMS_BASE_URL,IT_SMS_SECRET_KEY,IT_SMS_FROM);
        }

        //static readonly Regex regex = new Regex(@"^(13[0-9]|15[0-9]|18[0-9])\d{8}$",RegexOptions.Compiled);
        [RestfulAuthorize]
        public ActionResult Send(string phone, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return this.RenderError(r => r.Message = "text is empty");
            }

            try
            {
                var rsp = _client.Post(new SMSSendRequest()
                {
                    Data = new {mobile = phone, content = text}
                });

                if (!rsp.Status)
                {
                    return this.RenderError(r => r.Message = "Failed to send sms message, please try again.");
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex);
                return this.RenderError(r => r.Message = "发送短信失败，请联系管理员!");
            }

            return this.RenderSuccess<dynamic>(null);
        }
    }
}
