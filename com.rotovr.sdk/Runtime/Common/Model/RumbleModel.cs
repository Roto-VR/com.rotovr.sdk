using System;
using System.Collections.Generic;

namespace com.rotovr.sdk
{
    [Serializable]
    class RumbleModel
    {
        public RumbleModel(float duration, int power)
        {
            Duration = duration;
            Power = power;
        }
        
        public RumbleModel(string json)
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            
            Duration = Convert.ToSingle(dict["Duration"]);
            Power = Convert.ToInt32(dict["Power"]);
        }

        public float Duration { get; }
        public int Power { get; }
        
        
        public string ToJson()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("Duration", Duration);
            dict.Add("Power", Power);
            
            return Json.Serialize(dict);
        }
    }
}