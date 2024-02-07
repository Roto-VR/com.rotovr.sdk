using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RotoVR.SDK.Enum;
using RotoVR.SDK.Model;
using UnityEngine;
using HidLibrary;
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

        public UsbConnector()
        {
        }

        private static UsbConnector m_instance;

        const UInt16 k_vid = 0x04D9;
        const UInt16 k_pid = 0xB564;

        byte[] m_usbMessage = new byte[19];

        //HidDevice m_hidDevice;
        RotoDataModel m_runtimeModel;
        private UnityMainThreadDispatcher m_dispatcher;
        IntPtr m_device;
        Thread m_connectionThread;


        bool m_reaDevice;
        public event Action<ConnectionStatus> OnConnectionStatus;
        public event Action<RotoDataModel> OnRotoDataChange;


        public void Connect()
        {
            m_dispatcher = UnityMainThreadDispatcher.Instance();
            m_connectionThread = new Thread(ConnectToDevice);
            m_connectionThread.Start();
        }

        void ConnectToDevice()
        {
            // m_hidDevice = HidDevices.Enumerate(k_vid, k_pid).FirstOrDefault();
            //  if (m_hidDevice == null)
            return;


            //  m_hidDevice.Inserted += DeviceAttachedHandler;
            // m_hidDevice.Removed += DeviceRemovedHandler;
            //  m_hidDevice.MonitorDeviceEvents = true;


            // var setFeatureTask = Task.Run(async () =>
            // {
            //     m_hidDevice.WriteFeatureData(ConfigureFeature());
            //
            //     await Task.Delay(2000);

            //  m_hidDevice.OpenDevice();
            // });
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

        // private void OnReport(HidReport report)
        // {
        //     if (!m_hidDevice.IsConnected)
        //         return;
        //
        //     var rowData = report.Data;
        //
        //     int index = 0;
        //
        //     foreach (var element in rowData)
        //     {
        //         Debug.LogError($"element[{index}]: {element}");
        //         index++;
        //     }
        // }

        public void Disconnect()
        {
            Debug.LogError("Disconnect");


            // if (m_hidDevice != null)
            // {
            //     m_reaDevice = false;
            //     SendDisconnect();
            //     m_hidDevice.Inserted -= DeviceAttachedHandler;
            //     m_hidDevice.Removed -= DeviceRemovedHandler;
            //     m_hidDevice.CloseDevice();
            OnConnectionStatus?.Invoke(ConnectionStatus.Disconnected);
            if (m_connectionThread != null)
                m_connectionThread.Abort();
            //  }
        }

        private void DeviceRemovedHandler()
        {
            Debug.LogError("DeviceRemovedHandler");
            m_reaDevice = false;
            Disconnect();
        }

        private void DeviceAttachedHandler()
        {
            Debug.LogError("DeviceAttachedHandler");

            var setFeatureTask = Task.Run(async () =>
            {
                // m_hidDevice.WriteFeatureData(ConfigureFeature());
                //
                // await Task.Delay(500);
                //
                // m_hidDevice.ReadFeatureData(out var featureData);
                //
                // for (int i = 0; i < featureData.Length; i++)
                // {
                //     Debug.LogError($"feature element[{i}]: {featureData[i]}");
                // }


                // UnityMainThreadDispatcher.Instance().Enqueue(() =>
                // {
                //     if (m_hidDevice.IsConnected)
                //         OnConnectionStatus?.Invoke(ConnectionStatus.Connected);
                // });
                m_reaDevice = true;

                SendConnect();

                // while (m_reaDevice)
                // {
                //     await Task.Delay(500);
                //     ReadDevice();
                // }
            });


            //  SendConnect();
        }

        void SendConnect()
        {
            Debug.LogError("SendConnect");

            byte[] message = new byte[32];
            for (int i = 0; i < message.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        message[i] = 0xF1;
                        break;
                    case 1:
                        message[i] = 0x41;
                        break;
                    case 18:
                        message[i] = 0x32;
                        break;
                    default:
                        message[i] = 0x00;
                        break;
                }
            }

            int index = 0;
            foreach (var el in message)
            {
                Debug.LogError($"write message el[{index}]: {el}");
                index++;
            }

            // var write = Task.Run(async () =>
            // {
            //     m_hidDevice.WriteReport(new HidReport(32, new HidDeviceData(message, HidDeviceData.ReadStatus.Success)),
            //         (success) => { Debug.LogError($"Connect Write: {success}"); });
            //
            //     await Task.Delay(1000);
            // });


            //  write.Wait();


            //
            //
            // var read = Task.Run(() =>
            // {
            //     Debug.LogError("Read");
            //
            //     m_hidDevice.Read((result) =>
            //     {
            //         Debug.LogError("Read result");
            //
            //         index = 0;
            //         foreach (var el in result.Data)
            //         {
            //             Debug.LogError($"new el[{index}]: {el}");
            //             index++;
            //         }
            //     });
            // });
            // read.Wait();

            //   if (m_hidDevice.IsConnected)
            //       OnConnectionStatus?.Invoke(ConnectionStatus.Connected);

            m_reaDevice = true;

            Task.Run(async () =>
            {
                while (m_reaDevice)
                {
                    await Task.Delay(100);
                    ReadDevice();
                }
            });
        }

        void ReadDevice()
        {
            int index;

            string message = String.Empty;
            var read = Task.Run(() =>
            {
                // m_hidDevice.ReadReport((result) =>
                // {
                //     foreach (var el in result.Data)
                //     {
                //         message = $"{message} {el}";
                //     }
                //
                //     Debug.LogError($"message[{result.Data.Length}]: {message}");
                // });
            });
        }

        void SendDisconnect()
        {
            byte[] message = new byte[32];
            for (int i = 0; i < message.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        message[i] = 0xF1;
                        break;
                    case 1:
                        message[i] = 0x5A;
                        break;
                    case 18:
                        message[i] = 0x4B;
                        break;
                    default:
                        message[i] = 0x00;
                        break;
                }
            }

            // var write = Task.Run(async () =>
            // {
            //     m_hidDevice.Write(message, (success) => { Debug.LogError($"Disconnect Write: {success}"); });
            // });
        }

        bool IsConnectedAndOpen()
        {
            return true; //m_hidDevice != null && m_hidDevice.IsConnected;
        }

        public void SetMode(ModeModel model)
        {
            Debug.LogError("----------------------SetMode---------------");

            if (!IsConnectedAndOpen())
                return;

            Debug.LogError("22");

            ResetMessage();

            m_usbMessage[0] = (byte)(0xF1 & 0xFF);
            m_usbMessage[1] = (byte)'S';

            switch (model.Mode)
            {
                case "IdleMode":
                    m_usbMessage[2] = (byte)(0x00 & 0xFF);
                    break;
                case "Calibration":
                    m_usbMessage[2] = (byte)(0x01 & 0xFF);
                    break;
                case "HeadTrack":
                    m_usbMessage[2] = (byte)(0x02 & 0xFF);
                    break;
                case "FreeMode":
                    m_usbMessage[2] = (byte)(0x03 & 0xFF);
                    break;
                case "CockpitMode":
                    m_usbMessage[2] = (byte)(0x04 & 0xFF);
                    break;
            }

            switch (model.ModeParametersModel.MovementMode)
            {
                case "Smooth":
                    m_usbMessage[3] = (byte)(0x00 & 0xFF);
                    break;
                case "Jerky":
                    m_usbMessage[3] = (byte)(0x01 & 0xFF);
                    break;
            }

            Debug.LogError($"Set Mode: {model.Mode}");

            m_usbMessage[9] = (byte)(model.ModeParametersModel.TargetCockpit & 0xFF);
            m_usbMessage[11] = (byte)(40 & 0xFF);
            m_usbMessage[12] = (byte)(model.ModeParametersModel.MaxPower & 0xFF);
            m_usbMessage[14] = (byte)(0x01 & 0xFF);

            byte sum = ByteSum(m_usbMessage);
            m_usbMessage[18] = sum;
            Task.Run(() =>
            {
                //  m_hidDevice.Write(m_usbMessage, (result) => { Debug.LogError($"Write Set Mode: {result}"); });
            });


            // var read = m_hidDevice.ReadAsync();
            //
            // read.Wait();
            //
            // var result = read.Result;
            // int index = 0;
            // foreach (var bt in result.Data)
            // {
            //     Debug.LogError($"bt[{index}]: {bt}");
            //     index++;
            // }
        }

        public void TurnToAngle(RotateToAngleModel model)
        {
            if (!IsConnectedAndOpen())
                return;

            ResetMessage();

            m_usbMessage[0] = (byte)(0xF1 & 0xFF);
            m_usbMessage[1] = (byte)'M';
            m_usbMessage[2] = (byte)0x01 & 0xFF;

            if (model.Direction.Equals("Right"))
            {
                m_usbMessage[3] = (byte)(0x52 & 0xFF);
            }
            else
            {
                m_usbMessage[3] = (byte)(0x4C & 0xFF);
            }

            var angle = model.Angle;

            if (angle == 360)
                angle -= 1;

            if (angle >= 256)
            {
                m_usbMessage[4] = (byte)0x01 & 0xFF;
                m_usbMessage[5] = (byte)((angle - 256) & 0xFF);
            }
            else
            {
                m_usbMessage[4] = (byte)0x00 & 0xFF;
                m_usbMessage[5] = (byte)(angle & 0xFF);
            }

            m_usbMessage[6] = (byte)(model.Power & 0xFF);
            m_usbMessage[7] = (byte)0x00 & 0xFF;

            byte sum = ByteSum(m_usbMessage);
            m_usbMessage[18] = sum;

            Task.Run(() =>
            {
                //  m_hidDevice.Write(m_usbMessage, (result) => { Debug.LogError($"Write Turn To Angle: {result}"); });
            });

            // var task = m_hidDevice.WriteAsync(m_usbMessage);
            // task.Wait();
            //
            //
            // //  m_hidDevice.ReadReport(OnReport);
            // var read = m_hidDevice.ReadAsync();
            //
            // read.Wait();
            //
            // Debug.LogError("2");
            //
            // var result = read.Result;
            // int index = 0;
            // foreach (var bt in result.Data)
            // {
            //     Debug.LogError($"bt[{index}]: {bt}");
            //     index++;
            // }
        }

        public void PlayRumble(RumbleModel model)
        {
            if (!IsConnectedAndOpen())
                return;

            ResetMessage();

            m_usbMessage[0] = (byte)(0xF1 & 0xFF);
            m_usbMessage[1] = (byte)'M';
            m_usbMessage[2] = (byte)(0x00 & 0xFF);
            m_usbMessage[3] = (byte)(0x00 & 0xFF);
            m_usbMessage[4] = (byte)(0x00 & 0xFF);
            m_usbMessage[5] = (byte)(0x00 & 0xFF);
            m_usbMessage[6] = (byte)(0x00 & 0xFF);
            m_usbMessage[7] = (byte)0x01 & 0xFF;
            m_usbMessage[8] = (byte)(model.Power & 0xFF);
            m_usbMessage[9] = (byte)(((int)(model.Duration * 10)) & 0xFF);

            byte sum = ByteSum(m_usbMessage);
            m_usbMessage[18] = sum;

            Task.Run(() =>
            {
                //  m_hidDevice.Write(m_usbMessage, (result) => { Debug.LogError($"Write Turn To Angle: {result}"); });
            });

            // var task = m_hidDevice.WriteAsync(m_usbMessage);
            // task.Wait();
            //
            //
            // //  m_hidDevice.ReadReport(OnReport);
            // var read = m_hidDevice.ReadAsync();
            //
            // read.Wait();
            //
            // Debug.LogError("2");
            //
            // var result = read.Result;
            // int index = 0;
            // foreach (var bt in result.Data)
            // {
            //     Debug.LogError($"bt[{index}]: {bt}");
            //     index++;
            // }
        }

        void ResetMessage()
        {
            for (int i = 0; i < m_usbMessage.Length; i++)
            {
                m_usbMessage[i] = (byte)(0 & 0xFF);
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
                    model.Angle = rawData[6] & 0xFF;
                    break;
                case 1:
                    int angle = rawData[6] & 0xFF;
                    model.Angle = (angle + 256);
                    break;
            }

            model.TargetCockpit = rawData[9] & 0xFF;
            model.MaxPower = rawData[12] & 0xFF;
            return model;
        }
    }
}