using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class PlayRumbleMessage : BleMessage
    {
        public PlayRumbleMessage(string data) : base(MessageType.PlayRumble, data)
        {
        }
    }
}