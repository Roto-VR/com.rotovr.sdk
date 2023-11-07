using System;
using System.Collections.Generic;
using RotoVR.SDK.BLE;
using RotoVR.SDK.Message;

namespace RotoVR.SDK.Receiver
{
    public class MessageReceiver : IMessageReceiver
    {
        BleAdapter m_BleAdapter;
      
        Dictionary<string, List<Action<string>>> m_JsonSubscribers = new();

        public MessageReceiver(BleAdapter bleAdapter)
        {
            m_BleAdapter = bleAdapter;
            m_BleAdapter.OnJsonMessageReceived += OnJsonMessageReceivedHandler;
        }

        void OnJsonMessageReceivedHandler(BleJsonMessage msg)
        {
            if (m_JsonSubscribers.TryGetValue(msg.Command, out var list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Invoke(msg.Data);
                }
            }
        }

        public void Subscribe(string command, Action<string> action)
        {
            if (!m_JsonSubscribers.TryGetValue(command, out var list))
            {
                list = new List<Action<string>>();
                m_JsonSubscribers.Add(command, list);
            }

            list.Add(action);
        }

        public void UnSubscribe(string command, Action<string> action)
        {
            if (m_JsonSubscribers.TryGetValue(command, out var list))
            {
                list.Remove(action);
            }
        }

        public void Dispose()
        {
            m_BleAdapter.OnJsonMessageReceived -= OnJsonMessageReceivedHandler;
            m_JsonSubscribers.Clear();
        }
    }
}
