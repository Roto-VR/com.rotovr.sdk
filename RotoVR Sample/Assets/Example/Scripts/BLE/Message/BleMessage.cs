using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class BleMessage
    {
        public BleMessage(MessageType messageType, byte[] data = null)
        {
            MessageType = messageType;
            Data = data;
        }

        public MessageType MessageType { get; }
        public byte[] Data { get; }
    }
}
