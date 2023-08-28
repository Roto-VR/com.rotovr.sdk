using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class TurnToAngleMessage : BleMessage
    {
        public TurnToAngleMessage(byte[] data)
            : base(MessageType.TurnToAngle, data) { }
    }
}
