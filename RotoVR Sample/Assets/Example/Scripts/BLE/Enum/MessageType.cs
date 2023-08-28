namespace Example.BLE.Enum
{
    public enum MessageType : byte
    {
        Scan = 0,
        FinishedDiscovering = 1,
        DeviceFound = 2,
        Connect = 3,
        Disconnect = 4,
        TurnOnAngle = 5,
        TurnToAngle = 6,
    }
}
