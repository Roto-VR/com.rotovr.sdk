using System;
using Example.BLE.Enum;

namespace Example.BLE.Receiver
{
    public interface IMessageReceiver : IDisposable
    {
        void Subscribe(MessageType type, Action<byte[]> action);
        void UnSubscribe(MessageType type, Action<byte[]> action);
        void Subscribe(string command, Action<string> action);
        void UnSubscribe(string command, Action<string> action);
    }
}
