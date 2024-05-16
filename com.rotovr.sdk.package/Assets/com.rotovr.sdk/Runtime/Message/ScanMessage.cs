
namespace com.rotovr.sdk
{
    public class ScanMessage : BleMessage
    {
        public ScanMessage() : base(MessageType.Scan)
        {
        }
    }
}