using SimpleUdp;
using System.Text;

namespace RotoVR.Communication.Network
{
    public class UdpService : INetworkService
    {
        public event Action<string> OnSystemLog;
        public event Action<byte[]> OnMessage;

        UdpEndpoint m_udpServer;
        private int m_port = 56686;

        public void Start()
        {
            m_udpServer = new UdpEndpoint("127.0.0.1", m_port);
            m_udpServer.EndpointDetected += EndpointDetected;
            m_udpServer.DatagramReceived += DatagramReceived;
        }

        public void Stop()
        {
            m_udpServer.EndpointDetected -= EndpointDetected;
            m_udpServer.DatagramReceived -= DatagramReceived;
            m_udpServer.Dispose();     
        }

        void EndpointDetected(object sender, EndpointMetadata md)
        {
            Console.WriteLine("Endpoint detected: " + md.Ip + ":" + md.Port);
        }

        void DatagramReceived(object sender, Datagram dg)
        {        
            OnMessage?.Invoke(dg.Data);
        }
    }
}
