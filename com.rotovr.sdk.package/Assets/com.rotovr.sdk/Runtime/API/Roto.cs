using System;
using System.Collections;
using Newtonsoft.Json;
using RotoVR.SDK.BLE;
using RotoVR.SDK.Enum;
using RotoVR.SDK.Message;
using RotoVR.SDK.Model;
using UnityEngine;

namespace RotoVR.SDK.API
{
    public class Roto
    {
        static Roto m_roto;

        public static Roto GetManager()
        {
            if (m_roto == null)
                m_roto = new Roto();

            return m_roto;
        }

        RotoDataModel m_RotoData = new();
        readonly string m_Calibrationkey = "CalibrationKey";
        Transform m_ObservableTarger;
        Coroutine m_TargetRoutine;
        bool m_IsInit = false;
        int m_StartTargetAngle;
        int m_StartRotoAngle;

        /// <summary>
        /// Invoke when change roto vr mode
        /// </summary>
        public event Action<ModeType> OnRotoMode;

        /// <summary>
        /// Invoke when change connection status of roto vr
        /// </summary>
        public event Action<ConnectionStatus> OnConnectionStatus;

        /// <summary>
        /// Invoke to directly call command in java library
        /// </summary>
        /// <param name="command">Method name in java library</param>
        /// <param name="data">Data which we wont to send as Json</param>
        public void Call(string command, string data)
        {
            BleManager.Instance.Call(command, data);
        }

        /// <summary>
        /// Invoke to send BleMessage to java library
        /// </summary>
        /// <param name="message">Ble message</param>
        public void SendMessage(BleMessage message)
        {
            Call(message.MessageType.ToString(), message.Data);
        }

        /// <summary>
        /// Subscribe to ble json message
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="action">Handler</param>
        public void Subscribe(string command, Action<string> action) => BleManager.Instance.Subscribe(command, action);

        /// <summary>
        /// Subscribe from ble json message
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="action">Handler</param>
        public void UnSubscribe(string command, Action<string> action) =>
            BleManager.Instance.UnSubscribe(command, action);

        /// <summary>
        /// Invoke for ble sdk initialization
        /// </summary>
        public void Initialize()
        {
            if (m_IsInit)
                return;

            m_IsInit = true;

            BleManager.Instance.Init();
            Subscribe(MessageType.ModelChanged.ToString(), OnModelChangeHandler);
            Subscribe(MessageType.DeviceConnected.ToString(),
                (data) => { OnConnectionStatus?.Invoke(ConnectionStatus.Connected); });
            Subscribe(MessageType.Disconnected.ToString(),
                (data) => { OnConnectionStatus?.Invoke(ConnectionStatus.Disconnected); });
        }

        void OnModelChangeHandler(string data)
        {
            RotoDataModel model = JsonConvert.DeserializeObject<RotoDataModel>(data);

            if (model.Mode != m_RotoData.Mode)
            {
                if (System.Enum.TryParse(model.Mode, out ModeType value))
                {
                    OnRotoMode.Invoke(value);
                }
            }

            m_RotoData = model;
        }

        /// <summary>
        /// Scan environment to find devices 
        /// </summary>
        public void Scan()
        {
            SendMessage(new ScanMessage());
        }

        /// <summary>
        /// Connect to device
        /// </summary>
        /// <param name="deviceName">Data with device parameters</param>
        public void Connect(string deviceName)
        {
            SendMessage(new ConnectMessage(JsonConvert.SerializeObject(new DeviceDataModel(deviceName, string.Empty))));
        }

        /// <summary>
        /// Disconnect from current device
        /// </summary>
        /// <param name="deviceData">Data with device parameters</param>
        public void Disconnect(string deviceData)
        {
            SendMessage(new DisconnectMessage(deviceData));
        }

        /// <summary>
        /// Set RotoVR mode
        /// </summary>
        /// <param name="mode">Mode type</param>
        public void SetMode(ModeType mode)
        {
            SendMessage(new SetModeMessage(mode.ToString()));
        }

        /// <summary>
        /// Calibrate RotoVR as zero rotation
        /// </summary>
        /// <param name="calibrationMode"></param>
        public void Calibration(CalibrationMode calibrationMode)
        {
            switch (calibrationMode)
            {
                case CalibrationMode.SetCurrent:
                    PlayerPrefs.SetInt(m_Calibrationkey, m_RotoData.Angle);
                    break;
                case CalibrationMode.SetLast:
                    if (PlayerPrefs.HasKey(m_Calibrationkey))
                    {
                        var defaultAngle = PlayerPrefs.GetInt(m_Calibrationkey);
                        RotateToAngle(GetDirection(defaultAngle), defaultAngle, 100);
                    }
                    else
                        RotateToAngle(GetDirection(0), 0, 100);

                    break;
                case CalibrationMode.SetToZero:
                    RotateToAngle(GetDirection(0), 0, 100);
                    break;
            }
        }

        /// <summary>
        /// Rotate RotoVR to angle
        /// </summary>
        /// <param name="angle">The value of angle</param>
        /// <param name="direction">Rotate direction</param>
        /// <param name="power">Rotational power. In range 0-100</param>
        /// <summary>
        /// Turn RotoVR to angle
        /// </summary>
        /// <param name="angle">The value of angle</param>
        /// <param name="direction">Rotate direction</param>
        /// <param name="power">Rotational power. In range 0-100</param>
        public void RotateToAngle(Direction direction, int angle, int power)
        {
            SendMessage(new RotateToAngleMessage(
                JsonConvert.SerializeObject(new RotateToAngleModel(angle, power, direction.ToString()))));
        }

        /// <summary>
        /// Rotate RotoVR to angle, use automaticaly close direction
        /// </summary>
        /// <param name="angle">The value of angle</param>
        /// <param name="power">Rotational power. In range 0-100</param>
        public void RotateToAngleCloserDirection(int angle, int power)
        {
            SendMessage(new RotateToAngleMessage(
                JsonConvert.SerializeObject(new RotateToAngleModel(angle, power,
                    GetDirection(angle).ToString()))));
        }

        /// <summary>
        /// Turn RotoVR on angle
        /// </summary>
        /// <param name="angle">The value of angle</param>
        /// <param name="direction"></param>
        /// <param name="power">Rotational power. In range 0-100</param>
        public void RotateOnAngle(Direction direction, int angle, int power)
        {
            int targetAngle = 0;

            switch (direction)
            {
                case Direction.Left:
                    targetAngle = m_RotoData.Angle - angle;
                    break;
                case Direction.Right:
                    targetAngle = m_RotoData.Angle + angle;
                    break;
            }

            SendMessage(new RotateToAngleMessage(
                JsonConvert.SerializeObject(new RotateToAngleModel(NormalizeAngle(targetAngle), power,
                    direction.ToString()))));
        }


        /// <summary>
        /// Follow rotation of a target object
        /// </summary>
        /// <param name="target">Target object which rotation need to follow</param>
        public void FollowTarget(MonoBehaviour behaviour, Transform target)
        {
            m_ObservableTarger = target;
            m_StartTargetAngle = (int)m_ObservableTarger.eulerAngles.y;
            m_StartRotoAngle = m_RotoData.Angle;

            if (m_TargetRoutine != null)
            {
                behaviour.StopCoroutine(m_TargetRoutine);
                m_TargetRoutine = null;
            }

            m_TargetRoutine = behaviour.StartCoroutine(FollowTargetRoutine());
        }

        /// <summary>
        /// Start head tracking routine
        /// </summary>
        /// <param name="target">Target headset representation</param>
        public void StartHeadTracking(MonoBehaviour behaviour, Transform target)
        {
            m_ObservableTarger = target;
            m_StartTargetAngle = (int)m_ObservableTarger.eulerAngles.y;
            m_StartRotoAngle = m_RotoData.Angle;

            if (m_TargetRoutine != null)
            {
                behaviour.StopCoroutine(m_TargetRoutine);
                m_TargetRoutine = null;
            }

            m_TargetRoutine = behaviour.StartCoroutine(HeadTrackingRoutine());
        }

        /// <summary>
        /// Stop routine
        /// </summary>
        public void StopRoutine(MonoBehaviour behaviour)
        {
            if (m_TargetRoutine != null)
            {
                behaviour.StopCoroutine(m_TargetRoutine);
                m_TargetRoutine = null;
                m_ObservableTarger = null;
            }
        }

        /// <summary>
        /// Play rumble
        /// </summary>
        /// <param name="duration">Duration of rumble</param>
        /// <param name="power">Power of rumble</param>
        public void Rumble(float duration, int power)
        {
            SendMessage(new PlayRumbleMessage(JsonConvert.SerializeObject(new RumbleModel(duration, power))));
        }

        IEnumerator FollowTargetRoutine()
        {
            if (m_ObservableTarger == null)
                Debug.LogError("For Had Tracking Mode you need to set target transform");
            else
            {
                float deltaTime = 0;
                int rotoAngle = 0;

                yield return new WaitForSeconds(0.5f);
                SetMode(ModeType.FreeMode);

                while (true)
                {
                    deltaTime += Time.deltaTime;

                    if (deltaTime > 0.1f)
                    {
                        var currentAngle = (int)m_ObservableTarger.eulerAngles.y;
                        var angle = currentAngle - m_StartTargetAngle;

                        if (angle != 0)
                        {
                            angle = NormalizeAngle(angle);

                            rotoAngle = m_StartRotoAngle + angle;
                            rotoAngle = NormalizeAngle(rotoAngle);

                            RotateToAngle(GetDirection(rotoAngle), rotoAngle, 100);
                        }

                        deltaTime = 0;
                    }

                    yield return null;
                }
            }
        }

        IEnumerator HeadTrackingRoutine()
        {
            if (m_ObservableTarger == null)
                Debug.LogError("For Had Tracking Mode you need to set target transform");
            else
            {
                float deltaTime = 0;
                while (true)
                {
                    yield return null;
                    deltaTime += Time.deltaTime;

                    if (deltaTime > 0.1f)
                    {
                        int currentTargetAngle = (int)m_ObservableTarger.eulerAngles.y;
                        var deltaTargetAngle = currentTargetAngle - m_StartTargetAngle;

                        var currentRotoAngle = m_RotoData.Angle;
                        var deltaRotoAngle = currentRotoAngle - m_StartRotoAngle;

                        var angle = deltaTargetAngle - deltaRotoAngle;

                        if (angle != 0)
                        {
                            angle = NormalizeAngle(angle);
                            angle += m_StartRotoAngle;
                            RotateToAngle(Direction.Left, NormalizeAngle(angle), 100);
                        }

                        deltaTime = 0;
                    }
                }
            }
        }

        Direction GetDirection(int targetAngle)
        {
            if (targetAngle > m_RotoData.Angle)
            {
                if (Mathf.Abs(targetAngle - m_RotoData.Angle) > 180)
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
                if (Mathf.Abs(targetAngle - m_RotoData.Angle) > 180)
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Left;
                }
            }
        }

        int NormalizeAngle(int angle)
        {
            if (angle < 0)
                angle += 360;
            else if (angle > 360)
                angle -= 360;

            return angle;
        }
    }
}