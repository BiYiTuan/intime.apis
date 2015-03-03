using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.message.Messages
{
    public class DeleteMessage:BaseMessage
    {

        public override int ActionType
        {
            get { return (int)MessageAction.UpdateEntity; }
            set { }
        }

    }
}
