using RotoVR.Common.Model;
using RotoVR.Communication.Enum;
using RotoVR.Communication.TCP;
using RotoVR.Communication.UDP;
using RotoVR.Communication.USB;
using RotoVR.MotionCompensation;

namespace RotoVR.Communication;

public class CommunicationLayer : ICommunicationLayer
{
    private IConnector m_usbConnector = new UsbConnector();
    private ITcpService m_TcpService = new TcpService();
    private ICompensationBridge m_compensationBridge;

    public event Action<ConnectionStatus> OnConnectionStatus;


    public void Start()
    {
        m_usbConnector.OnConnectionStatus += ConnectionStatusHandler;    
        m_TcpService.OnMessage += OnTcpMessageHandler;
        m_TcpService.Start();
    }

    public void Stop()
    {
        m_usbConnector.Disconnect();

       
        m_usbConnector.OnConnectionStatus -= ConnectionStatusHandler;
        m_TcpService.OnMessage -= OnTcpMessageHandler;
        m_TcpService.Stop();
    }

    public void Inject(ICompensationBridge bridge)
    {
        m_compensationBridge = bridge;
    }

    private void ConnectionStatusHandler(ConnectionStatus status)
    {
        OnConnectionStatus?.Invoke(status);

        Console.WriteLine($"Change connection status: {status}");

        switch (status)
        {
            case ConnectionStatus.Connected:
                m_usbConnector.OnReadData += OnReadDataHandler;
                m_compensationBridge.Start();
                break;
            case ConnectionStatus.Disconnected:
                m_usbConnector.OnReadData -= OnReadDataHandler;
                m_compensationBridge.Stop();
                   
             
                break;
        }
    }

    private void OnReadDataHandler(RotoDataModel data)
    {
        m_compensationBridge.SetRotoData(data);
    }

    private void OnTcpMessageHandler(byte[] rawData)
    {
        m_usbConnector.MessageDelivery(rawData);
    }
}