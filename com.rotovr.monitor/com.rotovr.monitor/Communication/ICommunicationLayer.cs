using RotoVR.Communication.Enum;

namespace RotoVR.Communication;

public interface ICommunicationLayer
{
    event Action<string> OnSystemLog;
    event Action<ConnectionStatus> OnConnectionStatus;
    void Connect(CommunicationType type);
    void Disconnect();
}