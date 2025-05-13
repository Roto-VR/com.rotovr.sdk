using System;

namespace com.rotovr.sdk
{
    /// <summary>
    /// Parameters for switching chair modes.
    /// </summary>
    public struct ModeParams
    {
        /// <summary>
        /// The maximum rotation angle limit in cockpit mode.
        /// Acceptable values range from 60 to 140 degrees.
        /// </summary>
        public int CockpitAngleLimit;

        /// <summary>
        /// Defines the movement mode when stopping the chair.
        /// Use <see cref="MovementMode.Smooth"/> for a gradual stop, 
        /// or <see cref="MovementMode.Jerky"/> for an abrupt stop.
        /// </summary>
        public MovementMode MovementMode;

        /// <summary>
        /// The maximum rotational power of the chair.
        /// Acceptable values range from 30 to 100.
        /// </summary>
        public int MaxPower;
    }
    

    [Serializable]
    class ModeParametersModel
    {
        public ModeParametersModel(ModeParams modeParams)
        {
            TargetCockpit = modeParams.CockpitAngleLimit;
            MaxPower = modeParams.MaxPower;
            MovementMode = modeParams.MovementMode.ToString();
        }
        
        public ModeParametersModel(int targetCockpit, int maxPower)
        {
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
            MovementMode = "Smooth";
        }

        public ModeParametersModel(int targetCockpit, int maxPower, string movementMode)
        {
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
            MovementMode = movementMode;
        }

        public int TargetCockpit { get; set; }
        public int MaxPower { get; set; }
        public string MovementMode { get; set; } 
    }
}