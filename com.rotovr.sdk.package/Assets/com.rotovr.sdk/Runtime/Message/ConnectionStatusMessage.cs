using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class ConnectionStatusMessage: BleMessage
    {
        public ConnectionStatusMessage(MessageType messageType, string data = "")
            : base(messageType, data) { }
    }
}
