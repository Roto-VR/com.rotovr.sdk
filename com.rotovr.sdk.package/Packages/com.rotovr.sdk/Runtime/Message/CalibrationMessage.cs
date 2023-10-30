using RotoVR.SDK.Enum;

namespace RotoVR.SDK.Message
{
    public class CalibrationMessage: BleMessage
    {
        public CalibrationMessage( string data = "")
            : base(MessageType.Calibration, data) { }
    }
}
