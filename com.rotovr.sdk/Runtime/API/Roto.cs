using System;
using System.Collections;

#if !NO_UNITY
using UnityEngine;
#endif


namespace com.rotovr.sdk
{
    /// <summary>
    /// The Roto class provides an API to interact with the RotoVR chair, allowing for communication with the device, 
    /// mode setting, rotation control, calibration, and connection management.
    /// </summary>
    public class Roto
    {
        static Roto s_Roto;

        /// <summary>
        /// Gets the singleton instance of the Roto manager.
        /// Ensures only one instance of Roto exists throughout the application.
        /// </summary>
        /// <returns>The singleton instance of the Roto manager.</returns>
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

#if !NO_UNITY
        readonly string m_CalibrationKey = "CalibrationKey";
        Transform m_ObservableTarget;
        Coroutine m_TargetRoutine;
#endif
        bool m_IsInit;
        float m_StartTargetAngle;
        int m_StartRotoAngle;
        ConnectionType m_ConnectionType;


        /// <summary>
        /// The current connection status of the RotoVR chair.
        /// Possible statuses: Disconnected, Connecting, or Connected.
        /// </summary>
        public ConnectionStatus ConnectionStatus { get; private set; } = ConnectionStatus.Disconnected;

        /// <summary>
        /// Event triggered when the system mode changes.
        /// </summary>
        public event Action<ModeType> OnRotoMode;

        /// <summary>
        /// Event triggered when chair data changes.
        /// </summary>
        public event Action<RotoDataModel> OnDataChanged;

        /// <summary>
        /// Event triggered when the system connection status changes.
        /// </summary>
        public event Action<ConnectionStatus> OnConnectionStatus;

        void Call(string command, string data)
        {
#if !NO_UNITY
            BleManager.Instance.Call(command, data);
#endif
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
        public void Subscribe(string command, Action<string> action)
        {
#if !NO_UNITY
            BleManager.Instance.Subscribe(command, action);
#endif
        }

        /// <summary>
        /// Subscribe from ble json message
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="action">Handler</param>
        public void UnSubscribe(string command, Action<string> action)
        {
#if !NO_UNITY
            BleManager.Instance.UnSubscribe(command, action);
#endif
        }
        

        /// <summary>
        /// Initializes the Roto manager with a specified connection type.
        /// </summary>
        /// <param name="connectionType">The connection type (e.g., Simulation or Chair) used for communication.</param>
        public void Initialize(ConnectionType connectionType)
        {
            if (m_IsInit)
                return;

            m_IsInit = true;
            m_ConnectionType = connectionType;
            
#if !UNITY_EDITOR && !NO_UNITY
            BleManager.Instance.Init();
            Subscribe(MessageType.ModelChanged.ToString(), OnModelChangeHandler);
            Subscribe(MessageType.DeviceConnected.ToString(),
                (data) => { OnConnectionStatus?.Invoke(ConnectionStatus.Connected); });
            Subscribe(MessageType.Disconnected.ToString(),
                (data) => { OnConnectionStatus?.Invoke(ConnectionStatus.Disconnected); });
#endif
        }

        void OnConnectionStatusChange(ConnectionStatus status)
        {
            ConnectionStatus = status;
            OnConnectionStatus?.Invoke(status);
        }

        void OnModelChangeHandler(string data)
        {

            var model = new RotoDataModel(data);

            if (model.Mode != m_RotoData.Mode)
            {
                if (Enum.TryParse(model.Mode, out ModeType value))
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
                if (Enum.TryParse(model.Mode, out ModeType value))
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
        /// Connects to a specified device by name.
        /// </summary>
        /// <param name="deviceName">The name of the device to connect to.</param>
        internal void Connect(string deviceName)
        {
#if !UNITY_EDITOR && !NO_UNITY
            if (m_ConnectedDevice == null)
            {
                void Connected(string data)
                {
                    s_Roto.UnSubscribe(MessageType.Connected.ToString(), Connected);
                    m_ConnectedDevice = new DeviceDataModel(data);
                }

                s_Roto.Subscribe(MessageType.Connected.ToString(), Connected);

                SendMessage(
                    new ConnectMessage(new DeviceDataModel(deviceName, string.Empty).ToJson()));
            }
            else
            {
                SendMessage(new ConnectMessage(m_ConnectedDevice.ToJson()));
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
        /// Disconnects from the currently connected device.
        /// </summary>
        /// <param name="deviceName">The name of the device to disconnect from.</param>
        internal void Disconnect(string deviceName)
        {
#if !UNITY_EDITOR && !NO_UNITY
            if (m_ConnectedDevice != null && m_ConnectedDevice.Name == deviceName)
            {
                SendMessage(new DisconnectMessage(m_ConnectedDevice.ToJson()));
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
                OnConnectionStatusChange(ConnectionStatus.Disconnected);
            }

#endif
        }

        /// <summary>
        /// Sets the mode for the RotoVR chair with specific mode parameters.
        /// </summary>
        /// <param name="mode">The mode to set for the chair (e.g., HeadTrack, FreeMode).</param>
        /// <param name="modeParams">The mode parameters (e.g., power, angle limits) to configure the chair in the specified mode.</param>
        public void SetMode(ModeType mode, ModeParams modeParams)
        {
            var parametersModel = new ModeParametersModel(modeParams);

#if !UNITY_EDITOR && !NO_UNITY
            SendMessage(
                new SetModeMessage(new ModeModel(mode.ToString(), parametersModel).ToJson())
                );
#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.SetMode(new ModeModel(mode.ToString(), parametersModel));
            }
#endif
        }

        /// <summary>
        /// Sets the power of the RotoVR chair when in FreeMode. The power value must be between 30 and 100.
        /// </summary>
        /// <param name="power">The rotation power to set for the chair (valid range is 30-100).</param>
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
        /// Calibrates the RotoVR chair, resetting the angle based on the specified calibration mode.
        /// </summary>
        /// <param name="calibrationMode">The calibration mode (e.g., set to zero, set to last position).</param>
        public void Calibration(CalibrationMode calibrationMode)
        {
            switch (calibrationMode)
            {
#if !NO_UNITY
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
#endif
                case CalibrationMode.SetToZero:
                    RotateToAngle(GetDirection(0, m_RotoData.Angle), 0, 100);
                    break;
            }
        }

        /// <summary>
        /// Rotates the RotoVR chair to the specified angle.
        /// This is applicable only in the Calibration or CockpitMode.
        /// </summary>
        /// <param name="direction">The direction in which to rotate (e.g., left or right).</param>
        /// <param name="angle">The angle to rotate to.</param>
        /// <param name="power">The power of rotation (valid range is 0-100).</param>
        public void RotateToAngle(Direction direction, int angle, int power)
        {
            if (angle == m_RotoData.Angle)
                return;
#if !UNITY_EDITOR && !NO_UNITY
            SendMessage(new RotateToAngleMessage(
                new RotateToAngleModel(angle, power, direction.ToString()).ToJson()));
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
        /// Rotates the chair to the closest angle, choosing the best direction automatically.
        /// </summary>
        /// <param name="angle">The target angle to rotate to.</param>
        /// <param name="power">The power of rotation (valid range is 0-100).</param>
        public void RotateToClosestAngleDirection(int angle, int power)
        {
            if (angle == m_RotoData.Angle)
                return;
#if !UNITY_EDITOR && !NO_UNITY
            var rotateToAngleModel = new RotateToAngleModel(angle, power,
                GetDirection(angle, m_RotoData.Angle).ToString());
            SendMessage(new RotateToAngleMessage(rotateToAngleModel.ToJson()));
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
        public void Rotate(Direction direction, int angle, int power)
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
#if !UNITY_EDITOR && !NO_UNITY
            var rotateToAngleModel = new RotateToAngleModel(NormalizeAngle(targetAngle), power, direction.ToString());
            SendMessage(new RotateToAngleMessage(rotateToAngleModel.ToJson()));
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
        
        
#if !NO_UNITY 
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
#endif

        /// <summary>
        /// Plays a rumble effect on the chair with a specified duration and power.
        /// </summary>
        /// <param name="duration">The duration of the rumble in seconds.</param>
        /// <param name="power">The power of the rumble (valid range is 0-100).</param>
        public void Rumble(float duration, int power)
        {
#if !UNITY_EDITOR && !NO_UNITY
            var rumbleModel = new RumbleModel(duration, power);
            SendMessage(new PlayRumbleMessage(rumbleModel.ToJson()));
#else
            if (m_ConnectionType == ConnectionType.Chair)
            {
                UsbConnector.Instance.PlayRumble(new RumbleModel(duration, power));
            }
#endif
        }

#if !NO_UNITY
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
                            // fix delta being massive when crossing 0
                            if (delta > 180)
                                delta = 360 - delta;

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
#endif
        
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
                if (Math.Abs(targetAngle - sourceAngle) > 180)
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
                if (Math.Abs(targetAngle - sourceAngle) > 180)
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