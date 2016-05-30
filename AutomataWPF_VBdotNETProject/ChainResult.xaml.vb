Public Class ChainResult
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.WindowStartupLocation = Windows.WindowStartupLocation.CenterScreen
    End Sub

    Sub SetMessage(ByVal StringMsg As String)
        Me.LabelResult.Content = StringMsg
    End Sub

    Private Sub RadButton1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RadButton1.Click
        Me.Close()
    End Sub
End Class
