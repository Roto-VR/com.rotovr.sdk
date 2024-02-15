using System;
using System.Threading;
using System.Threading.Tasks;
using RotoVR.SDK.Enum;
using RotoVR.SDK.Model;
using UnityEngine;
using PimDeWitte.UnityMainThreadDispatcher;

namespace com.rotovr.sdk.Runtime.USB
{
    public class UsbConnector
    {
        public static UsbConnector Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new UsbConnector();
                return m_instance;
            }
        }

        private static UsbConnector m_instance;

        const UInt16 k_vid = 0x04D9;
        const UInt16 k_pid = 0xB564;

        byte[] m_usbMessage = new byte[19];
        byte[] m_writeBuffer = new byte[33];
        byte[] m_readMessage = new byte[19];
        static RotoDataModel m_runtimeModel;
        UnityMainThreadDispatcher m_dispatcher;
        IntPtr m_device;
        Thread m_connectionThread;
        static int m_messageSize;
        static bool m_initPacket;
        static bool m_reaDevice;
        public event Action<ConnectionStatus> OnConnectionStatus;
        public event Action<RotoDataModel> OnDataChange;


        public void Connect()
        {
            m_dispatcher = UnityMainThreadDispatcher.Instance();
            m_connectionThread = new Thread(ConnectToDevice);
            m_connectionThread.Start();
        }

        void ConnectToDevice()
        {
            m_device = Native.OpenFirstHIDDevice(k_vid, k_pid);

            if (m_device == IntPtr.Zero)
                return;

            byte[] feature = ConfigureFeature();
            Native.SetFeature(m_device, ConfigureFeature(), (ushort)feature.Length);
            var success = Native.GetFeature(m_device, feature, 9);

            Debug.Log($"Set Feature success: {success}");
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

        string LogBuffer(byte[] data)
        {
            if (data.Length == 0)
                return "Buffer is Empty";

            string message = $"";
            message = $"{message} {BitConverter.ToString(data).Replace("-", "  ")}";

            return message;
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

        public void Disconnect()
        {
            Debug.Log("Disconnect");
            m_reaDevice = false;

            SendDisconnect(() =>
            {
                if (m_device != IntPtr.Zero)
                {
                    Native.CloseHIDDevice(m_device);
                    m_dispatcher.Enqueue(() => { OnConnectionStatus?.Invoke(ConnectionStatus.Disconnected); });
                }


                if (m_connectionThread != null)
                    m_connectionThread.Abort();
            });
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

            Debug.Log($"Send connect message: {LogBuffer(message)}");

            Task.Run(() =>
            {
                var success = Native.WriteFile(m_device, message);
                Debug.Log($"Write file success: {success}");

                m_dispatcher.Enqueue(() => { OnConnectionStatus?.Invoke(ConnectionStatus.Connected); });
            });
        }

        void ReadDevice()
        {
            var result = Native.ReadFile(m_device, out var buffer, 33);

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
                    m_dispatcher.Enqueue(() => { OnDataChange?.Invoke(m_runtimeModel); });
                }
            }
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
                var success = Native.WriteFile(m_device, message);
                Debug.Log($"Disconnect success: {success}");
            });

            write.Wait();
            action?.Invoke();
        }

        bool IsConnectedAndOpen()
        {
            return m_device != IntPtr.Zero;
        }

        public void SetMode(ModeModel model)
        {
            if (!IsConnectedAndOpen())
                return;

            ResetMessage();

            m_usbMessage[0] = 0xF1;
            m_usbMessage[1] = (byte)'S';

            switch (model.Mode)
            {
                case "IdleMode":
                    m_usbMessage[2] = 0x00;
                    break;
                case "Calibration":
                    m_usbMessage[2] = 0x01;
                    break;
                case "HeadTrack":
                    m_usbMessage[2] = 0x02;
                    break;
                case "FreeMode":
                    m_usbMessage[2] = 0x03;
                    break;
                case "CockpitMode":
                    m_usbMessage[2] = 0x04;
                    break;
            }

            switch (model.ModeParametersModel.MovementMode)
            {
                case "Smooth":
                    m_usbMessage[3] = 0x00;
                    break;
                case "Jerky":
                    m_usbMessage[3] = 0x01;
                    break;
            }

            Debug.Log($"Set Mode: {model.Mode}");

            m_usbMessage[9] = (byte)(model.ModeParametersModel.TargetCockpit);
            m_usbMessage[11] = 40;
            m_usbMessage[12] = (byte)(model.ModeParametersModel.MaxPower);
            m_usbMessage[14] = 0x01;

            byte sum = ByteSum(m_usbMessage);
            m_usbMessage[18] = sum;

            Task.Run(() => { Native.WriteFile(m_device, PrepareWriteBuffer(m_usbMessage)); });
        }

        byte[] PrepareWriteBuffer(byte[] message)
        {
            for (int i = 0; i < m_writeBuffer.Length; i++)
            {
                m_writeBuffer[i] = 0x00;
            }

            m_writeBuffer[0] = 0x00;
            m_writeBuffer[1] = 19;
            for (int i = 0; i < message.Length; i++)
            {
                m_writeBuffer[i + 2] = message[i];
            }

            return m_writeBuffer;
        }

        public void TurnToAngle(RotateToAngleModel model)
        {
            if (!IsConnectedAndOpen())
                return;

            ResetMessage();

            m_usbMessage[0] = 0xF1;
            m_usbMessage[1] = (byte)'M';
            m_usbMessage[2] = 0x01;

            if (model.Direction.Equals("Right"))
            {
                m_usbMessage[3] = 0x52;
            }
            else
            {
                m_usbMessage[3] = 0x4C;
            }

            var angle = model.Angle;

            if (angle == 360)
                angle -= 1;

            if (angle >= 256)
            {
                m_usbMessage[4] = 0x01;
                m_usbMessage[5] = (byte)((angle - 256));
            }
            else
            {
                m_usbMessage[4] = 0x00;
                m_usbMessage[5] = (byte)angle;
            }

            m_usbMessage[6] = (byte)model.Power;
            m_usbMessage[7] = 0x00;

            byte sum = ByteSum(m_usbMessage);
            m_usbMessage[18] = sum;

            Task.Run(() =>
            {
                var result = Native.WriteFile(m_device, PrepareWriteBuffer(m_usbMessage));
                Debug.Log($"Turn To Angle success: {result}");
            });
        }

        public void PlayRumble(RumbleModel model)
        {
            if (!IsConnectedAndOpen())
                return;

            ResetMessage();

            m_usbMessage[0] = 0xF1;
            m_usbMessage[1] = (byte)'M';
            m_usbMessage[2] = 0x00;
            m_usbMessage[3] = 0x00;
            m_usbMessage[4] = 0x00;
            m_usbMessage[5] = 0x00;
            m_usbMessage[6] = 0x00;
            m_usbMessage[7] = 0x01;
            m_usbMessage[8] = (byte)model.Power;
            m_usbMessage[9] = (byte)(int)(model.Duration * 10);

            byte sum = ByteSum(m_usbMessage);
            m_usbMessage[18] = sum;

            Task.Run(() =>
            {
                var result = Native.WriteFile(m_device, PrepareWriteBuffer(m_usbMessage));
                Debug.Log($"Play Rumble success: {result}");
            });
        }

        void ResetMessage()
        {
            for (int i = 0; i < m_usbMessage.Length; i++)
            {
                m_usbMessage[i] = 0x00;
            }
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
    }
}