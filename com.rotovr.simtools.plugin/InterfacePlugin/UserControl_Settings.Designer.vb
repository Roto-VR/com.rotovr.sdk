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
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.rad_Binary = New System.Windows.Forms.RadioButton()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.rad_Decimal = New System.Windows.Forms.RadioButton()
        Me.cb_OutputBits = New System.Windows.Forms.ComboBox()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.rad_Hex = New System.Windows.Forms.RadioButton()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.cb_PacketRate = New System.Windows.Forms.ComboBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.cb_StopMS = New System.Windows.Forms.ComboBox()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.cb_StartMS = New System.Windows.Forms.ComboBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.txt_StartUpOutput = New System.Windows.Forms.TextBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.txt_ShutDownOutput = New System.Windows.Forms.TextBox()
        Me.txt_InterfaceOutput = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'cb_StopBits
        '
        Me.cb_StopBits.BackColor = System.Drawing.SystemColors.Window
        Me.cb_StopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_StopBits.FormattingEnabled = True
        Me.cb_StopBits.Items.AddRange(New Object() {"-", "1", "1.5 ", "2"})
        Me.cb_StopBits.Location = New System.Drawing.Point(478, 61)
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
        Me.cb_Parity.Location = New System.Drawing.Point(375, 61)
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
        Me.cb_DateBits.Location = New System.Drawing.Point(306, 61)
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
        Me.cb_BPS.Location = New System.Drawing.Point(210, 61)
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
        Me.cb_ComPort.Location = New System.Drawing.Point(121, 61)
        Me.cb_ComPort.MaxDropDownItems = 25
        Me.cb_ComPort.Name = "cb_ComPort"
        Me.cb_ComPort.Size = New System.Drawing.Size(86, 21)
        Me.cb_ComPort.TabIndex = 242
        Me.cb_ComPort.TabStop = False
        '
        'Label20
        '
        Me.Label20.ForeColor = System.Drawing.Color.White
        Me.Label20.Location = New System.Drawing.Point(486, 37)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(85, 22)
        Me.Label20.TabIndex = 271
        Me.Label20.Text = "Stop Bits"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label19
        '
        Me.Label19.ForeColor = System.Drawing.Color.White
        Me.Label19.Location = New System.Drawing.Point(383, 37)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(85, 22)
        Me.Label19.TabIndex = 270
        Me.Label19.Text = "Parity"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label18
        '
        Me.Label18.ForeColor = System.Drawing.Color.White
        Me.Label18.Location = New System.Drawing.Point(307, 37)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(64, 22)
        Me.Label18.TabIndex = 269
        Me.Label18.Text = "Data Bits"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label17
        '
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(217, 37)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(78, 22)
        Me.Label17.TabIndex = 268
        Me.Label17.Text = "BitsPerSec"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label16
        '
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(130, 37)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(69, 22)
        Me.Label16.TabIndex = 267
        Me.Label16.Text = "ComPort"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'rad_Binary
        '
        Me.rad_Binary.AutoSize = True
        Me.rad_Binary.Checked = True
        Me.rad_Binary.ForeColor = System.Drawing.Color.White
        Me.rad_Binary.Location = New System.Drawing.Point(390, 108)
        Me.rad_Binary.Name = "rad_Binary"
        Me.rad_Binary.Size = New System.Drawing.Size(54, 17)
        Me.rad_Binary.TabIndex = 251
        Me.rad_Binary.TabStop = True
        Me.rad_Binary.Text = "Binary"
        Me.rad_Binary.UseVisualStyleBackColor = True
        '
        'Label50
        '
        Me.Label50.BackColor = System.Drawing.Color.Transparent
        Me.Label50.ForeColor = System.Drawing.Color.White
        Me.Label50.Location = New System.Drawing.Point(98, 150)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(101, 19)
        Me.Label50.TabIndex = 158
        Me.Label50.Text = "Startup - Output"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label51
        '
        Me.Label51.ForeColor = System.Drawing.Color.White
        Me.Label51.Location = New System.Drawing.Point(98, 179)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(101, 19)
        Me.Label51.TabIndex = 263
        Me.Label51.Text = "Interface - Output"
        Me.Label51.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'rad_Decimal
        '
        Me.rad_Decimal.AutoSize = True
        Me.rad_Decimal.ForeColor = System.Drawing.Color.White
        Me.rad_Decimal.Location = New System.Drawing.Point(456, 108)
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
        Me.cb_OutputBits.Location = New System.Drawing.Point(221, 105)
        Me.cb_OutputBits.MaxDropDownItems = 25
        Me.cb_OutputBits.Name = "cb_OutputBits"
        Me.cb_OutputBits.Size = New System.Drawing.Size(70, 21)
        Me.cb_OutputBits.TabIndex = 248
        Me.cb_OutputBits.TabStop = False
        '
        'Label48
        '
        Me.Label48.BackColor = System.Drawing.Color.Transparent
        Me.Label48.ForeColor = System.Drawing.Color.White
        Me.Label48.Location = New System.Drawing.Point(506, 214)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(26, 18)
        Me.Label48.TabIndex = 259
        Me.Label48.Text = "ms"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label52
        '
        Me.Label52.ForeColor = System.Drawing.Color.White
        Me.Label52.Location = New System.Drawing.Point(98, 208)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(101, 19)
        Me.Label52.TabIndex = 264
        Me.Label52.Text = "Shutdown - Output"
        Me.Label52.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'rad_Hex
        '
        Me.rad_Hex.AutoSize = True
        Me.rad_Hex.ForeColor = System.Drawing.Color.White
        Me.rad_Hex.Location = New System.Drawing.Point(531, 108)
        Me.rad_Hex.Name = "rad_Hex"
        Me.rad_Hex.Size = New System.Drawing.Size(44, 17)
        Me.rad_Hex.TabIndex = 253
        Me.rad_Hex.Text = "Hex"
        Me.rad_Hex.UseVisualStyleBackColor = True
        '
        'Label49
        '
        Me.Label49.BackColor = System.Drawing.Color.Transparent
        Me.Label49.ForeColor = System.Drawing.Color.White
        Me.Label49.Location = New System.Drawing.Point(534, 210)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(71, 20)
        Me.Label49.TabIndex = 258
        Me.Label49.Text = "HW Stop"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label53
        '
        Me.Label53.ForeColor = System.Drawing.Color.White
        Me.Label53.Location = New System.Drawing.Point(104, 97)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(112, 26)
        Me.Label53.TabIndex = 265
        Me.Label53.Text = "Output - Bit Range"
        Me.Label53.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'cb_PacketRate
        '
        Me.cb_PacketRate.BackColor = System.Drawing.SystemColors.Window
        Me.cb_PacketRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_PacketRate.FormattingEnabled = True
        Me.cb_PacketRate.IntegralHeight = False
        Me.cb_PacketRate.Items.AddRange(New Object() {"-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50"})
        Me.cb_PacketRate.Location = New System.Drawing.Point(458, 181)
        Me.cb_PacketRate.MaxDropDownItems = 10
        Me.cb_PacketRate.Name = "cb_PacketRate"
        Me.cb_PacketRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cb_PacketRate.Size = New System.Drawing.Size(47, 21)
        Me.cb_PacketRate.TabIndex = 245
        Me.cb_PacketRate.TabStop = False
        '
        'Label46
        '
        Me.Label46.BackColor = System.Drawing.Color.Transparent
        Me.Label46.ForeColor = System.Drawing.Color.White
        Me.Label46.Location = New System.Drawing.Point(506, 155)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(26, 18)
        Me.Label46.TabIndex = 256
        Me.Label46.Text = "ms"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label60
        '
        Me.Label60.ForeColor = System.Drawing.Color.White
        Me.Label60.Location = New System.Drawing.Point(304, 96)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(79, 26)
        Me.Label60.TabIndex = 266
        Me.Label60.Text = "Output - Type"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'cb_StopMS
        '
        Me.cb_StopMS.BackColor = System.Drawing.SystemColors.Window
        Me.cb_StopMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_StopMS.FormattingEnabled = True
        Me.cb_StopMS.IntegralHeight = False
        Me.cb_StopMS.Items.AddRange(New Object() {"-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "100", "250", "500", "1000", "1800", "2600", "3400", "4200", "5000"})
        Me.cb_StopMS.Location = New System.Drawing.Point(458, 210)
        Me.cb_StopMS.MaxDropDownItems = 10
        Me.cb_StopMS.Name = "cb_StopMS"
        Me.cb_StopMS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cb_StopMS.Size = New System.Drawing.Size(47, 21)
        Me.cb_StopMS.TabIndex = 257
        Me.cb_StopMS.TabStop = False
        '
        'Label47
        '
        Me.Label47.BackColor = System.Drawing.Color.Transparent
        Me.Label47.ForeColor = System.Drawing.Color.White
        Me.Label47.Location = New System.Drawing.Point(534, 152)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(71, 20)
        Me.Label47.TabIndex = 255
        Me.Label47.Text = "HW Start"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cb_StartMS
        '
        Me.cb_StartMS.BackColor = System.Drawing.SystemColors.Window
        Me.cb_StartMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cb_StartMS.FormattingEnabled = True
        Me.cb_StartMS.IntegralHeight = False
        Me.cb_StartMS.Items.AddRange(New Object() {"-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "100", "250", "500", "1000", "1800", "2600", "3400", "4200", "5000"})
        Me.cb_StartMS.Location = New System.Drawing.Point(458, 152)
        Me.cb_StartMS.MaxDropDownItems = 10
        Me.cb_StartMS.Name = "cb_StartMS"
        Me.cb_StartMS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cb_StartMS.Size = New System.Drawing.Size(47, 21)
        Me.cb_StartMS.TabIndex = 254
        Me.cb_StartMS.TabStop = False
        '
        'Label44
        '
        Me.Label44.BackColor = System.Drawing.Color.Transparent
        Me.Label44.ForeColor = System.Drawing.Color.White
        Me.Label44.Location = New System.Drawing.Point(506, 185)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(26, 18)
        Me.Label44.TabIndex = 247
        Me.Label44.Text = "ms"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txt_StartUpOutput
        '
        Me.txt_StartUpOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_StartUpOutput.Location = New System.Drawing.Point(204, 152)
        Me.txt_StartUpOutput.Name = "txt_StartUpOutput"
        Me.txt_StartUpOutput.Size = New System.Drawing.Size(251, 21)
        Me.txt_StartUpOutput.TabIndex = 260
        Me.txt_StartUpOutput.TabStop = False
        '
        'Label45
        '
        Me.Label45.BackColor = System.Drawing.Color.Transparent
        Me.Label45.ForeColor = System.Drawing.Color.White
        Me.Label45.Location = New System.Drawing.Point(534, 181)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(71, 20)
        Me.Label45.TabIndex = 246
        Me.Label45.Text = "Output Rate"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txt_ShutDownOutput
        '
        Me.txt_ShutDownOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_ShutDownOutput.Location = New System.Drawing.Point(204, 210)
        Me.txt_ShutDownOutput.Name = "txt_ShutDownOutput"
        Me.txt_ShutDownOutput.Size = New System.Drawing.Size(251, 21)
        Me.txt_ShutDownOutput.TabIndex = 261
        Me.txt_ShutDownOutput.TabStop = False
        '
        'txt_InterfaceOutput
        '
        Me.txt_InterfaceOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_InterfaceOutput.Location = New System.Drawing.Point(204, 181)
        Me.txt_InterfaceOutput.Name = "txt_InterfaceOutput"
        Me.txt_InterfaceOutput.Size = New System.Drawing.Size(251, 21)
        Me.txt_InterfaceOutput.TabIndex = 262
        Me.txt_InterfaceOutput.TabStop = False
        '
        'UserControl_Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.txt_InterfaceOutput)
        Me.Controls.Add(Me.cb_DateBits)
        Me.Controls.Add(Me.cb_ComPort)
        Me.Controls.Add(Me.txt_ShutDownOutput)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label45)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.txt_StartUpOutput)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label44)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.cb_StartMS)
        Me.Controls.Add(Me.cb_BPS)
        Me.Controls.Add(Me.Label47)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.cb_StopMS)
        Me.Controls.Add(Me.Label60)
        Me.Controls.Add(Me.Label46)
        Me.Controls.Add(Me.cb_Parity)
        Me.Controls.Add(Me.cb_PacketRate)
        Me.Controls.Add(Me.Label53)
        Me.Controls.Add(Me.Label49)
        Me.Controls.Add(Me.cb_StopBits)
        Me.Controls.Add(Me.rad_Hex)
        Me.Controls.Add(Me.Label52)
        Me.Controls.Add(Me.Label48)
        Me.Controls.Add(Me.cb_OutputBits)
        Me.Controls.Add(Me.rad_Decimal)
        Me.Controls.Add(Me.Label51)
        Me.Controls.Add(Me.Label50)
        Me.Controls.Add(Me.rad_Binary)
        Me.ForeColor = System.Drawing.Color.White
        Me.Name = "UserControl_Settings"
        Me.Size = New System.Drawing.Size(705, 275)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cb_StopBits As System.Windows.Forms.ComboBox
    Friend WithEvents cb_Parity As System.Windows.Forms.ComboBox
    Friend WithEvents cb_DateBits As System.Windows.Forms.ComboBox
    Friend WithEvents cb_BPS As System.Windows.Forms.ComboBox
    Friend WithEvents cb_ComPort As System.Windows.Forms.ComboBox
    Friend WithEvents rad_Binary As Windows.Forms.RadioButton
    Friend WithEvents Label50 As Windows.Forms.Label
    Friend WithEvents Label51 As Windows.Forms.Label
    Friend WithEvents rad_Decimal As Windows.Forms.RadioButton
    Friend WithEvents cb_OutputBits As Windows.Forms.ComboBox
    Friend WithEvents Label48 As Windows.Forms.Label
    Friend WithEvents Label52 As Windows.Forms.Label
    Friend WithEvents rad_Hex As Windows.Forms.RadioButton
    Friend WithEvents Label49 As Windows.Forms.Label
    Friend WithEvents Label53 As Windows.Forms.Label
    Friend WithEvents cb_PacketRate As Windows.Forms.ComboBox
    Friend WithEvents Label46 As Windows.Forms.Label
    Friend WithEvents Label60 As Windows.Forms.Label
    Friend WithEvents cb_StopMS As Windows.Forms.ComboBox
    Friend WithEvents Label47 As Windows.Forms.Label
    Friend WithEvents cb_StartMS As Windows.Forms.ComboBox
    Friend WithEvents Label44 As Windows.Forms.Label
    Friend WithEvents txt_StartUpOutput As Windows.Forms.TextBox
    Friend WithEvents Label45 As Windows.Forms.Label
    Friend WithEvents txt_ShutDownOutput As Windows.Forms.TextBox
    Friend WithEvents txt_InterfaceOutput As Windows.Forms.TextBox
End Class
