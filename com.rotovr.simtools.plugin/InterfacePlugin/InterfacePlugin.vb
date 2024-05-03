Option Strict On
Option Explicit On

Imports System.IO
Imports System.IO.Ports
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
    '///////////////////////////////////////////////////////////////////////////////
    '///                    Edit these 6 Sub/Functions Below                     ///
    '///////////////////////////////////////////////////////////////////////////////
    'Used when the program starts up.
    Private Sub StartUp()
        'all startup commands here

        'Load the Output Char's - used to get the CHAR's for 0 thru 255 - makes output faster
        LoadAsciiCodes()

    End Sub

    'Used when the program is shutting down / switching plugins.
    Private Sub ShutDown() Implements IPlugin_Interface_v3.ShutDown
        'all shutdown / cleanup commands here      
    End Sub

    'Initialize interface (centering routine - mainly used for optical systems)
    Private Sub Initialize()
        'If Needed, Initialize Interface here     
    End Sub

    'Used when the Game Starts  
    Private Function GameStart() As Boolean
        Try
            'Fix Output Strings - <ascii codes from number> - get ascii numbers
            StartupOutput = ReplaceWithAsciiCode(MyForm._InterfaceSettings._StartupOutput)
            InterfaceOutput = ReplaceWithAsciiCode(MyForm._InterfaceSettings._InterfaceOutput)
            ShutDownOutput = ReplaceWithAsciiCode(MyForm._InterfaceSettings._ShutdownOutput)

            'COM Port Assignments - set assignments to the com port
            Interface_ComWorker.PortName = MyForm._InterfaceSettings._ComPort
            Interface_ComWorker.BaudRate = CStr(MyForm._InterfaceSettings._BitsPerSec)
            Interface_ComWorker.DataBits = CStr(MyForm._InterfaceSettings._DataBits)
            Interface_ComWorker.Parity = MyForm._InterfaceSettings._ParityBits
            Interface_ComWorker.StopBits = MyForm._InterfaceSettings._StopBits

            'See if it opens
            If Interface_ComWorker.OpenPort = True Then
                'Startup
                bytCommand = Text.Encoding.Default.GetBytes(StartupOutput)
                If StartupOutput <> "" Then
                    Interface_ComWorker.WriteData(StartupOutput)
                End If
                Threading.Thread.Sleep(MyForm._InterfaceSettings._HWStartMS)

                'pause for external app to get data
                'Threading.Thread.Sleep(100)

                'Set Output Types Needed to speed up GameSend
                OutputBits = MyForm._InterfaceSettings._OutputBits
                OutputType = MyForm._InterfaceSettings._OutputType

                'Set Output String
                Return True
            End If
        Catch ex As Exception
        End Try

        Return False
    End Function

    'Used to send values to the Interface
    Private Sub Game_SendValues(A_Axis As IPlugin_Interface_v3.AxisPercent_Outputs, B_Axis As IPlugin_Interface_v3.AxisPercent_Outputs, C_Axis As IPlugin_Interface_v3.AxisPercent_Outputs) Implements IPlugin_Interface_v3.Game_SendValues
        Final_Output = InterfaceOutput
        If Final_Output.Contains("<Axis1a>") = True Then
            Dim FinalOut As String = GetOutPut(A_Axis._Axis1, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis1a>", FinalOut)
        End If
        If Final_Output.Contains("<Axis2a>") = True Then
            Dim FinalOut As String = GetOutPut(A_Axis._Axis2, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis2a>", FinalOut)
        End If
        If Final_Output.Contains("<Axis3a>") = True Then
            Dim FinalOut As String = GetOutPut(A_Axis._Axis3, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis3a>", FinalOut)
        End If
        If Final_Output.Contains("<Axis4a>") = True Then
            Dim FinalOut As String = GetOutPut(A_Axis._Axis4, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis4a>", FinalOut)
        End If
        If Final_Output.Contains("<Axis5a>") = True Then
            Dim FinalOut As String = GetOutPut(A_Axis._Axis5, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis5a>", FinalOut)
        End If
        If Final_Output.Contains("<Axis6a>") = True Then
            Dim FinalOut As String = GetOutPut(A_Axis._Axis6, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis6a>", FinalOut)
        End If
        If Final_Output.Contains("<Axis1b>") = True Then
            Dim FinalOut As String = GetOutPut(B_Axis._Axis1, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis1b>", FinalOut)
        End If
        If Final_Output.Contains("<Axis2b>") = True Then
            Dim FinalOut As String = GetOutPut(B_Axis._Axis2, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis2b>", FinalOut)
        End If
        If Final_Output.Contains("<Axis3b>") = True Then
            Dim FinalOut As String = GetOutPut(B_Axis._Axis3, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis3b>", FinalOut)
        End If
        If Final_Output.Contains("<Axis4b>") = True Then
            Dim FinalOut As String = GetOutPut(B_Axis._Axis4, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis4b>", FinalOut)
        End If
        If Final_Output.Contains("<Axis5b>") = True Then
            Dim FinalOut As String = GetOutPut(B_Axis._Axis5, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis5b>", FinalOut)
        End If
        If Final_Output.Contains("<Axis6b>") = True Then
            Dim FinalOut As String = GetOutPut(B_Axis._Axis6, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis6b>", FinalOut)
        End If
        If Final_Output.Contains("<Axis1c>") = True Then
            Dim FinalOut As String = GetOutPut(C_Axis._Axis1, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis1c>", FinalOut)
        End If
        If Final_Output.Contains("<Axis2c>") = True Then
            Dim FinalOut As String = GetOutPut(C_Axis._Axis2, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis2c>", FinalOut)
        End If
        If Final_Output.Contains("<Axis3c>") = True Then
            Dim FinalOut As String = GetOutPut(C_Axis._Axis3, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis3c>", FinalOut)
        End If
        If Final_Output.Contains("<Axis4c>") = True Then
            Dim FinalOut As String = GetOutPut(C_Axis._Axis4, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis4c>", FinalOut)
        End If
        If Final_Output.Contains("<Axis5c>") = True Then
            Dim FinalOut As String = GetOutPut(C_Axis._Axis5, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis5c>", FinalOut)
        End If
        If Final_Output.Contains("<Axis6c>") = True Then
            Dim FinalOut As String = GetOutPut(C_Axis._Axis6, OutputBits, OutputType)
            Final_Output = Final_Output.Replace("<Axis6c>", FinalOut)
        End If

        'Send Output
        Interface_ComWorker.WriteData(Final_Output)
    End Sub

    'Used when the Game Stops
    Private Sub GameStop()
        'Shutdown
        Threading.Thread.Sleep(MyForm._InterfaceSettings._HWStopMS)
        bytCommand = Text.Encoding.Default.GetBytes(ShutDownOutput)
        If ShutDownOutput <> "" Then
            Interface_ComWorker.WriteData(ShutDownOutput)
        End If
        Interface_ComWorker.ClosePort()
    End Sub

    '///////////////////////////////////////////////////////////////////////////////
    '///                PLACE EXTRA NEEDED CODE/FUNCTIONS HERE                   ///
    '///////////////////////////////////////////////////////////////////////////////
#Region "Extra Code Needed for this interface"
    Private Interface_ComWorker As New ComWorker
    Private OutputBits As String = ""
    Private OutputType As String = ""
    Private Final_Output As String = ""
    Private StartupOutput As String
    Private InterfaceOutput As String
    Private ShutDownOutput As String
    Private bytCommand As Byte()


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
    Private Function GetOutPut(ByVal InputPercent As Double, ByVal BitsNeeded As String, ByVal Type As String) As String
        Dim OutPut As Double = 0
        Dim FinalOutput As String = ""

        'Bits
        Select Case BitsNeeded
            Case "7"
                'Positives are one higher - then we add the bottom amount
                '64 + 63 = 127 7bit
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 64) + 63
                ElseIf InputPercent < 0 Then
                    OutPut = 63 - (InputPercent * -63)
                Else
                    'middle
                    OutPut = 63
                End If
            Case "8"
                'Positives are one higher - then we add the bottom amount
                '128 + 127 = 255 8bit
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 128) + 127
                ElseIf InputPercent < 0 Then
                    OutPut = 127 - (InputPercent * -127)
                Else
                    'middle
                    OutPut = 127
                End If
            Case "9"
                '511
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 256) + 255
                ElseIf InputPercent < 0 Then
                    OutPut = 255 - (InputPercent * -255)
                Else
                    OutPut = 255
                End If
            Case "10"
                '1023
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 512) + 511
                ElseIf InputPercent < 0 Then
                    OutPut = 511 - (InputPercent * -511)
                Else
                    OutPut = 511
                End If
            Case "11"
                '2047
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 1024) + 1023
                ElseIf InputPercent < 0 Then
                    OutPut = 1023 - (InputPercent * -1023)
                Else
                    OutPut = 1023
                End If
            Case "12"
                '4095
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 2048) + 2047
                ElseIf InputPercent < 0 Then
                    OutPut = 2047 - (InputPercent * -2047)
                Else
                    OutPut = 2047
                End If
            Case "13"
                '8191
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 4096) + 4095
                ElseIf InputPercent < 0 Then
                    OutPut = 4095 - (InputPercent * -4095)
                Else
                    OutPut = 4095
                End If
            Case "14"
                '16383
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 8192) + 8191
                ElseIf InputPercent < 0 Then
                    OutPut = 8191 - (InputPercent * -8191)
                Else
                    OutPut = 8191
                End If
            Case "15"
                '32767
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 16384) + 16383
                ElseIf InputPercent < 0 Then
                    OutPut = 16383 - (InputPercent * -16383)
                Else
                    OutPut = 16383
                End If
            Case "16"
                '65535
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 32768) + 32767
                ElseIf InputPercent < 0 Then
                    OutPut = 32767 - (InputPercent * -32767)
                Else
                    OutPut = 32767
                End If
            Case "17"
                '131071
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 65536) + 65535
                ElseIf InputPercent < 0 Then
                    OutPut = 65535 - (InputPercent * -65535)
                Else
                    OutPut = 65535
                End If
            Case "18"
                '262143
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 131072) + 131071
                ElseIf InputPercent < 0 Then
                    OutPut = 131071 - (InputPercent * -131071)
                Else
                    OutPut = 131071
                End If
            Case "19"
                '524287
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 262144) + 262143
                ElseIf InputPercent < 0 Then
                    OutPut = 262143 - (InputPercent * -262143)
                Else
                    OutPut = 262143
                End If
            Case "20"
                '1048575
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 524288) + 524287
                ElseIf InputPercent < 0 Then
                    OutPut = 524287 - (InputPercent * -524287)
                Else
                    OutPut = 524287
                End If
            Case "21"
                '2097151
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 1048576) + 1048575
                ElseIf InputPercent < 0 Then
                    OutPut = 1048575 - (InputPercent * -1048575)
                Else
                    OutPut = 1048575
                End If
            Case "22"
                '4194303
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 2097152) + 2097151
                ElseIf InputPercent < 0 Then
                    OutPut = 2097151 - (InputPercent * -2097151)
                Else
                    OutPut = 2097151
                End If
            Case "23"
                '8388607
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 4194304) + 4194303
                ElseIf InputPercent < 0 Then
                    OutPut = 4194303 - (InputPercent * -4194303)
                Else
                    OutPut = 4194303
                End If
            Case "24"
                '16777215
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 8388608) + 8388607
                ElseIf InputPercent < 0 Then
                    OutPut = 8388607 - (InputPercent * -8388607)
                Else
                    OutPut = 8388607
                End If
            Case "25"
                '33554431
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 16777216) + 16777215
                ElseIf InputPercent < 0 Then
                    OutPut = 16777215 - (InputPercent * -16777215)
                Else
                    OutPut = 16777215
                End If
            Case "26"
                '67108863
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 33554432) + 33554431
                ElseIf InputPercent < 0 Then
                    OutPut = 33554431 - (InputPercent * -33554431)
                Else
                    OutPut = 33554431
                End If
            Case "27"
                '134217727
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 67108864) + 67108863
                ElseIf InputPercent < 0 Then
                    OutPut = 67108863 - (InputPercent * -67108863)
                Else
                    OutPut = 67108863
                End If
            Case "28"
                '268435455
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 134217728) + 134217727
                ElseIf InputPercent < 0 Then
                    OutPut = 134217727 - (InputPercent * -134217727)
                Else
                    OutPut = 134217727
                End If
            Case "29"
                '536870911
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 268435456) + 268435455
                ElseIf InputPercent < 0 Then
                    OutPut = 268435455 - (InputPercent * -268435455)
                Else
                    OutPut = 268435455
                End If
            Case "30"
                '1073741823
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 536870912) + 536870911
                ElseIf InputPercent < 0 Then
                    OutPut = 536870911 - (InputPercent * -536870911)
                Else
                    OutPut = 536870911
                End If
            Case "31"
                '2147483647
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 1073741824) + 1073741823
                ElseIf InputPercent < 0 Then
                    OutPut = 1073741823 - (InputPercent * -1073741823)
                Else
                    OutPut = 1073741823
                End If
            Case "32"
                '4294967295
                If InputPercent > 0 Then
                    OutPut = (InputPercent * 2147483648) + 2147483647
                ElseIf InputPercent < 0 Then
                    OutPut = 2147483647 - (InputPercent * -2147483647)
                Else
                    OutPut = 2147483647
                End If
        End Select

        'Convert to an integer
        OutPut = Math.Round(OutPut, 0)

        'Type
        Select Case Type
            Case "Binary"
                FinalOutput = GetChr(OutPut)
                'output same number of char's always
                If CInt(BitsNeeded) > 8 Then
                    If FinalOutput.Length = 1 Then
                        FinalOutput = CStr(AsciiCodes(0)) & FinalOutput
                    End If
                End If
                If CInt(BitsNeeded) > 16 Then
                    If FinalOutput.Length = 2 Then
                        FinalOutput = CStr(AsciiCodes(0)) & FinalOutput
                    End If
                End If
                If CInt(BitsNeeded) > 24 Then
                    If FinalOutput.Length = 3 Then
                        FinalOutput = CStr(AsciiCodes(0)) & FinalOutput
                    End If
                End If
            Case "Hex"
                FinalOutput = GetHex(OutPut)
            Case "Decimal"
                FinalOutput = CStr(OutPut)
        End Select
        Return FinalOutput
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

    'Com port not found
    Private Sub CatchError(value As String) Handles MyForm._Error
        Log(IPlugin_Interface_v3.Error_Level.Warning, _InterfaceName & " Interface #" & _InterfaceNumber & value)
    End Sub
#End Region
End Class

'My COM Port object
#Region "    ComWorker"
Public Class ComWorker
    Public Event MsgRecieved(msg As String)

#Region "Manager Variables"

    'global
    Private comPort As New SerialPort()
    'Private write As Boolean = True
#End Region

#Region "Manager Properties"
    'BaudRate
    Public Property BaudRate() As String = "115200"

    'Parity
    Public Property Parity() As String = "None"

    'StopBits
    Public Property StopBits() As String = "1"

    'DataBits
    Public Property DataBits() As String = "8"

    'PortName
    Public Property PortName() As String = "4123"
#End Region

#Region "OpenPort"
    Public Function OpenPort() As Boolean
        Try
            'check if the port is open
            If comPort.IsOpen = True Then
                comPort.Close()
            End If
            'SerialPort Object Properties
            'ComPort
            comPort.BaudRate = Integer.Parse(BaudRate)
            'DataBits
            comPort.DataBits = Integer.Parse(DataBits)
            'StopBits
            comPort.StopBits = DirectCast([Enum].Parse(GetType(StopBits), StopBits), StopBits)
            'Parity
            comPort.Parity = DirectCast([Enum].Parse(GetType(Parity), Parity), Parity)
            'PortName
            comPort.PortName = PortName
            'open port             
            comPort.Open()

            'set encoding
            comPort.Encoding = Text.Encoding.Default

            'display message
            'RaiseEvent MsgRecieved("Port opened at " + DateTime.Now + "" + Environment.NewLine + "")
            'if opened return true
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "ClosePort "
    Public Sub ClosePort()
        If comPort.IsOpen Then
            'RaiseEvent MsgRecieved("Port closed at " + DateTime.Now + "" + Environment.NewLine + "")
            comPort.Close()
        End If
    End Sub
#End Region

#Region "WriteData"
    Public Function WriteData(ByVal msg As String) As Boolean
        Try
            If comPort.IsOpen = True Then
                'send message
                comPort.Write(msg)
                Return True
            End If
        Catch ex As Exception
        End Try

        'no send
        Return False

        'comPort.Encoding = Text.Encoding.Default
        'port is open?
        'If Not (comPort.IsOpen = True) Then
        '    comPort.Open()
        'End If
        'Try
        '    'send message
        '    comPort.Write(msg)
        '    Return True
        'Catch ex As Exception
        '    Return False
        'End Try
    End Function
#End Region

#Region "ReceivedData"
    Public Sub ComPort_DataReceived(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
        Try
            Dim msg As String = comPort.ReadExisting()
            'return the data
            RaiseEvent MsgRecieved(msg)
        Catch ex As Exception
        End Try
    End Sub
#End Region
End Class
#End Region



