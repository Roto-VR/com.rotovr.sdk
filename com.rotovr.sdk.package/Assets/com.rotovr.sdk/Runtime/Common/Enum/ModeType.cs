namespace com.rotovr.sdk
{
    /// <summary>
    /// Chair mode type.
    /// </summary>
    public enum ModeType : byte
    {
        /// <summary>
        /// Ignores all commands to rotate еру chair.
        /// </summary>
        IdleMode = 0x00,
        
        /// <summary>
        /// Chair Calibration mode.
        /// </summary>
        Calibration = 0x01,
        
        /// <summary>
        /// Head tracking mode. Chair will follow user headset.
        /// </summary>
        HeadTrack = 0x02,
        
        /// <summary>
        /// Allows user to rotate the chair without any restrictions.
        /// </summary>
        FreeMode = 0x03,
        
        /// <summary>
        /// Allows uer rotate the chair, but angles limit will apply.
        /// </summary>
        CockpitMode = 0x04,
        
        /// <summary>
        /// Mode switch failed.
        /// </summary>
        Error = 0x05,
        
        /// <summary>
        /// Artificial mode. Uses <see cref="HeadTrack"/> mode under the hood.
        /// Allows chair ti follow gameobject rotation in your scene.
        /// </summary>
        FollowObject = 0x06,
    }
}