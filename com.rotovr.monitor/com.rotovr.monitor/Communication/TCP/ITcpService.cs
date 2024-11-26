namespace RotoVR.Communication.TCP;

public interface ITcpService
{
    event Action<string> OnSystemLog;
    event Action<byte[]> OnMessage;
    void Start();
    void Stop();
    
}