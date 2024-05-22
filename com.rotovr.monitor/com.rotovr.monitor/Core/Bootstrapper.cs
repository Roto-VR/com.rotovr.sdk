using RotoVR.Communication;
using RotoVR.MotionCompensation;

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
            m_monitor.BindConnectionLayer(new CommunicationLayer());
            m_monitor.BindCompensationBridge(new CompensationBridge());
            complete?.Invoke();
        }
    }
}