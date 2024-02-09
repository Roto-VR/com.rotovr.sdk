using System;

namespace RotoVR.SDK.Model
{
    [Serializable]
    public class RotoDataModel
    {
        public RotoDataModel()
        {
            Mode = string.Empty;
            Angle = 0;
        }

        public RotoDataModel(string mode, int angle, int targetCockpit, int maxPower)
        {
            Mode = mode;
            Angle = angle;
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
        }

        public string Mode { get; set; }
        public int Angle { get; set; }
        public int TargetCockpit { get; set; }
        public int MaxPower { get; set; }
    }
}