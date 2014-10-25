Imports System.Data.SqlClient

Public Class Form2
    Dim COMname As String
    Dim com1 As IO.Ports.SerialPort
    Dim Currentdata As String
    Dim data(20) As Integer




    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Timer1.Stop()

        For Each sp As String In My.Computer.Ports.SerialPortNames
            COMname = sp
            ListBox1.Items.Add(sp)
        Next

        For i = 0 To 19
            data(i) = 0
            data(0) = 10
            Chart1.Series("Series1").Points.Add(data(i))
        Next


    End Sub

    Function ReceiveSerialData() As String
        Dim checkstring As String = ""
        Try
            com1.ReadTimeout = 100
            checkstring = com1.ReadChar()
        Catch ex As TimeoutException
        End Try
        If checkstring <> "" Then
            Currentdata = checkstring
        End If
        Return Currentdata
    End Function



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Start" Then
            Timer1.Start()
            com1 = My.Computer.Ports.OpenSerialPort("COM3", 9600)
            com1.DtrEnable = True
            Button1.Text = "Stop"
        Else
            Timer1.Stop()
            com1.Close()
            Button1.Text = "Start"
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim CurrentTime As Date
        CurrentTime = Now
        Dim yValue As Integer = ReceiveSerialData()
        ListBox2.Items.Add(yValue.ToString + "           " + CurrentTime.ToString)
        Chart1.Series("Series1").Points.RemoveAt(0)
        Chart1.Series("Series1").Points.AddXY(CurrentTime.ToString.Substring(9, 11), yValue)



    End Sub


End Class
