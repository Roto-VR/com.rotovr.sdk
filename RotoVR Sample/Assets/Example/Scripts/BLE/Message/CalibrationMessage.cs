using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class CalibrationMessage: BleMessage
    {
        public CalibrationMessage( string data = "")
            : base(MessageType.Calibration, data) { }
    }
}
