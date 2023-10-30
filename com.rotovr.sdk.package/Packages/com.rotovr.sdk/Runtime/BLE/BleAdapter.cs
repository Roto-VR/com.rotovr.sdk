using UnityEngine;
using Newtonsoft.Json;
using RotoVR.SDK.Message;

namespace RotoVR.SDK.BLE
{
    public class BleAdapter : MonoBehaviour
    {
        public delegate void BleJsonMessageReceived(BleJsonMessage msg);

        public event BleJsonMessageReceived OnJsonMessageReceived;

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
