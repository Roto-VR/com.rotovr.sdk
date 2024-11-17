using System;
using System.Collections.Generic;

namespace com.rotovr.sdk
{
    [Serializable]
    class ModeModel
    {
        public ModeModel(string mode, ModeParametersModel parametersModel)
        {
            Mode = mode;
            ModeParametersModel = parametersModel;
        }

        public ModeModel(string json)
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            
            Mode = dict["Mode"].ToString();
            
            var modeParamsDict = dict["ModeParametersModel"] as Dictionary<string, object>;

            var targetCockpit = Convert.ToInt32(modeParamsDict["TargetCockpit"]);
            var maxPower = Convert.ToInt32(modeParamsDict["MaxPower"]);
            var movementMode = modeParamsDict["MovementMode"].ToString();
            ModeParametersModel = new ModeParametersModel(targetCockpit, maxPower, movementMode);
        }

        public string Mode { get; set; }
        public ModeParametersModel ModeParametersModel { get; set; }

        public string ToJson()
        {
            
            var dict = new Dictionary<string, object>();
            dict.Add("Mode", Mode);
            var modeParametersModel = new Dictionary<string, object>();
            modeParametersModel.Add("TargetCockpit", ModeParametersModel.TargetCockpit);
            modeParametersModel.Add("MaxPower", ModeParametersModel.MaxPower);
            modeParametersModel.Add("MovementMode", ModeParametersModel.MovementMode);
            dict.Add("ModeParametersModel", modeParametersModel);

            return Json.Serialize(dict);
        }
    }
}