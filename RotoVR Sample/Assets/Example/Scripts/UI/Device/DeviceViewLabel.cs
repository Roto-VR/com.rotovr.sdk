using UnityEngine;
using UnityEngine.UI;

namespace Example.UI.Device
{
    public class DeviceViewLabel : MonoBehaviour
    {
        private string m_DeviceUuid = string.Empty;
        private string m_DeviceName = string.Empty;

        [SerializeField]
        private Text m_DeviceUuidTextLabel;
        [SerializeField]
        private Text m_DeviceNameTextLabel;
        [SerializeField]
        private Button m_DeviceButton;

        public void Show(string uuid, string name)
        {
            m_DeviceUuidTextLabel.text = uuid;
            m_DeviceNameTextLabel.text = name;
        }

        public void Connect()
        {
            // if (!_isConnected)
            // {
            //     // _connectCommand = new ConnectToDevice(_deviceUuid, OnConnected, OnDisconnected);
            //     // BleManager.Instance.QueueCommand(_connectCommand);
            // }
            // else
            // {
            //     //  _connectCommand.Disconnect();
            // }
        }

        public void SubscribeToExampleService()
        {
            // //Replace these Characteristics with YOUR device's characteristics
            // _readFromCharacteristic = new ReadFromCharacteristic(_deviceUuid, "180c", "2a56", (byte[] value) =>
            // {
            //     Debug.Log(Encoding.UTF8.GetString(value));
            // });
            // BleManager.Instance.QueueCommand(_readFromCharacteristic);
        }

        private void OnConnected(string deviceUuid)
        {
            // _previousColor = _deviceButtonImage.color;
            // _deviceButtonImage.color = _onConnectedColor;
            //
            // _isConnected = true;
            // _deviceButtonText.text = "Disconnect";
            //
            // SubscribeToExampleService();
        }

        private void OnDisconnected(string deviceUuid)
        {
            // _deviceButtonImage.color = _previousColor;
            //
            // _isConnected = false;
            // _deviceButtonText.text = "Connect";
        }
    }
}
