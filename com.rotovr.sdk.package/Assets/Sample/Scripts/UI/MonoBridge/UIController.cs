using System;
using System.Collections.Generic;
using Example.UI.Device;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using RotoVR.SDK.Components;
using RotoVR.SDK.Model;

namespace Example.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] RotoBehaviour m_RotoBerhaviour;

        [SerializeField] MainMenuBlock m_MainMenuBlock;
        [SerializeField] RotoVrBlock m_RotoVrBlock;
        List<DeviceViewLabel> m_DeviceLabelList = new();
        DeviceDataModel m_CurrentDeviceModel;
        int m_CurrentAngle;


        void Awake()
        {
            m_RotoBerhaviour.Connect();

            m_RotoVrBlock.CalibrationButton.onClick.AddListener(() => { SetVrMode(VrMode.RotateOn); });

            m_RotoVrBlock.TurnLeft.onClick.AddListener(() => { });

            m_RotoVrBlock.TurnRight.onClick.AddListener(() => { });

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