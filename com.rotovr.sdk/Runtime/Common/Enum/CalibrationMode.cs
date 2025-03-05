
namespace com.rotovr.sdk
{
    /// <summary>
    /// Defines the different calibration modes for the Roto VR chair.
    /// These modes determine how the chair sets its default rotation reference point.
    /// </summary>
    public enum CalibrationMode
    {
        /// <summary>
        /// Rotates the chair to 0 degrees (forward-facing position) 
        /// and sets it as the default rotation reference.
        /// </summary>
        SetToZero, 
        
#if !NO_UNITY
        /// <summary>
        /// Sets the chair's current rotation angle as the new default reference point.
        /// This allows users to define a custom forward direction based on their current position.
        /// </summary>
        SetCurrent,  
        
        /// <summary>
        /// Restores the last saved calibration data.
        /// The chair will rotate to the last calibrated position and use it as the default rotation reference.
        /// </summary>
        SetLast,
#endif
    }
}