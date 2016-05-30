Imports AutomataWPF_VBdotNETProject.ProjectEngines.GraphicsEngine.ModelsProvider.AutomatonModel
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.BasicAutomata.BaseAutomaton
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.BasicAutomata

Namespace ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata
    Public Class eNFAAutomaton
        Inherits BaseAutomaton

        Sub New()

        End Sub

        Public Overrides Function CheckChainBelongToLanguage(ByVal inComingChain As String,
                                                             ByRef mainCanvas As Canvas,
                                                             ByRef mainWindow As MainWindow) As Boolean
            Dim IsMatch As Boolean = False
            ManipulateChain(inComingChain, 0, Me.InitialState, IsMatch)
            Return IsMatch
        End Function

        Private Sub ManipulateChain(ByVal inComingChain As String,
                                         ByVal currentManipulatedIndexInChain As Integer,
                                         ByVal currentManipulatedIndexInTransitionTable As Integer,
                                         ByRef IsMatch As Boolean)
            Dim stateEnumerable1 = From StateIndex _
                    In Me.TransitionTable(currentManipulatedIndexInTransitionTable)(inComingChain(currentManipulatedIndexInChain)) _
                    Where Me.FinalStates.Contains(StateIndex)

            Dim stateEnumerable2 = From StateIndexEp _
                    In Me.TransitionTable(currentManipulatedIndexInTransitionTable)("ep") _
                    Where Me.FinalStates.Contains(StateIndexEp)

            If (currentManipulatedIndexInChain = inComingChain.Length - 1) And _
               ((stateEnumerable1.Count() <> 0) Or _
                (stateEnumerable2.Count() <> 0)) Then
                IsMatch = True
                Exit Sub
            Else
                If (currentManipulatedIndexInChain <> inComingChain.Length - 1) Then
                    Dim listOfStatesToSearchIn As List(Of Integer) = FindEClosureOfState(currentManipulatedIndexInTransitionTable)
                    For Each stateTransition As Integer In FindListOfTransitions(FindEClosureOfState(currentManipulatedIndexInTransitionTable),
                                                                                 inComingChain(currentManipulatedIndexInChain))
                        For Each stateClosure As Integer In FindEClosureOfState(stateTransition)
                            If (Not (IsMatch)) Then
                                ManipulateChain(inComingChain, currentManipulatedIndexInChain + 1, stateClosure, IsMatch)
                            End If
                        Next
                    Next
                Else
                    IsMatch = False
                End If
            End If
        End Sub

        Public Overrides Sub DrawAutomatonGraphicalModelContainer(ByRef myCanvas As Canvas)
            myAutomatonModel = New AutomatonGraphicalModel(myCanvas, Me)
        End Sub

        Private nfaAutomatonToReturn As NFAAutomaton
        Protected Overrides Sub InitializeDataMembers()
            MyBase.InitializeDataMembers()
            myTransitionTable = New List(Of Dictionary(Of String, List(Of Integer)))

        End Sub

        Public Overrides Function ConvertTo_NFAAutomaton() As NFAAutomaton
            nfaAutomatonToReturn = New NFAAutomaton()
            InitializeNFAAutomaton()
            BuildNFAAutomatonTransitionUp()
            FindFinalStatesForNFAAutomata()
            nfaAutomatonToReturn.SetOfStates = nfaAutomatonToReturn.TransitionTable.Count
            Return (nfaAutomatonToReturn)
        End Function

        Private Sub FindFinalStatesForNFAAutomata()
            Dim FinalStateFoundInList As Boolean = False
            Dim ListToManipulate As List(Of Integer) = FindEClosureOfState(Me.InitialState)
            For Each intItem As Integer In ListToManipulate
                If (Me.FinalStates.Contains(intItem)) Then
                    FinalStateFoundInList = True
                    Exit For
                End If
            Next
            If (FinalStateFoundInList = True) Then
                ' F' = F Union q0
                nfaAutomatonToReturn.FinalStates.AddRange(Me.FinalStates)
                nfaAutomatonToReturn.FinalStates.Add(Me.InitialState)
            Else
                ' F' = F
                nfaAutomatonToReturn.FinalStates = Me.FinalStates
            End If
        End Sub

        Private Sub InitializeNFAAutomaton()
            nfaAutomatonToReturn.Alphabet = Me.Alphabet
            nfaAutomatonToReturn.Alphabet.Remove("ep")
            nfaAutomatonToReturn.InitialState = Me.InitialState
            InitializeNFATransitionTable(nfaAutomatonToReturn)
        End Sub

        Private Sub InitializeNFATransitionTable(ByRef nfaAutomaton As NFAAutomaton)
            For eNFAListScanner = 0 To Me.myTransitionTable.Count - 1
                nfaAutomaton.TransitionTable.Add(New Dictionary(Of String, List(Of Integer)))
                For Each Key As String In Me.TransitionTable(eNFAListScanner).Keys
                    If (Key <> "ep") Then
                        nfaAutomaton.TransitionTable(nfaAutomaton.TransitionTable.Count - 1).Add(Key, New List(Of Integer))
                    End If
                Next
            Next
        End Sub

        Private Sub BuildNFAAutomatonTransitionUp()
            For NFAScanner = 0 To nfaAutomatonToReturn.TransitionTable.Count - 1
                For Each Key As String In nfaAutomatonToReturn.TransitionTable(NFAScanner).Keys
                    ' e.Closure
                    Dim EClosureList As List(Of Integer) = FindEClosureOfState(NFAScanner)
                    ' Transition
                    Dim ListOfTransition As List(Of Integer) = FindListOfTransitions(EClosureList, Key)
                    ' Union Of Closures
                    Dim ListToAdd As New List(Of Integer)
                    For Each intItem As Integer In ListOfTransition
                        ListToAdd.AddRange(FindEClosureOfState(intItem))
                    Next
                    MakeListUnique(ListToAdd)
                    nfaAutomatonToReturn.TransitionTable(NFAScanner)(Key).AddRange(ListToAdd)
                Next
            Next
        End Sub

        Private Sub MakeListUnique(ByRef listToReturn As List(Of Integer))
            Dim tempList As New List(Of Integer)
            For Each intItem As Integer In listToReturn
                If (Not (tempList.Contains(intItem))) Then
                    tempList.Add(intItem)
                End If
            Next
            listToReturn = tempList
        End Sub

        Private Function FindListOfTransitions(ByVal eClosureList As List(Of Integer), ByRef Key As String) As List(Of Integer)
            Dim ListOfTransitionToReturn As New List(Of Integer)
            For Each intItem As Integer In eClosureList
                ListOfTransitionToReturn.AddRange(Me.TransitionTable(intItem)(Key))
            Next
            Return ListOfTransitionToReturn
        End Function

        Private Sub EClosureState(ByVal indexOfStateIneNFAAutomatonTransitionTable As Integer,
                                  ByRef listToReturn As List(Of Integer))
            'Free CheckingList Resources and fill it with new "e" states
            Dim CheckingList As New List(Of Integer)
            CheckingList.AddRange(Me.TransitionTable(indexOfStateIneNFAAutomatonTransitionTable)("ep"))

            'Recursion call for every state in current "e" state
            For checkingScanner = 0 To CheckingList.Count - 1
                'If (((Me.TransitionTable(CheckingList(checkingScanner)))("ep").Count <> 0)) Then
                ' AND (CheckingList(checkingScanner) <> indexOfStateIneNFAAutomatonTransitionTable)
                'If (Not (CheckingList.Contains(CheckingList(checkingScanner)))) Then
                If (Not (listToReturn.Contains(CheckingList(checkingScanner)))) Then
                    'Add New States To Closure
                    listToReturn.Add(CheckingList(checkingScanner))
                    MakeListUnique(listToReturn)
                    EClosureState(CheckingList(checkingScanner), listToReturn)
                End If
                'End If
                'End If
            Next
        End Sub

        Private Function FindEClosureOfState(ByVal NFAScanner As Integer) As List(Of Integer)
            Dim EClosureListToReturn As New List(Of Integer)
            EClosureState(NFAScanner, EClosureListToReturn)
            'Add Current State to closure
            EClosureListToReturn.Add(NFAScanner)
            Return EClosureListToReturn
        End Function

    End Class
End Namespace