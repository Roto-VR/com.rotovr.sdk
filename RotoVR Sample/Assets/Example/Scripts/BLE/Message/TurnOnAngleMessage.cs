using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class TurnOnAngleMessage: BleMessage
    {
        public TurnOnAngleMessage( byte[] data )
            : base(MessageType.TurnOnAngle, data) { }
    }
}
