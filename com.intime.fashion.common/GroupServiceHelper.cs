using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;

namespace com.intime.fashion.common
{
    public static class GroupServiceHelper
    {
        public static bool SendHttpMessage(string groupPointConvertUrl,string publickey,string privatekey, dynamic p,out string errorMsg)
        {
            errorMsg = string.Empty;
            if (string.IsNullOrEmpty(groupPointConvertUrl))
                throw new ArgumentException("groupPointConvertUrl is empty");
            if (string.IsNullOrEmpty(publickey) || string.IsNullOrEmpty(privatekey))
                throw new ArgumentException("private/public key is empty");

            dynamic requestBody = ConstructRequestBody(publickey, privatekey, p);
            var client = WebRequest.CreateHttp(groupPointConvertUrl);
            client.ContentType = "application/json";
            client.Method = "Post";
            using (var request = client.GetRequestStream())
            using (var streamWriter = new StreamWriter(request))
            {
                string requestStr = JsonConvert.SerializeObject(requestBody);
               
                streamWriter.Write(requestStr);
            }
            StringBuilder sb = new StringBuilder();
            using (var response = client.GetResponse())
            {
                var body = response.GetResponseStream();
                using (var streamReader = new StreamReader(body, Encoding.UTF8))
                {
                    
                        sb.Append(streamReader.ReadToEnd());
                }
            }
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(sb.ToString());

            if (jsonResponse != null &&
                jsonResponse.IsSuccessful == true)
            {      

                return true;
            }
            else
            {
                errorMsg = jsonResponse.Message;
                return false;
            }
        }
        private static dynamic ConstructRequestBody(string publickey, string privatekey, dynamic data)
        { 
            Dictionary<string, dynamic> query = new Dictionary<string, dynamic>();
            query.Add("key", publickey);
            query.Add("nonce", new Random(1000).Next().ToString());
            query.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ"));


            var signingValue = new StringBuilder();
            var signedValue = string.Empty;
            foreach (var s in query.Values.ToArray().OrderBy(s => s))
                signingValue.Append(s);
            Logger.Debug(string.Format("signed value:{0}", signingValue.ToString()));
            using (HMACSHA1 hmac = new HMACSHA1(Encoding.ASCII.GetBytes(privatekey)))
            {
                var hashValue = hmac.ComputeHash(Encoding.ASCII.GetBytes(signingValue.ToString()));
                signedValue = Convert.ToBase64String(hashValue);
               
            }
            query.Add("sign", signedValue);
            query.Add("data",data);

            return new { 
                key = query["key"],
                nonce = int.Parse(query["nonce"]),
                timestamp = query["timestamp"],
                sign = query["sign"],
                data = query["data"]

            };

        }
         private static ILog Logger
        {
            get {
                return ServiceLocator.Current.Resolve<ILog>();
            }
        }
    }
}
