
using System;

#if !NO_UNITY
using UnityEngine;
#endif

namespace com.rotovr.sdk
{
#if NO_UNITY
    public class RotoBehaviour 
   
#else
    public class RotoBehaviour : MonoSingleton<RotoBehaviour>
#endif
    {
        /// <summary>
        /// Behaviour mode. Works only in an editor. Select Runtime if you have rotoVR chair, select Simulation if you don't have the chair and want to simulate it behaviour
        /// </summary>
#if !NO_UNITY
        [SerializeField] 
#endif
        ConnectionType m_ConnectionType;

        internal ConnectionType ConnectionType
        {
            get => m_ConnectionType;
            set => m_ConnectionType = value;
        }

        /// <summary>
        /// Setup on the component in a scene roto vr device name
        /// </summary>
#if !NO_UNITY
        [SerializeField] 
#endif     
        string m_DeviceName = "rotoVR Base Station";
        
        public string DeviceName
        {
            get => m_DeviceName;
            set => m_DeviceName = value;
        }

        /// <summary>
        /// Setup on the component in a scene working mode
        /// </summary>
#if !NO_UNITY
        [SerializeField] 
#endif
        RotoModeType m_ModeType;
        
        internal RotoModeType Mode
        {
            get => m_ModeType;
            set => m_ModeType = value;
        }

#if !NO_UNITY
        /// <summary>
        /// For Head Tracking Move need to setup a target to observe a rotation
        /// </summary>
        [SerializeField] Transform m_Target;
        
        public Transform Target
        {
            get => m_Target;
            set => m_Target = value;
        }
#endif
        
        Roto m_Roto;
        bool m_IsInit;
       

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

#if !NO_UNITY 
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
#else
        public RotoBehaviour()
        {
            InitRoto();
        }
#endif
        
        //TODO why do we even need it, of that's a singleton.
        /*
        void OnDestroy()
        {
            m_Roto.OnConnectionStatus -= OnConnectionStatusHandler;
            m_Roto.OnRotoMode -= OnRotoModeHandler;
            m_Roto.Disconnect(m_DeviceName);
        }*/

        /// <summary>
        /// Initialisation of the component
        /// </summary>
       
#if NO_UNITY 
        public 
#endif
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
                            m_Roto.SetMode(ModeType.FreeMode, new ModeParams {CockpitAngleLimit = 0, MaxPower = 30});
                            break;
                        case RotoModeType.CockpitMode:
                            m_Roto.SetMode(ModeType.CockpitMode, new ModeParams {CockpitAngleLimit = 140, MaxPower = 30});
                            break;

#if !NO_UNITY
                        case RotoModeType.HeadTrack:
                            m_Roto.SetMode(ModeType.HeadTrack, new ModeParams {CockpitAngleLimit = 0, MaxPower = 30});
                            m_Roto.StartHeadTracking(this, m_Target);
                            break;
                       
                        case RotoModeType.FollowObject:
                            m_Roto.SetMode(ModeType.HeadTrack, new ModeParams {CockpitAngleLimit = 0, MaxPower = 100});
                            m_Roto.FollowTarget(this, m_Target);
                            break;
#endif
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
#if !NO_UNITY
           m_Roto.StopRoutine(this);
#endif

            switch (mode)
            {
                case ModeType.FreeMode:
                    m_Roto.SetMode(mode, new ModeParams {CockpitAngleLimit = 0, MaxPower = 30});
                    OnModeChanged?.Invoke(mode);
                    break;
                case ModeType.CockpitMode:
                    m_Roto.SetMode(mode, new ModeParams {CockpitAngleLimit = 140, MaxPower = 30});
                    break;
#if !NO_UNITY
                case ModeType.HeadTrack:
                    m_Roto.SetMode(mode, new ModeParams {CockpitAngleLimit = 0, MaxPower = 30});
                    m_Roto.StartHeadTracking(this, m_Target);
                    break;
              
                case ModeType.FollowObject:
                    m_Roto.SetMode(mode, new ModeParams {CockpitAngleLimit = 0, MaxPower = 100});
                    m_Roto.FollowTarget(this, m_Target);
                    OnModeChanged?.Invoke(mode);
                    break;
#endif
            }
        }

        /// <summary>
        /// Switch RotoVR mode with custom parameters.
        /// </summary>
        /// <param name="mode">Simulation mode.</param>
        /// <param name="modeParams">Mode parameters.</param>
        public void SwitchMode(ModeType mode, ModeParams modeParams)
        {
#if !NO_UNITY
            m_Roto.StopRoutine(this);
#endif
            switch (mode)
            {
                case ModeType.FreeMode:
                    m_Roto.SetMode(mode, modeParams);
                    break;
                case ModeType.CockpitMode:
                    m_Roto.SetMode(mode, modeParams);
                    break;
#if !NO_UNITY
                case ModeType.HeadTrack:
                    m_Roto.SetMode(mode, modeParams);
                    m_Roto.StartHeadTracking(this, m_Target);
                    break;
                case ModeType.FollowObject:
                    m_Roto.SetMode(ModeType.HeadTrack, modeParams);
                    m_Roto.FollowTarget(this, m_Target);
                    OnModeChanged?.Invoke(mode);
                    break;
#endif
            }
        }

        /// <summary>
        /// Set RotoVR power. Working only in Free Mode 
        /// </summary>
        /// <param name="power">Value of rotation power in range 30-100</param>
        public void SetPower(int power) => m_Roto.SetPower(power);
    }
}
