

#if !NO_UNITY
using System;
using UnityEngine;


namespace com.rotovr.sdk
{
    class BleManager : MonoSingleton<BleManager>
    {
       
        BleAdapter m_BleAdapter;
        IMessageReceiver m_MessageReceiver;
        bool m_IsInitialized;
        internal static AndroidJavaClass m_AndroidClass;
        internal static AndroidJavaObject m_AndroidLibrary = null;
        
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

        
        //TODO why do we even need it, of that's a singleton.
        /*
        void Dispose()
        {
            m_AndroidLibrary?.Dispose();
            m_MessageReceiver?.Dispose();
            if (m_BleAdapter != null)
                Destroy(m_BleAdapter.gameObject);
        }*/

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
#endif