using RotoVR.Common.Model;
using RotoVR.Communication;
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

        private ICompensationBridge m_compensationBridge;
        private ICommunicationLayer m_communicationLayer;
        private ApplicationViewState m_applicationViewState;
        private const int CP_NOCLOSE_BUTTON = 0x200;

        public void BindConnectionLayer(ICommunicationLayer communicationLayer)
        {
            m_communicationLayer = communicationLayer;
        }

        public void BindCompensationBridge(ICompensationBridge compensationBridge)
        {
            m_compensationBridge = compensationBridge;
            compensationBridge.Init();
            m_communicationLayer.Inject(compensationBridge);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (m_components != null))
            {
                m_components.Dispose();
            }

            m_communicationLayer.Stop();

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
            if (WindowState == FormWindowState.Normal)
                return;

            WindowState = FormWindowState.Normal;
            ClientSize = new Size(1024, 768);
            SetAppViewState(ApplicationViewState.Console);

            m_openConsoleMenuItem.Text = "Hide";
            m_openConsoleMenuItem.Click -= OnClickOpenConsole;
            m_openConsoleMenuItem.Click += OnClickHideConsole;
        }

        private void OnClickHideConsole(object sender, EventArgs e)
        {
            m_openConsoleMenuItem.Text = "Open";
            m_openConsoleMenuItem.Click -= OnClickHideConsole;
            m_openConsoleMenuItem.Click += OnClickOpenConsole;
            WindowState = FormWindowState.Minimized;
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

        // TODO Remove after debug 
        // #region DEBUG PART
        //
        // private void InitOffset(object sender, EventArgs e)
        // {
        //     m_compensationBridge.SetCompensationValue(new CompensationModel(-10, 15));
        // }
        //
        // private void StartMC(object sender, EventArgs e)
        // {
        //     m_compensationBridge.Start();
        // }
        //
        // private void StopMC(object sender, EventArgs e)
        // {
        //     m_compensationBridge.Stop();
        // }
        //
        // private void RunMC(object sender, EventArgs e)
        // {
        //     m_compensationBridge.SetRotoData(new RotoDataModel("", 90, 0, 0));
        // }

       // #endregion
    }
}