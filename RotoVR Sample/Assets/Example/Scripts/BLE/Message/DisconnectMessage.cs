using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class DisconnectMessage : BleMessage
    {
        public DisconnectMessage()
            : base(MessageType.Disconnect) { }
    }
}
