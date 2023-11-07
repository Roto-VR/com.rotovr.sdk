using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class DisconnectMessage : BleMessage
    {
        public DisconnectMessage(string data)
            : base(MessageType.Disconnect, data) { }
    }
}
