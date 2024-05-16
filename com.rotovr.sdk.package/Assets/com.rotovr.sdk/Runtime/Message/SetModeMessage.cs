
namespace com.rotovr.sdk
{
    public class SetModeMessage : BleMessage
    {
        public SetModeMessage(string data)
            : base(MessageType.SetMode, data)
        {
        }
    }
}