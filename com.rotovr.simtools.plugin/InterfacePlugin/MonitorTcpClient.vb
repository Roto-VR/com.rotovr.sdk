Imports System.Net.Sockets
Imports System.Threading.Tasks

Public Class MonitorTcpClient

    Private tcpClient As TcpClient
    Private adress As String = "127.0.0.1"
    Private port As Integer = 56685
    Private stream As NetworkStream
    Private currentAngle = 0
    Private isConnected As Boolean
    Private xOffset As Decimal
    Private yOffset As Decimal

    Public Sub SetOffset(x As Decimal, y As Decimal)
        xOffset = x
        yOffset = y
    End Sub

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

            Dim headtrackData(20) As Byte

            Await Task.Delay(500)
            'Send command to connect to a chair
            headtrackData(2) = Convert.ToByte(1)
            stream.Write(headtrackData, 0, headtrackData.Length)

            Await Task.Delay(1000)
            'Send command to set headtracking mode
            headtrackData(2) = Convert.ToByte(2)
            stream.Write(headtrackData, 0, headtrackData.Length)

            Await Task.Delay(500)
            'Send player offset
            headtrackData(2) = Convert.ToByte(4)
            'Set X offset
            If xOffset < 0 Then
                headtrackData(11) = Convert.ToByte(1)
                xOffset *= -1
            Else
                headtrackData(11) = Convert.ToByte(0)
            End If

            Dim xWholePart As Integer = Int(Math.Truncate(xOffset))
            headtrackData(12) = Convert.ToByte(xWholePart)

            Dim xFractionalPart As Integer = (xOffset - Int(xOffset)) * 100
            headtrackData(13) = Convert.ToByte(xFractionalPart)
            'Set Y offset

            If yOffset < 0 Then
                headtrackData(14) = Convert.ToByte(1)
                yOffset *= -1
            Else
                headtrackData(14) = Convert.ToByte(0)
            End If

            Dim yWholePart As Integer = Int(Math.Truncate(yOffset))
            headtrackData(15) = Convert.ToByte(yWholePart)

            Dim yFractionalPart As Integer = (yOffset - Int(yOffset)) * 100
            headtrackData(16) = Convert.ToByte(yFractionalPart)

            stream.Write(headtrackData, 0, headtrackData.Length)

            Await Task.Delay(200)
            'Send angle
            While isConnected
                Try
                    Dim angleData(20) As Byte

                    angleData(2) = Convert.ToByte(3)

                    If currentAngle > 256 Then
                        angleData(4) = Convert.ToByte(1)
                        angleData(5) = Convert.ToByte(currentAngle - 256)
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
            ' Reconnect()
        End Try

    End Sub

    Public Sub Disconnect()
        Task.Run(Sub() DisconnectClient())
    End Sub

    Private Async Sub DisconnectClient()
        isConnected = False
        Try
            If tcpClient.Connected Then
                Dim headtrackData(10) As Byte
                headtrackData(2) = Convert.ToByte(10)
                Await Task.Delay(200)

                stream.Write(headtrackData, 0, headtrackData.Length)
                stream.Close()
                tcpClient.Close()
            End If
        Catch es As Exception
        End Try
    End Sub

    Private Sub Reconnect()
        Task.Run(Sub() WaitAndReconnect())
    End Sub
    Private Async Sub WaitAndReconnect()
        isConnected = False

        If stream IsNot Nothing Then
            stream.Close()
        End If

        If tcpClient IsNot Nothing Then
            tcpClient.Close()
        End If

        Await Task.Delay(3000)
        Connect()
    End Sub



End Class


