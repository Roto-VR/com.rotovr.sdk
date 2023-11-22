using System;
using UnityEngine;
using UnityEngine.UI;
using RotoVR.SDK.Components;
using RotoVR.SDK.Enum;
using TMPro;

namespace Example.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] RotoBehaviour m_RotoBerhaviour;
        [SerializeField] ConnectionBlock m_ConnectionBlock;
        [SerializeField] CalibrationBlock m_CalibrationBlock;
        [SerializeField] RotoVrBlock m_RotoVrBlock;
        [SerializeField] ModeBlock m_ModeBlock;

        void Awake()
        {
            m_ConnectionBlock.ConnectionButton.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.Connect();
                m_ConnectionBlock.ConnectionButton.gameObject.SetActive(false);
                m_ConnectionBlock.Connecting.SetActive(true);
            });

            m_CalibrationBlock.CalibrationAsCurrentButton.onClick.AddListener(() =>
            {
                Calibration(CalibrationMode.SetCurrent);
            });
            m_CalibrationBlock.CalibrationAsPrevButton.onClick.AddListener(() =>
            {
                Calibration(CalibrationMode.SetLast);
            });
            m_CalibrationBlock.CalibrationAsZeroButton.onClick.AddListener(() =>
            {
                Calibration(CalibrationMode.SetToZero);
            });

            m_RotoVrBlock.TurnLeft.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.RotateOnAngle(Direction.Left, 30, (int)(m_RotoVrBlock.RotatePower.value * 100));
            });

            m_RotoVrBlock.TurnRight.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.RotateOnAngle(Direction.Right, 30, (int)(m_RotoVrBlock.RotatePower.value * 100));
            });
            m_RotoVrBlock.PlayRumble.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.Rumble((int)(m_RotoVrBlock.RumbleDuration.value * 10),
                    (int)(m_RotoVrBlock.RumblePower.value * 100));
            });

            m_RotoVrBlock.RumbleDurationView.text = $"Duration {m_RotoVrBlock.RumbleDuration.value * 10f} seconds";

            m_RotoVrBlock.RumbleDuration.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RumbleDurationView.text = $"Duration {val * 10f} seconds";
            });

            m_RotoVrBlock.RumblePowerView.text = $"Power {m_RotoVrBlock.RumblePower.value * 100f} %";

            m_RotoVrBlock.RumblePower.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RumblePowerView.text = $"Power {val * 100f} %";
            });

            m_RotoVrBlock.RotatePowerView.text = $"Power {m_RotoVrBlock.RotatePower.value * 100f} %";

            m_RotoVrBlock.RotatePower.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RotatePowerView.text = $"Power {val * 100f} %";
            });

            m_RotoVrBlock.RotationModeToggle.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RotationBlock.SetActive(val);
                m_RotoVrBlock.ApplyBlock.SetActive(!val);
            });

            m_RotoVrBlock.RotationBlock.SetActive(m_RotoVrBlock.RotationModeToggle.isOn);
            m_RotoVrBlock.ApplyBlock.SetActive(!m_RotoVrBlock.RotationModeToggle.isOn);


            m_RotoVrBlock.KeyBoardButton.onClick.AddListener(() => { m_RotoVrBlock.Keyboard.SetActive(true); });

            KeyboardButton.OnClick += SetValue;

            void SetValue(string val)
            {
                if (val == "Clear")
                {
                    m_RotoVrBlock.KeyboardView.text = "0";
                    return;
                }

                var old = m_RotoVrBlock.KeyboardView.text;

                var newText = old;
                
                if (newText == "0")
                    newText = val;
                else
                    newText += val;

                int angle = int.Parse(newText);
                if (angle > 360)
                {
                    m_RotoVrBlock.KeyboardView.text = old;
                }
                else
                    m_RotoVrBlock.KeyboardView.text = newText;
            }

            m_RotoVrBlock.ApplyAngle.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.RotateToAngleByCloserDirection(int.Parse(m_RotoVrBlock.KeyboardView.text),
                    (int)(m_RotoVrBlock.RotatePower.value * 100));
            });

            m_ModeBlock.ModeSelector.onValueChanged.AddListener((val) =>
            {
                switch (val)
                {
                    case 0:
                        //FreeMode
                        m_RotoBerhaviour.SwitchMode(ModeType.FreeMode);
                        m_RotoVrBlock.MovementBlock.SetActive(true);
                        break;
                    case 1:
                        //Custom Headtrack
                        m_RotoBerhaviour.SwitchMode(ModeType.CustomHeadTrack);
                        m_RotoVrBlock.MovementBlock.SetActive(false);
                        break;
                    case 2:
                        //Custom Headtrack
                        m_RotoBerhaviour.SwitchMode(ModeType.CustomHeadTrack);
                        m_RotoVrBlock.MovementBlock.SetActive(false);
                        break;
                    case 3:
                        //CockpitMode
                        m_RotoBerhaviour.SwitchMode(ModeType.HeadTrack);
                        m_RotoVrBlock.MovementBlock.SetActive(true);
                        break;
                }
            });


            m_RotoBerhaviour.OnConnectionStatusChanged += OnConnectionHandler;
            m_RotoBerhaviour.OnModeChanged += OnModeChangedHandler;

            SetUIState(UIState.Connection);
        }

        private void OnModeChangedHandler(ModeType modeType)
        {
            switch (modeType)
            {
                case ModeType.FreeMode:
                case ModeType.CockpitMode:

                    break;
                case ModeType.HeadTrack:

                    break;
            }
        }

        private void OnConnectionHandler(ConnectionStatus status)
        {
            if (status == ConnectionStatus.Connected)
            {
                SetUIState(UIState.Calibration);
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
                    m_ConnectionBlock.ConnectionPanel.SetActive(true);
                    m_CalibrationBlock.CalibrationPanel.SetActive(false);
                    m_RotoVrBlock.RotoVrPanel.SetActive(false);
                    m_ModeBlock.ModePanel.SetActive(false);
                    break;
                case UIState.Calibration:
                    m_ConnectionBlock.ConnectionPanel.SetActive(false);
                    m_CalibrationBlock.CalibrationPanel.SetActive(true);
                    m_RotoVrBlock.RotoVrPanel.SetActive(false);
                    m_ModeBlock.ModePanel.SetActive(false);
                    break;
                case UIState.Roto:
                    m_CalibrationBlock.CalibrationPanel.SetActive(false);
                    m_RotoVrBlock.RotoVrPanel.SetActive(true);
                    m_RotoVrBlock.MovementBlock.SetActive(true);
                    m_ModeBlock.ModePanel.SetActive(true);
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
        }

        [Serializable]
        public class RotoVrBlock
        {
            public GameObject RotoVrPanel;
            public GameObject MovementBlock;
            public Toggle RotationModeToggle;
            public GameObject ApplyBlock;
            public GameObject RotationBlock;
            public Slider RotatePower;
            public TMP_Text RotatePowerView;
            public Button KeyBoardButton;
            public GameObject Keyboard;
            public TMP_Text KeyboardView;
            public Button ApplyAngle;
            public Button TurnLeft;
            public Button TurnRight;
            public Button PlayRumble;
            public Slider RumbleDuration;
            public TMP_Text RumbleDurationView;
            public Slider RumblePower;
            public TMP_Text RumblePowerView;
        }

        [Serializable]
        public class ModeBlock
        {
            public GameObject ModePanel;
            public TMP_Dropdown ModeSelector;
        }
    }
}