using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace RotoVR.Monitor
{
    partial class MonitorForm
    {
    
        private IContainer m_components = null;
        private NotifyIcon m_notifyIcon;
        private ContextMenuStrip m_notifyIconContextMenu;
    
        private void Initialize()
        {
            m_components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MonitorForm));
           
            m_notifyIconContextMenu = new ContextMenuStrip(m_components);
            m_notifyIcon= GetNotifyIcon(m_components,resources);
            m_notifyIconContextMenu.SuspendLayout();
            
            SuspendLayout();
       
            m_notifyIconContextMenu.Items.AddRange(new ToolStripItem[] { GetApplicationConnectMenuItem(), GetOpenConsoleMenuItem(), GetAboutMenuItem(), GetApplicationQuitMenuItem() });
            m_notifyIconContextMenu.Name = "contextMenuStrip";
            m_notifyIconContextMenu.Size = new Size(150, 92);

            InitFormView(resources);
     
            m_notifyIconContextMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        ToolStripMenuItem GetApplicationQuitMenuItem()
        {
            ToolStripMenuItem iten = new ToolStripMenuItem();
            iten.Name = "applicationQuit";
            iten.Size = new Size(149, 22);
            iten.Text = "Quit";
            iten.Click += ApplicationQuitMenuItemHandler;
            return iten;
        }

        ToolStripMenuItem GetAboutMenuItem()
        {
            ToolStripMenuItem iten = new ToolStripMenuItem();
            iten.Name = "about";
            iten.Size = new Size(149, 22);
            iten.Text = "About";
            return iten;
        }

        ToolStripMenuItem GetOpenConsoleMenuItem()
        {
            ToolStripMenuItem iten = new ToolStripMenuItem();
            iten.Name = "openConsole";
            iten.Size = new Size(149, 22);
            iten.Text = "Open Console";
            iten.Click += OnClickOpenConsole;
            return iten;
        }

        NotifyIcon GetNotifyIcon(IContainer container,ComponentResourceManager resources)
        {
            var notifyIcon = new NotifyIcon(container);
            notifyIcon.BalloonTipText = "Monitor is started";
            notifyIcon.ContextMenuStrip = m_notifyIconContextMenu;
            notifyIcon.Icon = (Icon)resources.GetObject("NotifyIcon.Icon");
            notifyIcon.Text = "RotoVR Monitor";
            notifyIcon.Visible = true;
            return notifyIcon;
        }

        ToolStripMenuItem GetApplicationConnectMenuItem()
        {
            ToolStripMenuItem iten = new ToolStripMenuItem();
            iten.DropDownItems.AddRange(new ToolStripItem[] { GetConnectUsbMenuItem(), GetConnectBleMenuItem() });
            iten.Name = "applicationConnect";
            iten.Size = new Size(149, 22);
            iten.Text = "Connect";
            return iten;
        }
        
        ToolStripMenuItem GetApplicationDisconnectMenuItem()
        {
            ToolStripMenuItem iten = new ToolStripMenuItem();
           
            iten.Name = "applicationDisconnect";
            iten.Size = new Size(149, 22);
            iten.Text = "Disconnect";
            iten.Click += OnClickApplicationDisconnect;
            return iten;
        }

        ToolStripMenuItem GetConnectUsbMenuItem()
        {
            ToolStripMenuItem iten = new ToolStripMenuItem();
            iten.MergeAction = MergeAction.MatchOnly;
            iten.Name = "connectUSB";
            iten.Size = new Size(180, 22);
            iten.Text = "USB";
            iten.Click += OnClickUsbMenuItemMConnect;
            return iten;
        }
        
        ToolStripMenuItem GetConnectBleMenuItem()
        {
            ToolStripMenuItem iten = new ToolStripMenuItem();
            iten.Name = "connectBLE";
            iten.Size = new Size(180, 22);
            iten.Text = "BLE";
            iten.Click += OnClickBleMenuItemMConnect;
            return iten;
        }

        void InitFormView(ComponentResourceManager resources)
        {
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(933, 519);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "MonitorForm";
            ShowInTaskbar = false;
            Text = "RotoVR Monitor";
            SizeChanged += MonitorFormSizeChanged;
        }

        void ConnectedHandler()
        {
            m_notifyIconContextMenu.Items.Clear();
            m_notifyIconContextMenu.Items.AddRange(
                new ToolStripItem[] { GetApplicationDisconnectMenuItem(), GetOpenConsoleMenuItem(), GetAboutMenuItem(), GetApplicationQuitMenuItem() });
            m_notifyIcon.BalloonTipText = "Monitor was successfully connected";
            m_notifyIcon.ShowBalloonTip(100);
        }

        void DisconnectedHandler()
        {
            m_notifyIconContextMenu.Items.Clear();
            m_notifyIconContextMenu.Items.AddRange(
                new ToolStripItem[] { GetApplicationConnectMenuItem(), GetOpenConsoleMenuItem(), GetAboutMenuItem(), GetApplicationQuitMenuItem() });
            m_notifyIcon.BalloonTipText = "Monitor was disconnected";
            m_notifyIcon.ShowBalloonTip(100);
        }
        
        private void ConnectionErrorHandler()
        {
            m_notifyIconContextMenu.Items.Clear();
            m_notifyIconContextMenu.Items.AddRange(
                new ToolStripItem[] { GetApplicationConnectMenuItem(), GetOpenConsoleMenuItem(), GetAboutMenuItem(), GetApplicationQuitMenuItem() });
            m_notifyIcon.BalloonTipText = "Connection failed, please try again";
            m_notifyIcon.ShowBalloonTip(100);
        }

    }
}