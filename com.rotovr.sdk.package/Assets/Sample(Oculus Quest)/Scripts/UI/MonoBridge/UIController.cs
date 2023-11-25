using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RotoVR.SDK.Components;
using RotoVR.SDK.Enum;
using RotoVR.SDK.Model;
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
        Coroutine m_TelemetryRoutine;
        RotoDataModel m_CachedDataModel = new();
        float m_StartTargetAngle;
        int m_StartRotoAngle;

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
                m_RotoBerhaviour.Rumble((m_RotoVrBlock.RumbleDuration.value * 10),
                    (int)(m_RotoVrBlock.RumblePower.value * 100));
            });

            m_RotoVrBlock.RumbleDurationView.text =
                $"Duration {RoundFloat(m_RotoVrBlock.RumbleDuration.value * 10f)} seconds";

            m_RotoVrBlock.RumbleDuration.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RumbleDurationView.text = $"Duration {RoundFloat(val * 10f)} seconds";
            });

            m_RotoVrBlock.RumblePowerView.text = $"Power {RoundFloat(m_RotoVrBlock.RumblePower.value * 100f)} %";

            m_RotoVrBlock.RumblePower.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RumblePowerView.text = $"Power {RoundFloat(val * 100f)} %";
            });

            m_RotoVrBlock.RotatePowerView.text = $"Power {RoundFloat(m_RotoVrBlock.RotatePower.value * 100f)} %";

            m_RotoVrBlock.RotatePower.onValueChanged.AddListener((val) =>
            {
                m_RotoVrBlock.RotatePowerView.text = $"Power {RoundFloat(val * 100f)} %";
            });

            m_ModeBlock.ModeSelector.onValueChanged.AddListener((val) =>
            {
                switch (val)
                {
                    case 0:
                        //FreeMode
                        m_RotoBerhaviour.SwitchMode(ModeType.FreeMode);
                        m_RotoVrBlock.MovementBlock.SetActive(true);
                        m_RotoVrBlock.SensitivityPanel.SetActive(false);
                        StopTelemetry();

                        break;

                    case 1:
                        //Custom Headtrack
                        m_RotoBerhaviour.SwitchMode(ModeType.HeadTrack);
                        m_RotoVrBlock.MovementBlock.SetActive(false);
                        m_RotoVrBlock.SensitivityPanel.SetActive(true);
                        StartTelemetry();

                        break;
                    case 2:
                        //CockpitMode
                        m_RotoBerhaviour.SwitchMode(ModeType.CockpitMode);
                        m_RotoVrBlock.MovementBlock.SetActive(true);
                        m_RotoVrBlock.SensitivityPanel.SetActive(false);
                        StopTelemetry();

                        break;
                }
            });


            m_RotoBerhaviour.OnConnectionStatusChanged += OnConnectionHandler;
            m_RotoBerhaviour.OnModeChanged += OnModeChangedHandler;
            m_RotoBerhaviour.OnDataChanged += OnDataChangedHandler;
            SetUIState(UIState.Connection);
        }

        private void OnDestroy()
        {
            m_RotoBerhaviour.OnConnectionStatusChanged -= OnConnectionHandler;
            m_RotoBerhaviour.OnModeChanged -= OnModeChangedHandler;
            m_RotoBerhaviour.OnDataChanged -= OnDataChangedHandler;
        }

        float RoundFloat(float val)
        {
            return (float)Math.Round((decimal)val, 1);
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

        void StartTelemetry()
        {
            if (m_TelemetryRoutine == null)
            {
                m_TelemetryRoutine = StartCoroutine(ViewTelemetry());
            }
        }

        void StopTelemetry()
        {
            if (m_TelemetryRoutine != null)
            {
                StopCoroutine(m_TelemetryRoutine);
                m_TelemetryRoutine = null;
            }
        }

        private void OnDataChangedHandler(RotoDataModel model)
        {
            m_CachedDataModel = model;
        }

        IEnumerator ViewTelemetry()
        {
            m_StartTargetAngle = m_RotoBerhaviour.Target.eulerAngles.y;
            m_StartRotoAngle = m_CachedDataModel.Angle;

            while (true)
            {
                yield return null;

                SetChairAngle(m_CachedDataModel.Angle);
                SetHeadsetAbsoluteAngle(NormalizeAngle(m_RotoBerhaviour.Target.eulerAngles.y));

                float currentTargetAngle = NormalizeAngle(m_RotoBerhaviour.Target.eulerAngles.y);
                float deltaTargetAngle = currentTargetAngle - m_StartTargetAngle;

                float currentRotoAngle = m_CachedDataModel.Angle;
                float deltaRotoAngle = currentRotoAngle - m_StartRotoAngle;

                float angle = deltaTargetAngle - deltaRotoAngle;

                angle = NormalizeAngle(angle);
                angle += m_StartRotoAngle;
                SetTargetAngle((int)NormalizeAngle(angle));
            }
        }

        float NormalizeAngle(float angle)
        {
            if (angle < 0)
                angle += 360;
            else if (angle > 360)
                angle -= 360;

            return angle;
        }

        void SetChairAngle(int angle)
        {
            m_RotoVrBlock.RotoAngleView.text = $"Chair angle: {angle}";
        }

        void SetHeadsetAbsoluteAngle(float angle)
        {
            m_RotoVrBlock.HeadsetAbsoluteAngleView.text = $"Headset absolute angle: {angle}";
        }

        void SetTargetAngle(int angle)
        {
            m_RotoVrBlock.TargetAngleView.text = $"Target angle: {angle}";
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
            public Slider RotatePower;
            public TMP_Text RotatePowerView;
            public Button TurnLeft;
            public Button TurnRight;
            public Button PlayRumble;
            public Slider RumbleDuration;
            public TMP_Text RumbleDurationView;
            public Slider RumblePower;
            public TMP_Text RumblePowerView;
            public GameObject SensitivityPanel;
            public TMP_Text RotoAngleView;
            public TMP_Text HeadsetAbsoluteAngleView;
            public TMP_Text TargetAngleView;
        }

        [Serializable]
        public class ModeBlock
        {
            public GameObject ModePanel;
            public TMP_Dropdown ModeSelector;
        }
    }
}