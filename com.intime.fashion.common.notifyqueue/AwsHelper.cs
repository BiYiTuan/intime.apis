using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.SQS;
using Amazon.SQS.Model;
using com.intime.fashion.common.config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace com.intime.fashion.common.notifyqueue
{
    public static class AwsHelper
    {
        private static IAmazonSQS sqs;
        private static string queueUrl;
        private static object lockObject = new object();
        private static IAmazonS3 s3;
        public static void SendMessage(string typeName,Func<object> messageComposer)
        {
           
            Task.Factory.StartNew(() => {
                EnsureQueue();
                SendMessageRequest sendMessageRequest = new SendMessageRequest();
                sendMessageRequest.QueueUrl = queueUrl;
                sendMessageRequest.MessageBody = JsonConvert.SerializeObject(new { type = typeName, data = messageComposer() });

                sqs.SendMessage(sendMessageRequest);
            });
           
        }

        private static void EnsureQueue()
        {
            if (!string.IsNullOrEmpty(queueUrl))
                return;
            EnsureSqs();
            lock (lockObject)
            {
                CreateQueueRequest sqsRequest = new CreateQueueRequest();
                sqsRequest.QueueName = CommonConfiguration<AwsConfig>.Current.QUEUE ;
                if (sqsRequest.QueueName == null)
                {
                    return;
                }
                CreateQueueResponse createQueueResponse = sqs.CreateQueue(sqsRequest);
                queueUrl = createQueueResponse.QueueUrl;
            }
        }

        private static void EnsureSqs()
        {
            if (sqs == null)
            {
                lock (lockObject)
                {
                    sqs = AWSClientFactory.CreateAmazonSQSClient();

                }
            }
        }
        private static void EnsureS3()
        {
            if (s3 == null)
            {
                lock (lockObject)
                {
                    s3 = AWSClientFactory.CreateAmazonS3Client(new AmazonS3Config() {
                        ServiceURL = "s3.amazonaws.com",
                         UseHttp = true
                    });

                }
            }
        }

       
    }
}
