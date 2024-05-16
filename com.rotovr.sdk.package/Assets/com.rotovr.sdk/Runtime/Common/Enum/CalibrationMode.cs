namespace com.rotovr.sdk
{
    public enum CalibrationMode
    {
        /// <summary>
        /// Rotate the chair to 0 degrees and use as default rotation
        /// </summary>
        SetToZero, 
        
        /// <summary>
        /// Set current angle as default rotation
        /// </summary>
        SetCurrent,  
        
        /// <summary>
        /// Set last calibration data, rotate the chair to this data and use as default rotation
        /// </summary>
        SetLast,
    }
}