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
        float m_StartTargetAngle;
        float m_StartRotoAngle;

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

            Debug.LogError($"OnModelChangeHandler angle: {model.Angle}");
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
            int defaultAngle = 0;
            switch (calibrationMode)
            {
                case CalibrationMode.SetCurrent:
                    defaultAngle = m_RotoData.Angle;
                    PlayerPrefs.SetInt(m_Calibrationkey, defaultAngle);
                    break;
                case CalibrationMode.SetLast:
                    if (PlayerPrefs.HasKey(m_Calibrationkey))
                    {
                        defaultAngle = PlayerPrefs.GetInt(m_Calibrationkey);
                        RotateToAngle(GetDirection(defaultAngle), defaultAngle, 100);
                    }

                    break;
                case CalibrationMode.SetToZero:
                    defaultAngle = 0;
                    RotateToAngle(GetDirection(defaultAngle), defaultAngle, 100);
                    break;
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

        /// <summary>
        /// Turn RotoVR to angle
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

        public void RotateToAngleByCloserDirection(int angle, int power)
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
            int newAngle = 0;


            switch (direction)
            {
                case Direction.Left:
                    newAngle = m_RotoData.Angle - angle;
                    if (newAngle < 0)
                        newAngle = 360 + newAngle;
                    break;
                case Direction.Right:
                    newAngle = m_RotoData.Angle + angle;
                    if (newAngle > 360)
                        newAngle -= 360;
                    break;
            }

            SendMessage(new RotateToAngleMessage(
                JsonConvert.SerializeObject(new RotateToAngleModel(newAngle, power, direction.ToString()))));
        }

        /// <summary>
        /// Observe rotation of a target object
        /// </summary>
        /// <param name="target">Target object which rotation need to observe</param>
        public void AddToAngleObservable(MonoBehaviour behaviour, Transform target)
        {
            m_ObservableTarger = target;
            m_StartTargetAngle = m_ObservableTarger.eulerAngles.y;
            m_StartRotoAngle = m_RotoData.Angle;

            if (m_TargetRoutine != null)
            {
                behaviour.StopCoroutine(m_TargetRoutine);
                m_TargetRoutine = null;
            }

            m_TargetRoutine = behaviour.StartCoroutine(ObserveToAngleTarget());
        }

        /// <summary>
        /// Observe rotation of a target object
        /// </summary>
        /// <param name="target">Target object which rotation need to observe</param>
        public void AddOnAngleObservable(MonoBehaviour behaviour, Transform target)
        {
            m_ObservableTarger = target;
            m_StartTargetAngle = m_ObservableTarger.eulerAngles.y;
            m_StartRotoAngle = m_RotoData.Angle;

            if (m_TargetRoutine != null)
            {
                behaviour.StopCoroutine(m_TargetRoutine);
                m_TargetRoutine = null;
            }

            Debug.LogError(
                $"AddOnAngleObservable   m_StartTargetAngle: {m_StartTargetAngle}  m_StartRotoAngle: {m_StartRotoAngle}");

            m_TargetRoutine = behaviour.StartCoroutine(ObserveOnAngleTarget());
        }

        /// <summary>
        /// Stop observable flow
        /// </summary>
        public void RemoveObservable(MonoBehaviour behaviour)
        {
            if (m_TargetRoutine != null)
            {
                behaviour.StopCoroutine(m_TargetRoutine);
                m_TargetRoutine = null;
                m_ObservableTarger = null;
            }
        }

        IEnumerator ObserveToAngleTarget()
        {
            if (m_ObservableTarger == null)
                Debug.LogError("For Had Tracking Mode you need to set target transform");
            else
            {
                float deltaTime = 0;
                float rotoAngle = 0;
                Direction direction = Direction.Left;

                yield return new WaitForSeconds(0.5f);
                SetMode(ModeType.FreeMode);

                while (true)
                {
                    deltaTime += Time.deltaTime;

                    if (deltaTime > 0.1f)
                    {
                        var currentTargetAngle = m_ObservableTarger.eulerAngles.y;
                        var deltaTargetAngle = currentTargetAngle - m_StartTargetAngle;

                        var currentRotoAngle = m_RotoData.Angle;
                        var deltaRotoAngle = currentRotoAngle - m_StartRotoAngle;

                        var realCurrentTargetAngle = deltaTargetAngle - deltaRotoAngle;

                        var angle = realCurrentTargetAngle;

                        if (angle != 0)
                        {
                            if (angle > 360)
                                angle -= 360;

                            rotoAngle = m_StartRotoAngle + angle;
                            if (rotoAngle >= 360)
                                rotoAngle -= 360;
                            else if (rotoAngle < 0)
                                rotoAngle += 360;

                            direction = GetDirection((int)rotoAngle);
                            RotateToAngle(direction, (int)rotoAngle, 100);
                        }

                        deltaTime = 0;
                    }

                    yield return null;
                }
            }
        }

        IEnumerator ObserveOnAngleTarget()
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
                        var currentRotoAngle = m_RotoData.Angle;
                        var deltaRotoAngle = currentRotoAngle - m_StartRotoAngle;


                        var currentTargetAngle = m_ObservableTarger.eulerAngles.y;
                        var deltaTargetAngle = currentTargetAngle - m_StartTargetAngle;

                        var angle = m_StartRotoAngle + deltaTargetAngle - deltaRotoAngle;

                        if (angle >= 360)
                            angle -= 360;
                        else if (angle < 0)
                            angle += 360;
                        Debug.LogError(
                            $"RotateToAngle currentRotoAngle: {currentRotoAngle}  deltaRotoAngle: {deltaRotoAngle} rotoAngle: {angle}  ");
                        RotateToAngle(Direction.Left, (int)angle, 100);

                        deltaTime = 0;
                    }
                }
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
    }
}