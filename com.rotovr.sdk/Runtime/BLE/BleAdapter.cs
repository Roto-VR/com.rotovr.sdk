#if !NO_UNITY
using UnityEngine;
#endif

namespace com.rotovr.sdk
{ 
#if NO_UNITY
    class BleAdapter
#else
    class BleAdapter : MonoBehaviour
#endif
    {
        public delegate void BleJsonMessageReceived(BleJsonMessage msg);

        public event BleJsonMessageReceived OnJsonMessageReceived;

        /// <summary>
        /// The method that the Java library will send their json data to.
        /// </summary>
        public void OnBleStringMessage(string data)
        {
            Debug.Log($"Incoming message type {data}");
            var message = new BleJsonMessage(data);
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
