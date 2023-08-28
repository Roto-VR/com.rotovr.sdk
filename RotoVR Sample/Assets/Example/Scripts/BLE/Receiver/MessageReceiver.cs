using System;
using System.Collections.Generic;
using Example.BLE.Enum;
using Example.BLE.Message;
using UnityEngine;

namespace Example.BLE.Receiver
{
    public class MessageReceiver : IMessageReceiver
    {
        BleAdapter m_BleAdapter;
        Dictionary<MessageType, List<Action<byte[]>>> m_Subscribers = new();
        Dictionary<string, List<Action<string>>> m_JsonSubscribers = new();

        public MessageReceiver(BleAdapter bleAdapter)
        {
            m_BleAdapter = bleAdapter;
            m_BleAdapter.OnMessageReceived += OnMessageReceivedHandler;
            m_BleAdapter.OnJsonMessageReceived += OnJsonMessageReceivedHandler;
        }

        void OnJsonMessageReceivedHandler(BleJsonMessage msg)
        {
            Debug.LogError($"OnJsonMessageReceivedHandler {msg.Command}");
            if (m_JsonSubscribers.TryGetValue(msg.Command, out var list))
            {
                Debug.LogError($"list.Count {list.Count}");

                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Invoke(msg.Data);
                }
            }
        }

        void OnMessageReceivedHandler(BleMessage msg)
        {
            if (m_Subscribers.TryGetValue(msg.MessageType, out var list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Invoke(msg.Data);
                }
            }
        }

        public void Subscribe(MessageType type, Action<byte[]> action)
        {
            if (!m_Subscribers.TryGetValue(type, out var list))
            {
                list = new List<Action<byte[]>>();
                m_Subscribers.Add(type, list);
            }

            list.Add(action);
        }

        public void UnSubscribe(MessageType type, Action<byte[]> action)
        {
            if (m_Subscribers.TryGetValue(type, out var list))
            {
                list.Remove(action);
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

            Debug.LogError($"Subscribe {command}");
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
            m_BleAdapter.OnMessageReceived -= OnMessageReceivedHandler;
            m_Subscribers.Clear();
        }
    }
}
