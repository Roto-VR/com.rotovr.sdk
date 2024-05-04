using RotoVR.Communication.Enum;

namespace RotoVR.Communication;

public interface ICommunicationLayer
{
    event Action<string> OnSystemLog;
    event Action<ConnectionStatus> OnConnectionStatus;
    void Start();
    void Stop();
    void Connect(CommunicationType type);
    void Disconnect();
}