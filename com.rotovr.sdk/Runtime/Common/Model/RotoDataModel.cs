using System;
using System.Collections.Generic;

namespace com.rotovr.sdk
{
    /// <summary>
    /// Represents the state of the chair, including its mode, angle, cockpit settings, and power limits.
    /// This model is used to capture and serialize the current chair state.
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

        /// <summary>
        /// Converts the chair state into a JSON string representation.
        /// </summary>
        /// <returns>A JSON string representing the current state of the chair.</returns>
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

        /// <summary>
        /// Gets or sets the current mode of the chair (e.g., "FreeMode", "Calibration").
        /// </summary>
        public string Mode { get; set; }
        
        /// <summary>
        /// Gets the <see cref="ModeType"/> value corresponding to the current chair mode.
        /// Parses the string <see cref="Mode"/> into the appropriate <see cref="ModeType"/> enum value.
        /// </summary>
        public ModeType ModeType => EnumUtility.ParseOrDefault<ModeType>(Mode);
        
        /// <summary>
        /// Gets or sets the current rotation angle of the chair (in degrees).
        /// </summary>
        public int Angle { get; set; }
        
        /// <summary>
        /// Gets or sets the target cockpit angle limit. The value should be within the range of 60-140 degrees.
        /// </summary>
        public int TargetCockpit { get; set; }
        
        /// <summary>
        /// Gets or sets the maximum rotation power of the chair. The value should be between 30 and 100.
        /// </summary>
        public int MaxPower { get; set; }
    }
}