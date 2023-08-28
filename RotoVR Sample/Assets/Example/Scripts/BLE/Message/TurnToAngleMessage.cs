using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class TurnToAngleMessage : BleMessage
    {
        public TurnToAngleMessage(string data)
            : base(MessageType.TurnToAngle, data) { }
    }
}
