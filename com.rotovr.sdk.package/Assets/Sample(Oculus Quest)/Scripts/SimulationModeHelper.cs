using RotoVR.SDK.Components;
using RotoVR.SDK.Enum;
using UnityEngine;

namespace Sample_Oculus_Quest_.Scripts
{
    public class SimulationModeHelper : MonoBehaviour
    {
        [SerializeField] RotoBehaviour m_RotoBehaviour;
        [SerializeField] AutoRotation m_Component;

        private void Awake()
        {
            m_Component.enabled = false;
            m_RotoBehaviour.OnModeChanged += OnModeHandle;
        }

        private void OnModeHandle(ModeType mode)
        {
            switch (mode)
            {
                case ModeType.SimulationMode:
                    m_Component.StartRotation();
                    break;
                default:
                    m_Component.StopRotation();
                    break;
            }
        }
    }
}