using System;
using System.Collections.Generic;
using Example.BLE.API;
using Example.BLE.Enum;
using Example.BLE.Message;
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
        MainMenuBlock m_MainMenuBlock;
        [SerializeField]
        RotoVrBlock m_RotoVrBlock;
        List<DeviceViewLabel> m_DeviceLabelList = new();
        DeviceDataModel m_CurrentDeviceModel;

        void Awake()
        {
            API.Initialize();

            API.Subscribe(MessageType.DeviceFound.ToString(), DeviceFoundHandler);
            API.Subscribe(MessageType.Connected.ToString(), DeviceConnectedHandler);
            API.Subscribe(MessageType.Disconnected.ToString(), DeviceDisconnectedConnectedHandler);

            m_MainMenuBlock.ScanButton.onClick.AddListener(API.Scan);

            m_RotoVrBlock.DisconnectButton.onClick.AddListener(() =>
            {
                API.Disconnect(JsonConvert.SerializeObject(m_CurrentDeviceModel));
            });

            m_RotoVrBlock.TurnLeft.onClick.AddListener(() =>
            {
                API.TurnOnAngle(JsonConvert.SerializeObject(new RotateToAngleModel(20, 100, Direction.Left.ToString())));
            });

            m_RotoVrBlock.TurnRight.onClick.AddListener(() =>
            {
                API.TurnOnAngle(JsonConvert.SerializeObject(new RotateToAngleModel(20, 100, Direction.Right.ToString())));
            });

            SetUIState(UIState.Scan);
        }

        void DeviceFoundHandler(string data)
        {
            DeviceDataModel model = JsonConvert.DeserializeObject<DeviceDataModel>(data);
            DeviceViewLabel label = Instantiate(m_MainMenuBlock.SourceLabel, m_MainMenuBlock.DeviseLabelsPanel).GetComponent<DeviceViewLabel>();
            label.Init(model, ConnectToDevice);

            m_DeviceLabelList.Add(label);
        }

        void ConnectToDevice(DeviceDataModel model)
        {
            m_CurrentDeviceModel = model;
            API.Connect(JsonConvert.SerializeObject(model));
        }

        void DeviceConnectedHandler(string data)
        {
            SetUIState(UIState.RotoVr);
        }

        void DeviceDisconnectedConnectedHandler(string data)
        {
            SetUIState(UIState.Scan);
        }

        void SetUIState(UIState state)
        {
            switch (state)
            {
                case UIState.Scan:
                    m_MainMenuBlock.ConnectionPanel.SetActive(true);
                    m_RotoVrBlock.RotoVrPanel.SetActive(false);
                    break;
                case UIState.RotoVr:

                    foreach (var element in m_DeviceLabelList)
                    {
                        Destroy(element.gameObject);
                    }

                    m_DeviceLabelList.Clear();

                    m_MainMenuBlock.ConnectionPanel.SetActive(false);
                    m_RotoVrBlock.RotoVrPanel.SetActive(true);
                    break;
            }
        }

        public enum UIState
        {
            Scan,
            RotoVr,
        }

        [Serializable]
        public class MainMenuBlock
        {
            public GameObject ConnectionPanel;
            public Button ScanButton;
            public DeviceViewLabel SourceLabel;
            public Transform DeviseLabelsPanel;
        }

        [Serializable]
        public class RotoVrBlock
        {
            public GameObject RotoVrPanel;
            public Button DisconnectButton;
            public Button TurnLeft;
            public Button TurnRight;
        }
    }
}
