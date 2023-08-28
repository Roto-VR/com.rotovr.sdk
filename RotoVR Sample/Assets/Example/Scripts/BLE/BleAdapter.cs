using System;
using Example.BLE.Enum;
using Example.BLE.Message;
using UnityEngine;
using Newtonsoft.Json;

namespace Example.BLE
{
    public class BleAdapter : MonoBehaviour
    {
        public delegate void BleMessageReceived(BleMessage msg);

        public delegate void BleJsonMessageReceived(BleJsonMessage msg);

        public event BleMessageReceived OnMessageReceived;
        public event BleJsonMessageReceived OnJsonMessageReceived;

        /// <summary>
        /// The method that the Java library will send their byte data to.
        /// </summary>
        public void OnBleMessage(byte[] data)
        {
            int type = data[0];

            int capacity = data.Length - 1;

            byte[] messageBlock = new byte[capacity];

            Buffer.BlockCopy(data, 0, messageBlock, 1, capacity);
            OnMessageReceived?.Invoke(new BleMessage((MessageType)type, messageBlock));
        }
        /// <summary>
        /// The method that the Java library will send their json data to.
        /// </summary>
        public void OnBleStringMessage(string data)
        {
            Debug.LogError($"Incoming message type {data}");
            BleJsonMessage message = JsonConvert.DeserializeObject<BleJsonMessage>(data);
            OnJsonMessageReceived?.Invoke(message);
        }

        /// <summary>
        /// The method that the Java library will send their logs to.
        /// </summary>
        public void OnLogMessage(string msg) => Debug.Log(msg);

        /// <summary>
        /// The method that the Java library will send their errors to.
        /// </summary>
        public void OnLogErrorMessage(string msg) => Debug.LogError(msg);
    }
}
