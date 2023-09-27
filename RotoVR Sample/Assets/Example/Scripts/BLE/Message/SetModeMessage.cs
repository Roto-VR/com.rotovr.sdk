using Example.BLE.Enum;

namespace Example.BLE.Message
{
    public class SetModeMessage : BleMessage
    {
        public SetModeMessage(ModeType modeType, string data = "")
            : base(MessageType.SetMode, data) { }
    }
}
