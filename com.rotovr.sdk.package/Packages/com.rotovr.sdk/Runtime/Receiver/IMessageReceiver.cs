using System;

namespace RotoVR.SDK.Receiver
{
    public interface IMessageReceiver : IDisposable
    {
        void Subscribe(string command, Action<string> action);
        void UnSubscribe(string command, Action<string> action);
    }
}
