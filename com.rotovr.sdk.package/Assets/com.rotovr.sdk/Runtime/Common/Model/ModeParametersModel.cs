using System;

namespace com.rotovr.sdk
{
    /// <summary>
    /// Mode switch params.
    /// </summary>
    public struct ModeParams
    {
        /// <summary>
        /// Target cockpit angle limit. Values can range from 60 to 140.
        /// </summary>
        public int CockpitAngleLimit;

        /// <summary>
        /// Movement Mode.
        /// Use <see cref="MovementMode.Smooth"/>, for smooth stop and <see cref="MovementMode.Jerky"/>, for hard stop.
        /// </summary>
        public MovementMode MovementMode;

        /// <summary>
        /// Max value of the chair rotation power.Сan take values in range 30-100.
        /// </summary>
        public int MaxPower;
    }
    

    [Serializable]
    public class ModeParametersModel
    {

        public ModeParametersModel(ModeParams modeParams)
        {
            TargetCockpit = modeParams.CockpitAngleLimit;
            MaxPower = modeParams.MaxPower;
            MovementMode = modeParams.ToString();
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