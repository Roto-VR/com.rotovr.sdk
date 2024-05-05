using RotoVR.Common.Model;
using RotoVR.Communication.BLE;
using RotoVR.Communication.Enum;
using RotoVR.Communication.TCP;
using RotoVR.Communication.UDP;
using RotoVR.Communication.USB;
using RotoVR.MotionCompensation;

namespace RotoVR.Communication;

public class CommunicationLayer : ICommunicationLayer
{
    private IConnector m_usbConnector = new UsbConnector();
    private IConnector m_bleConnector = new BleConnector();
    private ITcpService m_TcpService = new TcpService();
    private CommunicationType m_communicationType;
    private ICompensationBridge m_compensationBridge;

    public event Action<ConnectionStatus> OnConnectionStatus;


    public void Start()
    {
        m_communicationType = CommunicationType.Usb;

        m_usbConnector.OnConnectionStatus += ConnectionStatusHandler;
        m_usbConnector.OnCompensationModel += OnCompensationModelHandler;
        m_usbConnector.OnReadData += OnReadDataHandler;

        m_TcpService.OnMessage += OnTcpMessageHandler;
        m_TcpService.Start();
    }

    public void Stop()
    {
        m_usbConnector.Disconnect();
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
        switch (status)
        {
            case ConnectionStatus.Connected:
                m_compensationBridge.Start();
                break;
            case ConnectionStatus.Disconnected:
                m_compensationBridge.Stop();
                m_usbConnector.OnConnectionStatus -= ConnectionStatusHandler;
                m_usbConnector.OnCompensationModel -= OnCompensationModelHandler;
                m_usbConnector.OnReadData -= OnReadDataHandler;
                break;
        }
    }

    private void OnReadDataHandler(RotoDataModel data)
    {
        m_compensationBridge.SetRotoData(data);
    }

    private void OnCompensationModelHandler(CompensationModel model)
    {
        m_compensationBridge.SetCompensationValue(model);
    }


    private void OnTcpMessageHandler(byte[] rawData)
    {
        m_usbConnector.MessageDelivery(rawData);
    }
}