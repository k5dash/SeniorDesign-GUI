Imports System.Data.SqlClient

Public Class Form1
    Dim COMname As String
    Dim com1 As IO.Ports.SerialPort
    Dim Currentdata As String
    Dim data(20) As Integer
    Dim pre_val As Double
    Dim ID As Integer

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Form3.Close()
    End Sub




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Stop()
        Me.Location = New Point(100, 100)

        Form2.PowerDataSet.Clear()
        Form2.TableAdapterManager.UpdateAll(Form2.PowerDataSet)
        Form2.TableTableAdapter.Fill(Form2.PowerDataSet.Table)
        Dim i As Integer
        For i = 1 To NumMeters
            ComboBox1.Items.Add(i)
        Next i
        Label8.Text = RanID

        ComboBox1.SelectedIndex = 0

        Dim row_num As Integer = Form2.PowerDataSet.Tables("Table").Rows.Count
        ID = Val(Form2.PowerDataSet.Tables("Table").Rows(row_num - 1).Item(0)) + 1



        For Each sp As String In My.Computer.Ports.SerialPortNames
            COMname = sp
            ListBox1.Items.Add(sp)
        Next

        For i = 0 To 19
            data(i) = 0
            data(0) = 1024
            Chart1.Series("Series1").Points.Add(data(i))
        Next
        Me.TableBindingSource.MoveNext()

    End Sub

    Function ReceiveSerialData() As String
        Dim checkstring As String = ""
        Try
            com1.ReadTimeout = 100
            checkstring = com1.ReadExisting
        Catch ex As TimeoutException
        End Try
        If checkstring <> "" And Val(checkstring) > 100 Then
            Currentdata = checkstring
        End If
        Return Currentdata
    End Function



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Start" Then
            Timer1.Start()
            com1 = My.Computer.Ports.OpenSerialPort(COMname, 9600)
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

        'Chart2.Series("Series1").Points.RemoveAt(0)
        Chart2.Series("Series1").Points.AddXY(CurrentTime.ToString, yValue)

        Dim dsNewRow As DataRow
        dsNewRow = Form2.PowerDataSet.Tables("Table").NewRow
        dsNewRow.Item("ID") = ID
        ID = ID + 1
        dsNewRow.Item("RanID") = RanID
        dsNewRow.Item("Meter#") = NumMeters
        dsNewRow.Item("Power") = yValue
        dsNewRow.Item("Time") = CurrentTime.ToLongTimeString
        dsNewRow.Item("Date") = CurrentTime.ToLongDateString
        Form2.PowerDataSet.Tables("Table").Rows.Add(dsNewRow)


    End Sub


    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Form2.TableAdapterManager.UpdateAll(Form2.PowerDataSet)
        If Button1.Text = "Stop" Then
            Timer1.Stop()
            com1.Close()
            Button1.Text = "Start"
        End If
        Form2.Show()



    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Form2.TableAdapterManager.UpdateAll(Form2.PowerDataSet)
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Label6.Text = ComboBox1.Text
    End Sub


End Class
