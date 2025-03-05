namespace com.rotovr.sdk
{
    /// <summary>
    /// Defines how the Roto VR chair stops its movement.
    /// Determines whether the chair decelerates smoothly or stops abruptly.
    /// </summary>
    public enum MovementMode
    {
        /// <summary>
        /// The chair gradually slows down to a stop, ensuring a smooth and comfortable experience.
        /// </summary>
        Smooth, 
        
        /// <summary>
        /// The chair stops abruptly without gradual deceleration, resulting in a sudden halt.
        /// </summary>
        Jerky, 
    }
}