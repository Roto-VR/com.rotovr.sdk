using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using RotoVR.SDK.Components;
using RotoVR.SDK.Enum;
using RotoVR.SDK.Model;
using TMPro;
using SimulationMode = UnityEngine.SimulationMode;

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
        string m_Log;

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

                        m_RotoVrBlock.MovementBlock.SetActive(true);
                        m_RotoVrBlock.SensitivityPanel.SetActive(false);
                        m_ModeBlock.SimulationModePanel.SetActive(false);
                        break;

                    case 1:
                        //Headtrack

                        m_RotoVrBlock.MovementBlock.SetActive(false);
                        m_RotoVrBlock.SensitivityPanel.SetActive(true);
                        m_ModeBlock.SimulationModePanel.SetActive(true);


                        break;
                    case 2:
                        //CockpitMode

                        m_RotoVrBlock.MovementBlock.SetActive(true);
                        m_RotoVrBlock.SensitivityPanel.SetActive(false);
                        m_ModeBlock.SimulationModePanel.SetActive(false);
                        break;
                }
            });

            m_ModeBlock.ApplyButton.onClick.AddListener(() =>
            {
                switch (m_ModeBlock.ModeSelector.value)
                {
                    case 0:
                        m_RotoBerhaviour.SwitchMode(ModeType.FreeMode);
                        StopTelemetry();
                        break;
                    case 1:

                        MovementMode mode =
                            (MovementMode)m_ModeBlock.SimulationModeSelector.value;

                        m_RotoBerhaviour.SwitchMode(ModeType.HeadTrack,
                            new ModeParametersModel(0, 100, mode.ToString()));
                        StartTelemetry();
                        break;
                    case 2:
                        m_RotoBerhaviour.SwitchMode(ModeType.CockpitMode);
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
                m_Log = String.Empty;
                m_TelemetryRoutine = StartCoroutine(ViewTelemetry());
            }
        }

        void StopTelemetry()
        {
            if (m_TelemetryRoutine != null)
            {
                //  WriteToFile(m_Log);
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
            m_StartTargetAngle = NormalizeAngle(m_RotoBerhaviour.Target.eulerAngles.y);
            m_StartRotoAngle = m_CachedDataModel.Angle;

            string message = String.Empty;
            Direction direction = Direction.Left;
            float angle = 0;
            float deltaTargetAngle = 0;
            float deltaRotoAngle = 0;
            float lastTargetAngle = m_StartTargetAngle;
            while (true)
            {
                yield return null;

                float currentTargetAngle = NormalizeAngle(m_RotoBerhaviour.Target.eulerAngles.y);
                float currentRotoAngle = m_CachedDataModel.Angle;

                direction = GetDirection((int)currentTargetAngle, (int)lastTargetAngle);

                deltaTargetAngle = GetDelta(m_StartTargetAngle, currentTargetAngle, direction);
                deltaRotoAngle = GetDelta(m_StartRotoAngle, currentRotoAngle, direction);

                angle = deltaTargetAngle - deltaRotoAngle;

                message += $"Chair angle: {m_CachedDataModel.Angle}{Environment.NewLine}";

                message += $"Headset angle: {currentTargetAngle}{Environment.NewLine}";

                angle = NormalizeAngle(angle);

                angle += m_CachedDataModel.Angle;
                var normalizeAngle = (int)NormalizeAngle(angle);

                message += $"Target angle: {normalizeAngle}";

                SetChairAngle(message);
                message = String.Empty;

                lastTargetAngle = currentTargetAngle;
            }
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

        float GetDelta(float startAngle, float currentAngle, Direction direction)
        {
            float delta = 0;

            switch (direction)
            {
                case Direction.Left:
                    if (currentAngle < startAngle)
                        delta = currentAngle - startAngle;
                    else
                    {
                        delta = currentAngle - startAngle - 360;
                    }

                    break;
                case Direction.Right:
                    if (currentAngle > startAngle)
                        delta = currentAngle - startAngle;
                    else
                    {
                        delta = currentAngle - startAngle + 360;
                    }

                    break;
            }

            return delta;
        }

        public void WriteToFile(string message)
        {
            var path = Path.Combine(Application.persistentDataPath, "RotoVRLogs");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            path += $"/{DateTime.Now.ToLongTimeString()}.log";

            try
            {
                StreamWriter fileWriter = new StreamWriter(path, true);

                fileWriter.Write(message);
                fileWriter.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("[Utils Script]: Cannot write in the GPS File Log - Exception: " + e);
            }
        }

        float NormalizeDirection(float angle)
        {
            if (angle > 180)
            {
                angle -= 360;
            }
            else if (angle < -180)
                angle += 360;

            return angle;
        }

        float NormalizeAngle(float angle)
        {
            if (angle < 0)
                angle += 360;
            else if (angle > 360)
                angle -= 360;

            return angle;
        }

        void SetChairAngle(string message)
        {
            m_RotoVrBlock.RotoAngleView.text = message;
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
        }

        [Serializable]
        public class ModeBlock
        {
            public GameObject ModePanel;
            public TMP_Dropdown ModeSelector;
            public GameObject SimulationModePanel;
            public TMP_Dropdown SimulationModeSelector;
            public Button ApplyButton;
        }
    }
}