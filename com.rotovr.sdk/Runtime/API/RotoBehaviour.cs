using System;

#if !NO_UNITY
using UnityEngine;
#endif

namespace com.rotovr.sdk
{
#if NO_UNITY
    public class RotoBehaviour
#else
    
    /// <summary>
    /// The <see cref="RotoBehaviour"/> class simplifies the integration of the RotoVR SDK into Unity applications.
    /// It provides easy access to controlling and configuring the RotoVR chair's movement, mode, and connection status.
    /// This class handles both the setup and interaction with the RotoVR system, including connection management, mode switching,
    /// and chair movement control.
    /// </summary>
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

        
        /// <summary>
        /// The device name used to identify the RotoVR chair in the scene.
        /// Default is "rotoVR Base Station".
        /// </summary>
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
        [SerializeField] Transform m_Target;
        
        /// <summary>
        /// The target Transform that the chair follows when Head Tracking Mode or Follow Object Mode is enabled.
        /// </summary>
        public Transform Target
        {
            get => m_Target;
            set => m_Target = value;
        }
#endif

        Roto m_Roto;
        bool m_IsInit;

        /// <summary>
        /// The current connection status of the RotoVR chair.
        /// Possible statuses: Disconnected, Connecting, or Connected.
        /// </summary>
        public ConnectionStatus ConnectionStatus
        {
            get
            {
                if (m_Roto == null)
                    return ConnectionStatus.Disconnected;

                return m_Roto.ConnectionStatus;
            }
        }


        /// <summary>
        /// Event triggered when the system connection status changes.
        /// </summary>
        public event Action<ConnectionStatus> OnConnectionStatusChanged;

        /// <summary>
        /// Event triggered when the system mode changes.
        /// </summary>
        public event Action<ModeType> OnModeChanged;

        /// <summary>
        /// Event triggered when chair data changes.
        /// </summary>
        public event Action<RotoDataModel> OnDataChanged;

#if !NO_UNITY
        protected override void Awake()
        {
           base.Awake();
        }
#else
        public RotoBehaviour()
        {
            m_ConnectionType = ConnectionType.Chair;
            InitRoto();
        }
#endif

        /// <summary>
        /// Initializes the RotoBehaviour component. This method must be called after all properties have been set.
        /// It sets up necessary events, and initializes the Roto system with the specified connection type.
        /// </summary>
        public  void InitRoto()
        {
            if (m_IsInit)
                return;

            m_IsInit = true;
            m_Roto = Roto.GetManager();
            m_Roto.OnConnectionStatus += OnConnectionStatusHandler;
            m_Roto.OnRotoMode += OnRotoModeHandler;
            m_Roto.OnDataChanged += OnDataChangedHandler;
            m_Roto.Initialize(m_ConnectionType);
        }

        void OnDataChangedHandler(RotoDataModel data)
        {
            OnDataChanged?.Invoke(data);
        }

        void OnRotoModeHandler(ModeType mode)
        {
            OnModeChanged?.Invoke(mode);
        }

        ModeType GetModeType(RotoModeType rotoModeType)
        {
            switch (m_ModeType)
            {
                case RotoModeType.FreeMode:
                    return ModeType.FreeMode;
                case RotoModeType.CockpitMode:
                    return ModeType.CockpitMode;
                case RotoModeType.HeadTrack:
                    return ModeType.HeadTrack;
                case RotoModeType.FollowObject:
                    return ModeType.FollowObject;
            }

            return ModeType.IdleMode;
        }

        
        /// <summary>
        /// Updates the RotoVR system's behavior according to the current properties.
        /// This method switches the system mode based on the selected <see cref="RotoModeType"/>.
        /// </summary>
        public void UpdatBehaviour()
        {
            var modeType = GetModeType(Mode);
            SwitchMode(modeType);
        }

        void OnConnectionStatusHandler(ConnectionStatus status)
        {
            switch (status)
            {
                case ConnectionStatus.Connecting:

                    break;
                case ConnectionStatus.Connected:
                    UpdatBehaviour();
                    break;
                case ConnectionStatus.Disconnected:
                    break;
            }

            OnConnectionStatusChanged?.Invoke(status);
        }

        /// <summary>
        /// Connects to the RotoVR system using the  device name specified within <see cref="m_DeviceName"/> property.
        /// </summary>
        public void Connect()
        {
            if (!m_IsInit)
                InitRoto();
            m_Roto.Connect(m_DeviceName);
        }

        /// <summary>
        /// Disconnects from the RotoVR chair.
        /// </summary>
        public void Disconnect()
        {
            m_Roto.Disconnect(m_DeviceName);
        }

        /// <summary>
        /// Initiates the calibration process for the RotoVR chair.
        /// </summary>
        /// <param name="mode">The calibration mode to use (e.g., <see cref="CalibrationMode"/>).</param>
        public void Calibration(CalibrationMode mode)
        {
            m_Roto.Calibration(mode);
        }

        /// <summary>
        /// Rotates the chair by a specified angle and direction with the given power.
        /// </summary>
        /// <param name="direction">The direction of rotation (e.g., clockwise or counterclockwise).</param>
        /// <param name="angle">The angle in degrees to rotate the chair.</param>
        /// <param name="power">The power level for rotation, typically between 0 and 100.</param>
        public void Rotate(Direction direction, int angle, int power) =>
            m_Roto.Rotate(direction, angle, power);

        /// <summary>
        /// Rotates the chair to a specific angle in the specified direction with the given power.
        /// </summary>
        /// <param name="direction">The direction of rotation (e.g., clockwise or counterclockwise).</param>
        /// <param name="angle">The target angle to rotate the chair to.</param>
        /// <param name="power">The power level for rotation, typically between 0 and 100.</param>
        public void RotateToAngle(Direction direction, int angle, int power) =>
            m_Roto.RotateToAngle(direction, angle, power);

        /// <summary>
        /// Rotates the chair to the closest angle based on the current direction and power.
        /// </summary>
        /// <param name="angle">The target angle to rotate to.</param>
        /// <param name="power">The power level for rotation, typically between 0 and 100.</param>
        public void RotateToClosestAngleDirection(int angle, int power) =>
            m_Roto.RotateToClosestAngleDirection(angle, power);

        /// <summary>
        /// Activates the rumble feature of the RotoVR chair for a specified duration and power level.
        /// </summary>
        /// <param name="time">The duration for the rumble effect in seconds.</param>
        /// <param name="power">The power level for rumble, typically between 0 and 100.</param>
        public void Rumble(float time, int power) => m_Roto.Rumble(time, power);

        /// <summary>
        /// Switches the RotoVR chair to a specific operational mode (e.g., FreeMode, CockpitMode, etc.).
        /// </summary>
        /// <param name="mode">The mode to switch to.</param>
        public void SwitchMode(ModeType mode)
        {
#if !NO_UNITY
           m_Roto.StopRoutine(this);
#endif

            switch (mode)
            {
                case ModeType.FreeMode:
                    m_Roto.SetMode(mode, new ModeParams { CockpitAngleLimit = 0, MaxPower = 30 });
                    OnModeChanged?.Invoke(mode);
                    break;
                case ModeType.CockpitMode:
                    m_Roto.SetMode(mode, new ModeParams { CockpitAngleLimit = 140, MaxPower = 30 });
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
        /// Switches the RotoVR chair to a specific operational mode with custom parameters.
        /// </summary>
        /// <param name="mode">The mode to switch to (e.g., FreeMode, CockpitMode, etc.).</param>
        /// <param name="modeParams">Custom parameters for the mode.</param>
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
        /// Sets the rotational power of the RotoVR chair. This is only effective in Free Mode.
        /// </summary>
        /// <param name="power">The power level for rotation, typically between 30 and 100.</param>
        public void SetPower(int power) => m_Roto.SetPower(power);
    }
}