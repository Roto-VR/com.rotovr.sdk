
namespace com.rotovr.sdk
{
    class ConnectMessage : BleMessage
    {
        public ConnectMessage(string data)
            : base(MessageType.Connect, data) { }
    }
}
