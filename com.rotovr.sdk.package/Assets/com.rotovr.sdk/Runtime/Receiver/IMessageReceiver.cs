using System;

namespace com.rotovr.sdk
{
    interface IMessageReceiver : IDisposable
    {
        void Subscribe(string command, Action<string> action);
        void UnSubscribe(string command, Action<string> action);
    }
}
