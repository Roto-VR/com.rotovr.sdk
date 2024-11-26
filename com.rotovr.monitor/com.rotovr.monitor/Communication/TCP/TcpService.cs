using System.Net;
using Net.TcpServer;
using RotoVR.Communication.TCP;

namespace RotoVR.Communication.UDP;

public class TcpService : ITcpService
{
    public event Action<string> OnSystemLog;
    public event Action<byte[]> OnMessage;
    private int m_port = 56685;

    private TcpServer tcpServer;

    public void Start()
    {
        tcpServer = new TcpServer(IPAddress.Any, m_port);

        tcpServer.Start((connection) =>
        {
            connection.OnAccept = client => { Console.WriteLine($"OnAccept: {client}"); };
            connection.OnReceive = (client, data) => { OnMessage?.Invoke(data); };
            connection.OnError = (client, ex) => { Console.WriteLine($"OnError: {client} {ex.Message}"); };
            connection.OnClose = (client, isCloseByClient) =>
            {
                Console.WriteLine($"OnClose: {client} {(isCloseByClient ? "by client" : "by server")}");
            };
        });
    }


    public void Stop()
    {
        tcpServer.Stop();
        OnSystemLog?.Invoke($"[{nameof(TcpService)}] Server stop");
    }
}