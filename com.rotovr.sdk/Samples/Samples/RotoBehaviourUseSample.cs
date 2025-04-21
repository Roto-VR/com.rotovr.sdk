#if !NO_UNITY

using UnityEngine;
using UnityEngine.UI;

namespace com.rotovr.sdk.sample
{
    
    /// <summary>
    /// Script that demonstrates how to use the RotoBehaviour component.
    /// It provides a simple UI to interact with a Roto char, allowing users to:
    ///  * Connect to the Roto system
    ///  * Switch between different modes (Idle, Free Rotate, Follow Object)
    ///  * Trigger a rotation action
    /// </summary>
    public class RotoBehaviourUseSample : MonoBehaviour
    {

        [Header("Buttons")][SerializeField] Button m_ConnectButton;
        [SerializeField] Button m_RotateButton;
        [SerializeField] Button m_FollowButton;

        [Header("Roto")][SerializeField] RotoBehaviour m_Roto;

        ModeType m_CurrentMode;

        void Start()
        {
            m_Roto.OnDataChanged += OnDataChanged;
            m_Roto.OnModeChanged += OnModeChanged;
            m_Roto.OnConnectionStatusChanged += OnConnectionStatusChanged;

            m_ConnectButton.onClick.AddListener(() =>
            {
                // Connect and set mode to idle so char would not move.
                m_Roto.Connect();
                m_Roto.SwitchMode(ModeType.IdleMode);
            });

            m_RotateButton.onClick.AddListener(() =>
            {
                if (m_CurrentMode != ModeType.FreeMode)
                {
                    m_Roto.SwitchMode(ModeType.FreeMode);
                }

                m_Roto.Rotate(Direction.Right, 90, 1);
            });

            m_FollowButton.onClick.AddListener(() =>
            {
                if (m_CurrentMode != ModeType.FollowObject)
                {
                    m_Roto.SwitchMode(ModeType.FollowObject);
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
    }
}
#endif