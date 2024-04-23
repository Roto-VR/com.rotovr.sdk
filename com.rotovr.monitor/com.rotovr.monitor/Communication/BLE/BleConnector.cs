using RotoVR.Communication.Enum;

namespace RotoVR.Communication.BLE
{
    internal class BleConnector : IConnector
    {
        public event Action<string> OnSystemLog;
        public event Action<ConnectionStatus> OnConnectionStatus;

        public void Connect()
        {
        }

        public void Disconnect()
        {
        }
    }
}