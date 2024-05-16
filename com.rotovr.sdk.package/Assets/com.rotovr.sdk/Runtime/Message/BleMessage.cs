namespace com.rotovr.sdk
{
    public class BleMessage
    {
        public BleMessage(MessageType messageType, string data = "")
        {
            MessageType = messageType;
            Data = data;
        }

        public MessageType MessageType { get; }
        public string Data { get; }
    }
}
