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
        Me.Icon = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NumericUpDownX = New System.Windows.Forms.NumericUpDown()
        Me.OffsetX = New System.Windows.Forms.Label()
        Me.OffsetY = New System.Windows.Forms.Label()
        Me.NumericUpDownY = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.Icon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownY, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Icon
        '
        Me.Icon.BackColor = System.Drawing.Color.Transparent
        Me.Icon.Image = Global.RotoVR_InterfacePlugin.My.Resources.Resources.roto_icon
        Me.Icon.Location = New System.Drawing.Point(577, 0)
        Me.Icon.Name = "Icon"
        Me.Icon.Size = New System.Drawing.Size(128, 128)
        Me.Icon.TabIndex = 263
        Me.Icon.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(209, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(244, 26)
        Me.Label1.TabIndex = 264
        Me.Label1.Text = "RotoVR Monitor Plugin"
        '
        'NumericUpDownX
        '
        Me.NumericUpDownX.DecimalPlaces = 2
        Me.NumericUpDownX.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NumericUpDownX.Location = New System.Drawing.Point(251, 102)
        Me.NumericUpDownX.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.NumericUpDownX.Name = "NumericUpDownX"
        Me.NumericUpDownX.Size = New System.Drawing.Size(70, 23)
        Me.NumericUpDownX.TabIndex = 265
        '
        'OffsetX
        '
        Me.OffsetX.AutoSize = True
        Me.OffsetX.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OffsetX.ForeColor = System.Drawing.Color.Black
        Me.OffsetX.Location = New System.Drawing.Point(178, 104)
        Me.OffsetX.Name = "OffsetX"
        Me.OffsetX.Size = New System.Drawing.Size(67, 17)
        Me.OffsetX.TabIndex = 266
        Me.OffsetX.Text = "Offset X"
        '
        'OffsetY
        '
        Me.OffsetY.AutoSize = True
        Me.OffsetY.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OffsetY.ForeColor = System.Drawing.Color.Black
        Me.OffsetY.Location = New System.Drawing.Point(340, 104)
        Me.OffsetY.Name = "OffsetY"
        Me.OffsetY.Size = New System.Drawing.Size(67, 17)
        Me.OffsetY.TabIndex = 268
        Me.OffsetY.Text = "Offset Y"
        '
        'NumericUpDownY
        '
        Me.NumericUpDownY.DecimalPlaces = 2
        Me.NumericUpDownY.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NumericUpDownY.Location = New System.Drawing.Point(413, 102)
        Me.NumericUpDownY.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.NumericUpDownY.Name = "NumericUpDownY"
        Me.NumericUpDownY.Size = New System.Drawing.Size(70, 23)
        Me.NumericUpDownY.TabIndex = 267
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(285, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 19)
        Me.Label2.TabIndex = 269
        Me.Label2.Text = "Pilot offset"
        '
        '
        'UserControl_Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.OffsetY)
        Me.Controls.Add(Me.NumericUpDownY)
        Me.Controls.Add(Me.OffsetX)
        Me.Controls.Add(Me.NumericUpDownX)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Icon)
        Me.ForeColor = System.Drawing.Color.White
        Me.Name = "UserControl_Settings"
        Me.Size = New System.Drawing.Size(705, 275)
        CType(Me.Icon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownY, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Icon As Windows.Forms.PictureBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents NumericUpDownX As Windows.Forms.NumericUpDown
    Friend WithEvents OffsetX As Windows.Forms.Label
    Friend WithEvents OffsetY As Windows.Forms.Label
    Friend WithEvents NumericUpDownY As Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As Windows.Forms.Label
End Class
