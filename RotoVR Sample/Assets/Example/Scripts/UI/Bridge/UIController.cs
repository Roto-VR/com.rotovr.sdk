using Example.BLE.API;
using Example.BLE.Enum;
using Example.BLE.Model;
using Example.UI.Device;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Example.UI.Bridge
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        Button m_ScanButton;
        [SerializeField]
        DeviceViewLabel m_SourceLabel;
        [SerializeField]
        Transform m_DeviseLabelsPanel;

        void Awake()
        {
            API.Initialize();

            API.Subscribe(MessageType.DeviceFound.ToString(), DeviceFoundHandler);

            m_ScanButton.onClick.AddListener(() =>
            {
                API.Scan();
            });
        }

        void DeviceFoundHandler(string data)
        {
            DeviceDataModel model = JsonConvert.DeserializeObject<DeviceDataModel>(data);

            DeviceViewLabel label = Instantiate(m_SourceLabel, m_DeviseLabelsPanel).GetComponent<DeviceViewLabel>();
            label.Show(model.Address, model.Name);
        }
    }
}
