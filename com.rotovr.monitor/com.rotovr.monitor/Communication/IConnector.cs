using RotoVR.Common.Model;
using RotoVR.Communication.Enum;

namespace RotoVR.Communication
{
    public interface IConnector
    {
        event Action<string> OnSystemLog;
        event Action<ConnectionStatus> OnConnectionStatus;

        event Action<RotoDataModel> OnReadData;
        void Connect();
        void Disconnect();
        void MessageDelivery(byte[] rawData);
    }
}