using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class DisconnectMessage : BleMessage
    {
        public DisconnectMessage(string data)
            : base(MessageType.Disconnect, data) { }
    }
}
