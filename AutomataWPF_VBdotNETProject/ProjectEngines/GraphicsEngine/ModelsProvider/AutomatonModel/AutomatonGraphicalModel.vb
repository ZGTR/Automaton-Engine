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
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.BasicAutomata
Imports _3DTools

Namespace ProjectEngines.GraphicsEngine.ModelsProvider.AutomatonModel
    Public Class AutomatonGraphicalModel

#Region "Properties"
        Private myAutomaton As BaseAutomaton
        Public Property Automaton() As BaseAutomaton
            Get
                Return myAutomaton
            End Get
            Set(ByVal value As BaseAutomaton)
                myAutomaton = value
            End Set
        End Property

        Private myContainer As Canvas
        Public Property Container() As Canvas
            Get
                Return myContainer
            End Get
            Set(ByVal value As Canvas)
                myContainer = value
            End Set
        End Property

        Private myGraphicalNodesList As List(Of GraphicalNode)
        Public Property GraphicalNodesList() As List(Of GraphicalNode)
            Get
                Return myGraphicalNodesList
            End Get
            Set(ByVal value As List(Of GraphicalNode))
                myGraphicalNodesList = value
            End Set
        End Property

        Private myGraphicalLinesList As List(Of GraphicalLine)
        Public Property GraphicalLinesList() As List(Of GraphicalLine)
            Get
                Return myGraphicalLinesList
            End Get
            Set(ByVal value As List(Of GraphicalLine))
                myGraphicalLinesList = value
            End Set
        End Property


        Private myLabelsIDsList As List(Of Label)
        Public Property LabelsIDsListroperty() As List(Of Label)
            Get
                Return myLabelsIDsList
            End Get
            Set(ByVal value As List(Of Label))
                myLabelsIDsList = value
            End Set
        End Property

        Private myLabelsTransitionsList As List(Of Label)
        Public Property LabelsTransitionsList() As List(Of Label)
            Get
                Return myLabelsTransitionsList
            End Get
            Set(ByVal value As List(Of Label))
                myLabelsTransitionsList = value
            End Set
        End Property

#End Region

        Public Sub New(ByRef container As Canvas, ByVal automaton As BaseAutomaton)
            InitializeDataMemeber(container, automaton)
            BuildGraphicalAutomaton()
        End Sub

        Sub InitializeDataMemeber(ByRef container As Canvas, ByVal automaton As BaseAutomaton)
            myContainer = container
            myAutomaton = automaton
        End Sub

        Private Sub BuildGraphicalAutomaton()
            BuildAllGraphicalNodes()
            BuildAllGraphicalLines()
            BuildAllLabelsIDsList()
            BuildAllLabelsTransitionsList()
        End Sub

        Public Sub BuildAllLabelsIDsList()
            myLabelsIDsList = New List(Of Label)
            For Each currentGraphicalNode As GraphicalNode In myGraphicalNodesList
                Dim labelToAdd As New Label() With { _
                    .Content = "q" + currentGraphicalNode.StateIDIndex.ToString(),
                    .Margin = New Thickness(currentGraphicalNode.GraphicShape.Data.Bounds.Location.X + 25,
                                            currentGraphicalNode.GraphicShape.Data.Bounds.Location.Y + 10,
                                            0, 0),
                    .Foreground = Brushes.White
                }
                myLabelsIDsList.Add(labelToAdd)
            Next
            AddLabelsIDsListToContainer()
        End Sub

        Public Sub BuildAllLabelsTransitionsList()
            myLabelsTransitionsList = New List(Of Label)
            For Each currentGraphicalLine As GraphicalLine In myGraphicalLinesList
                ' Finding Bezier curve middle point
                Dim dummyFigure As PathFigureCollection = currentGraphicalLine.GraphicShape.Data.GetValue( _
                    PathGeometry.FiguresProperty)
                Dim figure As PathFigure = dummyFigure(0)
                Dim bezier As BezierSegment = figure.Segments(0)
                ' place label in Bezier curve's middel point
                Dim myThickness As New Thickness
                ManipulateMarginCoordinates(myThickness, currentGraphicalLine, bezier)
                If (Not (AnotherLabelIsAlreadyAtSamePlace(myThickness))) Then
                    Dim labelToAdd As New Label() With { _
                        .Content = currentGraphicalLine.AlphabetSymbol.ToString(),
                        .Margin = myThickness,
                        .Foreground = Brushes.White
                    }
                    myLabelsTransitionsList.Add(labelToAdd)
                Else
                    myThickness.Left += 10
                    Dim labelToAdd As New Label() With { _
                        .Content = ", " + currentGraphicalLine.AlphabetSymbol.ToString(),
                        .Margin = myThickness,
                        .Foreground = Brushes.White
                    }
                    myLabelsTransitionsList.Add(labelToAdd)
                End If
            Next
            AddLabelsTransitionsListToContainer()
        End Sub

        Private Sub ManipulateMarginCoordinates(ByRef myThickness As Thickness,
                                                ByVal currentGraphicalLine As GraphicalLine,
                                                ByVal bezier As BezierSegment)
            Dim Node1stXLocation As Double = currentGraphicalLine.NodeGraphical1st.GraphicShape.Data.Bounds.Location.X
            Dim Node1stYLocation As Double = currentGraphicalLine.NodeGraphical1st.GraphicShape.Data.Bounds.Location.Y
            Dim Node2ndXLocation As Double = currentGraphicalLine.NodeGraphical2nd.GraphicShape.Data.Bounds.Location.X
            Dim Node2ndYLocation As Double = currentGraphicalLine.NodeGraphical2nd.GraphicShape.Data.Bounds.Location.Y
            Dim BezierXAmount As Double = Math.Abs(bezier.Point3.X - bezier.Point1.X) / 3
            Dim BezierYAmount As Double = Math.Abs(bezier.Point3.Y - bezier.Point1.Y) / 3
            If ((Node1stXLocation < Node2ndXLocation) And (Node1stYLocation < Node2ndYLocation)) Then
                myThickness = New Thickness(Node1stXLocation + BezierXAmount, _
                                            Node1stYLocation + BezierYAmount, _
                                            0, 0)
            Else
                If ((Node1stXLocation < Node2ndXLocation) And (Node1stYLocation > Node2ndYLocation)) Then
                    myThickness = New Thickness(Node1stXLocation + BezierXAmount, _
                                                Node1stYLocation - BezierYAmount, _
                                                0, 0)
                Else
                    If ((Node1stXLocation > Node2ndXLocation) And (Node1stYLocation < Node2ndYLocation)) Then
                        myThickness = New Thickness(Node1stXLocation - BezierXAmount, _
                                                    Node1stYLocation + BezierYAmount, _
                                                    0, 0)
                    Else
                        If ((Node1stXLocation > Node2ndXLocation) And (Node1stYLocation > Node2ndYLocation)) Then
                            myThickness = New Thickness(Node1stXLocation - BezierXAmount, _
                                                        Node1stYLocation - BezierYAmount, _
                                                        0, 0)
                        Else
                            myThickness = New Thickness(Node1stXLocation - 20, _
                                                           Node1stYLocation - 20, _
                                                           0, 0)
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function AnotherLabelIsAlreadyAtSamePlace(ByVal myThickness As Thickness) As Boolean
            For Each Label As Label In myLabelsTransitionsList
                If Label.Margin = myThickness Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Function findUpperPoint(ByVal point1 As Point, ByVal point2 As Point, ByVal msg As String) As Point
            Dim pointToReturn As New Point()
            If (msg = "X") Then
                If (point1.X < point2.X) Then
                    pointToReturn = point1
                Else
                    pointToReturn = point2
                End If
            Else
                If (point1.Y < point2.Y) Then
                    pointToReturn = point1
                Else
                    pointToReturn = point2
                End If
            End If
            Return pointToReturn
        End Function


        Private Sub AddGraphicalNodesToContainer()
            For Each graphicalNode As GraphicalNode In myGraphicalNodesList
                myContainer.Children.Add(graphicalNode.GraphicShape)
            Next
        End Sub

        Private Sub AddGraphicalLinesToContainer()
            For Each graphicalLine As GraphicalLine In myGraphicalLinesList
                myContainer.Children.Add(graphicalLine.GraphicShape)
            Next
        End Sub

        Private Sub AddLabelsIDsListToContainer()
            For Each label As Label In myLabelsIDsList
                myContainer.Children.Add(label)
            Next
        End Sub

        Private Sub AddLabelsTransitionsListToContainer()
            For Each label As Label In myLabelsTransitionsList
                myContainer.Children.Add(label)
            Next
        End Sub

        Private Sub BuildAllGraphicalLines()
            myGraphicalLinesList = New List(Of GraphicalLine)()
            For Each myGraphicalNode In myGraphicalNodesList
                ' Link Current Graphical Node With Others
                For Each symbol As String In Me.Automaton.Alphabet
                    Dim listOfConnectedNodesIDs As List(Of Integer) = GetConnectedNodesIDsList(myGraphicalNode, symbol)
                    MakeListUnique(listOfConnectedNodesIDs)
                    For Each targettedGraphicalNodeID As Integer In listOfConnectedNodesIDs
                        'If (Not (NodesAlreadyConnected(myGraphicalNode.StateIDIndex, targettedGraphicalNodeID))) Then
                        Dim currentLine As New GraphicalLine(myGraphicalNode, _
                                                             GetGraphicalNode(targettedGraphicalNodeID),
                                                             symbol)
                        myGraphicalLinesList.Add(currentLine)
                        'End If
                    Next
                Next
            Next
            AddGraphicalLinesToContainer()
        End Sub

        Public Sub ReBuildGraphicalLines()
            RemoveCurrentLines()
            For Each currentGraphicalLine As GraphicalLine In myGraphicalLinesList
                currentGraphicalLine.ReInitializeGeometry()
            Next
            AddGraphicalLinesToContainer()
        End Sub

        Public Sub ReBuildGraphicalLinesWithMouseDragging(ByVal newPosition As Point,
                                                          ByVal matchedGraphicalNode As GraphicalNode)
            Dim isStartingPoint As Boolean = False
            For Each currentGraphicalLine As GraphicalLine In myGraphicalLinesList
                isStartingPoint = False
                If (LineIsConnected(currentGraphicalLine, matchedGraphicalNode, isStartingPoint)) Then
                    currentGraphicalLine.ReInitializeGeometryWithMouseDragging(newPosition, isStartingPoint)
                End If
            Next
        End Sub

        Private Function LineIsConnected(ByVal currentGraphicalLine As GraphicalLine,
                                         ByVal matchedGraphicalNode As GraphicalNode,
                                         ByRef isStartingPoint As Boolean) As Boolean
            Dim IsConnected As Boolean = False
            isStartingPoint = False
            If (currentGraphicalLine.NodeGraphical1st Is matchedGraphicalNode) Then
                IsConnected = True
                isStartingPoint = True
            End If
            If (currentGraphicalLine.NodeGraphical2nd Is matchedGraphicalNode) Then
                IsConnected = True
                isStartingPoint = False
            End If
            Return IsConnected
        End Function

        Private Sub RemoveCurrentLines()
            For Each CurrentGraphicalLine As GraphicalLine In myGraphicalLinesList
                If Me.myContainer.Children.Contains(CurrentGraphicalLine.GraphicShape) Then
                    Me.Container.Children.Remove(CurrentGraphicalLine.GraphicShape)
                End If
            Next
        End Sub

        Private Function GetGraphicalNode(ByVal targettedGraphicalNodeId As Integer) As GraphicalNode
            Dim graphicalNodeToReturn As GraphicalNode
            For i = 0 To myGraphicalNodesList.Count - 1
                If (myGraphicalNodesList(i).StateIDIndex = targettedGraphicalNodeId) Then
                    graphicalNodeToReturn = myGraphicalNodesList(i)
                End If
            Next
            Return graphicalNodeToReturn
        End Function

        Private Function NodesAlreadyConnected(ByVal NodeIndex1 As Integer, ByVal NodeIndex2 As Integer) As Boolean
            Dim IsAlreadyConnected As Boolean = False
            For Each lineNode As GraphicalLine In Me.myGraphicalLinesList
                If (LineLinks(lineNode, NodeIndex1, NodeIndex2)) Then
                    IsAlreadyConnected = True
                End If
            Next
            Return IsAlreadyConnected
        End Function

        Private Function LineLinks(ByVal lineNode As GraphicalLine,
                                   ByVal nodeIndex1 As Integer,
                                   ByVal nodeIndex2 As Integer) As Boolean
            Dim LineIsALink As Boolean = False
            Dim LinkTo1stNodeFullfilled As Boolean = False
            Dim LinkTo2ndNodeFullfilled As Boolean = False
            ' Manipulate 1st Link
            If ((lineNode.NodeGraphical1st.StateIDIndex = nodeIndex1) _
                Or (lineNode.NodeGraphical1st.StateIDIndex = nodeIndex2)) Then
                LinkTo1stNodeFullfilled = True
            End If
            ' Manipulate 2nd Link
            If ((lineNode.NodeGraphical2nd.StateIDIndex = nodeIndex2) _
                Or (lineNode.NodeGraphical2nd.StateIDIndex = nodeIndex2)) Then
                LinkTo2ndNodeFullfilled = True
            End If
            ' Manipulate 1st Link and 2nd Link
            If ((LinkTo1stNodeFullfilled) And (LinkTo2ndNodeFullfilled)) Then
                LineIsALink = True
            End If
            Return LineIsALink
        End Function

        Private Sub BuildAllGraphicalNodes()
            myGraphicalNodesList = New List(Of GraphicalNode)()
            Dim x As Integer = 50
            Dim y As Integer = 50
            For i = 0 To Me.Automaton.TransitionTable.Count - 1
                Dim graphicalNode As New GraphicalNode(i, New Point(x, y))
                x += 100
                ' Add new GraphicalNode to TransitionModelsList
                myGraphicalNodesList.Add(graphicalNode)
            Next
            AddGraphicalNodesToContainer()
        End Sub

        Private Function GetConnectedNodesIDsList(ByVal graphicalNode As GraphicalNode,
                                                  ByVal symbol As String) As List(Of Integer)
            Dim listToReturn As New List(Of Integer)
            listToReturn.AddRange(Me.Automaton.TransitionTable(graphicalNode.StateIDIndex)(symbol))
            Return listToReturn
        End Function

        Private Sub MakeListUnique(ByRef listToReturn As List(Of Integer))
            Dim tempList As New List(Of Integer)
            For Each intItem As Integer In listToReturn
                If (Not (tempList.Contains(intItem))) Then
                    tempList.Add(intItem)
                End If
            Next
            listToReturn = tempList
        End Sub

    End Class
End Namespace