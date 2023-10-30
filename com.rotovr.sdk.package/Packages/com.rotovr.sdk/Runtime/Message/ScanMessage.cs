using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class ScanMessage : BleMessage
    {
        public ScanMessage() : base(MessageType.Scan)
        {
        }
    }
}