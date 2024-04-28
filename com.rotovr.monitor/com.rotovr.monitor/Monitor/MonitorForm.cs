using RotoVR.Common.Model;
using RotoVR.Communication;
using RotoVR.Communication.Enum;
using RotoVR.Core;
using RotoVR.MotionCompensation;

namespace RotoVR.Monitor
{
    public partial class MonitorForm : Form, IMonitor
    {
        public MonitorForm()
        {
            new Bootstrapper(this).Bootstrap(() =>
            {
                Initialize();
                WindowState = FormWindowState.Minimized;
            });
        }

        private ICommunicationLayer m_communicationLayer;
        private ICompensationBridge m_compensationBridge;
        private ApplicationViewState m_applicationViewState;
        private const int CP_NOCLOSE_BUTTON = 0x200;

        public void BindConnectionLayer(ICommunicationLayer communicationLayer)
        {
            m_communicationLayer = communicationLayer;
            m_communicationLayer.OnConnectionStatus += OnConnectionStatusHandler;
            m_communicationLayer.OnSystemLog += OnSystemLogHandler;
        }

        public void BindCompensationBridge(ICompensationBridge compensationBridge)
        {
            m_compensationBridge = compensationBridge;
            m_compensationBridge.Init();
        }

        protected override void Dispose(bool disposing)
        {
            m_communicationLayer.OnConnectionStatus -= OnConnectionStatusHandler;
            m_communicationLayer.OnSystemLog -= OnSystemLogHandler;
            m_communicationLayer.Disconnect();
            if (disposing && (m_components != null))
            {
                m_components.Dispose();
            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// Hide "Close" button for the form
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void MonitorFormSizeChanged(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                return;

            SizeChanged -= MonitorFormSizeChanged;
            m_notifyIcon.ShowBalloonTip(300);
        }

        private void OnClickOpenConsole(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ClientSize = new Size(1024, 768);
            SetAppViewState(ApplicationViewState.Console);
        }

        private void ApplicationQuitMenuItemHandler(object sender, EventArgs e)
        {
            DialogResult dialog = new DialogResult();

            dialog = MessageBox.Show("Do you want to close?", "RotoVR Monitor", MessageBoxButtons.YesNo);

            if (dialog == DialogResult.Yes)
            {
                Environment.Exit(1);
            }
        }

        private void OnClickUsbMenuItemMConnect(object sender, EventArgs e)
        {
            m_communicationLayer.Connect(CommunicationType.Usb);
        }

        private void OnClickBleMenuItemMConnect(object sender, EventArgs e)
        {
            m_communicationLayer.Connect(CommunicationType.Ble);
        }

        private void OnClickApplicationDisconnect(object sender, EventArgs e)
        {
            m_communicationLayer.Disconnect();
        }

        private void OnConnectionStatusHandler(ConnectionStatus status)
        {
            switch (status)
            {
                case ConnectionStatus.Connected:
                    ConnectedHandler();
                    break;
                case ConnectionStatus.Disconnected:
                    DisconnectedHandler();
                    break;
                case ConnectionStatus.Error:
                    ConnectionErrorHandler();
                    break;
            }
        }

        private void OnSystemLogHandler(string message)
        {
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SetAppViewState(ApplicationViewState.Settings);
        }

        private void ConsoleButton_Click(object? sender, EventArgs e)
        {
            SetAppViewState(ApplicationViewState.Console);
        }


        enum ApplicationViewState
        {
            Default,
            Console,
            Settings,
        }

        private void CompensationValueChanged(object sender, EventArgs e)
        {
            m_compensationBridge.SetCompensationValue(new CompensationModel(m_positionX.Value,m_positionY.Value));
        }
    }
}