#if !NO_UNITY

using UnityEngine;
using UnityEngine.UI;

namespace com.rotovr.sdk.sample
{
    /// <summary>
    /// Script that demonstrates how to use the Roto component.
    /// It provides a simple UI to interact with a Roto char, allowing users to:
    ///  * Connect to the Roto system
    ///  * Switch between different modes (Idle, Free Rotate, Follow Object)
    ///  * Trigger a rotation action
    /// </summary>
    /// 
    public class RotoAPIUseSample : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button m_ConnectButton;
        [SerializeField] Button m_RotateButton;
        [SerializeField] Button m_FollowButton;
        
        [Header("Tracking")]
        [SerializeField] Transform m_FollowTarget;
        
        Roto m_Roto;
        ModeType m_CurrentMode;

       
        void Start()
        {
            m_Roto = Roto.GetManager();
            m_Roto.OnConnectionStatus += OnConnectionStatusChanged;
            m_Roto.OnRotoMode += OnModeChanged;
            m_Roto.OnDataChanged += OnDataChanged;
            m_Roto.Initialize(ConnectionType.Simulation);
            
            m_ConnectButton.onClick.AddListener(() =>
            {
                // Connect and set mode to idle so char would not move.
                m_Roto.Connect("rotoVR Base Station");
                SwitchMode(ModeType.IdleMode);
            });

            m_RotateButton.onClick.AddListener(() =>
            {
                if (m_CurrentMode != ModeType.FreeMode)
                {
                    SwitchMode(ModeType.FreeMode);
                }

                m_Roto.Rotate(Direction.Right, 90, 1);
            });

            m_FollowButton.onClick.AddListener(() =>
            {
                if (m_CurrentMode != ModeType.FollowObject)
                {
                    SwitchMode(ModeType.FollowObject);
                }
            });
        }
        
        void OnConnectionStatusChanged(ConnectionStatus connectionStatus)
        {
            Debug.Log($"{nameof(RotoBehaviourUseSample)}::{nameof(OnConnectionStatusChanged)} -> {connectionStatus}");
        }

        void OnModeChanged(ModeType modeType)
        {
            m_CurrentMode = modeType;
            Debug.Log($"{nameof(RotoBehaviourUseSample)}::{nameof(OnModeChanged)} -> {modeType}");
        }

        void OnDataChanged(RotoDataModel data)
        {
            Debug.Log($"{nameof(RotoBehaviourUseSample)}::{nameof(OnDataChanged)} -> {data.ToJson()}");
        }
        
        
        void SwitchMode(ModeType mode)
        {

            m_Roto.StopRoutine(this);
            switch (mode)
            {
                case ModeType.FreeMode:
                    m_Roto.SetMode(mode, new ModeParams { CockpitAngleLimit = 0, MaxPower = 30 });
                    OnModeChanged(mode);
                    break;
                case ModeType.CockpitMode:
                    m_Roto.SetMode(mode, new ModeParams { CockpitAngleLimit = 140, MaxPower = 30 });
                    break;

                case ModeType.HeadTrack:
                    m_Roto.SetMode(mode, new ModeParams {CockpitAngleLimit = 0, MaxPower = 30});
                    m_Roto.StartHeadTracking(this, m_FollowTarget);
                    break;
              
                case ModeType.FollowObject:
                    m_Roto.SetMode(mode, new ModeParams {CockpitAngleLimit = 0, MaxPower = 100});
                    m_Roto.FollowTarget(this, m_FollowTarget);
                    OnModeChanged(mode);
                    break;
            }
        }

    }
}
#endif