
namespace com.rotovr.sdk
{
    public class DisconnectMessage : BleMessage
    {
        public DisconnectMessage(string data)
            : base(MessageType.Disconnect, data) { }
    }
}
