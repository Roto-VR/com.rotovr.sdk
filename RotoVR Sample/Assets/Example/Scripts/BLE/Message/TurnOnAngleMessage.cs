using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class TurnOnAngleMessage : BleMessage
    {
        public TurnOnAngleMessage(string data)
            : base(MessageType.TurnOnAngle, data) { }
    }
}
