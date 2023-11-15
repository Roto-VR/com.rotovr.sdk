using System;
using RotoVR.SDK.Receiver;
using UnityEngine;

namespace RotoVR.SDK.BLE
{
    public class BleManager : MonoBehaviour
    {
        public static BleManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    var manager = new GameObject("BleManager");
                    s_Instance = manager.AddComponent<BleManager>();
                }

                return s_Instance;
            }
        }

        static BleManager s_Instance;
        BleAdapter m_BleAdapter;
        IMessageReceiver m_MessageReceiver;
        bool m_IsInitialized;
        internal static AndroidJavaClass m_AndroidClass;
        internal static AndroidJavaObject m_AndroidLibrary = null;

        void OnDestroy() => Dispose();

        public void Init()
        {
            if (m_IsInitialized)
                return;

            var adapter = new GameObject(nameof(BleAdapter));
            m_BleAdapter = adapter.AddComponent<BleAdapter>();
            adapter.transform.SetParent(transform);

            InitMessageReceiver(m_BleAdapter);
            InitAndroidLibrary();

            m_IsInitialized = true;
        }

        void InitMessageReceiver(BleAdapter bleAdapter)
        {
            m_MessageReceiver = new MessageReceiver(bleAdapter);
        }

        void InitAndroidLibrary()
        {
            m_AndroidClass = new AndroidJavaClass("com.rotovr.unitybleplugin.BlePluginInstance");
            m_AndroidLibrary = m_AndroidClass.CallStatic<AndroidJavaObject>("GetInstance");
        }

        void Dispose()
        {
            m_AndroidLibrary?.Dispose();
            m_MessageReceiver?.Dispose();
            if (m_BleAdapter != null)
                Destroy(m_BleAdapter.gameObject);
        }

        public void Call(string command, string data)
        {
            if (string.IsNullOrEmpty(data))
                m_AndroidLibrary?.Call(command);
            else
                m_AndroidLibrary?.Call(command, data);
        }

        public void Subscribe(string command, Action<string> action) => m_MessageReceiver.Subscribe(command, action);

        public void UnSubscribe(string command, Action<string> action) =>
            m_MessageReceiver.UnSubscribe(command, action);
    }
}