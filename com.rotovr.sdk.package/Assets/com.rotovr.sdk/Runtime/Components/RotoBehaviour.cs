using System;
using RotoVR.SDK.API;
using RotoVR.SDK.Enum;
using RotoVR.SDK.Model;
using UnityEngine;


namespace RotoVR.SDK.Components
{
    public class RotoBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Setup on the component in a scene roto vr device name
        /// </summary>
        [SerializeField] string m_DeviceName = "rotoVR Base Station";

        /// <summary>
        /// Setup on the component in a scene working mode
        /// </summary>
        [SerializeField] ModeType m_ModeType;

        /// <summary>
        /// For Head Tracking Move need to setup a target to observe a rotation
        /// </summary>
        [SerializeField] Transform m_Target;

        Roto m_Roto;
        bool m_IsInit;
        public Transform Target => m_Target;

        /// <summary>
        /// Action invoke when the system connection status changed
        /// </summary>
        public event Action<ConnectionStatus> OnConnectionStatusChanged;

        /// <summary>
        /// Action invoke when the system mode type changed
        /// </summary>
        public event Action<ModeType> OnModeChanged;

        /// <summary>
        /// Invoke when a chare data changed
        /// </summary>
        public event Action<RotoDataModel> OnDataChanged;

        float m_StartTargetAngle = 0;

        void Awake()
        {
            var behaviour = FindObjectOfType<RotoBehaviour>();
            if (behaviour != null && behaviour != this)
                Destroy(behaviour);

            InitRoto();
        }

        void OnDestroy()
        {
            m_Roto.OnConnectionStatus -= OnConnectionStatusHandler;
            m_Roto.OnRotoMode -= OnRotoModeHandler;
            m_Roto.Disconnect(m_DeviceName);
        }

        /// <summary>
        /// Initialisation of the component
        /// </summary>
        void InitRoto()
        {
            if (m_IsInit)
                return;

            m_IsInit = true;
            m_Roto = Roto.GetManager();
            m_Roto.OnConnectionStatus += OnConnectionStatusHandler;
            m_Roto.OnRotoMode += OnRotoModeHandler;
            m_Roto.OnDataChanged += (data) => { OnDataChanged?.Invoke(data); };
            m_Roto.Initialize();
        }

        void OnRotoModeHandler(ModeType mode)
        {
            switch (mode)
            {
                case ModeType.FreeMode:
                case ModeType.CockpitMode:
                case ModeType.IdleMode:
                case ModeType.Calibration:
                case ModeType.Error:


                    break;
                case ModeType.HeadTrack:

                    break;
            }

            OnModeChanged?.Invoke(mode);
        }

        void OnConnectionStatusHandler(ConnectionStatus status)
        {
            switch (status)
            {
                case ConnectionStatus.Connecting:

                    break;
                case ConnectionStatus.Connected:

                    switch (m_ModeType)
                    {
                        case ModeType.FreeMode:
                            m_Roto.SetMode(m_ModeType, new ModeParametersModel(0, 30));
                            break;
                        case ModeType.HeadTrack:
                            m_Roto.SetMode(m_ModeType, new ModeParametersModel(0, 30));
                            var headCamera = Camera.main;
                            if (headCamera != null)
                                m_Roto.StartHeadTracking(this, headCamera.gameObject.transform);
                            break;
                        case ModeType.CockpitMode:
                            m_Roto.SetMode(m_ModeType, new ModeParametersModel(140, 30));
                            break;
                        case ModeType.SimulationMode:
                            m_Roto.SetMode(ModeType.FreeMode, new ModeParametersModel(0, 100));
                            m_Roto.FollowTarget(this, m_Target);
                            break;
                    }

                    break;
                case ConnectionStatus.Disconnected:

                    break;
            }

            OnConnectionStatusChanged?.Invoke(status);
        }

        /// <summary>
        /// Connect to RotoVR
        /// </summary>
        public void Connect()
        {
            if (!m_IsInit)
                InitRoto();
            m_Roto.Connect(m_DeviceName);
        }

        /// <summary>
        /// Calibrate the chair
        /// </summary>
        /// <param name="mode">Calibration mode</param>
        public void Calibration(CalibrationMode mode)
        {
            m_Roto.Calibration(mode);
        }

        /// <summary>
        /// Rotate on angle
        /// </summary>
        /// <param name="direction">Direction of rotation</param>
        /// <param name="angle">Angle which we need rotate the chair on</param>
        /// <param name="power">Rotational power. In range 0-100</param>
        public void RotateOnAngle(Direction direction, int angle, int power) =>
            m_Roto.RotateOnAngle(direction, angle, power);

        /// <summary>
        /// Rotate to angle
        /// </summary>
        /// <param name="direction">Direction of rotation</param>
        /// <param name="angle">Angle which we need rotate the chair to</param>
        /// <param name="power">Rotational power. In range 0-100</param>
        public void RotateToAngle(Direction direction, int angle, int power) =>
            m_Roto.RotateToAngle(direction, angle, power);

        public void RotateToAngleByCloserDirection(int angle, int power) =>
            m_Roto.RotateToAngleCloserDirection(angle, power);

        /// <summary>
        /// Play rumble
        /// </summary>
        /// <param name="time">Duration</param>
        /// <param name="power">Power</param>
        public void Rumble(float time, int power) => m_Roto.Rumble(time, power);

        /// <summary>
        /// Switch RotoVr mode 
        /// </summary>
        /// <param name="mode">New mode</param>
        public void SwitchMode(ModeType mode)
        {
            m_Roto.StopRoutine(this);

            switch (mode)
            {
                case ModeType.FreeMode:
                    m_Roto.SetMode(mode, new ModeParametersModel(0, 30));
                    break;
                case ModeType.HeadTrack:
                    OnModeChanged += OnModeChangedHandler;

                    m_Roto.SetMode(mode, new ModeParametersModel(0, 30));
                    var headCamera = Camera.main;
                    if (headCamera != null)
                        m_Roto.StartHeadTracking(this, headCamera.gameObject.transform);
                    break;
                case ModeType.CockpitMode:
                    m_Roto.SetMode(mode, new ModeParametersModel(140, 30));
                    break;
                case ModeType.SimulationMode:
                    m_Roto.SetMode(ModeType.FreeMode, new ModeParametersModel(0, 100));
                    m_Roto.FollowTarget(this, m_Target);
                    break;
            }


            void OnModeChangedHandler(ModeType newMode)
            {
                switch (newMode)
                {
                    case ModeType.HeadTrack:
                        OnModeChanged -= OnModeChangedHandler;
                        m_Roto.StartHeadTracking(this, m_Target);
                        break;
                }
            }
        }

        /// <summary>
        /// Switch RotoVr mode with custom parameters
        /// </summary>
        /// <param name="mode">Simulation mode</param>
        /// <param name="parametersModel">Mode parameters</param>
        public void SwitchMode(ModeType mode, ModeParametersModel parametersModel)
        {
            m_Roto.StopRoutine(this);

            switch (mode)
            {
                case ModeType.FreeMode:
                    m_Roto.SetMode(mode, parametersModel);
                    break;
                case ModeType.HeadTrack:
                    OnModeChanged += OnModeChangedHandler;

                    m_Roto.SetMode(mode, parametersModel);
                    var headCamera = Camera.main;
                    if (headCamera != null)
                        m_Roto.StartHeadTracking(this, headCamera.gameObject.transform);
                    break;
                case ModeType.CockpitMode:
                    m_Roto.SetMode(mode, parametersModel);
                    break;
                case ModeType.SimulationMode:
                    m_Roto.SetMode(ModeType.FreeMode, parametersModel);
                    m_Roto.FollowTarget(this, m_Target);
                    break;
            }


            void OnModeChangedHandler(ModeType newMode)
            {
                switch (newMode)
                {
                    case ModeType.HeadTrack:
                        OnModeChanged -= OnModeChangedHandler;
                        m_Roto.StartHeadTracking(this, m_Target);
                        break;
                }
            }
        }
    }
}