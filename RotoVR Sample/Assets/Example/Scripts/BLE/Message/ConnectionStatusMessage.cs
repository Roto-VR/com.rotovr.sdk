using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class ConnectionStatusMessage: BleMessage
    {
        public ConnectionStatusMessage(MessageType messageType, string data = "")
            : base(messageType, data) { }
    }
}
