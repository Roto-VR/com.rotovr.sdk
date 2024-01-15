using System;

namespace RotoVR.SDK.Model
{
    [Serializable]
    public class ModeModel
    {
        public ModeModel()
        {
        }

        public ModeModel(string mode, ModeParametersModel parametersModel)
        {
            Mode = mode;
            ModeParametersModel = parametersModel;
        }

        public string Mode { get; set; }
        public ModeParametersModel ModeParametersModel { get; set; }
    }

    [Serializable]
    public class ModeParametersModel
    {
        public ModeParametersModel(int targetCockpit, int maxPower)
        {
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
        }

        public ModeParametersModel(int targetCockpit, int maxPower, string simulationMode)
        {
            TargetCockpit = targetCockpit;
            MaxPower = maxPower;
            SimulationMode = simulationMode;
        }

        public int TargetCockpit { get; set; }
        public int MaxPower { get; set; }
        public string SimulationMode { get; set; }
    }
}