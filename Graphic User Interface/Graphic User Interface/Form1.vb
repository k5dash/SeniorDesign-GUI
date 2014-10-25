Imports System.Data.SqlClient

Public Class Form1
    Dim COMname As String
    Dim com1 As IO.Ports.SerialPort
    Dim Currentdata As String
    Dim data(20) As Integer
    Dim pre_currentdata As Double




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Timer1.Stop()
        For Each sp As String In My.Computer.Ports.SerialPortNames
            COMname = sp
            ListBox1.Items.Add(sp)
        Next

        For i = 0 To 19
            data(i) = 0
            data(0) = 1000
            Chart1.Series("Series1").Points.Add(data(i))
        Next
        Me.TableBindingSource.MoveNext()


    End Sub

    Function ReceiveSerialData() As String
        Dim checkstring As String = ""
        Try
            com1.ReadTimeout = 100
            checkstring = Val(com1.ReadExisting)


        Catch ex As TimeoutException
        End Try
        If checkstring <> "" And Val(checkstring) / pre_currentdata * 10 > 5 Then
            Currentdata = checkstring
        End If
        pre_currentdata = checkstring


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
        Chart1.Series("Series1").Points.AddXY(CurrentTime.ToString, yValue)



    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Me.TableBindingSource.AddNew()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Me.Validate()
        Me.TableBindingSource.EndEdit()
    End Sub
End Class
