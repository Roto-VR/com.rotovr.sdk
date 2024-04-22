using System;
using RotoVR.Communication.USB;

namespace RotoVR.Core
{
    public class Bootstrapper : IBootstrapper
    {
        private IMonitor m_monitor;

        public Bootstrapper(IMonitor monitor)
        {
            m_monitor = monitor;
        }

        public void Bootstrap(Action complete)
        {
            m_monitor.BindConnector(new UsbConnector());
            complete?.Invoke();
        }
    }
}