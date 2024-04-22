using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace RotoVR.Monitor
{
    partial class MonitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private ContextMenuStrip NotifyIconContextMenu;
        private ToolStripMenuItem about;
        private ToolStripMenuItem openConsole;
        private System.Windows.Forms.ToolStripMenuItem applicationQuit;
        private ToolStripMenuItem applicationConnect;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        
        /// <summary>
        /// Hide "Close" button for the form
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON ;
                return myCp;
            }
        }
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            m_connector.Disconnect();
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorForm));
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.applicationConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.openConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.about = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.BalloonTipText = "RotoVR Monitor is started";
            this.NotifyIcon.ContextMenuStrip = this.NotifyIconContextMenu;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "RotoVR Monitor";
            this.NotifyIcon.Visible = true;
            // 
            // NotifyIconContextMenu
            // 
            this.NotifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.applicationConnect, this.openConsole, this.about, this.applicationQuit });
            this.NotifyIconContextMenu.Name = "contextMenuStrip";
            this.NotifyIconContextMenu.Size = new System.Drawing.Size(150, 92);
            // 
            // applicationConnect
            // 
            this.applicationConnect.Name = "applicationConnect";
            this.applicationConnect.Size = new System.Drawing.Size(149, 22);
            this.applicationConnect.Text = "Connect";
            this.applicationConnect.Click += new System.EventHandler(this.ApplicationConnectHandler);
            // 
            // openConsole
            // 
            this.openConsole.Name = "openConsole";
            this.openConsole.Size = new System.Drawing.Size(149, 22);
            this.openConsole.Text = "Open Console";
            this.openConsole.Click += new System.EventHandler(this.OpenConsoleHandler);
            // 
            // about
            // 
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(149, 22);
            this.about.Text = "About";
            // 
            // applicationQuit
            // 
            this.applicationQuit.Name = "applicationQuit";
            this.applicationQuit.Size = new System.Drawing.Size(149, 22);
            this.applicationQuit.Text = "Quit";
            this.applicationQuit.Click += new System.EventHandler(this.ApplicationQuitHandler);
            // 
            // MonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MonitorForm";
            this.ShowInTaskbar = false;
            this.Text = "RotoVR Monitor";
            this.SizeChanged += new System.EventHandler(this.MonitorFormSizeChanged);
            this.NotifyIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

      

    }
}