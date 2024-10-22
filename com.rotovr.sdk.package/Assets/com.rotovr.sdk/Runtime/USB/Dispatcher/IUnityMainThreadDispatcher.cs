using System;

namespace com.rotovr.sdk
{
    public interface IUnityMainThreadDispatcher
    {
        public void Enqueue(Action action);
    }
}
