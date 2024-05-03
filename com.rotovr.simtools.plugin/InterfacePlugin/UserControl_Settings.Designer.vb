<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UserControl_Settings
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cb_StopBits = New System.Windows.Forms.ComboBox()
        Me.cb_Parity = New System.Windows.Forms.ComboBox()
        Me.cb_DateBits = New System.Windows.Forms.ComboBox()
        Me.cb_BPS = New System.Windows.Forms.ComboBox()
        Me.cb_ComPort = New System.Windows.Forms.ComboBox()
        Me.rad_Binary = New System.Windows.Forms.RadioButton()
        Me.rad_Decimal = New System.Windows.Forms.RadioButton()
        Me.cb_OutputBits = New System.Windows.Forms.ComboBox()
        Me.rad_Hex = New System.Windows.Forms.RadioButton()
        Me.cb_PacketRate = New System.Windows.Forms.ComboBox()
        Me.cb_StopMS = New System.Windows.Forms.ComboBox()
        Me.cb_StartMS = New System.Windows.Forms.ComboBox()
        Me.txt_StartUpOutput = New System.Windows.Forms.TextBox()
        Me.txt_ShutDownOutput = New System.Windows.Forms.TextBox()
        Me.txt_InterfaceOutput = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cb_StopBits
        '
        Me.cb_StopBits.BackColor = System.Drawing.SystemColors.Window
        Me.cb_StopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_StopBits.FormattingEnabled = True
        Me.cb_StopBits.Items.AddRange(New Object() {"-", "1", "1.5 ", "2"})
        Me.cb_StopBits.Location = New System.Drawing.Point(3, 121)
        Me.cb_StopBits.MaxDropDownItems = 25
        Me.cb_StopBits.Name = "cb_StopBits"
        Me.cb_StopBits.Size = New System.Drawing.Size(100, 21)
        Me.cb_StopBits.TabIndex = 250
        Me.cb_StopBits.TabStop = False
        '
        'cb_Parity
        '
        Me.cb_Parity.AutoCompleteCustomSource.AddRange(New String() {"Even", "Odd", "None", "Mark", "Space"})
        Me.cb_Parity.BackColor = System.Drawing.SystemColors.Window
        Me.cb_Parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_Parity.FormattingEnabled = True
        Me.cb_Parity.Items.AddRange(New Object() {"-", "Even", "Odd", "None", "Mark", "Space"})
        Me.cb_Parity.Location = New System.Drawing.Point(3, 84)
        Me.cb_Parity.MaxDropDownItems = 25
        Me.cb_Parity.Name = "cb_Parity"
        Me.cb_Parity.Size = New System.Drawing.Size(100, 21)
        Me.cb_Parity.TabIndex = 249
        Me.cb_Parity.TabStop = False
        '
        'cb_DateBits
        '
        Me.cb_DateBits.AutoCompleteCustomSource.AddRange(New String() {"4", "5", "6", "7", "8"})
        Me.cb_DateBits.BackColor = System.Drawing.SystemColors.Window
        Me.cb_DateBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_DateBits.FormattingEnabled = True
        Me.cb_DateBits.Items.AddRange(New Object() {"-", "7", "8"})
        Me.cb_DateBits.Location = New System.Drawing.Point(0, 57)
        Me.cb_DateBits.MaxDropDownItems = 25
        Me.cb_DateBits.Name = "cb_DateBits"
        Me.cb_DateBits.Size = New System.Drawing.Size(66, 21)
        Me.cb_DateBits.TabIndex = 244
        Me.cb_DateBits.TabStop = False
        '
        'cb_BPS
        '
        Me.cb_BPS.BackColor = System.Drawing.SystemColors.Window
        Me.cb_BPS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_BPS.FormattingEnabled = True
        Me.cb_BPS.Items.AddRange(New Object() {"-", "9600", "14400", "19200", "28800", "31250", "38400", "57600", "115200", "230400", "250000", "460800", "500000", "921600"})
        Me.cb_BPS.Location = New System.Drawing.Point(0, 30)
        Me.cb_BPS.MaxDropDownItems = 25
        Me.cb_BPS.Name = "cb_BPS"
        Me.cb_BPS.Size = New System.Drawing.Size(93, 21)
        Me.cb_BPS.TabIndex = 243
        Me.cb_BPS.TabStop = False
        '
        'cb_ComPort
        '
        Me.cb_ComPort.BackColor = System.Drawing.SystemColors.Window
        Me.cb_ComPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_ComPort.FormattingEnabled = True
        Me.cb_ComPort.Items.AddRange(New Object() {"-"})
        Me.cb_ComPort.Location = New System.Drawing.Point(3, 3)
        Me.cb_ComPort.MaxDropDownItems = 25
        Me.cb_ComPort.Name = "cb_ComPort"
        Me.cb_ComPort.Size = New System.Drawing.Size(86, 21)
        Me.cb_ComPort.TabIndex = 242
        Me.cb_ComPort.TabStop = False
        '
        'rad_Binary
        '
        Me.rad_Binary.AutoSize = True
        Me.rad_Binary.Checked = True
        Me.rad_Binary.ForeColor = System.Drawing.Color.White
        Me.rad_Binary.Location = New System.Drawing.Point(126, 58)
        Me.rad_Binary.Name = "rad_Binary"
        Me.rad_Binary.Size = New System.Drawing.Size(54, 17)
        Me.rad_Binary.TabIndex = 251
        Me.rad_Binary.TabStop = True
        Me.rad_Binary.Text = "Binary"
        Me.rad_Binary.UseVisualStyleBackColor = True
        '
        'rad_Decimal
        '
        Me.rad_Decimal.AutoSize = True
        Me.rad_Decimal.ForeColor = System.Drawing.Color.White
        Me.rad_Decimal.Location = New System.Drawing.Point(133, 88)
        Me.rad_Decimal.Name = "rad_Decimal"
        Me.rad_Decimal.Size = New System.Drawing.Size(63, 17)
        Me.rad_Decimal.TabIndex = 252
        Me.rad_Decimal.Text = "Decimal"
        Me.rad_Decimal.UseVisualStyleBackColor = True
        '
        'cb_OutputBits
        '
        Me.cb_OutputBits.BackColor = System.Drawing.SystemColors.Window
        Me.cb_OutputBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_OutputBits.FormattingEnabled = True
        Me.cb_OutputBits.Items.AddRange(New Object() {"-", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32"})
        Me.cb_OutputBits.Location = New System.Drawing.Point(126, 16)
        Me.cb_OutputBits.MaxDropDownItems = 25
        Me.cb_OutputBits.Name = "cb_OutputBits"
        Me.cb_OutputBits.Size = New System.Drawing.Size(70, 21)
        Me.cb_OutputBits.TabIndex = 248
        Me.cb_OutputBits.TabStop = False
        '
        'rad_Hex
        '
        Me.rad_Hex.AutoSize = True
        Me.rad_Hex.ForeColor = System.Drawing.Color.White
        Me.rad_Hex.Location = New System.Drawing.Point(136, 121)
        Me.rad_Hex.Name = "rad_Hex"
        Me.rad_Hex.Size = New System.Drawing.Size(44, 17)
        Me.rad_Hex.TabIndex = 253
        Me.rad_Hex.Text = "Hex"
        Me.rad_Hex.UseVisualStyleBackColor = True
        '
        'cb_PacketRate
        '
        Me.cb_PacketRate.BackColor = System.Drawing.SystemColors.Window
        Me.cb_PacketRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_PacketRate.FormattingEnabled = True
        Me.cb_PacketRate.IntegralHeight = False
        Me.cb_PacketRate.Items.AddRange(New Object() {"-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50"})
        Me.cb_PacketRate.Location = New System.Drawing.Point(289, 210)
        Me.cb_PacketRate.MaxDropDownItems = 10
        Me.cb_PacketRate.Name = "cb_PacketRate"
        Me.cb_PacketRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cb_PacketRate.Size = New System.Drawing.Size(47, 21)
        Me.cb_PacketRate.TabIndex = 245
        Me.cb_PacketRate.TabStop = False
        '
        'cb_StopMS
        '
        Me.cb_StopMS.BackColor = System.Drawing.SystemColors.Window
        Me.cb_StopMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_StopMS.FormattingEnabled = True
        Me.cb_StopMS.IntegralHeight = False
        Me.cb_StopMS.Items.AddRange(New Object() {"-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "100", "250", "500", "1000", "1800", "2600", "3400", "4200", "5000"})
        Me.cb_StopMS.Location = New System.Drawing.Point(289, 239)
        Me.cb_StopMS.MaxDropDownItems = 10
        Me.cb_StopMS.Name = "cb_StopMS"
        Me.cb_StopMS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cb_StopMS.Size = New System.Drawing.Size(47, 21)
        Me.cb_StopMS.TabIndex = 257
        Me.cb_StopMS.TabStop = False
        '
        'cb_StartMS
        '
        Me.cb_StartMS.BackColor = System.Drawing.SystemColors.Window
        Me.cb_StartMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_StartMS.FormattingEnabled = True
        Me.cb_StartMS.IntegralHeight = False
        Me.cb_StartMS.Items.AddRange(New Object() {"-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "100", "250", "500", "1000", "1800", "2600", "3400", "4200", "5000"})
        Me.cb_StartMS.Location = New System.Drawing.Point(289, 181)
        Me.cb_StartMS.MaxDropDownItems = 10
        Me.cb_StartMS.Name = "cb_StartMS"
        Me.cb_StartMS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cb_StartMS.Size = New System.Drawing.Size(47, 21)
        Me.cb_StartMS.TabIndex = 254
        Me.cb_StartMS.TabStop = False
        '
        'txt_StartUpOutput
        '
        Me.txt_StartUpOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_StartUpOutput.Location = New System.Drawing.Point(-2, 152)
        Me.txt_StartUpOutput.Name = "txt_StartUpOutput"
        Me.txt_StartUpOutput.Size = New System.Drawing.Size(251, 21)
        Me.txt_StartUpOutput.TabIndex = 260
        Me.txt_StartUpOutput.TabStop = False
        '
        'txt_ShutDownOutput
        '
        Me.txt_ShutDownOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_ShutDownOutput.Location = New System.Drawing.Point(1, 212)
        Me.txt_ShutDownOutput.Name = "txt_ShutDownOutput"
        Me.txt_ShutDownOutput.Size = New System.Drawing.Size(251, 21)
        Me.txt_ShutDownOutput.TabIndex = 261
        Me.txt_ShutDownOutput.TabStop = False
        '
        'txt_InterfaceOutput
        '
        Me.txt_InterfaceOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_InterfaceOutput.Location = New System.Drawing.Point(1, 181)
        Me.txt_InterfaceOutput.Name = "txt_InterfaceOutput"
        Me.txt_InterfaceOutput.Size = New System.Drawing.Size(251, 21)
        Me.txt_InterfaceOutput.TabIndex = 262
        Me.txt_InterfaceOutput.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.RotoVR_InterfacePlugin.My.Resources.Resources.roto_icon
        Me.PictureBox1.Location = New System.Drawing.Point(577, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(128, 128)
        Me.PictureBox1.TabIndex = 263
        Me.PictureBox1.TabStop = False
        '
        'UserControl_Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.txt_InterfaceOutput)
        Me.Controls.Add(Me.cb_DateBits)
        Me.Controls.Add(Me.cb_ComPort)
        Me.Controls.Add(Me.txt_ShutDownOutput)
        Me.Controls.Add(Me.txt_StartUpOutput)
        Me.Controls.Add(Me.cb_StartMS)
        Me.Controls.Add(Me.cb_BPS)
        Me.Controls.Add(Me.cb_StopMS)
        Me.Controls.Add(Me.cb_Parity)
        Me.Controls.Add(Me.cb_PacketRate)
        Me.Controls.Add(Me.cb_StopBits)
        Me.Controls.Add(Me.rad_Hex)
        Me.Controls.Add(Me.cb_OutputBits)
        Me.Controls.Add(Me.rad_Decimal)
        Me.Controls.Add(Me.rad_Binary)
        Me.ForeColor = System.Drawing.Color.White
        Me.Name = "UserControl_Settings"
        Me.Size = New System.Drawing.Size(705, 275)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cb_StopBits As System.Windows.Forms.ComboBox
    Friend WithEvents cb_Parity As System.Windows.Forms.ComboBox
    Friend WithEvents cb_DateBits As System.Windows.Forms.ComboBox
    Friend WithEvents cb_BPS As System.Windows.Forms.ComboBox
    Friend WithEvents cb_ComPort As System.Windows.Forms.ComboBox
    Friend WithEvents rad_Binary As Windows.Forms.RadioButton
    Friend WithEvents rad_Decimal As Windows.Forms.RadioButton
    Friend WithEvents cb_OutputBits As Windows.Forms.ComboBox
    Friend WithEvents rad_Hex As Windows.Forms.RadioButton
    Friend WithEvents cb_PacketRate As Windows.Forms.ComboBox
    Friend WithEvents cb_StopMS As Windows.Forms.ComboBox
    Friend WithEvents cb_StartMS As Windows.Forms.ComboBox
    Friend WithEvents txt_StartUpOutput As Windows.Forms.TextBox
    Friend WithEvents txt_ShutDownOutput As Windows.Forms.TextBox
    Friend WithEvents txt_InterfaceOutput As Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As Windows.Forms.PictureBox
End Class
