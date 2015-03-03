using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.intime.fashion.common.message
{
    public interface IMessageSender
    {
        bool SendMessageReliable(BaseMessage message);

        bool SendMessageReliable(BaseMessage message, Action<BaseMessage> preMessageHandler);

        bool SendMessageReliable(BaseMessage message, Action<BaseMessage> preMessageHandler,string messageTopic);
    }
}
