using System;
using System.Collections.Generic;

namespace com.rotovr.sdk
{
    /// <summary>
    /// Chair state model.
    /// </summary>
    [Serializable]
    public class RotoDataModel
    {

        internal RotoDataModel(string json)
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            
            Mode = dict["Mode"].ToString();
            Angle = Convert.ToInt32(dict["Angle"]);
            TargetCockpit = Convert.ToInt32(dict["TargetCockpit"]);
            MaxPower = Convert.ToInt32(dict["MaxPower"]);
        }
        
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

        public string ToJson()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("Mode", Mode);
            dict.Add("ModeType", (int) ModeType);
            dict.Add("Angle", Angle);
            dict.Add("TargetCockpit", TargetCockpit);
            dict.Add("MaxPower", MaxPower);

            return Json.Serialize(dict);
        }

        public string Mode { get; set; }

        public ModeType ModeType => EnumUtility.ParseOrDefault<ModeType>(Mode);
        
        public int Angle { get; set; }
        public int TargetCockpit { get; set; }
        public int MaxPower { get; set; }
    }
}