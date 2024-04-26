using RotoVR.Communication.Enum;
using RotoVR.Communication.Model;

namespace RotoVR.Communication
{
    public interface IConnector
    {
        event Action<string> OnSystemLog;
        event Action<ConnectionStatus> OnConnectionStatus;

        event Action<RotoDataModel> OnReadData;
        void Connect();
        void Disconnect();
    }
}