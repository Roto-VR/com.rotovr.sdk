using System;

namespace RotoVR.SDK.Model
{
    [Serializable]
    public class ModeModel
    {
        public ModeModel()
        {
        }

        public ModeModel(string mode, int targetCockpit, int maxPower)
        {
            Mode = mode;
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
        }

        public string Mode { get; set; }
        public int TargetCockpit { get; set; }
        public int MaxPower { get; set; }
    }
}