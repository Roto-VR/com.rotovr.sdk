using RotoVR.Communication;
using RotoVR.MotionCompensation;

namespace RotoVR.Core
{
    public interface IMonitor
    {
        void BindConnectionLayer(ICommunicationLayer communicationLayer);
        void BindCompensationBridge(ICompensationBridge compensationBridge);
    }
}