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
        private NumericUpDown m_positionX;
        private NumericUpDown m_positionY;
        private Label m_positionXLabel;
        private Label m_positionYLabel;
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
            iten.Text = "Open";
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
          //  iten.DropDownItems.AddRange(new ToolStripItem[] { GetConnectUsbMenuItem(), GetConnectBleMenuItem() });
            iten.Name = "applicationConnect";
            iten.Size = new Size(149, 22);
            iten.Text = "Connect";
            iten.Click += OnClickUsbMenuItemMConnect;
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
           // BackColor = SystemColors.Desktop;
            ClientSize = new Size(933, 519);
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

        Button SettingsButton()
        {
            Button button = new Button();
            button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button.Location = new Point(840, 12);
            button.Name = "SettingsButton";
            button.Size = new Size(77, 41);
            button.TabIndex = 2;
            button.Text = "Settings";
            button.UseVisualStyleBackColor = true;
            button.Click += SettingsButton_Click;
            return button;
        }
        
        Button ConsoleButton()
        {
            Button button = new Button();
            button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button.Location = new Point(750, 12);
            button.Name = "ConsoleButton";
            button.Size = new Size(75, 41);
            button.TabIndex = 3;
            button.Text = "Console";
            button.UseVisualStyleBackColor = true;
            button.Click += ConsoleButton_Click;
            return button;
        }

        void SetAppViewState(ApplicationViewState viewState)
        {
            if(m_applicationViewState==viewState)
                return;

            m_applicationViewState = viewState;
            switch (m_applicationViewState)
            {
                case ApplicationViewState.Console:
                    InitConsoleView();
                    break;
                case ApplicationViewState.Settings:
                    InitSettingsView();
                    break;
            }
        }

        void InitSettingsView()
        {
            var compensationValue = m_compensationBridge.GetCompensationModel();
            
            m_positionX = new NumericUpDown();
            m_positionX.Minimum = -100;
            m_positionX.Maximum = 100;
            m_positionX.DecimalPlaces = 2;
            m_positionX.Value = compensationValue.X;
            
            m_positionY = new NumericUpDown();
            m_positionY.Minimum = -100;
            m_positionY.Maximum = 100;
            m_positionY.DecimalPlaces = 2;
         
            m_positionY.Value = compensationValue.Y;
            m_positionXLabel = new Label();
            m_positionYLabel = new Label();
            
            m_positionX.Location = new Point(86, 22);
            m_positionX.Name = "PositionX";
            m_positionX.Size = new Size(59, 23);
            m_positionX.TabIndex = 0;
      
            m_positionY.Location = new Point(238, 22);
            m_positionY.Name = "PositionY";
            m_positionY.Size = new Size(56, 23);
            m_positionY.TabIndex = 1;
        
            m_positionXLabel.AutoSize = true;
            m_positionXLabel.Location = new Point(20, 25);
            m_positionXLabel.Name = "PositionXLabel";
            m_positionXLabel.Size = new Size(60, 15);
            m_positionXLabel.TabIndex = 4;
            m_positionXLabel.Text = "Position X";
        
            m_positionYLabel.AutoSize = true;
            m_positionYLabel.Location = new Point(172, 25);
            m_positionYLabel.Name = "PositionYLabel";
            m_positionYLabel.Size = new Size(60, 15);
            m_positionYLabel.TabIndex = 5;
            m_positionYLabel.Text = "Position Y";
            
            Controls.Add(m_positionX);
            Controls.Add(m_positionY);
        
            Controls.Add(m_positionXLabel);
            Controls.Add(m_positionYLabel);
            
            m_positionX.ValueChanged += CompensationValueChanged;
            m_positionY.ValueChanged += CompensationValueChanged;
        }

        void InitConsoleView()
        {
            if (m_positionX != null)
            {
                Controls.Remove(m_positionX);
                Controls.Remove(m_positionY);

                Controls.Remove(m_positionXLabel);
                Controls.Remove(m_positionYLabel);

                m_positionX.ValueChanged -= CompensationValueChanged;
                m_positionY.ValueChanged -= CompensationValueChanged;
                
                m_positionX = null;
                m_positionY = null;
                m_positionXLabel = null;
                m_positionYLabel = null;
            }
        }
    }
}