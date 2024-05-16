
namespace com.rotovr.sdk
{
    public class ConnectMessage : BleMessage
    {
        public ConnectMessage(string data)
            : base(MessageType.Connect, data) { }
    }
}
