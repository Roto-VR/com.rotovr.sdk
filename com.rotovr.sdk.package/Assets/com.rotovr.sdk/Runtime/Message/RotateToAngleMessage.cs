
namespace com.rotovr.sdk
{
    public class RotateToAngleMessage : BleMessage
    {
        public RotateToAngleMessage(string data)
            : base(MessageType.TurnToAngle, data) { }
    }
}
