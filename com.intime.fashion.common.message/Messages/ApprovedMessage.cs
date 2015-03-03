
namespace com.intime.fashion.common.message.Messages
{
    public class ApprovedMessage : BaseMessage
    {
        public override int ActionType
        {
            get { return (int) MessageAction.Approved; }
        }

        public override int SourceType {
            get { return (int) MessageSourceType.DaogouApply; }
        }
    }
}
