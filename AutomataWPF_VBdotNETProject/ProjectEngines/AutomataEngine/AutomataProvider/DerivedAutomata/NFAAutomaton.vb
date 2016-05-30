Imports AutomataWPF_VBdotNETProject.ProjectEngines.GraphicsEngine.ModelsProvider.AutomatonModel
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata.DFAAutomata
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.BasicAutomata

Namespace ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata


    Public Class NFAAutomaton
        Inherits BaseAutomaton

        Private dfaAutomatonToReturn As DFAAutomaton
        Private dummyDFATransitionTable As Dictionary(Of List(Of Integer), Dictionary(Of String, List(Of Integer)))
        Private listOfRemainedListsOfStatesToManipulate As List(Of List(Of Integer))
        Private listOfIDOfRemainedListsOfStatesToManipulate As List(Of Integer)

        Sub New()

        End Sub

        Public Overrides Sub DrawAutomatonGraphicalModelContainer(ByRef myCanvas As Canvas)
            myAutomatonModel = New AutomatonGraphicalModel(myCanvas, Me)
        End Sub

        Protected Overrides Sub InitializeDataMembers()
            MyBase.InitializeDataMembers()
            Me.myTransitionTable = New List(Of Dictionary(Of String, List(Of Integer)))

        End Sub

        Private Function AddNewEntranceForDummyDFATable(ByVal listOfNewStates As List(Of Integer)) As Integer
            dummyDFATransitionTable.Add(listOfNewStates, New Dictionary(Of String, List(Of Integer)))
            ' Manipulate Alphabet Symbols
            For Each AlphabetSymbol As String In Me.Alphabet
                dummyDFATransitionTable(listOfNewStates). _
                    Add(AlphabetSymbol, New List(Of Integer))
            Next
            Return dummyDFATransitionTable.Count - 1
        End Function

        Private Function AddNewEntranceInDFAToReturnTable() As Integer
            dfaAutomatonToReturn.TransitionTable.Add(New Dictionary(Of String, List(Of Integer)))
            ' Manipulate Alphabet Symbols
            For Each KeyStr As String In dfaAutomatonToReturn.Alphabet
                dfaAutomatonToReturn.TransitionTable(dfaAutomatonToReturn.TransitionTable.Count - 1).Add(KeyStr, New List(Of Integer))
            Next
            Return dfaAutomatonToReturn.TransitionTable.Count - 1
        End Function

        Private Function AddNewEntranceForListOfLists() As Integer
            listOfRemainedListsOfStatesToManipulate.Add(New List(Of Integer))
            Return listOfRemainedListsOfStatesToManipulate.Count - 1
        End Function

        Private Sub Initialize1stStateInListOfLists()
            Dim ListOfInteger As New List(Of Integer)
            ListOfInteger.Add(Me.InitialState)
            AddNewEntranceForDummyDFATable(ListOfInteger)
        End Sub

        Public Overrides Function ConvertTo_DFAAutomaton() As DFAAutomaton
            dfaAutomatonToReturn = New DFAAutomaton()
            dummyDFATransitionTable = New Dictionary(Of List(Of Integer), Dictionary(Of String, List(Of Integer)))
            listOfIDOfRemainedListsOfStatesToManipulate = New List(Of Integer)
            listOfRemainedListsOfStatesToManipulate = New List(Of List(Of Integer))
            'Create new state for the 1st state in the original NFA Automaton
            Initialize1stStateInListOfLists()
            'Build DFA Automaton
            Dim ListOfInteger As New List(Of Integer)
            ListOfInteger.Add(Me.InitialState)
            BuildDummyDFAAutomatonUp(ListOfInteger)
            dfaAutomatonToReturn.SetDummyDFATransitionTable(Me.dummyDFATransitionTable)
            dfaAutomatonToReturn.Alphabet = Me.Alphabet
            dfaAutomatonToReturn.InitialState = 0
            BuildDFATransitionTableFromDummyDFATable()
            FindDFAAutomatonToReturnFinalStates()
            dfaAutomatonToReturn.SetOfStates = dfaAutomatonToReturn.TransitionTable.Count
            Return (dfaAutomatonToReturn)
        End Function

        Private Sub FindDFAAutomatonToReturnFinalStates()
            Dim Counter = 0
            For Each dummyDFATableScanner As List(Of Integer) In dummyDFATransitionTable.Keys
                For Each intItemNFA As Integer In Me.FinalStates
                    Dim IsFound = False
                    For Each intItemDummyTable As Integer In dummyDFATableScanner
                        If (intItemDummyTable = intItemNFA) Then
                            If (Not (dfaAutomatonToReturn.FinalStates.Contains(Counter))) Then
                                dfaAutomatonToReturn.FinalStates.Add(Counter)
                            End If
                        End If
                    Next
                Next
                Counter += 1
            Next
        End Sub

        Private Function FindListOfTransitions(ByVal stateNum As Integer, ByRef Key As String) As List(Of Integer)
            Dim ListOfTransitionToReturn As New List(Of Integer)
            ListOfTransitionToReturn.AddRange(Me.TransitionTable(stateNum)(Key))
            Return ListOfTransitionToReturn
        End Function

        Private Sub BuildDummyDFAAutomatonUp(ByVal superKeyListToManipulate As List(Of Integer))
            For indexOfAlphabet As Integer = 0 To Me.Alphabet.Count - 1
                Dim ListOfNewStates As New List(Of Integer)
                For Each intItem As Integer In superKeyListToManipulate
                    ListOfNewStates.AddRange(FindListOfTransitions(intItem, Me.Alphabet(indexOfAlphabet)))
                Next
                MakeListUnique(ListOfNewStates)
                If (ListOfNewStates.Count <> 0) Then
                    dummyDFATransitionTable(FindKey(superKeyListToManipulate))(Me.Alphabet(indexOfAlphabet)).AddRange(ListOfNewStates)
                    If (Not (ListIsManipulatedBefore(ListOfNewStates))) Then
                        AddNewEntranceForDummyDFATable(ListOfNewStates)
                        BuildDummyDFAAutomatonUp(ListOfNewStates)
                    End If
                End If
            Next
        End Sub

        Private Sub BuildDFATransitionTableFromDummyDFATable()
            InitializeDFATransitionTableToReturn()
            Dim indexOfCurrentState As Integer = 0
            For Each SuperKey1stScanner As List(Of Integer) In dummyDFATransitionTable.Keys
                Dim InspectorCounter As Integer = 0
                For Each SuperKey2ndScanner As List(Of Integer) In dummyDFATransitionTable.Keys
                    'If (Not (KeysMatched(SuperKey1stScanner, SuperKey2ndScanner))) Then
                    For Each MinorSuperKey As String In dummyDFATransitionTable(SuperKey2ndScanner).Keys
                        If (KeysMatched(SuperKey1stScanner, dummyDFATransitionTable(SuperKey2ndScanner)(MinorSuperKey))) Then
                            'dfaAutomatonToReturn.TransitionTable(InspectorCounter)()
                            dfaAutomatonToReturn.TransitionTable(InspectorCounter)(MinorSuperKey).Clear()
                            dfaAutomatonToReturn.TransitionTable(InspectorCounter)(MinorSuperKey).Add(indexOfCurrentState)
                        End If
                    Next
                    'End If
                    InspectorCounter += 1
                Next
                indexOfCurrentState += 1
            Next
        End Sub

        Private Sub InitializeDFATransitionTableToReturn()
            For i = 0 To dummyDFATransitionTable.Count - 1
                AddNewEntranceInDFAToReturnTable()
            Next
        End Sub

        Private Function KeysMatched(ByVal SuperKey1stScanner As List(Of Integer),
                                     ByVal SuperKey2ndScanner As List(Of Integer)) As Boolean
            Dim IsMatch As Boolean = True
            Dim FoundInList As Boolean = False
            If SuperKey1stScanner.Count = SuperKey2ndScanner.Count Then
                For Each i1 As Integer In SuperKey1stScanner
                    FoundInList = False
                    For Each i2 As Integer In SuperKey2ndScanner
                        If (i1 = i2) Then
                            FoundInList = True
                        End If
                    Next
                    If (FoundInList = False) Then
                        IsMatch = False
                        Exit For
                    End If
                Next
            Else
                IsMatch = False
            End If
            Return IsMatch
        End Function

        Private Function FindKey(ByVal greatKeyListToManipulate As List(Of Integer)) As List(Of Integer)
            For Each dummyDFAKeyList As List(Of Integer) In dummyDFATransitionTable.Keys
                Dim Counter As Integer = 0
                For Each intItem As Integer In dummyDFAKeyList
                    If (greatKeyListToManipulate.Contains(intItem)) Then
                        Counter += 1
                    End If
                Next
                If ((dummyDFAKeyList.Count = greatKeyListToManipulate.Count) And (Counter = dummyDFAKeyList.Count)) Then
                    Return dummyDFAKeyList
                End If
            Next
        End Function

        Private Function MakeListUnique(ByRef listToReturn As List(Of Integer)) As List(Of Integer)
            Dim tempList As New List(Of Integer)
            For Each intItem As Integer In listToReturn
                If (Not (tempList.Contains(intItem))) Then
                    tempList.Add(intItem)
                End If
            Next
            listToReturn = tempList
            Return tempList
        End Function

        Private Function ListIsManipulatedBefore(ByVal listToManipulate As List(Of Integer)) As Boolean
            For Each dummyDFAKeyList As List(Of Integer) In dummyDFATransitionTable.Keys
                Dim Counter As Integer = 0
                For Each intItem As Integer In listToManipulate
                    If (dummyDFAKeyList.Contains(intItem)) Then
                        Counter += 1
                    End If
                Next
                If ((dummyDFAKeyList.Count = listToManipulate.Count) And (Counter = dummyDFAKeyList.Count)) Then
                    Return True
                End If
            Next
            Return False
        End Function

#Region "MethodsToDelete"
        'Private Sub BuildDFAAutomatonUp()
        '    'Dim ListScanner As Integer = 0
        '    'While (ListScanner < listOfRemainedListsOfStatesToManipulate.Count)
        '    '    ' Add New State To Table
        '    '    Dim CurrentManipulatedList As List(Of Integer) = listOfRemainedListsOfStatesToManipulate(ListScanner)
        '    '    dummyDFATransitionTable(AddNewEntranceForDummyDFATable()).

        '    '    For Each AlphabetSymbol As String In Me.Alphabet
        '    '        For Each intItem As Integer In CurrentManipulatedList
        '    '            FindListOfTransitions(intItem, AlphabetSymbol)
        '    '        Next
        '    '    Next

        '    '    ListScanner += 1
        '    'End While
        'End Sub

        'Private Sub Initialize1stStateInListOfLists()
        '    'Dim indexOfLastNewState = AddNewEntrance()
        '    'For Each AlphabetSymbol As String In Me.Alphabet
        '    '    If (Me.TransitionTable(0)(AlphabetSymbol).Count = 1) Then
        '    '        dfaAutomatonToReturn.TransitionTable(0)(AlphabetSymbol) = indexOfLastNewState
        '    '    Else
        '    '        AddNewEntrance()
        '    '        indexOfLastNewState += 1
        '    '        dfaAutomatonToReturn.TransitionTable(0)(AlphabetSymbol) = indexOfLastNewState
        '    '    End If
        '    'Next
        '    '/////////
        '    'AddNewEntranceForDummyDFATable()
        '    'For Each AlphabetSymbol As String In Me.Alphabet
        '    '    dummyDFATransitionTable(0)(AlphabetSymbol).AddRange(Me.TransitionTable(0)(AlphabetSymbol))
        '    'Next
        '    '////////
        '    'For Each AlphabetSymbol As String In Me.Alphabet
        '    '    listOfRemainedListsOfStatesToManipulate(AddNewEntranceForListOfLists()).AddRange(Me.TransitionTable(0)(AlphabetSymbol))
        '    'Next
        '    '////////
        '    ' Add 1st state to DummyList
        '    Dim ListOfInteger As New List(Of Integer)
        '    ListOfInteger.Add(Me.InitialState)
        '    AddNewEntranceForDummyDFATable(ListOfInteger)
        'End Sub


        'Private Function AddNewEntranceForDFAToReturn() As Integer
        '    dfaAutomatonToReturn.TransitionTable.Add(New Dictionary(Of String, Integer))
        '    ' Manipulate Alphabet Symbols
        '    For Each AlphabetSymbol As String In Me.Alphabet
        '        dfaAutomatonToReturn.TransitionTable(dfaAutomatonToReturn.TransitionTable.Count - 1). _
        '            Add(AlphabetSymbol, -1)
        '    Next
        '    Return dfaAutomatonToReturn.TransitionTable.Count - 1
        'End Function


#End Region

    End Class
End Namespace