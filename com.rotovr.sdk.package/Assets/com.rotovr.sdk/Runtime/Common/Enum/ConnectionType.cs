
namespace com.rotovr.sdk
{
    /// <summary>
    /// Chair connection type.
    /// Only used in the editor to define if we will connect to a real device or
    /// will simulate the connection.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Connection to the real chair.
        /// </summary>
        Chair,
        
        /// <summary>
        /// Emulate connection to the chair.
        /// </summary>
        Simulation,
    }
}