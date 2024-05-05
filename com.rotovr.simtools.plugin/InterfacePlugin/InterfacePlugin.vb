Option Strict On
Option Explicit On

Imports System.IO
Imports System.Threading
Imports Interface_PluginAPI_v3
Public Class InterfacePlugin
    Implements IPlugin_Interface_v3
#Region "    '/// BUILT IN DATA FOR SIMTOOLS ///"
    '/////////////////////////////////////////////////////
    '/// DO NOT CHANGE - DO NOT CHANGE - DO NOT CHANGE ///
    '/////////////////////////////////////////////////////

    Private ReadOnly _SavePath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SimTools3\Motion\Interface"
    Private _HasInitialized As Boolean = False 'SimTools sets this when it loads the dll 
    Private _InterfaceNumber As Integer 'SimTools sets this when it loads the dll
    Private Property _OnlineID As String Implements IPlugin_Interface_v3.OnlineID  'set what id number

    'our form object
    Private WithEvents MyForm As New UserControl_Settings() 'Our Settings Form for this interface

    'set the save button on or off in simtools
    Public Event SetSaveButton As IPlugin_Interface_v3.SetSaveButtonEventHandler Implements IPlugin_Interface_v3.SetSaveButton
    Private Sub InterfacePlugin_SetSaveButton(Value As Boolean) Handles MyForm.SetSave_Button
        RaiseEvent SetSaveButton(Value)
    End Sub

    'set the axis names
    Private Sub SetAxisNames(Axis1a As String, Axis2a As String, Axis3a As String, Axis4a As String, Axis5a As String, Axis6a As String, Axis1b As String, Axis2b As String, Axis3b As String, Axis4b As String, Axis5b As String, Axis6b As String, Axis1c As String, Axis2c As String, Axis3c As String, Axis4c As String, Axis5c As String, Axis6c As String) Implements IPlugin_Interface_v3.SetAxisNames
        MyForm.Set_AxisNames(Axis1a, Axis2a, Axis3a, Axis4a, Axis5a, Axis6a, Axis1b, Axis2b, Axis3b, Axis4b, Axis5b, Axis6b, Axis1c, Axis2c, Axis3c, Axis4c, Axis5c, Axis6c)
    End Sub

    'Save Settings
    Private Function Save_InterfaceAssignments() As Boolean Implements IPlugin_Interface_v3.SaveSettings
        'File Path
        Dim File_SavePath As String = _SavePath & _InterfaceNumber & "\" & _OnlineID & "\InterfaceAssignments.cfg"

        'see if we are changing or adding new settings
        Dim NewSettings As Boolean = True
        If File.Exists(File_SavePath) Then
            NewSettings = False
        End If

        'Create folder if its missing
        Dim Dir_Path As String = _SavePath & _InterfaceNumber & "\" & _OnlineID
        If Directory.Exists(Dir_Path) = False Then
            Directory.CreateDirectory(Dir_Path)
        End If

        'get current settings
        MyForm.LoadStrutureFromForm()

        'Create a BinaryFormatter object
        Dim BF As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()

        'Create a Memorystream
        Dim MS As New MemoryStream()

        'Serialization it in memory
        BF.Serialize(MS, MyForm._InterfaceSettings)

        'save backup settings first - Write to the binary file
        My.Computer.FileSystem.WriteAllBytes(_SavePath & _InterfaceNumber & "\" & _OnlineID & "\InterfaceAssignments.bak", MS.GetBuffer(), False)

        'save settings - Write to the binary file
        My.Computer.FileSystem.WriteAllBytes(_SavePath & _InterfaceNumber & "\" & _OnlineID & "\InterfaceAssignments.cfg", MS.GetBuffer(), False)

        'return if we are changing or adding new settings
        Return NewSettings
    End Function

    'Load saved settings
    Private Sub LoadSavedSettings()
        'clear the form
        MyForm.ClearSettingsWindow()

        'load structure
        MyForm.LoadStrutureFromForm()

        'try to load the regular settings
        Dim SettingsLoaded As Boolean = False 'loaded settings flag
        If SettingsLoaded = False Then
            Dim File_SavePath As String = _SavePath & _InterfaceNumber & "\" & _OnlineID & "\InterfaceAssignments.cfg"
            Try
                If File.Exists(File_SavePath) Then
                    'Make an array of bytes to hold the data
                    Dim bytes As Byte() = My.Computer.FileSystem.ReadAllBytes(File_SavePath)

                    'Create a BinaryFormatter object
                    Dim BF As New Runtime.Serialization.Formatters.Binary.BinaryFormatter With {
                        .Binder = New BindChanger()
                    }

                    'Cast the data back into the structure
                    MyForm._InterfaceSettings = DirectCast(BF.Deserialize(New MemoryStream(bytes)), UserControl_Settings.InterfaceAssignments)

                    'See if we have a comfix file to load
                    Dim ComFix_Path As String = _SavePath & _InterfaceNumber & "\" & _OnlineID & "\ComFix.cfg"
                    If File.Exists(ComFix_Path) Then
                        'update comport assignment
                        Using objReader As New StreamReader(ComFix_Path)
                            MyForm._InterfaceSettings._ComPort = objReader.ReadLine
                        End Using
                        'load the form 
                        MyForm.LoadFormFromStructure()
                        'save the new settings
                        Save_InterfaceAssignments()
                        'remove ComFix file
                        File.Delete(ComFix_Path)
                    Else
                        MyForm.LoadFormFromStructure()
                    End If
                    SettingsLoaded = True
                End If
            Catch ex As Exception
                'LOG main settings file corrupt

                'delete bad file
                File.Delete(File_SavePath)
                SettingsLoaded = False
            End Try
        End If

        'try to load the backup settings
        If SettingsLoaded = False Then
            Dim File_SavePath As String = _SavePath & _InterfaceNumber & "\" & _OnlineID & "\InterfaceAssignments.bak"
            Try
                If File.Exists(File_SavePath) Then
                    'Make an array of bytes to hold the data
                    Dim bytes As Byte() = My.Computer.FileSystem.ReadAllBytes(File_SavePath)

                    'Create a BinaryFormatter object
                    Dim BF As New Runtime.Serialization.Formatters.Binary.BinaryFormatter With {
                        .Binder = New BindChanger()
                    }

                    'Cast the data back into the structure
                    MyForm._InterfaceSettings = DirectCast(BF.Deserialize(New MemoryStream(bytes)), UserControl_Settings.InterfaceAssignments)

                    'See if we have a comfix file to load
                    Dim ComFix_Path As String = _SavePath & _InterfaceNumber & "\" & _OnlineID & "\ComFix.cfg"
                    If File.Exists(ComFix_Path) Then
                        'update comport assignment
                        Using objReader As New StreamReader(ComFix_Path)
                            MyForm._InterfaceSettings._ComPort = objReader.ReadLine
                        End Using
                        'remove ComFix file
                        File.Delete(ComFix_Path)
                    End If
                    'load the form 
                    MyForm.LoadFormFromStructure()
                    'replace the corrupt file
                    Save_InterfaceAssignments()
                End If
            Catch ex As Exception
                'LOG backup settings file corrupt

                'delete bad file
                File.Delete(File_SavePath)
            End Try
        End If

        'Reset the save button
        MyForm.CheckSaveButton() 'required - do not edit
    End Sub

    'so we can load the binary file with a structure in the dll
    Private Class BindChanger
        Inherits Runtime.Serialization.SerializationBinder
        Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As Type
            Dim typeToDeserialize As Type = Nothing
            Dim currentAssembly As String = Reflection.Assembly.GetExecutingAssembly().FullName
            typeToDeserialize = Type.[GetType](String.Format("{0}, {1}", typeName, currentAssembly))
            Return typeToDeserialize
        End Function
    End Class

    'initilize startup
    Private Sub MyInitialize() Implements IPlugin_Interface_v3.Initialize
        'run the Initialize routine
        Initialize()

        'Set the HasInitialized flag to True
        _HasInitialized = True
    End Sub

    'game start
    Private _Interface_Running As Boolean = False 'Only start if there is proper settings
    Private Function Game_Start() As Boolean Implements IPlugin_Interface_v3.GameStart
        'See if we have a LastUsed file (active interface plugin)
        If File.Exists(_SavePath & _InterfaceNumber & "\Interface" & _InterfaceNumber & "_LastUsed.cfg") Then
            'Only startup if there is a settings file AND we are not already running.
            If File.Exists(_SavePath & _InterfaceNumber & "\" & _OnlineID & "\InterfaceAssignments.cfg") = True And _Interface_Running = False Then
                _Interface_Running = GameStart()
                Return _Interface_Running
            End If
        End If

        'could not start
        Return False
    End Function

    'game end
    Private Sub Game_End() Implements IPlugin_Interface_v3.GameEnd
        'Stop Engine if it has been started
        If _Interface_Running = True Then
            _Interface_Running = False
            GameStop()
            'pause for external app / Interface to recieve the data
            Threading.Thread.Sleep(100)
        End If
    End Sub

    'set what interface number
    Private WriteOnly Property InterfaceNumber As Integer Implements IPlugin_Interface_v3.WhatInterface
        Set(value As Integer)
            _InterfaceNumber = value
        End Set
    End Property

    'gets the window and loads settings if found
    Private Function GetSettingsWindow() As Object Implements IPlugin_Interface_v3.GetSettingsWindow
        'Run Startup Commands Above
        StartUp()

        'see if there are any saved settings
        LoadSavedSettings()

        'return panel
        Return MyForm
    End Function

    'clear_Interface form
    Private Sub ResetSettingsWindow() Implements IPlugin_Interface_v3.ResetSettingsWindow
        'clear the form
        MyForm.ClearSettingsWindow()
        'load structure
        MyForm.LoadStrutureFromForm() 'required - do not edit
        'Reset the save button
        MyForm.CheckSaveButton() 'required - do not edit
    End Sub

    'get name
    Private ReadOnly Property InterfaceName As String Implements IPlugin_Interface_v3.InterfaceName
        Get
            Return _InterfaceName
        End Get
    End Property

    'get author
    Private ReadOnly Property PluginAuthorsName As String Implements IPlugin_Interface_v3.PluginAuthorsName
        Get
            Return _PluginAuthorsName
        End Get
    End Property

    'requires Initialization?
    Private ReadOnly Property Init_Required As Boolean Implements IPlugin_Interface_v3.InitRequired
        Get
            Return _RequiresInitialization
        End Get
    End Property

    'Has Initialized?
    Private ReadOnly Property Has_Initialized As Boolean Implements IPlugin_Interface_v3.Has_Initialized
        Get
            Return _HasInitialized
        End Get
    End Property

    'output rate
    Private ReadOnly Property OutPutRate As Integer Implements IPlugin_Interface_v3.OutputRate
        Get
            Return MyForm._InterfaceSettings._OutPutRateMS
        End Get
    End Property

    'get plugin version number
    Private ReadOnly Property PluginVersion As String Implements IPlugin_Interface_v3.PluginVersion
        Get
            Return Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
        End Get
    End Property

    'Paint the plugins form to match the skin
    Private Sub PaintSkin(GuiColors As IPlugin_Interface_v3._Colors) Implements IPlugin_Interface_v3.PaintSkin
        MyForm.PaintSkin(GuiColors)
    End Sub

    'Error Logging
    Event ErrorLog(ErrorLevel As IPlugin_Interface_v3.Error_Level, Message As String) Implements IPlugin_Interface_v3.ErrorLog
    Private Sub Log(ErrorLevel As IPlugin_Interface_v3.Error_Level, Message As String) Implements IPlugin_Interface_v3.Log
        RaiseEvent ErrorLog(ErrorLevel, Message)
    End Sub

    'ComFix Property
    Private Property ComPort As String Implements IPlugin_Interface_v3.ComPort
        Set(value As String)
            'so we can change the comport selected
            MyForm._InterfaceSettings._ComPort = value
            MyForm.LoadFormFromStructure()
            Save_InterfaceAssignments()
        End Set
        Get
            Return MyForm._InterfaceSettings._ComPort
        End Get
    End Property

    '/////////////////////////////////////////////////////
    '/// DO NOT CHANGE - DO NOT CHANGE - DO NOT CHANGE ///
    '/////////////////////////////////////////////////////
#End Region
    '///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///  SimTools Interface Plugin - Edit the Settings below to provide support for your new Interface plugin!  ///
    '///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '////////////////////////////////////////////////////////////////////////////
    '///         Per Interface Settings - Change for Each Interface           ///
    '////////////////////////////////////////////////////////////////////////////
    Private Const _PluginAuthorsName As String = "RotoVR" 'Authors Name Here
    Private Const _InterfaceName As String = "RotoVR" 'Full Name of the Interface (Please don't use the word 'Interface' in the name)
    Private Const _RequiresInitialization As Boolean = False 'The Initialize() routine below is only run if this is set to true

    Private monitorUDPClient As MonitorTcpClient

    '///////////////////////////////////////////////////////////////////////////////
    '///                    Edit these 6 Sub/Functions Below                     ///
    '///////////////////////////////////////////////////////////////////////////////
    'Used when the program starts up.
    Private Sub StartUp()
        'all startup commands here

        'Load the Output Char's - used to get the CHAR's for 0 thru 255 - makes output faster
        LoadAsciiCodes()
        monitorUDPClient = New MonitorTcpClient()

    End Sub

    'Used when the program is shutting down / switching plugins.
    Private Sub ShutDown() Implements IPlugin_Interface_v3.ShutDown
        'all shutdown / cleanup commands here
        '


    End Sub

    'Initialize interface (centering routine - mainly used for optical systems)
    Private Sub Initialize()
        'If Needed, Initialize Interface here
        '
    End Sub

    'Used when the Game Starts  
    Private Function GameStart() As Boolean
        monitorUDPClient.SetOffset(MyForm.NumericUpDownX.Value, MyForm.NumericUpDownY.Value)
        monitorUDPClient.Connect()
        Return True
    End Function


    Private index As Integer

    'Used to send values to the Interface

    Private Sub Game_SendValues(A_Axis As IPlugin_Interface_v3.AxisPercent_Outputs, B_Axis As IPlugin_Interface_v3.AxisPercent_Outputs, C_Axis As IPlugin_Interface_v3.AxisPercent_Outputs) Implements IPlugin_Interface_v3.Game_SendValues

        monitorUDPClient.ApplyAngle(GetOutPut(A_Axis._Axis1, OutputBits, OutputType))

    End Sub



    'Used when the Game Stops
    Private Sub GameStop()
        'Shutdown
        monitorUDPClient.Disconnect()

    End Sub

    '///////////////////////////////////////////////////////////////////////////////
    '///                PLACE EXTRA NEEDED CODE/FUNCTIONS HERE                   ///
    '///////////////////////////////////////////////////////////////////////////////
#Region "Extra Code Needed for this interface"
    Private OutputBits As String = ""
    Private OutputType As String = ""
    Private Final_Output As String = ""

    'Replaces output strings in a < # > with a chr - example <63> = ?
    Public Function ReplaceWithAsciiCode(ByVal InputString As String) As String
        If InputString = "-" Or InputString = "" Then
            Return InputString
        End If
        For x = 0 To 255
            Dim SearchString As String = ("<" & x.ToString & ">").ToString
            If InputString.Contains(SearchString) Then
                Dim ReplaceString As String = (Chr(x)).ToString
                InputString = InputString.Replace(SearchString, ReplaceString)
            End If
        Next
        Return InputString
    End Function

    'Chr ouput characters
    Private AsciiCodes As New ArrayList
    'load CHR codes for faster output
    Private Sub LoadAsciiCodes()
        AsciiCodes.Clear()
        Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252) 'force USA mode to get all char's needed
        For x = 0 To 255
            AsciiCodes.Add(enc.GetString(New Byte() {CByte(x)}))
        Next
    End Sub

    'Returns the final output scaled to BitsNeeded and with Type
    Private Function GetOutPut(ByVal InputPercent As Double, ByVal BitsNeeded As String, ByVal Type As String) As Double

        Return Math.Round((InputPercent + 1.0) * 360, 0)

    End Function

    'Returns output HEX value
    Private Function GetHex(ByVal HexCode As Double) As String
        Return Hex(HexCode)
    End Function

    'Returns output Chr value
    Private Function GetChr(ByVal CharCode As Double) As String
        Dim ChrStringOut As String = ""
        If CharCode > 16777215 Then
            Dim Multiplierx1 As Double = CharCode / 16777216
            If Multiplierx1 > 255 Then
                Multiplierx1 = 255
            End If
            Dim IntegralPart As Double = Math.Truncate(Multiplierx1)
            Try
                CharCode = CharCode - (IntegralPart * 16777216)
            Catch ex As Exception
                CharCode = 0
            End Try
            ChrStringOut = (AsciiCodes(CInt(IntegralPart))).ToString
        End If
        If CharCode > 65535 Then
            Dim Multiplierx1 As Double = CharCode / 65536
            If Multiplierx1 > 255 Then
                Multiplierx1 = 255
            End If
            Dim IntegralPart As Double = Math.Truncate(Multiplierx1)
            Try
                CharCode = CharCode - (CDbl(IntegralPart) * 65536)
            Catch ex As Exception
                CharCode = 0
            End Try
            ChrStringOut = ChrStringOut & (AsciiCodes(CInt(IntegralPart))).ToString
        End If
        If CharCode > 255 Then
            Dim Multiplierx1 As Double = CharCode / 256.0
            If Multiplierx1 > 255 Then
                Multiplierx1 = 255
            End If
            Dim IntegralPart As Double = Math.Truncate(Multiplierx1)
            Try
                CharCode = CharCode - (CDbl(IntegralPart) * 256)
            Catch ex As Exception
                CharCode = 0
            End Try
            ChrStringOut = ChrStringOut & (AsciiCodes(CInt(IntegralPart))).ToString
        End If
        ChrStringOut = ChrStringOut & (AsciiCodes(CInt(CharCode))).ToString
        Return ChrStringOut
    End Function

#End Region
End Class




