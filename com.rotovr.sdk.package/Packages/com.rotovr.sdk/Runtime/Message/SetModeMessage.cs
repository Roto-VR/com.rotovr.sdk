using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class SetModeMessage : BleMessage
    {
        public SetModeMessage(ModeType modeType, string data = "")
            : base(MessageType.SetMode, data) { }
    }
}
