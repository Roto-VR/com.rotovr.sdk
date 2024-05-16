
namespace com.rotovr.sdk
{
    class PlayRumbleMessage : BleMessage
    {
        public PlayRumbleMessage(string data) : base(MessageType.PlayRumble, data)
        {
        }
    }
}