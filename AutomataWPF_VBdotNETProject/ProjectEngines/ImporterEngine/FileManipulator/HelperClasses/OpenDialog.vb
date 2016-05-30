Namespace ProjectEngines.ImporterEngine.FileManipulator.HelperClasses
    Public Class OpenDialog

        Private dialog As Microsoft.Win32.OpenFileDialog
        Private myFileName As String
        Public Property FileName() As String
            Get
                Return myFileName
            End Get
            Set(ByVal value As String)
                myFileName = value
            End Set
        End Property

        Sub New()
            ' Create OpenFileDialog
            dialog = New Microsoft.Win32.OpenFileDialog()

            ' Set Dialog tilte
            dialog.Title = "Open Automaton File"

            ' Set filter for file extension and default file extension ".txt"
            dialog.DefaultExt = ".txt"
            dialog.Filter = "Text documents (.txt)|*.txt"
        End Sub

        Public Function ExecuteDialog() As Boolean

            ' Display OpenFileDialog
            Dim result As Nullable(Of Boolean)
            result = dialog.ShowDialog()

            ' Get the selected file name and display in a TextBox
            If (result = True) Then
                myFileName = dialog.FileName
                Return True
            End If
            Return False
        End Function

    End Class
End Namespace