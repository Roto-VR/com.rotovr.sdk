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

        public RotoDataModel(string mode, int angle)
        {
            Mode = mode;
            Angle = angle;
        }

        public string Mode { get; }
        public int Angle { get; }
    }
}