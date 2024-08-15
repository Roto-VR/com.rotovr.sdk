using System;
using System.Collections.Generic;
using com.rotovr.sdk;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Example.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] RotoBehaviour m_RotoBerhaviour;
        [SerializeField] ConnectionBlock m_ConnectionBlock;
        [SerializeField] RotoVrBlock m_RotoVrBlock;
        [SerializeField] ModeBlock m_ModeBlock;
        float m_StartTargetAngle;
        int m_StartRotoAngle;
        RotoDataModel m_CachedModel;

        void Awake()
        {
            m_ConnectionBlock.ConnectionButton.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.Connect();
                m_ConnectionBlock.ConnectionButton.gameObject.SetActive(false);
                m_ConnectionBlock.Connecting.SetActive(true);
            });

            m_RotoVrBlock.RotatePower.onValueChanged.AddListener((value) => { m_RotoBerhaviour.SetPower((int)(value * 100)); });

            m_RotoVrBlock.PlayRumble.onClick.AddListener(() => { m_RotoBerhaviour.Rumble((m_RotoVrBlock.RumbleDuration.value * 10), (int)(m_RotoVrBlock.RumblePower.value * 100)); });

            m_RotoVrBlock.RumbleDurationView.text = $"Duration {RoundFloat(m_RotoVrBlock.RumbleDuration.value * 10f)} seconds";

            m_RotoVrBlock.RumbleDuration.onValueChanged.AddListener((val) => { m_RotoVrBlock.RumbleDurationView.text = $"Duration {RoundFloat(val * 10f)} seconds"; });

            m_RotoVrBlock.RumblePowerView.text = $"Power {RoundFloat(m_RotoVrBlock.RumblePower.value * 100f)} %";

            m_RotoVrBlock.RumblePower.onValueChanged.AddListener((val) => { m_RotoVrBlock.RumblePowerView.text = $"Power {RoundFloat(val * 100f)} %"; });

            m_RotoVrBlock.RotatePowerView.text = $"Power {RoundFloat(m_RotoVrBlock.RotatePower.value * 100f)} %";

            m_RotoVrBlock.RotatePower.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RotatePowerView.text = $"Power {RoundFloat(val * 100f)} %";
                SetPower();
            });

            m_RotoVrBlock.RotateAngleView.text = $"Angle {-90 + RoundFloat(m_RotoVrBlock.RotateAngle.value * 180f)} ";

            m_RotoVrBlock.RotateAngle.onValueChanged.AddListener((val) =>
            {
                int angle = (int)(-180 + RoundFloat(val * 360f));
                m_RotoVrBlock.RotateAngleView.text = $"Angle {angle} ";
                HandleRotation(angle);
            });

            SetModeButtonsColor(ModeType.FreeMode);

            m_ModeBlock.Free.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.SwitchMode(ModeType.FreeMode);
                SetModeButtonsColor(ModeType.FreeMode);
            });
            m_ModeBlock.HeadTracking.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.SwitchMode(ModeType.HeadTrack, new ModeParams()
                {
                    CockpitAngleLimit = 0,
                    MaxPower = (int)(m_RotoVrBlock.RotatePower.value * 100),
                    MovementMode = m_ModeBlock.SimulationSelector.isOn ? MovementMode.Jerky : MovementMode.Smooth
                });
                SetModeButtonsColor(ModeType.HeadTrack);
            });
            m_ModeBlock.Follow.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.SwitchMode(ModeType.FollowObject,
                    new ModeParams()
                    {
                        CockpitAngleLimit = 0,
                        MaxPower = (int)(m_RotoVrBlock.RotatePower.value * 100),
                        MovementMode = m_ModeBlock.SimulationSelector.isOn ? MovementMode.Jerky : MovementMode.Smooth
                    });
                SetModeButtonsColor(ModeType.FollowObject);
            });
            m_ModeBlock.Cockpit.onClick.AddListener(() =>
            {
                m_RotoBerhaviour.SwitchMode(ModeType.CockpitMode);
                SetModeButtonsColor(ModeType.CockpitMode);
            });

            m_ModeBlock.SimulationSelector.onValueChanged.AddListener((select) =>
            {
                var colors = m_ModeBlock.SimulationSelector.colors;
                colors.normalColor = select ? m_ModeBlock.SelectedSimulation : m_ModeBlock.DeSelectedSimulation;
                m_ModeBlock.SimulationSelector.colors = colors;
                m_ModeBlock.SimulationNameField.text = select ? "Jerky" : "Smooth";
            });

            m_RotoBerhaviour.OnConnectionStatusChanged += OnConnectionHandler;
            m_RotoBerhaviour.OnDataChanged += OnDataChangedHandler;

            SetUIState(UIState.Connection);
        }


        void SetModeButtonsColor(ModeType modeType)
        {
            m_ModeBlock.Free.targetGraphic.color = m_ModeBlock.DeSelectedMode;
            m_ModeBlock.HeadTracking.targetGraphic.color = m_ModeBlock.DeSelectedMode;
            m_ModeBlock.Follow.targetGraphic.color = m_ModeBlock.DeSelectedMode;
            m_ModeBlock.Cockpit.targetGraphic.color = m_ModeBlock.DeSelectedMode;

            switch (modeType)
            {
                case ModeType.FreeMode:
                    m_ModeBlock.Free.targetGraphic.color = m_ModeBlock.SelectedMode;
                    break;
                case ModeType.HeadTrack:
                    m_ModeBlock.HeadTracking.targetGraphic.color = m_ModeBlock.SelectedMode;
                    break;
                case ModeType.FollowObject:
                    m_ModeBlock.Follow.targetGraphic.color = m_ModeBlock.SelectedMode;
                    break;
                case ModeType.CockpitMode:
                    m_ModeBlock.Cockpit.targetGraphic.color = m_ModeBlock.SelectedMode;
                    break;
            }
        }

        void HandleRotation(int angle)
        {
            if (angle < 0)
                angle += 360;

            if (m_CachedModel != null)
            {
                Vector3 targetEngle = Vector3.zero;
                switch (m_CachedModel.ModeType)
                {
                    case ModeType.FreeMode:
                    case ModeType.CockpitMode:
                        m_RotoBerhaviour.RotateToAngle(GetDirection(angle, m_CachedModel.Angle), angle, (int)(m_RotoVrBlock.RotatePower.value * 100));
                        break;
                    case ModeType.HeadTrack:
                    case ModeType.FollowObject:
                        targetEngle = m_RotoVrBlock.Target.transform.eulerAngles;
                        targetEngle.y = angle;
                        m_RotoVrBlock.Target.transform.eulerAngles = targetEngle;
                        break;
                }
            }
        }

        void SetPower()
        {
            m_RotoBerhaviour.SetPower((int)(m_RotoVrBlock.RotatePower.value * 100));
        }

        private void OnDestroy()
        {
            m_RotoBerhaviour.Disconnect();
            m_RotoBerhaviour.OnConnectionStatusChanged -= OnConnectionHandler;
            m_RotoBerhaviour.OnDataChanged -= OnDataChangedHandler;
        }

        private int m_Count = 0;
        private List<string> m_Lines = new();

        private void OnDataChangeRawDataHandler(string message)
        {
            string text = m_RotoVrBlock.ConsoleView.text;
            m_Lines.Add($"\n {message} ");
            text += $"\n {message} ";

            m_Count += 1;

            if (m_Count >= 100)
            {
                text = String.Empty;

                List<string> newLines = new();

                for (int i = m_Lines.Count - 50; i < m_Lines.Count; i++)
                {
                    text += m_Lines[i];
                    newLines.Add(m_Lines[i]);
                }

                m_Lines.Clear();
                m_Lines = newLines;
                m_Count = 0;
            }

            m_RotoVrBlock.ConsoleView.text = text;
        }

        float RoundFloat(float val)
        {
            return (float)Math.Round((decimal)val, 1);
        }

        private void OnConnectionHandler(ConnectionStatus status)
        {
            if (status == ConnectionStatus.Connected)
            {
                UsbConnector.Instance.OnDataChangeRawData += OnDataChangeRawDataHandler;
                Calibration(CalibrationMode.SetToZero);
                m_RotoVrBlock.ConsoleView.text = String.Empty;
            }
            else if (status == ConnectionStatus.Disconnected)
            {
                UsbConnector.Instance.OnDataChangeRawData -= OnDataChangeRawDataHandler;
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
                    m_ConnectionBlock.ConnectionPanel.SetActive(true);
                    m_ConnectionBlock.Connecting.SetActive(false);
                    m_RotoVrBlock.RotoVrPanel.SetActive(false);
                    break;
                case UIState.Roto:
                    m_ConnectionBlock.ConnectionPanel.SetActive(false);
                    m_RotoVrBlock.RotoVrPanel.SetActive(true);

                    break;
            }
        }


        private void OnDataChangedHandler(RotoDataModel model)
        {
            m_RotoVrBlock.RotoDataView.text = $"Mode: {model.ModeType}  Current angle: {model.Angle}";
            m_CachedModel = model;
        }

        Direction GetDirection(int targetAngle, int sourceAngle)
        {
            if (targetAngle > sourceAngle)
            {
                if (Mathf.Abs(targetAngle - sourceAngle) > 180)
                {
                    return Direction.Left;
                }
                else
                {
                    return Direction.Right;
                }
            }
            else
            {
                if (Mathf.Abs(targetAngle - sourceAngle) > 180)
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Left;
                }
            }
        }

        public enum UIState
        {
            Connection,
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
        public class RotoVrBlock
        {
            public GameObject RotoVrPanel;
            public GameObject Target;
            public Slider RotatePower;
            public TMP_Text RotatePowerView;
            public Slider RotateAngle;
            public TMP_Text RotateAngleView;
            public Button PlayRumble;
            public Slider RumbleDuration;
            public TMP_Text RumbleDurationView;
            public Slider RumblePower;
            public TMP_Text RumblePowerView;
            public TMP_Text RotoDataView;
            public TMP_InputField ConsoleView;
            public ScrollRect ScrollRect;
        }

        [Serializable]
        public class ModeBlock
        {
            public Button Free;
            public Button HeadTracking;
            public Button Follow;
            public Button Cockpit;
            public Toggle SimulationSelector;
            public TMP_Text SimulationNameField;
            public Color SelectedSimulation;
            public Color DeSelectedSimulation;

            public Color SelectedMode;
            public Color DeSelectedMode;
        }
    }
}