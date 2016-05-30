Imports System.IO
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Exception
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.Animation
Imports System.Collections.ObjectModel
Imports AutomataWPF_VBdotNETProject.ProjectEngines.GraphicsEngine.ModelsProvider.AutomatonModel
Imports AutomataWPF_VBdotNETProject.ProjectEngines.ImporterEngine.FileManipulator.HelperClasses
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata.DFAAutomata
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata
Imports _3DTools
Namespace ProjectEngines.ImporterEngine.FileManipulator
    Public Class FileManipulator

#Region "Properties"
        Private myFileTextList As List(Of String)
        Public Property FileTextList() As List(Of String)
            Get
                Return myFileTextList
            End Get
            Set(ByVal value As List(Of String))
                Try
                    myFileTextList = value
                Catch
                    MessageBox.Show("File Not Opened correctly.")
                End Try
            End Set
        End Property
#End Region

        Private myOpenDialog As OpenDialog

        Sub New()
            InitializeDataMember()
            Try
                ReadFile()
            Catch
                MessageBox.Show("File Didn't Open.")
            End Try
        End Sub

        Private Sub InitializeDataMember()
            Me.myOpenDialog = New OpenDialog()
            Me.myFileTextList = New List(Of String)
        End Sub

        Private Function ReadFile() As List(Of String)
            Try
                If (myOpenDialog.ExecuteDialog() = True) Then
                    Dim fileStream As New FileStream(myOpenDialog.FileName, FileMode.Open, FileAccess.Read)

                    'Declaring a FileStream to open the file named file.doc with access mode of reading
                    Dim StreamReader As New StreamReader(fileStream)

                    'Get Stream to the beginning of the file
                    StreamReader.BaseStream.Seek(0, SeekOrigin.Begin)

                    While (StreamReader.Peek() > -1)
                        myFileTextList.Add(StreamReader.ReadLine())
                    End While

                    StreamReader.Close()
                    Return myFileTextList
                End If
            Catch e As FileFormatException
                MessageBox.Show("File can't be opened")
            End Try
            Return myFileTextList
        End Function

        Public Function ConvertFileTo_DFAAutomaton() As DFAAutomaton
            Try
                Dim automaton As New DFAAutomaton
                automaton.SetOfStates = CType(myFileTextList(0).ToString(), Integer)
                automaton.Alphabet = GetAlphabet(myFileTextList(1))
                automaton.InitialState = CType(myFileTextList(2).ToString(), Integer)
                automaton.FinalStates = GetFinalStates(myFileTextList(3))
                automaton.TransitionTable = GeteNFATransitionTable(myFileTextList)
                Return automaton
            Catch
                MsgBox("File encryption is not valid.", MsgBoxStyle.Critical, "Error")
            End Try
        End Function

        Public Function ConvertFileTo_NFAAutomaton() As NFAAutomaton
            Try
                Dim automaton As New NFAAutomaton
                automaton.SetOfStates = CType(myFileTextList(0).ToString(), Integer)
                automaton.Alphabet = GetAlphabet(myFileTextList(1))
                automaton.InitialState = CType(myFileTextList(2).ToString(), Integer)
                automaton.FinalStates = GetFinalStates(myFileTextList(3))
                automaton.TransitionTable = GeteNFATransitionTable(myFileTextList)
                Return automaton
            Catch
                MsgBox("File encryption is not valid.", MsgBoxStyle.Critical, "Error")
            End Try
        End Function

        Public Function ConvertFileTo_eNFAAutomaton() As eNFAAutomaton
            Try
                Dim automaton As New eNFAAutomaton
                automaton.SetOfStates = CType(myFileTextList(0).ToString(), Integer)
                automaton.Alphabet = GetAlphabet(myFileTextList(1))
                automaton.InitialState = CType(myFileTextList(2).ToString(), Integer)
                automaton.FinalStates = GetFinalStates(myFileTextList(3))
                automaton.TransitionTable = GeteNFATransitionTable(myFileTextList)
                Return automaton
            Catch
                MsgBox("File encryption is not valid.", MsgBoxStyle.Critical, "Error")
            End Try
        End Function

    End Class
End Namespace