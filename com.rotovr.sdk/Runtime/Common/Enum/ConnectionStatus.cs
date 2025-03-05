namespace com.rotovr.sdk
{ 
    /// <summary>
    /// Represents the current connection status between the system and the chair.
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// The connection status is unknown. No attempt has been made to connect to the chair yet.
        /// </summary>
        Unknown,
        
        /// <summary>
        /// The system is in the process of connecting to the chair.
        /// </summary>
        Connecting,
        
        /// <summary>
        /// The system has successfully connected to the chair.
        /// </summary>
        Connected,
        
        /// <summary>
        /// The system has been disconnected from the chair.
        /// </summary>
        Disconnected,
    }
}
