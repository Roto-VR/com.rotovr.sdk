using System;

namespace com.rotovr.sdk
{
    /// <summary>
    /// Chair state model.
    /// </summary>
    [Serializable]
    public class RotoDataModel
    {
        
        /// <summary>
        /// Default state. FreeMod and 0 rotation.
        /// </summary>
        internal RotoDataModel()
        {
            Mode = ModeType.FreeMode.ToString();
            Angle = 0;
        }
        
        internal RotoDataModel(string mode, int angle, int targetCockpit, int maxPower)
        {
            Mode = mode;
            Angle = angle;
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
        }

        public string Mode { get; set; }

        public ModeType ModeType => EnumUtility.ParseOrDefault<ModeType>(Mode);
        
        public int Angle { get; set; }
        public int TargetCockpit { get; set; }
        public int MaxPower { get; set; }
    }
}