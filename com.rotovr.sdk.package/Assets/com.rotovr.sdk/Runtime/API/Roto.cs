using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace com.rotovr.sdk
{
    public class Roto
    {
        static Roto s_Roto;

        public static Roto GetManager()
        {
            if (s_Roto == null)
            {
                s_Roto = new Roto();
            }

            return s_Roto;
        }

        RotoDataModel m_RotoData = new();
        DeviceDataModel m_ConnectedDevice;
        readonly string m_CalibrationKey = "CalibrationKey";
        Transform m_ObservableTarget;
        Coroutine m_TargetRoutine;
        bool m_IsInit;
        float m_StartTargetAngle;
        int m_StartRotoAngle;
        ConnectionType m_ConnectionType;

        /// <summary>
        /// Invoked when chair mode is updated.
        /// </summary>
        public event Action<ModeType> OnRotoMode;

        /// <summary>
        /// Invoked when chair data is changed.
        /// </summary>
        public event Action<RotoDataModel> OnDataChanged;

        /// <summary>
        /// Invoked when connection status is updated.
        /// </summary>
        public event Action<ConnectionStatus> OnConnectionStatus;

        void Call(string command, string data)
        {
            BleManager.Instance.Call(command, data);
        }

        /// <summary>
        /// Invoke to send BleMessage to java library
        /// </summary>
        /// <param name="message">Ble message</param>
        void SendMessage(BleMessage message)
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
        /// Initialize with connection type.
        /// </summary>
        public void Initialize(ConnectionType connectionType)
        {
            if (m_IsInit)
                return;

            m_IsInit = true;
            m_ConnectionType = connectionType;

#if !UNITY_EDITOR
            BleManager.Instance.Init();
            Subscribe(MessageType.ModelChanged.ToString(), OnModelChangeHandler);
            Subscribe(MessageType.DeviceConnected.ToString(),
                (data) => { OnConnectionStatus?.Invoke(ConnectionStatus.Connected); });
            Subscribe(MessageType.Disconnected.ToString(),
                (data) => { OnConnectionStatus?.Invoke(ConnectionStatus.Disconnected); });
#else

#endif
        }

        void OnConnectionStatusChange(ConnectionStatus status)
        {
            OnConnectionStatus?.Invoke(status);
        }

        void OnModelChangeHandler(string data)
        {
            var model = JsonConvert.DeserializeObject<RotoDataModel>(data);

            if (model.Mode != m_RotoData.Mode)
            {
                if (System.Enum.TryParse(model.Mode, out ModeType value))
                {
                    OnRotoMode?.Invoke(value);
                }
            }

            OnDataChanged?.Invoke(model);
            m_RotoData = model;
        }

        void OnModelChangeHandler(RotoDataModel model)
        {
            if (model.Mode != m_RotoData.Mode)
            {
                if (System.Enum.TryParse(model.Mode, out ModeType value))
                {
                    OnRotoMode?.Invoke(value);
                }
            }

            OnDataChanged?.Invoke(model);
            m_RotoData = model;
        }

        void Scan()
        {
            SendMessage(new ScanMessage());
        }

        /// <summary>
        /// Connect to device.
        /// </summary>
        /// <param name="deviceName">Data with device parameters</param>
        internal void Connect(string deviceName)
        {
#if !UNITY_EDITOR
            if (m_ConnectedDevice == null)
            {
                void Connected(string data)
                {
                    s_Roto.UnSubscribe(MessageType.Connected.ToString(), Connected);
                    m_ConnectedDevice = JsonConvert.DeserializeObject<DeviceDataModel>(data);
                }

                s_Roto.Subscribe(MessageType.Connected.ToString(), Connected);

                SendMessage(
                    new ConnectMessage(JsonConvert.SerializeObject(new DeviceDataModel(deviceName, string.Empty))));
            }
            else
            {
                SendMessage(new ConnectMessage(JsonConvert.SerializeObject(m_ConnectedDevice)));
            }


#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.OnConnectionStatus += OnConnectionStatusChange;
                UsbConnector.Instance.OnDataChange += OnModelChangeHandler;
                UsbConnector.Instance.Connect();
            }
            else
            {
                OnConnectionStatusChange(ConnectionStatus.Connected);
            }

#endif
        }

        /// <summary>
        /// Disconnect from current device
        /// </summary>
        /// <param name="deviceName">Device name to disconnect</param>
        internal void Disconnect(string deviceName)
        {
#if !UNITY_EDITOR
            if (m_ConnectedDevice != null && m_ConnectedDevice.Name == deviceName)
            {
                SendMessage(new DisconnectMessage(JsonConvert.SerializeObject(m_ConnectedDevice)));
            }

#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.OnConnectionStatus -= OnConnectionStatusChange;
                UsbConnector.Instance.OnDataChange -= OnModelChangeHandler;
                UsbConnector.Instance.Disconnect();
            }
            else
            {
                OnConnectionStatusChange(ConnectionStatus.Connected);
            }

#endif
        }

        /// <summary>
        /// Set RotoVR mode
        /// </summary>
        /// <param name="mode">Mode type.</param>
        /// <param name="modeParams">Mode params</param>
        public void SetMode(ModeType mode, ModeParams modeParams)
        {
            var parametersModel = new ModeParametersModel(modeParams);

#if !UNITY_EDITOR
            SendMessage(
                new SetModeMessage(
                    JsonConvert.SerializeObject(new ModeModel(mode.ToString(), parametersModel))));
#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.SetMode(new ModeModel(mode.ToString(), parametersModel));
            }
#endif
        }

        /// <summary>
        /// Set RotoVR power. Only applicable for the FreeMode.
        /// </summary>
        /// <param name="power">Value of rotation power in range 30-100</param>
        public void SetPower(int power)
        {
            var modeParams = new ModeParams
            {
                CockpitAngleLimit = m_RotoData.TargetCockpit,
                MaxPower = power
            };

            SetMode(m_RotoData.ModeType, modeParams);
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
                    PlayerPrefs.SetInt(m_CalibrationKey, m_RotoData.Angle);
                    break;
                case CalibrationMode.SetLast:
                    if (PlayerPrefs.HasKey(m_CalibrationKey))
                    {
                        var defaultAngle = PlayerPrefs.GetInt(m_CalibrationKey);
                        RotateToAngle(GetDirection(defaultAngle, m_RotoData.Angle), defaultAngle, 100);
                    }
                    else
                        RotateToAngle(GetDirection(0, m_RotoData.Angle), 0, 100);

                    break;
                case CalibrationMode.SetToZero:
                    RotateToAngle(GetDirection(0, m_RotoData.Angle), 0, 100);
                    break;
            }
        }

        /// <summary>
        /// Rotate Chair to specified angle to angle.
        /// Only applicable when chair in <see cref="ModeType.Calibration"/> or <see cref="ModeType.CockpitMode"/>
        /// </summary>
        /// <param name="direction">Rotation direction.</param>
        /// <param name="angle">Rotation angle.</param>
        /// <param name="power">Rotational power. Can range from 0 to 100</param>
        public void RotateToAngle(Direction direction, int angle, int power)
        {
            if (angle == m_RotoData.Angle)
                return;
#if !UNITY_EDITOR
            SendMessage(new RotateToAngleMessage(
                JsonConvert.SerializeObject(new RotateToAngleModel(angle, power, direction.ToString()))));
#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.TurnToAngle(new RotateToAngleModel(angle, power, direction.ToString()));
            }
            else
            {
                m_RotoData = new RotoDataModel()
                {
                    Angle = angle
                };
                OnDataChanged?.Invoke(m_RotoData);
            }
#endif
        }

        /// <summary>
        /// Rotate RotoVR to angle, the direction will be picked automatically.
        /// </summary>
        /// <param name="angle">Rotation angle.</param>
        /// <param name="power">Rotational power. Can range from 0 to 100</param>
        public void RotateToAngleCloserDirection(int angle, int power)
        {
            if (angle == m_RotoData.Angle)
                return;
#if !UNITY_EDITOR
            SendMessage(new RotateToAngleMessage(
                JsonConvert.SerializeObject(new RotateToAngleModel(angle, power,
                    GetDirection(angle, m_RotoData.Angle).ToString()))));
#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.TurnToAngle(new RotateToAngleModel(angle, power,
                    GetDirection(angle, m_RotoData.Angle).ToString()));
            }
            else
            {
                m_RotoData = new RotoDataModel()
                {
                    Angle = angle
                };
                OnDataChanged?.Invoke(m_RotoData);
            }
#endif
        }

        /// <summary>
        /// Will rotate chair on specific angle with specified direction.
        /// </summary>
        /// <param name="angle">Rotation angle.</param>
        /// <param name="direction">Rotation direction.</param>
        /// <param name="power">Rotational power. Can range from 0 to 100.</param>
        public void RotateOnAngle(Direction direction, int angle, int power)
        {
            var targetAngle = 0;
            switch (direction)
            {
                case Direction.Left:
                    targetAngle = m_RotoData.Angle - angle;
                    break;
                case Direction.Right:
                    targetAngle = m_RotoData.Angle + angle;
                    break;
            }
#if !UNITY_EDITOR
            SendMessage(new RotateToAngleMessage(
                JsonConvert.SerializeObject(new RotateToAngleModel(NormalizeAngle(targetAngle), power,
                    direction.ToString()))));
#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.TurnToAngle(new RotateToAngleModel(NormalizeAngle(targetAngle), power,
                    direction.ToString()));
            }
            else
            {
                m_RotoData = new RotoDataModel()
                {
                    Angle = NormalizeAngle(targetAngle)
                };
                OnDataChanged?.Invoke(m_RotoData);
            }
#endif
        }

        /// <summary>
        /// Follow rotation of a target object
        /// </summary>
        /// <param name="behaviour">Target that will be used as the rotation preference.</param>
        /// <param name="target">Target object which rotation need to follow</param>
        public void FollowTarget(MonoBehaviour behaviour, Transform target)
        {
            m_ObservableTarget = target;
            m_StartTargetAngle = NormalizeAngle(m_ObservableTarget.eulerAngles.y);
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
        /// <param name="behaviour">Target that will be used as the rotation preference.</param>
        public void StartHeadTracking(MonoBehaviour behaviour, Transform target)
        {
            m_ObservableTarget = target;
            m_StartTargetAngle = NormalizeAngle(m_ObservableTarget.eulerAngles.y);
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
        internal void StopRoutine(MonoBehaviour behaviour)
        {
            if (m_TargetRoutine != null)
            {
                behaviour.StopCoroutine(m_TargetRoutine);
                m_TargetRoutine = null;
                m_ObservableTarget = null;
            }
        }

        /// <summary>
        /// Play rumble
        /// </summary>
        /// <param name="duration">Duration of rumble</param>
        /// <param name="power">Power of rumble</param>
        public void Rumble(float duration, int power)
        {
#if !UNITY_EDITOR
            SendMessage(new PlayRumbleMessage(JsonConvert.SerializeObject(new RumbleModel(duration, power))));
#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.PlayRumble(new RumbleModel(duration, power));
            }
#endif
        }

        IEnumerator FollowTargetRoutine()
        {
            if (m_ObservableTarget == null)
                Debug.LogError("For FollowObject Mode you need to set target transform");
            else
            {
                float deltaTime = 0;

                yield return new WaitForSeconds(0.5f);
                var modeParams = new ModeParams
                {
                    CockpitAngleLimit = 30,
                    MaxPower = 100
                };

                SetMode(ModeType.HeadTrack, modeParams);

                while (true)
                {
                    deltaTime += Time.deltaTime;

                    if (deltaTime > 0.1f)
                    {
                        var currentAngle = (int)m_ObservableTarget.eulerAngles.y;
                        var angle = currentAngle - m_StartTargetAngle;

                        if (angle != 0)
                        {
                            angle = NormalizeAngle(angle);

                            var rotoAngle = (int)(m_StartRotoAngle + angle);
                            rotoAngle = NormalizeAngle(rotoAngle);

                            var delta = Mathf.Abs(rotoAngle - m_RotoData.Angle);

                            if (delta > 2)
                                RotateToAngle(Direction.Left, rotoAngle, 30);
                        }

                        deltaTime = 0;
                    }

                    yield return null;
                }
            }
        }

        IEnumerator HeadTrackingRoutine()
        {
            if (m_ObservableTarget == null)
                Debug.LogError("For Had Tracking Mode you need to set target transform");
            else
            {
                var lastTargetAngle = NormalizeAngle(m_ObservableTarget.eulerAngles.y);

                float deltaTime = 0;
                while (true)
                {
                    yield return null;
                    deltaTime += Time.deltaTime;

                    if (deltaTime > 0.1f)
                    {
                        var currentTargetAngle = NormalizeAngle(m_ObservableTarget.eulerAngles.y);
                        float currentRotoAngle = m_RotoData.Angle;

                        var direction = GetDirection((int)currentTargetAngle, (int)lastTargetAngle);

                        var deltaTargetAngle = GetDelta(m_StartTargetAngle, currentTargetAngle, direction);
                        var deltaRotoAngle = GetDelta(m_StartRotoAngle, currentRotoAngle, direction);

                        var angle = deltaTargetAngle - deltaRotoAngle;

                        angle = NormalizeAngle(angle);
                        angle += m_RotoData.Angle;

                        RotateToAngle(Direction.Left, (int)NormalizeAngle(angle), 30);
                        deltaTime = 0;

                        lastTargetAngle = currentTargetAngle;
                    }
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

        float NormalizeAngle(float angle)
        {
            if (angle < 0)
                angle += 360;
            else if (angle > 360)
                angle -= 360;

            return angle;
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