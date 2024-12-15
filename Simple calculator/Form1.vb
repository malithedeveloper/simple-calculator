Public Class Form1
    Private errorOccured As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Button Then
                AddHandler CType(ctrl, Button).Click, AddressOf GlobalButtonClick
            End If
        Next
    End Sub

    Private Sub GlobalButtonClick(sender As Object, e As EventArgs)
        If Len(calculation_show.Text) > 15 AndAlso CType(sender, Button).Text <> "CE" AndAlso CType(sender, Button).Text <> "C" Then
            statustext.Text = "Maximum number of characters reached."
            DisableAllButtons()
            errorOccured = True
            Button1.Enabled = True
            Exit Sub
        End If
        statustext.Text = ""
        Select Case CType(sender, Button).Text
            Case "="
                Try
                    Dim expression As String = calculation_show.Text.Replace(",", ".").Replace("x", "*")
                    calculation_show.Text = New DataTable().Compute(expression, Nothing).ToString().Replace(".", ",")
                    history.Items.Add(expression.Replace("*", "x") + " = " + calculation_show.Text)
                Catch ex As Exception
                    calculation_show.Text = "ERROR " + (2147483648 + ex.HResult).ToString()
                    statustext.Text = "Click ""C"" button to continue."
                    errorOccured = True
                    DisableAllButtons()
                End Try
            Case "C"
                calculation_show.Text = ""
                statustext.Text = ""
                If errorOccured Then
                    errorOccured = False
                    EnableAllButtons()
                End If
            Case "CE"
                If String.IsNullOrEmpty(calculation_show.Text) = False Then
                    calculation_show.Text = calculation_show.Text.Substring(0, calculation_show.Text.Length - 1)
                    If errorOccured Then
                        errorOccured = False
                        EnableAllButtons()
                    End If
                End If
            Case Else
                calculation_show.Text += CType(sender, Button).Text
        End Select
    End Sub

    Private Sub DisableAllButtons()
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Button AndAlso ctrl.Name <> "panel" AndAlso ctrl.Name <> "clear" Then
                ctrl.Enabled = False
            End If
        Next
    End Sub

    Private Sub EnableAllButtons()
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Button AndAlso ctrl.Name <> "panel" AndAlso ctrl.Name <> "clear" Then
                ctrl.Enabled = True
            End If
        Next
    End Sub
    Private Sub history_DoubleClick(sender As Object, e As EventArgs) Handles history.DoubleClick
        calculation_show.Text = history.SelectedItem.ToString().Split("=")(1).Trim()
    End Sub
End Class
