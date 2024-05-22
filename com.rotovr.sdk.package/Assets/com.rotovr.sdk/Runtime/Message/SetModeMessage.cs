
namespace com.rotovr.sdk
{
    class SetModeMessage : BleMessage
    {
        public SetModeMessage(string data)
            : base(MessageType.SetMode, data)
        {
        }
    }
}