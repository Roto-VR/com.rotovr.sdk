
namespace com.rotovr.sdk
{
    public class ConnectionStatusMessage: BleMessage
    {
        public ConnectionStatusMessage(MessageType messageType, string data = "")
            : base(messageType, data) { }
    }
}
