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
        private ToolStripMenuItem m_openConsoleMenuItem;
        private Label m_consoleLabel;
        private Panel m_consolePanel;
        private Button m_settingsButton;
        private Button m_consoleButton;
        private string m_log;
        private void Initialize()
        {
            m_components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MonitorForm));
           
            m_notifyIconContextMenu = new ContextMenuStrip(m_components);
            m_notifyIcon= GetNotifyIcon(m_components,resources);
            m_notifyIconContextMenu.SuspendLayout();
            
            SuspendLayout();

            m_openConsoleMenuItem = GetOpenConsoleMenuItem();
                    
            m_notifyIconContextMenu.Items.AddRange(new ToolStripItem[] { m_openConsoleMenuItem, GetAboutMenuItem(), GetApplicationQuitMenuItem() });
            m_notifyIconContextMenu.Name = "contextMenuStrip";
            m_notifyIconContextMenu.Size = new Size(150, 92);

            InitFormView(resources);
     
            m_notifyIconContextMenu.ResumeLayout(false);
            ResumeLayout(false);
            m_communicationLayer.Start();
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
            iten.Text = "Open";
            iten.Click +=OnClickOpenConsole;
            return iten;
        }

        ToolStripMenuItem GetHideConsoleMenuItem()
        {
            ToolStripMenuItem iten = new ToolStripMenuItem();
            iten.Name = "hideConsole";
            iten.Size = new Size(149, 22);
            iten.Text = "Hide";
            iten.Click += OnClickHideConsole;
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
        void InitFormView(ComponentResourceManager resources)
        {
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 768);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "MonitorForm";
            ShowInTaskbar = false;
            Text = "RotoVR Monitor";
            SizeChanged += MonitorFormSizeChanged;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            
            Controls.Add(SettingsButton());
            Controls.Add(ConsoleButton());
        }
        Button SettingsButton()
        {
            m_settingsButton = new Button();
            m_settingsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            m_settingsButton.Location = new Point(944, 10);
            m_settingsButton.Name = "SettingsButton";
            m_settingsButton.Size = new Size(60, 30);
            m_settingsButton.TabIndex = 2;
            m_settingsButton.Text = "Settings";
            m_settingsButton.UseVisualStyleBackColor = true;
            m_settingsButton.Click += SettingsButton_Click;
            return m_settingsButton;
        }
        
        Button ConsoleButton()
        {
            m_consoleButton = new Button();
            m_consoleButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            m_consoleButton.Location = new Point(884, 10);
            m_consoleButton.Name = "ConsoleButton";
            m_consoleButton.Size = new Size(60, 30);
            m_consoleButton.TabIndex = 3;
            m_consoleButton.Text = "Console";
            m_consoleButton.UseVisualStyleBackColor = true;
            m_consoleButton.Click += ConsoleButton_Click;
            return m_consoleButton;
        }

        void SetAppViewState(ApplicationViewState viewState)
        {
            if(m_applicationViewState==viewState)
                return;

            m_applicationViewState = viewState;
            switch (m_applicationViewState)
            {
                case ApplicationViewState.Console:
                    m_settingsButton.BackColor = Color.White;
                    m_consoleButton.BackColor = Color.Silver;
                    InitConsoleView();
                    break;
                case ApplicationViewState.Settings:
                    m_consoleButton.BackColor = Color.White;
                    m_settingsButton.BackColor = Color.Silver;
                    InitSettingsView();
                    break;
            }
        }

        void InitSettingsView()
        {
            if (m_consolePanel != null)
            {
                Controls.Remove(m_consolePanel);
                m_consoleLabel = null;
            }
        }

        void InitConsoleView()
        {
            m_log = string.Empty;

            m_consolePanel = new Panel();
            m_consolePanel.AutoScroll = true;
            m_consolePanel.Location = new Point(20, 50);
            m_consolePanel.Size = new Size(984, 698);
            m_consolePanel.BackColor = Color.Black;
            m_consolePanel.VerticalScroll.Visible = true;
            
            m_consoleLabel = new Label();
            m_consoleLabel.Location = new Point(0, -10);
            m_consoleLabel.AutoSize = true;
            m_consoleLabel.ForeColor=Color.Silver;
            m_consoleLabel.TextAlign = ContentAlignment.TopLeft;
            m_consoleLabel.Padding = new Padding(5, 5, 5, 5);
            m_consoleLabel.Font= new Font("Arial", 10);
            m_consolePanel.Controls.Add(m_consoleLabel);
            Controls.Add(m_consolePanel);
        }
        private void SystemLogHandler(string log)
        {
            m_log= $"{m_log}{Environment.NewLine}{Environment.NewLine}{log}";
            m_consoleLabel.Text = m_log;
            m_consolePanel.VerticalScroll.Value = m_consolePanel.VerticalScroll.Maximum;
        }
        
        private void OnUdpMessageHandler(byte[] rawData)
        {
            m_log= $"{m_log}{Environment.NewLine}{Environment.NewLine} Incomming Message";
            m_consoleLabel.Text = m_log;
            m_consolePanel.VerticalScroll.Value = m_consolePanel.VerticalScroll.Maximum;
        }
    }
}