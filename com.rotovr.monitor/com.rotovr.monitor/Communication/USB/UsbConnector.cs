using RotoVR.Common.Model;
using RotoVR.Communication.Enum;

namespace RotoVR.Communication.USB
{
    public class UsbConnector : IConnector
    {
        const UInt16 k_vid = 0x04D9;
        const UInt16 k_pid = 0xB564;
        IntPtr m_device;
        static int m_messageSize;
        static bool m_initPacket;
        static bool m_reaDevice;
        byte[] m_readMessage = new byte[19];
        static RotoDataModel m_runtimeModel;
        private ConnectionStatus m_connectionStatus;

        public event Action<string> OnSystemLog;
        public event Action<ConnectionStatus> OnConnectionStatus;
        public event Action<RotoDataModel> OnReadData;

        public void Connect()
        {
            IntPtr pDll = UsbNative.LoadLibrary(@"HIDApi.dll");
            ConnectToDevice();
        }

        void ConnectToDevice()
        {
            m_device = UsbNative.OpenFirstHIDDevice(k_vid, k_pid);

            if (m_device == IntPtr.Zero)
                return;

            byte[] feature = ConfigureFeature();
            UsbNative.SetFeature(m_device, ConfigureFeature(), (ushort)feature.Length);
            UsbNative.GetFeature(m_device, feature, 9);

            Task.Run(async () =>
            {
                m_reaDevice = true;

                while (m_reaDevice)
                {
                    await Task.Delay(30);
                    ReadDevice();
                }
            });
        }

        public void Disconnect()
        {
            m_reaDevice = false;
            if (m_device != IntPtr.Zero)
            {
                UsbNative.CloseHIDDevice(m_device);
            }

            ChangeConnectionStatus(ConnectionStatus.Disconnected);
        }

        byte[] ConfigureFeature()
        {
            byte[] array = new byte[9];

            for (int i = 0; i < array.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        array[i] = 0x00;
                        break;
                    case 1:
                        array[i] = 0x01;
                        break;
                    case 2:
                        array[i] = 0x00;
                        break;
                    case 3:
                        array[i] = 0xC2;
                        break;
                    case 4:
                        array[i] = 0x01;
                        break;
                    case 5:
                        array[i] = 0x00;
                        break;
                    case 6:
                        array[i] = 0x01;
                        break;
                    case 7:
                        array[i] = 0x00;
                        break;
                    case 8:
                        array[i] = 0x08;
                        break;
                }
            }

            return array;
        }

        void ChangeConnectionStatus(ConnectionStatus status)
        {
            if (m_connectionStatus != status)
            {
                m_connectionStatus = status;
                OnConnectionStatus?.Invoke(m_connectionStatus);
            }
        }

        void ReadDevice()
        {
            var result = UsbNative.ReadFile(m_device, out var buffer, 33);
            if (!result)
            {
                if (m_connectionStatus == ConnectionStatus.Connected)
                {
                    Disconnect();
                }

                return;
            }

            ChangeConnectionStatus(ConnectionStatus.Connected);

            if (buffer[2] == 0xF1)
            {
                m_initPacket = true;
                for (int i = 0; i < m_readMessage.Length; i++)
                {
                    m_readMessage[i] = 0x00;
                }

                m_messageSize = 0;
                m_messageSize = buffer[1];
                for (int i = 0; i < m_messageSize; i++)
                {
                    m_readMessage[i] = buffer[i + 2];
                }
            }
            else
            {
                if (!m_initPacket)
                    return;

                int startIndex = m_messageSize;
                m_messageSize += buffer[1];

                for (int i = 0; i < buffer[1]; i++)
                {
                    var index = startIndex + i;
                    if (index < m_readMessage.Length)
                        m_readMessage[index] = buffer[i + 2];
                }

                if (m_messageSize >= 19)
                {
                    m_initPacket = false;

                    var sum = ByteSum(m_readMessage);
                    if (sum == m_readMessage[18])
                    {
                        m_runtimeModel = GetModel(m_readMessage);
                        OnReadData?.Invoke(m_runtimeModel);
                        Console.WriteLine(
                            $"ReadDevice: {LogBuffer(m_readMessage)}  chaireAngle: {m_runtimeModel.Angle}");
                    }
                }
            }
        }

        RotoDataModel GetModel(byte[] rawData)
        {
            RotoDataModel model = new RotoDataModel();
            switch (rawData[2])
            {
                case 0:
                    model.Mode = "IdleMode";
                    break;
                case 1:
                    model.Mode = "Calibration";
                    break;
                case 2:
                    model.Mode = "HeadTrack";
                    break;
                case 3:
                    model.Mode = "FreeMode";
                    break;
                case 4:
                    model.Mode = "CockpitMode";
                    break;
                case 5:
                    model.Mode = "Error";
                    break;
            }

            switch (rawData[5])
            {
                case 0:
                    model.Angle = rawData[6];
                    break;
                case 1:
                    int angle = rawData[6];
                    model.Angle = (angle + 256);
                    break;
            }

            return model;
        }

        string LogBuffer(byte[] data)
        {
            if (data.Length == 0)
                return "Buffer is Empty";

            string message = $"";
            message = $"{message} {BitConverter.ToString(data).Replace("-", "  ")}";

            return message;
        }

        byte ByteSum(byte[] blk)
        {
            byte sum = 0;

            for (int i = 0; i <= 17; i++)
            {
                sum = (byte)((sum + blk[i]) & 0xff);
            }

            return sum;
        }
    }
}