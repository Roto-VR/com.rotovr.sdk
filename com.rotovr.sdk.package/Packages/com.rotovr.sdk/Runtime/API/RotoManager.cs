using System;
using RotoVR.SDK.BLE;
using RotoVR.SDK.Enum;
using RotoVR.SDK.Message;

namespace RotoVR.SDK.API
{
    public class RotoManager
    {
        static RotoManager m_manager;

        public static RotoManager GetManager()
        {
            if (m_manager == null)
                m_manager = new RotoManager();

            return m_manager;
        }

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
        /// Get data from java library
        /// </summary>
        /// <param name="command">Method name in java library</param>
        /// <typeparam name="T">Returned type</typeparam>
        /// <returns>Requested data</returns>
        public T Get<T>(string command)
        {
            return BleManager.Instance.Get<T>(command);
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
        /// Request data from java library
        /// </summary>
        /// <param name="message">Ble message</param>
        /// <typeparam name="T">Returned type</typeparam>
        /// <returns>Requested data</returns>
        public T DataRequest<T>(BleMessage message)
        {
            return Get<T>(message.MessageType.ToString());
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
            BleManager.Instance.Init();
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
            SendMessage(new ConnectMessage(deviceName));
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
        /// Turn RotoVR to angle
        /// </summary>
        /// <param name="angle">The value of angle</param>
        /// <param name="time">The value of the time we need to turn</param>
        /// <param name="direction">Rotate direction</param>
        public void TurnToAngle(float angle, float time, Direction direction)
        {
        }

        /// <summary>
        /// Turn RotoVR on angle
        /// </summary>
        /// <param name="angle">The value of angle</param>
        /// <param name="time">The value of the time we need to turn</param>
        /// <param name="direction"></param>
        public void TurnOnAngle(string data)
        {
            SendMessage(new TurnOnAngleMessage(data));
        }

        /// <summary>
        /// Set RotoVR mode
        /// </summary>
        /// <param name="mode">Mode type</param>
        public void SetMode(ModeType mode)
        {
            SendMessage(new SetModeMessage(mode, "Example"));
        }

        /// <summary>
        /// Calibrate RotoVR as zero rotation
        /// </summary>
        public void Calibration()
        {
            SendMessage(new CalibrationMessage());
        }

        /// <summary>
        /// Invoke when change roto vr mode
        /// </summary>
        public event Action<ModeType> OnRotoMode;

        /// <summary>
        /// Invoke when change connection status of roto vr
        /// </summary>
        public event Action<ConnectionStatus> OnConnectionStatus;
    }
}