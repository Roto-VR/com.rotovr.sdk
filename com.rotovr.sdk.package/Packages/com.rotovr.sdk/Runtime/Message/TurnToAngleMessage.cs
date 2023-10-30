using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class TurnToAngleMessage : BleMessage
    {
        public TurnToAngleMessage(string data)
            : base(MessageType.TurnToAngle, data) { }
    }
}
