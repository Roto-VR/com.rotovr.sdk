namespace RotoVR.SDK.Enum
{
    public enum CalibrationMode
    {
        SetToZero, // Rotate the chair to 0 degrees and use as default rotation
        SetCurrent, // Set current angle as default rotation
        SetLast, //Set last calibration data, rotate the chair to this data and use as default rotation
    }
}