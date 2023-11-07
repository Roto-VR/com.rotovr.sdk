using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class ConnectMessage : BleMessage
    {
        public ConnectMessage(string data)
            : base(MessageType.Connect, data) { }
    }
}
