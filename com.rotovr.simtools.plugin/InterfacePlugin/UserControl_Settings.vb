Option Strict On
Option Explicit On

Public Class UserControl_Settings
    '///////////////////////////////////////////////////////////////////////////////
    '///    The Interface Settings Structure - Elements Needed for the Form      ///
    '///////////////////////////////////////////////////////////////////////////////
    Public _InterfaceSettings As New InterfaceAssignments
    <Serializable()> Public Structure InterfaceAssignments
        Public _ComPort As String '(REQUIRED - Do Not Change!) - used for auto resetup - if not required, just leave it blank
        Public _OutPutRateMS As Integer '(Required - Do Not Change) - this is the 'OutputRate' var that is returned to SimTools
        Public _BitsPerSec As Integer
        Public _DataBits As Integer
        Public _ParityBits As String
        Public _StopBits As String
        Public _OutputBits As String
        Public _OutputType As String
        Public _StartupOutput As String
        Public _HWStartMS As Integer
        Public _InterfaceOutput As String
        Public _ShutdownOutput As String
        Public _HWStopMS As Integer
    End Structure

    '///////////////////////////////////////////////////////////////////////////////
    '///  Edit the Subroutines below to provide support for your new Interface!  ///
    '///////////////////////////////////////////////////////////////////////////////
    'SimTools uses this to set the axis names
    Public Sub Set_AxisNames(Axis1a As String, Axis2a As String, Axis3a As String, Axis4a As String, Axis5a As String, Axis6a As String, Axis1b As String, Axis2b As String, Axis3b As String, Axis4b As String, Axis5b As String, Axis6b As String, Axis1c As String, Axis2c As String, Axis3c As String, Axis4c As String, Axis5c As String, Axis6c As String)
        'store the axis names for future use
        AxisNames.Axis1a = Axis1a
        AxisNames.Axis2a = Axis2a
        AxisNames.Axis3a = Axis3a
        AxisNames.Axis4a = Axis4a
        AxisNames.Axis5a = Axis5a
        AxisNames.Axis6a = Axis6a
        AxisNames.Axis1b = Axis1b
        AxisNames.Axis2b = Axis2b
        AxisNames.Axis3b = Axis3b
        AxisNames.Axis4b = Axis4b
        AxisNames.Axis5b = Axis5b
        AxisNames.Axis6b = Axis6b
        AxisNames.Axis1c = Axis1c
        AxisNames.Axis2c = Axis2c
        AxisNames.Axis3c = Axis3c
        AxisNames.Axis4c = Axis4c
        AxisNames.Axis5c = Axis5c
        AxisNames.Axis6c = Axis6c

        'set the axis names where needed
        Dim _StartUpOutput As String = _InterfaceSettings._StartupOutput.Replace("<Axis1a>", "<" & AxisNames.Axis1a & ">").Replace("<Axis2a>", "<" & AxisNames.Axis2a & ">").Replace("<Axis3a>", "<" & AxisNames.Axis3a & ">").Replace("<Axis4a>", "<" & AxisNames.Axis4a & ">").Replace("<Axis5a>", "<" & AxisNames.Axis5a & ">").Replace("<Axis6a>", "<" & AxisNames.Axis6a & ">")
        _StartUpOutput = _StartUpOutput.Replace("<Axis1b>", "<" & AxisNames.Axis1b & ">").Replace("<Axis2b>", "<" & AxisNames.Axis2b & ">").Replace("<Axis3b>", "<" & AxisNames.Axis3b & ">").Replace("<Axis4b>", "<" & AxisNames.Axis4b & ">").Replace("<Axis5b>", "<" & AxisNames.Axis5b & ">").Replace("<Axis6b>", "<" & AxisNames.Axis6b & ">")
        _StartUpOutput = _StartUpOutput.Replace("<Axis1c>", "<" & AxisNames.Axis1c & ">").Replace("<Axis2c>", "<" & AxisNames.Axis2c & ">").Replace("<Axis3c>", "<" & AxisNames.Axis3c & ">").Replace("<Axis4c>", "<" & AxisNames.Axis4c & ">").Replace("<Axis5c>", "<" & AxisNames.Axis5c & ">").Replace("<Axis6c>", "<" & AxisNames.Axis6c & ">")
        txt_StartUpOutput.Text = _StartUpOutput

        Dim _InterfaceOutput As String = _InterfaceSettings._InterfaceOutput.Replace("<Axis1a>", "<" & AxisNames.Axis1a & ">").Replace("<Axis2a>", "<" & AxisNames.Axis2a & ">").Replace("<Axis3a>", "<" & AxisNames.Axis3a & ">").Replace("<Axis4a>", "<" & AxisNames.Axis4a & ">").Replace("<Axis5a>", "<" & AxisNames.Axis5a & ">").Replace("<Axis6a>", "<" & AxisNames.Axis6a & ">")
        _InterfaceOutput = _InterfaceOutput.Replace("<Axis1b>", "<" & AxisNames.Axis1b & ">").Replace("<Axis2b>", "<" & AxisNames.Axis2b & ">").Replace("<Axis3b>", "<" & AxisNames.Axis3b & ">").Replace("<Axis4b>", "<" & AxisNames.Axis4b & ">").Replace("<Axis5b>", "<" & AxisNames.Axis5b & ">").Replace("<Axis6b>", "<" & AxisNames.Axis6b & ">")
        _InterfaceOutput = _InterfaceOutput.Replace("<Axis1c>", "<" & AxisNames.Axis1c & ">").Replace("<Axis2c>", "<" & AxisNames.Axis2c & ">").Replace("<Axis3c>", "<" & AxisNames.Axis3c & ">").Replace("<Axis4c>", "<" & AxisNames.Axis4c & ">").Replace("<Axis5c>", "<" & AxisNames.Axis5c & ">").Replace("<Axis6c>", "<" & AxisNames.Axis6c & ">")
        txt_InterfaceOutput.Text = _InterfaceOutput

        Dim _ShutDownOutput As String = _InterfaceSettings._ShutdownOutput.Replace("<Axis1a>", "<" & AxisNames.Axis1a & ">").Replace("<Axis2a>", "<" & AxisNames.Axis2a & ">").Replace("<Axis3a>", "<" & AxisNames.Axis3a & ">").Replace("<Axis4a>", "<" & AxisNames.Axis4a & ">").Replace("<Axis5a>", "<" & AxisNames.Axis5a & ">").Replace("<Axis6a>", "<" & AxisNames.Axis6a & ">")
        _ShutDownOutput = _ShutDownOutput.Replace("<Axis1b>", "<" & AxisNames.Axis1b & ">").Replace("<Axis2b>", "<" & AxisNames.Axis2b & ">").Replace("<Axis3b>", "<" & AxisNames.Axis3b & ">").Replace("<Axis4b>", "<" & AxisNames.Axis4b & ">").Replace("<Axis5b>", "<" & AxisNames.Axis5b & ">").Replace("<Axis6b>", "<" & AxisNames.Axis6b & ">")
        _ShutDownOutput = _ShutDownOutput.Replace("<Axis1c>", "<" & AxisNames.Axis1c & ">").Replace("<Axis2c>", "<" & AxisNames.Axis2c & ">").Replace("<Axis3c>", "<" & AxisNames.Axis3c & ">").Replace("<Axis4c>", "<" & AxisNames.Axis4c & ">").Replace("<Axis5c>", "<" & AxisNames.Axis5c & ">").Replace("<Axis6c>", "<" & AxisNames.Axis6c & ">")
        txt_ShutDownOutput.Text = _ShutDownOutput
    End Sub

    'SimTools uses this to (Load the Form from the Structure)
    Public Sub LoadFormFromStructure()
        'Interface Unpluged Reload Fix


        'load values
        If _InterfaceSettings._OutputType = "Binary" Then
            rad_Binary.Checked = True
        ElseIf _InterfaceSettings._OutputType = "Decimal" Then
            rad_Decimal.Checked = True
        ElseIf _InterfaceSettings._OutputType = "Hex" Then
            rad_Hex.Checked = True
        End If
        If _InterfaceSettings._HWStartMS = 0 Then

        Else

        End If
        If _InterfaceSettings._HWStopMS = 0 Then

        Else

        End If
        If _InterfaceSettings._OutPutRateMS = 0 Then

        Else

        End If
        txt_StartUpOutput.Text = _InterfaceSettings._StartupOutput
        txt_InterfaceOutput.Text = _InterfaceSettings._InterfaceOutput
        txt_ShutDownOutput.Text = _InterfaceSettings._ShutdownOutput
    End Sub

    'SimTools uses this to (Load the Structure from the Form)
    Public Sub LoadStrutureFromForm()

        If rad_Binary.Checked = True Then
            _InterfaceSettings._OutputType = "Binary"
        ElseIf rad_Decimal.Checked = True Then
            _InterfaceSettings._OutputType = "Decimal"
        Else
            _InterfaceSettings._OutputType = "Hex"
        End If


        'save the axis output with normal names (Example: <Axis1a>)
        Dim _StartUpOutput As String = txt_StartUpOutput.Text.Replace("<" & AxisNames.Axis1a & ">", "<Axis1a>").Replace("<" & AxisNames.Axis2a & ">", "<Axis2a>").Replace("<" & AxisNames.Axis3a & ">", "<Axis3a>").Replace("<" & AxisNames.Axis4a & ">", "<Axis4a>").Replace("<" & AxisNames.Axis5a & ">", "<Axis5a>").Replace("<" & AxisNames.Axis6a & ">", "<Axis6a>")
        _StartUpOutput = _StartUpOutput.Replace("<" & AxisNames.Axis1b & ">", "<Axis1b>").Replace("<" & AxisNames.Axis2b & ">", "<Axis2b>").Replace("<" & AxisNames.Axis3b & ">", "<Axis3b>").Replace("<" & AxisNames.Axis4b & ">", "<Axis4b>").Replace("<" & AxisNames.Axis5b & ">", "<Axis5b>").Replace("<" & AxisNames.Axis6b & ">", "<Axis6b>")
        _StartUpOutput = _StartUpOutput.Replace("<" & AxisNames.Axis1c & ">", "<Axis1c>").Replace("<" & AxisNames.Axis2c & ">", "<Axis2c>").Replace("<" & AxisNames.Axis3c & ">", "<Axis3c>").Replace("<" & AxisNames.Axis4c & ">", "<Axis4c>").Replace("<" & AxisNames.Axis5c & ">", "<Axis5c>").Replace("<" & AxisNames.Axis6c & ">", "<Axis6c>")
        _InterfaceSettings._StartupOutput = _StartUpOutput

        Dim _InterfaceOutput As String = txt_InterfaceOutput.Text.Replace("<" & AxisNames.Axis1a & ">", "<Axis1a>").Replace("<" & AxisNames.Axis2a & ">", "<Axis2a>").Replace("<" & AxisNames.Axis3a & ">", "<Axis3a>").Replace("<" & AxisNames.Axis4a & ">", "<Axis4a>").Replace("<" & AxisNames.Axis5a & ">", "<Axis5a>").Replace("<" & AxisNames.Axis6a & ">", "<Axis6a>")
        _InterfaceOutput = _InterfaceOutput.Replace("<" & AxisNames.Axis1b & ">", "<Axis1b>").Replace("<" & AxisNames.Axis2b & ">", "<Axis2b>").Replace("<" & AxisNames.Axis3b & ">", "<Axis3b>").Replace("<" & AxisNames.Axis4b & ">", "<Axis4b>").Replace("<" & AxisNames.Axis5b & ">", "<Axis5b>").Replace("<" & AxisNames.Axis6b & ">", "<Axis6b>")
        _InterfaceOutput = _InterfaceOutput.Replace("<" & AxisNames.Axis1c & ">", "<Axis1c>").Replace("<" & AxisNames.Axis2c & ">", "<Axis2c>").Replace("<" & AxisNames.Axis3c & ">", "<Axis3c>").Replace("<" & AxisNames.Axis4c & ">", "<Axis4c>").Replace("<" & AxisNames.Axis5c & ">", "<Axis5c>").Replace("<" & AxisNames.Axis6c & ">", "<Axis6c>")
        _InterfaceSettings._InterfaceOutput = _InterfaceOutput

        Dim _ShutDownOutput As String = txt_ShutDownOutput.Text.Replace("<" & AxisNames.Axis1a & ">", "<Axis1a>").Replace("<" & AxisNames.Axis2a & ">", "<Axis2a>").Replace("<" & AxisNames.Axis3a & ">", "<Axis3a>").Replace("<" & AxisNames.Axis4a & ">", "<Axis4a>").Replace("<" & AxisNames.Axis5a & ">", "<Axis5a>").Replace("<" & AxisNames.Axis6a & ">", "<Axis6a>")
        _ShutDownOutput = _ShutDownOutput.Replace("<" & AxisNames.Axis1b & ">", "<Axis1b>").Replace("<" & AxisNames.Axis2b & ">", "<Axis2b>").Replace("<" & AxisNames.Axis3b & ">", "<Axis3b>").Replace("<" & AxisNames.Axis4b & ">", "<Axis4b>").Replace("<" & AxisNames.Axis5b & ">", "<Axis5b>").Replace("<" & AxisNames.Axis6b & ">", "<Axis6b>")
        _ShutDownOutput = _ShutDownOutput.Replace("<" & AxisNames.Axis1c & ">", "<Axis1c>").Replace("<" & AxisNames.Axis2c & ">", "<Axis2c>").Replace("<" & AxisNames.Axis3c & ">", "<Axis3c>").Replace("<" & AxisNames.Axis4c & ">", "<Axis4c>").Replace("<" & AxisNames.Axis5c & ">", "<Axis5c>").Replace("<" & AxisNames.Axis6c & ">", "<Axis6c>")
        _InterfaceSettings._ShutdownOutput = _ShutDownOutput
    End Sub

    'SimTools uses this to (Clear the Form)
    Public Sub ClearSettingsWindow()
        rad_Binary.Checked = True
        txt_StartUpOutput.Text = ""
        txt_InterfaceOutput.Text = ""
        txt_ShutDownOutput.Text = ""
    End Sub

    'Optional - Paint the plugins form to match the skin
    Public Sub PaintSkin(GuiColors As Interface_PluginAPI_v3.IPlugin_Interface_v3._Colors)
        ''set form color
        'BackColor = GuiColors.BackColor

        ''set text color
        'For Each ctrl As Control In Controls
        '    'lables
        '    If TypeOf (ctrl) Is Label Then
        '        ctrl.ForeColor = GuiColors.TextColor
        '    End If
        '    'RadioButton
        '    If TypeOf (ctrl) Is RadioButton Then
        '        ctrl.ForeColor = GuiColors.TextColor
        '    End If
        'Next
    End Sub

    'This determines when the form has enough info to enable the 'Save' button. 
    Public Event SetSave_Button(Value As Boolean)
    Public Sub CheckSaveButton()
        Try

        Catch ex As Exception
            RaiseEvent SetSave_Button(False)
        End Try
    End Sub

    '///////////////////////////////////////////////////////////////////////////////
    '///              User Controls for the Plugins Settings Form                ///
    '///////////////////////////////////////////////////////////////////////////////
    Public Event _Error(Value As String)


    'Structure used to strore the axis names
    Private AxisNames As New MyAxis
    Private Structure MyAxis
        Public Axis1a As String
        Public Axis2a As String
        Public Axis3a As String
        Public Axis4a As String
        Public Axis5a As String
        Public Axis6a As String
        Public Axis1b As String
        Public Axis2b As String
        Public Axis3b As String
        Public Axis4b As String
        Public Axis5b As String
        Public Axis6b As String
        Public Axis1c As String
        Public Axis2c As String
        Public Axis3c As String
        Public Axis4c As String
        Public Axis5c As String
        Public Axis6c As String
    End Structure

End Class
