using RotoVR.Communication;

namespace RotoVR.Core
{
    public interface IMonitor
    {
        void BindConnector(IConnector usbConnector, IConnector bleConnector);
    }
}