namespace com.rotovr.sdk
{
    /// <summary>
    /// Represents the direction of rotation for the Roto VR chair.
    /// Used to specify whether the chair should rotate clockwise or counterclockwise.
    /// </summary>
    public enum Direction : byte
    {
        /// <summary>
        /// Rotate to the left (counterclockwise).
        /// </summary>
        Left = 0x00,
        
        /// <summary>
        /// Rotate to the right (clockwise).
        /// </summary>
        Right = 0x01,
    }
}
