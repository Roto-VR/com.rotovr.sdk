namespace com.rotovr.sdk
{ 
    public enum ConnectionStatus
    {
        /// <summary>
        /// We haven't yet performed any attempts to connect to teh chair.
        /// </summary>
        Unknown,
        
        /// <summary>
        /// Connection in progress.
        /// </summary>
        Connecting,
        
        /// <summary>
        /// Chair is connected.
        /// </summary>
        Connected,
        
        /// <summary>
        /// Chair is disconnected.
        /// </summary>
        Disconnected,
    }
}
