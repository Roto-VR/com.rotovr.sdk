namespace com.rotovr.sdk
{
    enum MessageType : byte
    {
        Scan = 0x00,
        FinishedDiscovering = 0x01,
        DeviceFound = 0x02,
        Connect = 0x03,
        Connected = 0x04,
        Disconnect = 0x05,
        Disconnected = 0x06,
        DeviceConnected = 0x07,
        TurnToAngle = 0x08,
        SetMode = 0x09,
        ModelChanged = 0x010,
        PlayRumble = 0x011,
    }
}