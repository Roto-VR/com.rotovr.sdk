using System;
using System.Collections.Generic;

namespace com.rotovr.sdk
{
    [Serializable]
    class RotateToAngleModel
    {
        public RotateToAngleModel(int angle, int power, string direction)
        {
            Angle = angle;
            Power = power;
            Direction = direction;
        }

        public RotateToAngleModel(string json)
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            
            Angle = Convert.ToInt32(dict["Angle"]);
            Power = Convert.ToInt32(dict["Power"]);
            Direction = dict["Direction"].ToString();
        }
        
        public int Angle { get; }
        public int Power { get; }
        public string Direction { get; }

        public string ToJson()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("Angle", Angle);
            dict.Add("Power", Power);
            dict.Add("Direction", Direction);
            
            return Json.Serialize(dict);

        }
    }
}
