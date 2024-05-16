using System;

namespace com.rotovr.sdk
{
    public interface IMessageReceiver : IDisposable
    {
        void Subscribe(string command, Action<string> action);
        void UnSubscribe(string command, Action<string> action);
    }
}
