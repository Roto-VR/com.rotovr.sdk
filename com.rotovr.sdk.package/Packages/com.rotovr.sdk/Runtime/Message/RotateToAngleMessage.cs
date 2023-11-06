using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class RotateToAngleMessage : BleMessage
    {
        public RotateToAngleMessage(string data)
            : base(MessageType.TurnToAngle, data) { }
    }
}
