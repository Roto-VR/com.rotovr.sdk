
namespace com.rotovr.sdk
{
    class DisconnectMessage : BleMessage
    {
        public DisconnectMessage(string data)
            : base(MessageType.Disconnect, data) { }
    }
}
