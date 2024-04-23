using RotoVR.Communication.Enum;

namespace RotoVR.Communication
{
    public interface IConnector
    {
        event Action<string> OnSystemLog;
        event Action<ConnectionStatus> OnConnectionStatus; 
        void Connect();
        void Disconnect();

    }
}