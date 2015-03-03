using Newtonsoft.Json;
using System;

namespace com.intime.fashion.common.IT
{
    public class Request
    {
        public Request(object jsonData):this(
            Config.IT_Service_Host,
            Config.IT_Service_From,
            Config.IT_Service_SecretKey,
            jsonData)
        {
        }

        public Request(string url,string appSecret,string secretKey,dynamic jsonData)
        {
            this.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Nonce = new Random(100).Next().ToString("D");
            this.Url = url;
            this.From = appSecret;
            this.SecretKey = secretKey;
            if (jsonData is string)
            {
                this.RequestParams = jsonData;
            }
            else
            {
                this.RequestParams = JsonConvert.SerializeObject(jsonData);
            }
        }

        public string Url { get; private set; }

        public string SecretKey { get; private set; }

        public string From { get; private set; }

        public string TimeStamp { get; private set; }

        public string Nonce { get; private set; }

        public string RequestParams { get; private set; }

        public string ValueToSign { get { return string.Format("{0}{1}{2}{3}", Nonce, TimeStamp, From, RequestParams); } }
    }
}
