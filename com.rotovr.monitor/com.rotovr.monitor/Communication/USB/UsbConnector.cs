namespace RotoVR.Communication.USB
{
    public class UsbConnector : IConnector
    {
        public UsbConnector()
        {
        }

        const UInt16 k_vid = 0x04D9;
        const UInt16 k_pid = 0xB564;
        IntPtr m_device;
        Thread m_connectionThread;
        static int m_messageSize;
        static bool m_initPacket;
        static bool m_reaDevice;
        byte[] m_usbMessage = new byte[19];
        byte[] m_writeBuffer = new byte[33];
        byte[] m_readMessage = new byte[19];
        static RotoDataModel m_runtimeModel;


        public void Connect()
        {
            IntPtr pDll = UsbNative.LoadLibrary(@"HIDApi.dll");
            ConnectToDevice();
        }

        public void Disconnect()
        {
            SendDisconnect(() =>
            {
                if (m_device != IntPtr.Zero)
                {
                    UsbNative.CloseHIDDevice(m_device);
                }

                if (m_connectionThread != null)
                    m_connectionThread.Abort();
            });
        }

        void ConnectToDevice()
        {
            m_device = UsbNative.OpenFirstHIDDevice(k_vid, k_pid);

            if (m_device == IntPtr.Zero)
                return;

            byte[] feature = ConfigureFeature();
            UsbNative.SetFeature(m_device, ConfigureFeature(), (ushort)feature.Length);
            var success = UsbNative.GetFeature(m_device, feature, 9);

            SendConnect();

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

        void ReadDevice()
        {
            var result = UsbNative.ReadFile(m_device, out var buffer, 33);

            if (!result)
                return;

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
                    m_runtimeModel = GetModel(m_readMessage);
                }
            }

            Console.WriteLine($"ReadDevice: {LogBuffer(m_readMessage)}");
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

        void SendConnect()
        {
            byte[] message = new byte[33];
            for (int i = 0; i < message.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        message[i] = 0x00;
                        break;
                    case 1:
                        message[i] = 19;
                        break;
                    case 2:
                        message[i] = 0xF1;
                        break;
                    case 3:
                        message[i] = 0x41;
                        break;
                    case 20:
                        message[i] = 0x32;
                        break;
                    default:
                        message[i] = 0x00;
                        break;
                }
            }

            Console.WriteLine($"Send connect message: {LogBuffer(message)}");

            Task.Run(() =>
            {
                var success = UsbNative.WriteFile(m_device, message);
                Console.WriteLine($"Write file success: {success}");
            });
        }

        void SendDisconnect(Action action)
        {
            byte[] message = new byte[33];
            for (int i = 0; i < message.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        message[i] = 0x00;
                        break;
                    case 1:
                        message[i] = 19;
                        break;
                    case 2:
                        message[i] = 0xF1;
                        break;
                    case 3:
                        message[i] = 0x5A;
                        break;
                    case 20:
                        message[i] = 0x4B;
                        break;
                    default:
                        message[i] = 0x00;
                        break;
                }
            }

            var write = Task.Run(() =>
            {
                var success = UsbNative.WriteFile(m_device, message);
                Console.WriteLine($"Disconnect success: {success}");
            });

            write.Wait();
            action?.Invoke();
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