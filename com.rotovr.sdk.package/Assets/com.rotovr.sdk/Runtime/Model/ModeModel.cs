using System;

namespace RotoVR.SDK.Model
{
    [Serializable]
    public class ModeModel
    {
        public ModeModel(string mode, ModeParametersModel parametersModel)
        {
            Mode = mode;
            ModeParametersModel = parametersModel;
        }

        public string Mode { get; set; }
        public ModeParametersModel ModeParametersModel { get; set; }
    }
}