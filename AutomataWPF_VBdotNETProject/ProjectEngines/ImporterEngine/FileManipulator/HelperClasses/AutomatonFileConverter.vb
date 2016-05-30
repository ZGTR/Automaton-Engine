Imports System.IO
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
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
Imports _3DTools

Namespace ProjectEngines.ImporterEngine.FileManipulator.HelperClasses

    Public Module AutomatonConverter

        Public Function GetAlphabet(ByVal str As String) As List(Of String)
            Dim strList As New List(Of String)
            strList = str.Split(",").ToList()
            Return strList
        End Function

        Public Function GetFinalStates(ByVal str As String) As List(Of Integer)
            Dim strList As New List(Of String)
            Dim intList As New List(Of Integer)
            strList = str.Split(",").ToList()
            intList = ConvertStringListToIntList(strList)
            Return intList
        End Function

        Private Function ConvertStringListToIntList(ByVal strList As List(Of String)) As List(Of Integer)
            Dim listToReturn As New List(Of Integer)
            For Each str As String In strList
                If (str <> "") Then
                    listToReturn.Add(CType(str, Integer))
                End If
            Next
            Return listToReturn
        End Function

        Public Function GeteNFATransitionTable(ByVal fileList As List(Of String)) _
                As List(Of Dictionary(Of String, List(Of Integer)))
            Dim ListToReturn As New List(Of Dictionary(Of String, List(Of Integer)))
            Dim ListOfKeys As List(Of String) = fileList(4).Split(vbTab).ToList()
            For i = 5 To fileList.Count - 1
                Dim ListOfStates As List(Of String) = fileList(i).Split(vbTab).ToList()
                ListToReturn.Add(New Dictionary(Of String, List(Of Integer)))
                For KeysScanner = 0 To ListOfKeys.Count - 1
                    ListToReturn(ListToReturn.Count - 1).Add(ListOfKeys(KeysScanner), New List(Of Integer))
                    ListToReturn(ListToReturn.Count - 1)(ListOfKeys(KeysScanner)).AddRange(
                        ConvertStringListToIntList(ListOfStates(KeysScanner).Split(",").ToList()))
                Next
            Next
            Return ListToReturn
        End Function

        Public Function GetDFATransitionTable(ByVal fileList As List(Of String)) _
        As List(Of Dictionary(Of String, List(Of Integer)))
            Dim ListToReturn As New List(Of Dictionary(Of String, List(Of Integer)))
            Dim ListOfKeys As List(Of String) = fileList(4).Split(vbTab).ToList()
            For i = 5 To fileList.Count - 1
                Dim ListOfStates As List(Of Integer) = ConvertStringListToIntList(fileList(i).Split(vbTab).ToList())
                ListToReturn.Add(New Dictionary(Of String, List(Of Integer)))
                For KeysScanner = 0 To ListOfKeys.Count - 1
                    ListToReturn(ListToReturn.Count - 1).Add(ListOfKeys(KeysScanner), New List(Of Integer))
                    ListToReturn(ListToReturn.Count - 1)(ListOfKeys(KeysScanner)).Add(ListOfStates(KeysScanner))
                Next
            Next
            Return ListToReturn
        End Function
        
    End Module
End Namespace