
namespace com.rotovr.sdk
{
    class ConnectionStatusMessage: BleMessage
    {
        public ConnectionStatusMessage(MessageType messageType, string data = "")
            : base(messageType, data) { }
    }
}
