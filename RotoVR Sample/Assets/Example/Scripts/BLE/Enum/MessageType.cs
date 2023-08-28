namespace Example.BLE.Enum
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
        TurnOnAngle = 7,
        TurnToAngle = 8,
    }
}
