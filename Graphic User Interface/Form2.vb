Public Class Form2
    Private Sub TableBindingNavigatorSaveItem_Click(sender As Object, e As EventArgs) Handles TableBindingNavigatorSaveItem.Click
        Me.Validate()
        Me.TableBindingSource.EndEdit()
        Me.TableAdapterManager.UpdateAll(Me.PowerDataSet)

    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Location = New Point(200, 200)
        'TODO: This line of code loads data into the 'PowerDataSet.Table' table. You can move, or remove it, as needed.
        Me.TableTableAdapter.Fill(Me.PowerDataSet.Table)
        TableDataGridView.AutoResizeColumn(2)

    End Sub


    Private Sub TableBindingNavigator_RefreshItems(sender As Object, e As EventArgs) Handles TableBindingNavigator.RefreshItems

    End Sub
End Class