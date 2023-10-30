using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class TurnOnAngleMessage : BleMessage
    {
        public TurnOnAngleMessage(string data)
            : base(MessageType.TurnOnAngle, data) { }
    }
}
