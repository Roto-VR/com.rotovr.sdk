using RotoVR.Common.Model;
using RotoVR.Communication.Enum;

namespace RotoVR.Communication.BLE
{
    internal class BleConnector : IConnector
    {
        public event Action<string> OnSystemLog;
        public event Action<ConnectionStatus> OnConnectionStatus;
        public event Action<RotoDataModel> OnReadData;

        public void Connect()
        {
            OnConnectionStatus?.Invoke(ConnectionStatus.Connected);
        }

        public void Disconnect()
        {
            OnConnectionStatus?.Invoke(ConnectionStatus.Disconnected);
        }

        public void MessageDelivery(byte[] rawData)
        {
            
        }
    }
}