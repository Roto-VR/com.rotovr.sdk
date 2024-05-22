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
        Public _XOffset As Decimal
        Public _YOffset As Decimal
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
    End Sub

    'SimTools uses this to (Load the Form from the Structure)
    Public Sub LoadFormFromStructure()
        'Interface Unpluged Reload Fix
        NumericUpDownX.Value = _InterfaceSettings._XOffset
        NumericUpDownY.Value = _InterfaceSettings._YOffset
    End Sub

    'SimTools uses this to (Load the Structure from the Form)
    Public Sub LoadStrutureFromForm()
        '
        _InterfaceSettings._XOffset = NumericUpDownX.Value
        _InterfaceSettings._YOffset = NumericUpDownY.Value

    End Sub

    'SimTools uses this to (Clear the Form)
    Public Sub ClearSettingsWindow()
        _InterfaceSettings._XOffset = 0
        _InterfaceSettings._YOffset = 0
    End Sub

    'Optional - Paint the plugins form to match the skin
    Public Sub PaintSkin(GuiColors As Interface_PluginAPI_v3.IPlugin_Interface_v3._Colors)

    End Sub

    'This determines when the form has enough info to enable the 'Save' button. 
    Public Event SetSave_Button(Value As Boolean)
    Public Sub CheckSaveButton()
        RaiseEvent SetSave_Button(True)
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
