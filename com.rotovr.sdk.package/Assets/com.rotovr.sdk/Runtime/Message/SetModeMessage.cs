using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class SetModeMessage : BleMessage
    {
        public SetModeMessage(string data)
            : base(MessageType.SetMode, data)
        {
        }
    }
}