
namespace com.rotovr.sdk
{
    class RotateToAngleMessage : BleMessage
    {
        public RotateToAngleMessage(string data)
            : base(MessageType.TurnToAngle, data) { }
    }
}
