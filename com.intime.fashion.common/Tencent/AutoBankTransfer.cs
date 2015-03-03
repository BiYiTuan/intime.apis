using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;

namespace com.intime.fashion.common.Tencent
{
    public class AutoBankTransfer
    {
        private static object lockObject = new object();
        private static AutoBankTransfer transferObject = null;
        private string operateUserId = string.Empty;
        private string operateUserPwd = string.Empty;
        private string parterId = string.Empty;
        private string caFilePath = string.Empty;
        private string certFilePath = string.Empty;
        private string staticIp;
        private StringBuilder debugInfo = new StringBuilder();
        private AutoBankTransfer()
        {
            operateUserId = Config.OP_USERID;
            operateUserPwd = CommonUtil.MD5_Encode(Config.OP_USERPWD,Config.DEFAULT_ENCODE);
            parterId = Config.PARTER_ID;
            caFilePath = Config.CA_FILE_PATH;
            certFilePath = Config.CERT_FILE_PATH;
            staticIp = Config.STATIC_PUBLIC_IP;
        }

        public static AutoBankTransfer Instance { get {
            if (transferObject == null)
            {
                lock (lockObject)
                {
                    if (transferObject == null)
                    {
                        transferObject = new AutoBankTransfer();
                    }
                }
            }
            return transferObject;
        } }

        public BatchTransferResponse BatchTransfer(BatchTransferRequest request)
        {
            EnsureRequest(request);
            return Send<BatchTransferResponse>(request);
        }

        public BatchQueryResponse Query(BatchQueryRequest request)
        {
            EnsureRequest(request);
            return Send<BatchQueryResponse>(request);
        }

        public string GetDebugLine()
        {
            return debugInfo.ToString();
        }
        private void resetDebug()
        {
            debugInfo.Clear();
        }
        private T Send<T>(BaseBatchRequest request) where T:BaseBatchResponse
        {
            resetDebug();
            var client = WebRequest.CreateHttp(Config.SERVICE_URI_BATCH_TRANSFER);
            client.ContentType = "application/x-www-form-urlencoded";
            client.Method = "Post";
            var certPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.CERT_FILE_PATH);
            client.ClientCertificates.Add(new X509Certificate2(certPath,Config.CERT_FILE_PWD));
            using (var webRequest = client.GetRequestStream())
            using (var streamWriter = new StreamWriter(webRequest, Config.DEFAULT_ENCODE))
            {
                string requestStr = request.EncodedFormat;

                streamWriter.Write(requestStr);
            }
            try
            {
                using (var response = client.GetResponse())
                {
                    
                    using (var body = response.GetResponseStream())
                    {
                        var serializer = new XmlSerializer(typeof(T));

                        var transferResponse = serializer.Deserialize(body) as T;
                        if (transferResponse == null)
                        {
                            debugInfo.AppendLine("batch send response deserialize fail");
                            return default(T);
                        }
                        if (!transferResponse.IsSuccess)
                            debugInfo.AppendLine(string.Format("batch send response fail, raw response:{0}_{1}", transferResponse.RetCode, transferResponse.RetMsg));
                        else
                            debugInfo.AppendLine(string.Format("batch send sucess:{0}",JsonConvert.SerializeObject(transferResponse)));
                        return transferResponse;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnsureRequest(BatchTransferRequest request)
        {
            if (request != null) {

                request.OperatePwdMd5 = operateUserPwd;
                request.OperateUser = operateUserId;
                request.SPId = parterId;
                request.OperateTime = DateTime.Now.ToString("yyyyMMddHHmmssSSS");
                request.ClientIp = staticIp;
            }
            
        }
        private void EnsureRequest(BatchQueryRequest request)
        {
            if (request != null)
            {

                request.OperatePwdMd5 = operateUserPwd;
                request.OperateUser = operateUserId;
                request.SPId = parterId;
                request.OperateTime = DateTime.Now.ToString("yyyyMMddHHmmssSSS");
                request.ClientIp = staticIp;
            }

        }
        
    }
}
