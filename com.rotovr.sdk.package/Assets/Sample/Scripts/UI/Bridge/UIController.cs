using System;
using System.Collections.Generic;
using Example.UI.Device;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using RotoVR.SDK.API;
using RotoVR.SDK.Enum;
using RotoVR.SDK.Model;

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
        int m_CurrentAngle;

        void Awake()
        {
            API.Initialize();

            API.Subscribe(MessageType.DeviceFound.ToString(), DeviceFoundHandler);
            API.Subscribe(MessageType.Connected.ToString(), DeviceConnectedHandler);
            API.Subscribe(MessageType.Disconnected.ToString(), DeviceDisconnectedConnectedHandler);


            m_RotoVrBlock.DisconnectButton.onClick.AddListener(() =>
            {
                API.Disconnect(JsonConvert.SerializeObject(m_CurrentDeviceModel));
            });

            m_RotoVrBlock.FreeModeButton.onClick.AddListener(() =>
            {
                API.SetMode(ModeType.FreeMode);
                SetVrMode(VrMode.RotateOn);
            });

            m_RotoVrBlock.CalibrationButton.onClick.AddListener(() =>
            {
                m_CurrentAngle = 0;
                API.Calibration();
                SetVrMode(VrMode.RotateOn);
            });

            m_RotoVrBlock.TurnLeft.onClick.AddListener(() =>
            {
                m_CurrentAngle -= 45;
                if (m_CurrentAngle < 0)
                    m_CurrentAngle = m_CurrentAngle += 360;
                API.TurnOnAngle(JsonConvert.SerializeObject(new RotateToAngleModel(m_CurrentAngle, 100, Direction.Left.ToString())));
            });

            m_RotoVrBlock.TurnRight.onClick.AddListener(() =>
            {
                m_CurrentAngle += 45;
                if (m_CurrentAngle > 360)
                    m_CurrentAngle -= 360;
                API.TurnOnAngle(JsonConvert.SerializeObject(new RotateToAngleModel(m_CurrentAngle, 100, Direction.Right.ToString())));
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
            SetUIState(UIState.Vr);
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
                case UIState.Vr:

                    foreach (var element in m_DeviceLabelList)
                    {
                        Destroy(element.gameObject);
                    }

                    m_DeviceLabelList.Clear();

                    m_MainMenuBlock.ConnectionPanel.SetActive(false);
                    m_RotoVrBlock.RotoVrPanel.SetActive(true);

                    SetVrMode(VrMode.WorkMode);

                    break;
            }
        }

        void SetVrMode(VrMode mode)
        {
            switch (mode)
            {
                case VrMode.Calibration:
                    //  m_RotoVrBlock.FreeModeButton.gameObject.SetActive(true);
                    m_RotoVrBlock.CalibrationButton.gameObject.SetActive(true);
                    m_RotoVrBlock.TurnLeft.gameObject.SetActive(false);
                    m_RotoVrBlock.TurnRight.gameObject.SetActive(false);
                    break;
                case VrMode.WorkMode:
                    //  m_RotoVrBlock.FreeModeButton.gameObject.SetActive(true);
                    m_RotoVrBlock.CalibrationButton.gameObject.SetActive(true);
                    m_RotoVrBlock.TurnLeft.gameObject.SetActive(false);
                    m_RotoVrBlock.TurnRight.gameObject.SetActive(false);
                    break;
                case VrMode.RotateOn:
                    // m_RotoVrBlock.FreeModeButton.gameObject.SetActive(false);
                    m_RotoVrBlock.CalibrationButton.gameObject.SetActive(false);
                    m_RotoVrBlock.TurnLeft.gameObject.SetActive(true);
                    m_RotoVrBlock.TurnRight.gameObject.SetActive(true);
                    break;
            }
        }

        public enum UIState
        {
            Scan,
            Vr,
        }

        public enum VrMode
        {
            Calibration,
            WorkMode,
            RotateOn,
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
            public Button FreeModeButton;
            public Button CalibrationButton;
            public Button TurnLeft;
            public Button TurnRight;
        }
    }
}
