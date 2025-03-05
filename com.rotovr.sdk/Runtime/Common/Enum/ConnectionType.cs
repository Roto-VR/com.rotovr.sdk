
namespace com.rotovr.sdk
{
    /// <summary>
    /// Specifies the type of connection used for the Roto VR chair.
    /// This setting is only applicable in the Unity Editor to determine whether 
    /// the system should connect to a physical Roto VR chair or simulate its behavior.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Establishes a connection with a physical Roto VR chair.
        /// This mode requires a real device to be connected.
        /// </summary>
        Chair,
        
        /// <summary>
        /// Simulates the chair's behavior without requiring a physical device.
        /// Useful for testing and development when a real chair is unavailable.
        /// </summary>
        Simulation,
    }
}