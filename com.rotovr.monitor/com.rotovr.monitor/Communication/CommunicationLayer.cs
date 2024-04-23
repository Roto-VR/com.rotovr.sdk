using RotoVR.Communication.BLE;
using RotoVR.Communication.Enum;
using RotoVR.Communication.USB;

namespace RotoVR.Communication;

public class CommunicationLayer : ICommunicationLayer
{
    private IConnector m_usbConnector = new UsbConnector();
    private IConnector m_bleConnector = new BleConnector();
    private CommunicationType m_communicationType;

    public event Action<string> OnSystemLog;
    public event Action<ConnectionStatus> OnConnectionStatus;

    public void Connect(CommunicationType type)
    {
        m_communicationType = type;
        switch (type)
        {
            case CommunicationType.Usb:
                m_usbConnector.OnConnectionStatus += ConnectionStatusHandler;
                m_usbConnector.OnSystemLog += OnSystemLogHandler;
                m_usbConnector.Connect();
                break;
            case CommunicationType.Ble:
                m_bleConnector.OnConnectionStatus += ConnectionStatusHandler;
                m_bleConnector.OnSystemLog += OnSystemLogHandler;
                m_bleConnector.Connect();
                break;
        }
    }

    public void Disconnect()
    {
        switch (m_communicationType)
        {
            case CommunicationType.Usb:
                m_usbConnector.Disconnect();
                break;
            case CommunicationType.Ble:
                m_bleConnector.Disconnect();
                break;
        }
    }

    private void ConnectionStatusHandler(ConnectionStatus status)
    {
        OnConnectionStatus?.Invoke(status);

        if (status == ConnectionStatus.Disconnected)
        {
            switch (m_communicationType)
            {
                case CommunicationType.Usb:
                    m_usbConnector.OnConnectionStatus -= ConnectionStatusHandler;
                    m_usbConnector.OnSystemLog -= OnSystemLogHandler;
                    break;
                case CommunicationType.Ble:
                    m_bleConnector.OnConnectionStatus -= ConnectionStatusHandler;
                    m_bleConnector.OnSystemLog -= OnSystemLogHandler;
                    break;
            }
        }
    }

    private void OnSystemLogHandler(string message)
    {
        OnSystemLog?.Invoke(message);
    }
}