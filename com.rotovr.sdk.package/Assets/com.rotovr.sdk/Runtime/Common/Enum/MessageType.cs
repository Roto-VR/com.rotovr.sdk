namespace RotoVR.SDK.Enum
{
    public enum MessageType : byte
    {
        Scan = 0,
        FinishedDiscovering = 1,
        DeviceFound = 2,
        Connect = 3,
        Connected = 4,
        Disconnect = 5,
        Disconnected = 6,
        DeviceConnected = 7,
        TurnToAngle = 8,
        SetMode = 9,
        ModelChanged = 10,
        PlayRumble = 11,
    }
}