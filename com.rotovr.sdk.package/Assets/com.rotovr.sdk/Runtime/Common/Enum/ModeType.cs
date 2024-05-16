namespace com.rotovr.sdk
{
    public enum ModeType : byte
    {
        IdleMode = 0x00,
        Calibration = 0x01,
        HeadTrack = 0x02,
        FreeMode = 0x03,
        CockpitMode = 0x04,
        Error = 0x05,
        FollowObject = 0x06,
    }
}