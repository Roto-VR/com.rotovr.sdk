using System.Collections;
using RotoVR.SDK.API;
using RotoVR.SDK.Enum;
using UnityEngine;

namespace RotoVR.SDK.Components
{
    public class RotoBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Setup on the component in a scene roto vr device name
        /// </summary>
        [SerializeField] string m_DeviceName = "rotoVR Base Station";

        /// <summary>
        /// Setup on the component in a scene working mode
        /// </summary>
        [SerializeField] ModeType m_ModeType;

        /// <summary>
        /// For Head Tracking Move need to setup a target to observe a rotation
        /// </summary>
        [SerializeField] Transform m_Target;

        RotoManager m_manager;
        Coroutine m_targetRoutine;

        void Awake()
        {
            var behaviour = FindObjectOfType<RotoBehaviour>();
            if (behaviour != null)
                Destroy(behaviour);

            InitRoto();
        }

        void OnDestroy()
        {
            m_manager.Disconnect(m_DeviceName);
            m_manager.OnConnectionStatus -= OnConnectionStatusHandler;
            m_manager.OnRotoMode -= OnRotoModeHandler;
        }

        /// <summary>
        /// Initialisation of the component
        /// </summary>
        void InitRoto()
        {
            m_manager = RotoManager.GetManager();
            m_manager.OnConnectionStatus += OnConnectionStatusHandler;
            m_manager.OnRotoMode += OnRotoModeHandler;
            m_manager.Initialize();
            m_manager.Connect(m_DeviceName);
        }

        private void OnRotoModeHandler(ModeType mode)
        {
            switch (mode)
            {
                case ModeType.FreeMode:
                case ModeType.CockpitMode:
                case ModeType.IdleMode:
                case ModeType.Calibration:
                case ModeType.Error:
                    if (m_targetRoutine != null)
                    {
                        StopCoroutine(m_targetRoutine);
                        m_targetRoutine = null;
                    }

                    break;
                case ModeType.HeadTrack:
                    m_targetRoutine = StartCoroutine(ObserveTarget());
                    break;
            }
        }

        void OnConnectionStatusHandler(ConnectionStatus status)
        {
            switch (status)
            {
                case ConnectionStatus.Connecting:

                    break;
                case ConnectionStatus.Connected:
                    m_manager.SetMode(m_ModeType);
                    break;
                case ConnectionStatus.Disconnected:

                    break;
            }
        }

        IEnumerator ObserveTarget()
        {
            yield return null;
            if (m_Target == null)
                Debug.LogError("For Had Tracking Mode you need to set target transform");
            else
            {
            }
        }
    }
}