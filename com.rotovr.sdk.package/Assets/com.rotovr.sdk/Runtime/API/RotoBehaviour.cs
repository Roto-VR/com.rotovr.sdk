using System;
using UnityEngine;

namespace com.rotovr.sdk
{
    public class RotoBehaviour : MonoSingleton<RotoBehaviour>
    {
        /// <summary>
        /// Behaviour mode. Works only in an editor. Select Runtime if you have rotoVR chair, select Simulation if you don't have the chair and want to simulate it behaviour
        /// </summary>
        [SerializeField] ConnectionType m_ConnectionType;

        /// <summary>
        /// Setup on the component in a scene roto vr device name
        /// </summary>
        [SerializeField] string m_DeviceName = "rotoVR Base Station";

        /// <summary>
        /// Setup on the component in a scene working mode
        /// </summary>
        [SerializeField] RotoModeType m_ModeType;

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


        protected override void Awake()
        {
            base.Awake();
            InitRoto();
        }


        /// <summary>
        /// An empty function, just to create RotoBehaviour instance via the code
        /// with the default params.
        /// </summary>
        public void Create()
        {
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
            m_Roto.Initialize(m_ConnectionType);
        }

        void OnRotoModeHandler(ModeType mode)
        {
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
                        case RotoModeType.FreeMode:
                            m_Roto.SetMode(ModeType.FreeMode, new ModeParams { CockpitAngleLimit = 0, MaxPower = 30 });
                            break;
                        case RotoModeType.HeadTrack:
                            m_Roto.SetMode(ModeType.HeadTrack, new ModeParams { CockpitAngleLimit = 0, MaxPower = 30 });
                            m_Roto.StartHeadTracking(this, m_Target);
                            break;
                        case RotoModeType.CockpitMode:
                            m_Roto.SetMode(ModeType.CockpitMode, new ModeParams { CockpitAngleLimit = 140, MaxPower = 30 });
                            break;
                        case RotoModeType.FollowObject:
                            m_Roto.SetMode(ModeType.HeadTrack, new ModeParams { CockpitAngleLimit = 0, MaxPower = 100 });
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
        /// Disconnect from RotoVR chair
        /// </summary>
        public void Disconnect()
        {
            m_Roto.Disconnect(m_DeviceName);
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
                    m_Roto.SetMode(mode, new ModeParams { CockpitAngleLimit = 0, MaxPower = 30 });
                    OnModeChanged?.Invoke(mode);
                    break;
                case ModeType.HeadTrack:
                    m_Roto.SetMode(mode, new ModeParams { CockpitAngleLimit = 0, MaxPower = 30 });
                    m_Roto.StartHeadTracking(this, m_Target);
                    break;
                case ModeType.CockpitMode:
                    m_Roto.SetMode(mode, new ModeParams { CockpitAngleLimit = 140, MaxPower = 30 });
                    break;
                case ModeType.FollowObject:
                    m_Roto.SetMode(mode, new ModeParams { CockpitAngleLimit = 0, MaxPower = 100 });
                    m_Roto.FollowTarget(this, m_Target);
                    OnModeChanged?.Invoke(mode);
                    break;
            }
        }

        /// <summary>
        /// Switch RotoVR mode with custom parameters.
        /// </summary>
        /// <param name="mode">Simulation mode.</param>
        /// <param name="modeParams">Mode parameters.</param>
        public void SwitchMode(ModeType mode, ModeParams modeParams)
        {
            m_Roto.StopRoutine(this);

            switch (mode)
            {
                case ModeType.FreeMode:
                    m_Roto.SetMode(mode, modeParams);
                    break;
                case ModeType.HeadTrack:
                    m_Roto.SetMode(mode, modeParams);
                    m_Roto.StartHeadTracking(this, m_Target);
                    break;
                case ModeType.CockpitMode:
                    m_Roto.SetMode(mode, modeParams);
                    break;
                case ModeType.FollowObject:
                    m_Roto.SetMode(ModeType.HeadTrack, modeParams);
                    m_Roto.FollowTarget(this, m_Target);
                    OnModeChanged?.Invoke(mode);
                    break;
            }
        }

        /// <summary>
        /// Set RotoVR power. Working only in Free Mode 
        /// </summary>
        /// <param name="power">Value of rotation power in range 30-100</param>
        public void SetPower(int power) => m_Roto.SetPower(power);
    }
}