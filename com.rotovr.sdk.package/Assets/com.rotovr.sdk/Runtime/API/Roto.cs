using System;
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
        bool m_IsInit = false;

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

            Debug.LogError($"OnModelChangeHandler current angle :{model.Angle}");

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
            Debug.LogError($"Connect to device: {deviceName} ");
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
                        RotateToAngle(GetCloseDirection(m_RotoData.Angle, defaultAngle), defaultAngle, 100);
                    }

                    break;
                case CalibrationMode.SetToZero:
                    defaultAngle = 0;
                    RotateToAngle(GetCloseDirection(m_RotoData.Angle, defaultAngle), defaultAngle, 100);
                    break;
            }
        }

        Direction GetCloseDirection(int currentAngle, int targetAngle)
        {
            int delta = Math.Abs(targetAngle - currentAngle);

            if (delta > 180)
                return Direction.Right;
            else
                return Direction.Left;
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
                        newAngle = 360 - newAngle;
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
        public void AddObservable(Transform target)
        {
            m_ObservableTarger = target;
        }

        /// <summary>
        /// Stop observable flow
        /// </summary>
        public void RemoveObservable()
        {
            m_ObservableTarger = null;
        }

        /// <summary>
        /// Play rumble
        /// </summary>
        /// <param name="duration">Duration of rumble</param>
        /// <param name="power">Power of rumble</param>
        public void Rumble(int duration, int power)
        {
            Debug.LogError($"Rumble  duration: {duration}  power: {power}");
            SendMessage(new PlayRumbleMessage(JsonConvert.SerializeObject(new RumbleModel(duration, power))));
        }
    }
}