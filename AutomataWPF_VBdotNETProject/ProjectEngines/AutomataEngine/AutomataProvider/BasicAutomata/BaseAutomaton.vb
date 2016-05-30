Imports AutomataWPF_VBdotNETProject.ProjectEngines.GraphicsEngine.ModelsProvider.AutomatonModel
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.RegularExpressions
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata.DFAAutomata
Imports AutomataWPF_VBdotNETProject.ProjectEngines.GraphicsEngine.AnimationProvider

Namespace ProjectEngines.AutomataEngine.AutomataProvider.BasicAutomata

    Public Class BaseAutomaton
#Region "Properties"
        Protected mySetOfStates As Integer
        Public Property SetOfStates() As Integer
            Get
                Return mySetOfStates
            End Get
            Set(ByVal value As Integer)
                mySetOfStates = value
            End Set
        End Property
        
        Protected myAlphabet As List(Of String)
        Public Property Alphabet() As List(Of String)
            Get
                Return myAlphabet
            End Get
            Set(ByVal value As List(Of String))
                myAlphabet = value
            End Set
        End Property
        
        Protected myTransitionTable As List(Of Dictionary(Of String, List(Of Integer)))
        Public Overridable Property TransitionTable() As List(Of Dictionary(Of String, List(Of Integer)))
            Get
                Return myTransitionTable
            End Get
            Set(ByVal value As List(Of Dictionary(Of String, List(Of Integer))))
                myTransitionTable = value
            End Set
        End Property
        
        Protected myInitialState As Integer
        Public Property InitialState() As Integer
            Get
                Return myInitialState
            End Get
            Set(ByVal value As Integer)
                myInitialState = value
            End Set
        End Property

        Protected myFinalStates As List(Of Integer)
        Public Property FinalStates() As List(Of Integer)
            Get
                Return myFinalStates
            End Get
            Set(ByVal value As List(Of Integer))
                myFinalStates = value
            End Set
        End Property

        Protected myAutomatonModel As AutomatonGraphicalModel
        Public Property AutomatonModel() As AutomatonGraphicalModel
            Get
                Return myAutomatonModel
            End Get
            Set(ByVal value As AutomatonGraphicalModel)
                myAutomatonModel = value
            End Set
        End Property

#End Region

        Sub New()
            InitializeDataMembers()
        End Sub

        Protected Overridable Sub InitializeDataMembers()
            Me.mySetOfStates = 0
            Me.myAlphabet = New List(Of String)
            Me.myTransitionTable = New List(Of Dictionary(Of String, List(Of Integer)))
            Me.myInitialState = 0
            Me.myFinalStates = New List(Of Integer)
        End Sub

        Public Overridable Function ConvertTo_RE() As RegularExpression

        End Function

        Public Overridable Function ConvertTo_NFAAutomaton() As NFAAutomaton

        End Function

        Public Overridable Function ConvertTo_DFAAutomaton() As DFAAutomaton

        End Function

        Public Overridable Function ConvertToMinDFA(ByVal PassedDFA As DFAAutomaton) As DFAAutomaton

        End Function

        Public Overridable Sub DrawAutomatonGraphicalModelContainer(ByRef myCanvas As Canvas)
            myAutomatonModel = New AutomatonGraphicalModel(myCanvas, Me)
        End Sub

        Public Overridable Function CheckChainBelongToLanguage(ByVal inComingChain As String,
                                                               ByRef mainCanvas As Canvas,
                                                               ByRef mainWindow As MainWindow) As Boolean
            Dim IsMatch As Boolean = False
            Dim ChainPathList As New List(Of Integer)
            Dim Result As Boolean = ManipulateChain(inComingChain, 0, Me.InitialState, IsMatch, ChainPathList)
            Animate_GraphicalNodeMovementByPath(ChainPathList, Me, mainCanvas, mainWindow)
            Return Result
        End Function

        Private Function ManipulateChain(ByVal inComingChain As String,
                                         ByVal currentManipulatedIndexInChain As Integer,
                                         ByVal currentManipulatedIndexInTransitionTable As Integer,
                                         ByRef IsMatch As Boolean,
                                         ByVal ChainPathList As List(Of Integer)) As Boolean
            Dim stateEnumerable = From StateIndex _
                    In Me.TransitionTable(currentManipulatedIndexInTransitionTable)(inComingChain(currentManipulatedIndexInChain)) _
                    Where Me.FinalStates.Contains(StateIndex)
            ChainPathList.Add(currentManipulatedIndexInTransitionTable)
            If (currentManipulatedIndexInChain = inComingChain.Length - 1) And (stateEnumerable.Count() <> 0) Then
                IsMatch = True
            Else
                If (currentManipulatedIndexInChain <> inComingChain.Length - 1) Then
                    For Each stateIndex As Integer In _
                        Me.TransitionTable(currentManipulatedIndexInTransitionTable)(inComingChain(currentManipulatedIndexInChain))
                        ManipulateChain(inComingChain, currentManipulatedIndexInChain + 1, stateIndex, IsMatch, ChainPathList)
                    Next
                Else
                    IsMatch = False
                End If
            End If
            Return IsMatch
        End Function
    End Class
End Namespace