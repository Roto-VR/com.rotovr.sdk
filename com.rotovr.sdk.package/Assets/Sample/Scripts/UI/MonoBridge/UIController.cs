using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace com.rotovr.sdk.sample
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] RotoBehaviourProxy m_RotoBerhaviourProxy;
        [SerializeField] ConnectionBlock m_ConnectionBlock;
        [SerializeField] CalibrationBlock m_CalibrationBlock;
        [SerializeField] RotoVrBlock m_RotoVrBlock;

        RotoBehaviour m_RotoBerhaviour;

        void Awake()
        {
            m_RotoBerhaviour = m_RotoBerhaviourProxy.RotoBehaviour;
            m_ConnectionBlock.ConnectionButton.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.Connect();
                m_ConnectionBlock.ConnectionButton.gameObject.SetActive(false);
                m_ConnectionBlock.Connecting.SetActive(true);
            });

            m_CalibrationBlock.CalibrationAsCurrentButton.onClick.AddListener(() =>
            {
#if !NO_UNITY
                Calibration(CalibrationMode.SetCurrent);
#endif
            });
            m_CalibrationBlock.CalibrationAsPrevButton.onClick.AddListener(() =>
            {
#if !NO_UNITY
                Calibration(CalibrationMode.SetLast);
#endif
            });
            m_CalibrationBlock.CalibrationAsZeroButton.onClick.AddListener(() => { Calibration(CalibrationMode.SetToZero); });

            m_CalibrationBlock.Disconnect.onClick.AddListener(() => { m_RotoBerhaviour.Disconnect(); });

            m_RotoVrBlock.RotationPowerView.text =
                $"Rotation power {RoundFloat(m_RotoVrBlock.RotationPower.value * 100f)} %";


            m_RotoVrBlock.RotationPower.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RotationPowerView.text =
                    $"Rotation power {RoundFloat(val * 100f)} %";
            });

            m_RotoVrBlock.TurnLeft.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.RotateOnAngle(Direction.Left, 20,
                    (int)(m_RotoVrBlock.RotationPower.value * 100));
            });

            m_RotoVrBlock.TurnRight.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.RotateOnAngle(Direction.Right, 20,
                    (int)(m_RotoVrBlock.RotationPower.value * 100));
            });


            m_RotoVrBlock.RumbleDurationView.text =
                $"Rumble duration {RoundFloat(m_RotoVrBlock.RumbleDuration.value * 10f)} seconds";

            m_RotoVrBlock.RumbleDuration.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RumbleDurationView.text =
                    $"Rumble duration {RoundFloat(val * 10f)} seconds";
            });


            m_RotoVrBlock.m_RumblePowerView.text =
                $"Rumble power {RoundFloat(m_RotoVrBlock.RumblePower.value * 100f)} %";


            m_RotoVrBlock.RumblePower.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.m_RumblePowerView.text =
                    $"Rumble power {RoundFloat(val * 100f)} %";
            });


            m_RotoVrBlock.PlayRumble.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.Rumble(m_RotoVrBlock.RumbleDuration.value * 10,
                    (int)(m_RotoVrBlock.RumblePower.value * 100));
            });


            m_RotoBerhaviour.OnConnectionStatusChanged += OnConnectionHandler;
            SetUIState(UIState.Connection);
        }

        float RoundFloat(float val)
        {
            return (float)Math.Round((decimal)val, 1);
        }

        private void OnConnectionHandler(ConnectionStatus status)
        {
            Debug.LogError($"status: {status}");
            if (status == ConnectionStatus.Connected)
            {
                SetUIState(UIState.Calibration);
            }
            else if (status == ConnectionStatus.Disconnected)
            {
                SetUIState(UIState.Connection);
            }
        }

        void Calibration(CalibrationMode mode)
        {
            m_RotoBerhaviour.Calibration(mode);
            SetUIState(UIState.Roto);
        }


        void SetUIState(UIState state)
        {
            switch (state)
            {
                case UIState.Connection:
                    m_ConnectionBlock.ConnectionButton.gameObject.SetActive(true);
                    m_ConnectionBlock.Connecting.SetActive(false);
                    m_ConnectionBlock.ConnectionPanel.SetActive(true);
                    m_CalibrationBlock.CalibrationPanel.SetActive(false);
                    m_RotoVrBlock.RotoVrPanel.SetActive(false);
                    break;
                case UIState.Calibration:
                    m_ConnectionBlock.ConnectionPanel.SetActive(false);
                    m_CalibrationBlock.CalibrationPanel.SetActive(true);
                    m_RotoVrBlock.RotoVrPanel.SetActive(false);
                    break;
                case UIState.Roto:
                    m_CalibrationBlock.CalibrationPanel.SetActive(false);
                    m_RotoVrBlock.RotoVrPanel.SetActive(true);
                    break;
            }
        }

        public enum UIState
        {
            Connection,
            Calibration,
            Roto,
        }

        [Serializable]
        public class ConnectionBlock
        {
            public GameObject ConnectionPanel;
            public Button ConnectionButton;
            public GameObject Connecting;
        }

        [Serializable]
        public class CalibrationBlock
        {
            public GameObject CalibrationPanel;
            public Button CalibrationAsCurrentButton;
            public Button CalibrationAsPrevButton;
            public Button CalibrationAsZeroButton;
            public Button Disconnect;
        }

        [Serializable]
        public class RotoVrBlock
        {
            public GameObject RotoVrPanel;
            public Slider RotationPower;
            public TMP_Text RotationPowerView;
            public Button TurnLeft;
            public Button TurnRight;
            public Button PlayRumble;
            public TMP_Text RumbleDurationView;
            public Slider RumbleDuration;
            public TMP_Text m_RumblePowerView;
            public Slider RumblePower;
        }
    }
}