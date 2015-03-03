using com.intime.fashion.common.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.intime.fashion.service.messages
{
    public abstract class MessageHandler
    {
        public abstract int SourceType { get; }

        public abstract int ActionType { get; }

        public virtual bool Work(BaseMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
