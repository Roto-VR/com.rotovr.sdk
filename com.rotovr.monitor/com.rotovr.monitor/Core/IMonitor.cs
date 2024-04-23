using RotoVR.Communication;

namespace RotoVR.Core
{
    public interface IMonitor
    {
        void BindConnectionLayer(ICommunicationLayer communicationLayer);
    }
}