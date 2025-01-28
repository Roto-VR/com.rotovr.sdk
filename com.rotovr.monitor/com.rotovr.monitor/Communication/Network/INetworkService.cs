namespace RotoVR.Communication.Network;

public interface INetworkService
{
    event Action<string> OnSystemLog;
    event Action<byte[]> OnMessage;
    void Start();
    void Stop();
    
}