using System;
using Example.BLE.Enum;
using Example.BLE.Message;

namespace Example.BLE.API
{
    public static class API
    {
        /// <summary>
        /// Invoke to directly call command in java library
        /// </summary>
        /// <param name="command">Method name in java library</param>
        /// <param name="data">Data which we wont to send</param>
        public static void Call(string command, byte[] data)
        {
            BleManager.Instance.Call(command, data);
        }

        /// <summary>
        /// Get data from java library
        /// </summary>
        /// <param name="command">Method name in java library</param>
        /// <typeparam name="T">Returned type</typeparam>
        /// <returns>Requested data</returns>
        public static T Get<T>(string command)
        {
            return BleManager.Instance.Get<T>(command);
        }

        /// <summary>
        /// Invoke to send BleMessage to java library
        /// </summary>
        /// <param name="message">Ble message</param>
        public static void SendMessage(BleMessage message)
        {
            Call(message.MessageType.ToString(), message.Data);
        }

        /// <summary>
        /// Request data from java library
        /// </summary>
        /// <param name="message">Ble message</param>
        /// <typeparam name="T">Returned type</typeparam>
        /// <returns>Requested data</returns>
        public static T DataRequest<T>(BleMessage message)
        {
            return Get<T>(message.MessageType.ToString());
        }

        /// <summary>
        /// Subscribe to ble json message
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="action">Handler</param>
        public static void Subscribe(string command, Action<string> action) => BleManager.Instance.Subscribe(command, action);

        /// <summary>
        /// Subscribe from ble json message
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="action">Handler</param>
        public static void UnSubscribe(string command, Action<string> action) => BleManager.Instance.UnSubscribe(command, action);

        /// <summary>
        /// Invoke for ble sdk initialization
        /// </summary>
        public static void Initialize()
        {
            BleManager.Instance.Init();
        }

        /// <summary>
        /// Scan environment to find devices 
        /// </summary>
        public static void Scan()
        {
            SendMessage(new ScanMessage());
        }

        /// <summary>
        /// Connect to device
        /// </summary>
        /// <param name="msg">Data with device parameters</param>
        public static void Connect(ConnectMessage msg)
        {
            SendMessage(msg);
        }

        /// <summary>
        /// Disconnect from current device
        /// </summary>
        public static void Disconnect()
        {
            SendMessage(new DisconnectMessage());
        }

        /// <summary>
        /// Turn RotoVR to angle
        /// </summary>
        /// <param name="angle">The value of angle</param>
        /// <param name="time">The value of the time we need to turn</param>
        /// <param name="direction">Rotate direction</param>
        public static void TurnToAngle(float angle, float time, Direction direction) { }

        /// <summary>
        /// Turn RotoVR on angle
        /// </summary>
        /// <param name="angle">The value of angle</param>
        /// <param name="time">The value of the time we need to turn</param>
        /// <param name="direction"></param>
        public static void TurnOnAngle(float angle, float time, Direction direction) { }
    }
}
