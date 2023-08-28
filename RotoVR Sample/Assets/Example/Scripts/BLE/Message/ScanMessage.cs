using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class ScanMessage : BleMessage
    {
        public ScanMessage()
            : base(MessageType.Scan) { }
    }
}
