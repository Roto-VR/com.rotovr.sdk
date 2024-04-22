using System;
using System.Windows.Forms;
using RotoVR.Communication;
using RotoVR.Core;

namespace RotoVR.Monitor
{
    public partial class MonitorForm : Form, IMonitor
    {
        public MonitorForm()
        {
            new Bootstrapper(this).Bootstrap(() =>
            {
                InitializeComponent();
                WindowState = FormWindowState.Minimized;
            });
        }

        private IConnector m_connector;

        private void MonitorFormSizeChanged(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                return;
            SizeChanged -= MonitorFormSizeChanged;
            NotifyIcon.ShowBalloonTip(500);
        }

        private void ApplicationConnectHandler(object sender, EventArgs e)
        {
            m_connector.Connect();
        }

        private void OpenConsoleHandler(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void ApplicationQuitHandler(object sender, EventArgs e)
        {
            DialogResult dialog = new DialogResult();

            dialog = MessageBox.Show("Do you want to close?", "RotoVR Monitor", MessageBoxButtons.YesNo);

            if (dialog == DialogResult.Yes)
            {
                Environment.Exit(1);
            }
        }

        public void BindConnector(IConnector connector)
        {
            m_connector = connector;
        }
    }
}