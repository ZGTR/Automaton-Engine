Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.RegularExpressions

Public Class REWindow
    Private manipulatedRE As RegularExpression
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.WindowStartupLocation = Windows.WindowStartupLocation.CenterScreen
    End Sub

    Public Function GetAlphabetString() As String
        Return TextboxAlphabet.Text.ToString()
    End Function

    Public Function GetRegularExpressionString() As String
        Return TextboxRE.Text.ToString()
    End Function

    Public Function GetRE() As RegularExpression
        Return manipulatedRE
    End Function

    Private Sub RadButton1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RadButton1.Click
        manipulatedRE = New RegularExpression(GetAlphabetString().Split(",").ToList(), Me.GetRegularExpressionString())
        Me.Close()
    End Sub
End Class
