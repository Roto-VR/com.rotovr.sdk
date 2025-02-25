using UnityEngine;

namespace com.rotovr.sdk.sample
{
    /// <summary>
    /// Proxy class for RotoBehaviour
    /// </summary>
    public class RotoBehaviourProxy : MonoBehaviour
    {
        public RotoBehaviour RotoBehaviour { get; private set; }

        [SerializeField] RotoModeType m_Mode;
        [SerializeField] ConnectionType m_ConnectionType;
        [SerializeField] string m_DeviceName = "rotoVR Base Station";
        [SerializeField] Transform m_Target;

        internal RotoModeType Mode
        {
            get => RotoBehaviour != null ? RotoBehaviour.Mode : m_Mode;

            set
            {
                if (RotoBehaviour != null)
                {
                    RotoBehaviour.Mode = value;
                }
                else
                {
                    m_Mode = value;
                }
            }
        }
        
        internal string DeviceName
        {
            get =>  RotoBehaviour != null ? RotoBehaviour.DeviceName : m_DeviceName;
            set
            {
                if (RotoBehaviour != null)
                {
                    RotoBehaviour.DeviceName = value;
                }
                else
                {
                    m_DeviceName = value;
                }
            }
        }
        
        public ConnectionType ConnectionType
        {
            get =>  RotoBehaviour != null ? RotoBehaviour.ConnectionType : m_ConnectionType;
            set
            {
                if (RotoBehaviour != null)
                {
                    RotoBehaviour.ConnectionType = value;
                }
                else
                {
                    m_ConnectionType = value;
                }
            }
        }
        
#if !NO_UNITY
        public Transform Target
        {
            get =>  RotoBehaviour != null ? RotoBehaviour.Target : m_Target;
            set
            {
                if (RotoBehaviour != null)
                {
                    RotoBehaviour.Target = value;
                }
                else
                {
                    m_Target = value;
                }
            }
        }
#else
        public Transform Target;
#endif

        void Awake()
        {
#if !NO_UNITY
            RotoBehaviour = GetComponent<RotoBehaviour>() != null 
                ? GetComponent<RotoBehaviour>() 
                : gameObject.AddComponent<RotoBehaviour>();
#else
            RotoBehaviour = new RotoBehaviour();
            UsbConnector.Instance.SetMainThreadDispatcher(SampleUnityMainThreadDispatcher.Instance());
#endif
            RotoBehaviour.DeviceName = m_DeviceName;
            RotoBehaviour.ConnectionType = m_ConnectionType;
            RotoBehaviour.Mode = m_Mode;
#if !NO_UNITY
            RotoBehaviour.Target = m_Target;
            RotoBehaviour.InitRoto();
#endif
           
            
        }
    }
}
