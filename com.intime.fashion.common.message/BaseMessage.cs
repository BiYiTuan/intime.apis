using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.intime.fashion.common.message
{
    public  class BaseMessage
    {
        private long _messageId = DateTime.UtcNow.Ticks;

        public virtual long MessageId
        {
            get
            {
                return _messageId;
            }
            set {
                _messageId = value;
            }
        }
        public int EntityId { get; set; }

        public virtual int ActionType { get; set; }

        public virtual int SourceType { get; set; }
        public string SourceNo { get; set; }
    }
}
