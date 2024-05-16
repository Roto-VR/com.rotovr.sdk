
namespace com.rotovr.sdk
{
    public class PlayRumbleMessage : BleMessage
    {
        public PlayRumbleMessage(string data) : base(MessageType.PlayRumble, data)
        {
        }
    }
}