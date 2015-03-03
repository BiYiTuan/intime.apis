using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.message.rabbit
{
    public class MessageProvider:IMessageCenterProvider
    {
        private MessageClient _client;

        private void EnsureClient()
        {
            if (_client == null)
                _client = new MessageClient();
        }
        public IMessageSender GetSender()
        {
            EnsureClient();
            return _client;
        }

        public IMessageReceiver GetReceiver()
        {
            EnsureClient();
            return _client;
        }
    }
}
