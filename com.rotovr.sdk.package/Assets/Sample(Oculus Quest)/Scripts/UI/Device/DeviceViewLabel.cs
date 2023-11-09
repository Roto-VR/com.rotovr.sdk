using System;
using RotoVR.SDK.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Example.UI.Device
{
    public class DeviceViewLabel : MonoBehaviour
    {
        [SerializeField]
        private Text m_DeviceUuidTextLabel;
        [SerializeField]
        private Text m_DeviceNameTextLabel;
        [SerializeField]
        private Button m_DeviceButton;

        public void Init(DeviceDataModel model, Action<DeviceDataModel> click)
        {
            m_DeviceUuidTextLabel.text = model.Address;
            m_DeviceNameTextLabel.text = model.Name;
            m_DeviceButton.onClick.AddListener(() => click?.Invoke(model));
        }

        void OnDestroy()
        {
            m_DeviceButton.onClick.RemoveAllListeners();
        }
    }
}
