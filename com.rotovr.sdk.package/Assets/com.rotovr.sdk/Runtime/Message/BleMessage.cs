namespace com.rotovr.sdk
{
    class BleMessage
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
