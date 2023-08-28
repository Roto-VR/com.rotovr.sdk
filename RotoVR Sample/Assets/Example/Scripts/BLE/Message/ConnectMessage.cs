using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class ConnectMessage : BleMessage
    {
        public ConnectMessage(byte[] data)
            : base(MessageType.Connect, data) { }
    }
}
