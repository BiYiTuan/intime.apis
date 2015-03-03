using com.intime.fashion.common.config;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace com.intime.fashion.common.message.rabbit
{
    class MessageClient:IMessageReceiver,IMessageSender
    {
        private ConnectionFactory _factory = new ConnectionFactory() { 
                HostName = RabbitClientConfiguration.Current.Host , 
                UserName = RabbitClientConfiguration.Current.UserName,
                Password = RabbitClientConfiguration.Current.Password};
        private IModel _model = null;
        private StringBuilder _debug = new StringBuilder();

        public bool SendMessageReliable(BaseMessage message)
        {
            return SendMessageReliable(message, null);
        }

        public bool SendMessageReliable(BaseMessage message, Action<BaseMessage> preMessageHandler)
        {
            return SendMessageReliable(message, preMessageHandler, RabbitClientConfiguration.Current.QueueName);
        }

        public bool SendMessageReliable(BaseMessage message, Action<BaseMessage> preMessageHandler, string messageTopic)
        {
            if (preMessageHandler != null)
                preMessageHandler(message);
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queueName = messageTopic;
                    channel.QueueDeclare(queueName, true, false, false, null);

                    var jsonMessage = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(jsonMessage);

                    var properties = channel.CreateBasicProperties();
                    properties.SetPersistent(true);
                    channel.BasicPublish("", queueName, properties, body);
                }
            }
            return true;
        }

        public void ReceiveReliable(Func<BaseMessage,bool> postMessageHandler) 
        {
            ReceiveReliable(postMessageHandler, RabbitClientConfiguration.Current.QueueName);
        }
        public void ReceiveReliable(Func<BaseMessage, bool> postMessageHandler, string messageTopic)
        {
            ReceiveReliable(postMessageHandler, messageTopic, false);
        }
        public void ReceiveReliable(Func<BaseMessage, bool> postMessageHandler, string messageTopic,bool discardFailMessage)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var _channel = channel;
                    var queueName = messageTopic;
                    channel.QueueDeclare(queueName, true, false, false, null);

                    channel.BasicQos(0, 1, false);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, false, consumer);
                    while (true)
                    {
                        try
                        {
                            var ea =
                                (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                            var body = ea.Body;
                            var rawMessage = Encoding.UTF8.GetString(body);

                            var message = JsonConvert.DeserializeObject<BaseMessage>(rawMessage);
                            if (message == null)
                                _debug.Append(rawMessage);
                            channel.BasicAck(ea.DeliveryTag, false);
                            if (!postMessageHandler(message) && !discardFailMessage)
                                SendMessageReliable(message,
                                    null,
                                    RabbitClientConfiguration.Current.FailQueue);
                        }
                        catch (OperationInterruptedException ex)
                        {
                            _debug.Append(ex.ToString());
                            break;
                        }
                    }
                }
            }
        }
        public void Cancel()
        {
            if (_model != null)
                _model.Close();
           
        }

        public string GetDebugInfo()
        {
            var debugInfo = _debug.ToString();
            _debug.Clear();
            return debugInfo;
        }


      

      
    }
}
