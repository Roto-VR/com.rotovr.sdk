using System;

namespace RotoVR.SDK.Model
{
    [Serializable]
    public class ModeParametersModel
    {
        public ModeParametersModel(int targetCockpit, int maxPower)
        {
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
            MovementMode = string.Empty;
        }

        public ModeParametersModel(int targetCockpit, int maxPower, string movementMode)
        {
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
            MovementMode = movementMode;
        }

        public int TargetCockpit { get; set; } //Target cockpit angle.Сan take values in range 60-140
        public int MaxPower { get; set; } // Max value of the chair rotation power.Сan take values in range 30-100
        public string MovementMode { get; set; } // Сan take values "Smooth", for smooth stop of the chair and "Jerky", for hard stop of the chair
    }
}
