Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks

Public Class MonitorTcpClient

    Private tcpClient As TcpClient
    Private adress As String = "127.0.0.1"
    Private port As Integer = 56685
    Private stream As NetworkStream
    Private currentAngle = 0
    Private isConnected As Boolean


    Public Sub Connect()
        Task.Run(Sub() StartClient())
    End Sub

    Public Sub ApplyAngle(angle As Double)

        currentAngle = angle

        If currentAngle > 360 Then
            currentAngle = currentAngle Mod 360
        End If

    End Sub

    Private Async Sub StartClient()
        Try
            tcpClient = New TcpClient(adress, port)
            stream = tcpClient.GetStream()
            isConnected = True

            Await Task.Delay(1000)

            Dim headtrackData(10) As Byte

            headtrackData(2) = Convert.ToByte(1)

            stream.Write(headtrackData, 0, headtrackData.Length)

            Await Task.Delay(1000)
            headtrackData(2) = Convert.ToByte(2)

            stream.Write(headtrackData, 0, headtrackData.Length)

            Await Task.Delay(200)

            While isConnected
                Try
                    Dim angleData(10) As Byte

                    angleData(2) = Convert.ToByte(3)

                    If currentAngle > 250 Then
                        angleData(4) = Convert.ToByte(1)
                        angleData(5) = Convert.ToByte(currentAngle - 250)
                    Else
                        angleData(4) = Convert.ToByte(0)
                        angleData(5) = Convert.ToByte(currentAngle)
                    End If
                    stream.Write(angleData, 0, angleData.Length)
                Catch es As Exception

                End Try
                Await Task.Delay(100)
            End While
        Catch es As Exception
            'isConnected = False
            'Connect()
        End Try

    End Sub

    Public Sub Disconnect()
        Task.Run(Sub() DisconnectClient())
    End Sub

    Private Async Sub DisconnectClient()
        isConnected = False
        If tcpClient.Connected Then
            Dim headtrackData(10) As Byte
            headtrackData(2) = Convert.ToByte(10)
            Await Task.Delay(200)

            stream.Write(headtrackData, 0, headtrackData.Length)
            stream.Close()
            tcpClient.Close()
        End If
    End Sub

End Class


