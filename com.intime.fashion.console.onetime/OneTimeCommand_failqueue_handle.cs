using CLAP;
using com.intime.fashion.common.config;
using com.intime.fashion.common.message;
using com.intime.fashion.service.messages;
using com.intime.fashion.service.messages.Message;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using com.intime.fashion.service;

namespace com.intime.fashion.console.onetime
{
    partial class OneTimeCommand
    {
        [Verb(IsDefault = false, Description = "handle fail queue messages", Aliases = "queue_handle_fail")]
        static void FailQueue_Handle()
        {
            var provider = ServiceLocator.Current.Resolve<IMessageCenterProvider>();
            var receiver = provider.GetReceiver();
            var handlers = GetHandler();
            receiver.ReceiveReliable(message =>
            {
                try
                {
                    var validHandlers = handlers.Where(h => h.SourceType == message.SourceType && (h.ActionType & message.ActionType) == message.ActionType);
                    foreach (var handler in validHandlers)
                    {

                        bool isSuccess = handler.Work(message);
                        if (!isSuccess)
                        {
                            _log.Warn(string.Format("message not handled:{0}", JsonConvert.SerializeObject(message)));
                            return false;
                        }


                    }
                    return true;
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                    _log.Error(receiver.GetDebugInfo());
                    return false;
                }
            },RabbitClientConfiguration.Current.FailQueue,true);
        }

        private static IEnumerable<MessageHandler> GetHandler()
        {
            var handlers = new List<MessageHandler>();
            handlers.Add(new ComboHandler());
            handlers.Add(new ProductHandler());
            handlers.Add(new InventoryHandler());
            handlers.Add(new OrderPaidHandler());
            handlers.Add(new GiftcardPaidHandler());
            return handlers;
        }

    }
}
