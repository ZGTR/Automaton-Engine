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
Imports _3DTools

Namespace ProjectEngines.GraphicsEngine.ModelsProvider.AutomatonModel
    Public Class GraphicalLine
        
#Region "Properties"
        Private myNodeGraphical1st As GraphicalNode
        Public Property NodeGraphical1st() As GraphicalNode
            Get
                Return myNodeGraphical1st
            End Get
            Set(ByVal value As GraphicalNode)
                myNodeGraphical1st = value
            End Set
        End Property

        Private myNodeGraphical2nd As GraphicalNode
        Public Property NodeGraphical2nd() As GraphicalNode
            Get
                Return myNodeGraphical2nd
            End Get
            Set(ByVal value As GraphicalNode)
                myNodeGraphical2nd = value
            End Set
        End Property
        
        Private myAlphabetSymbol As String
        Public Property AlphabetSymbol() As String
            Get
                Return myAlphabetSymbol
            End Get
            Set(ByVal value As String)
                myAlphabetSymbol = value
            End Set
        End Property

        Private myGraphicShape As Path
        Public Property GraphicShape() As Path
            Get
                Return myGraphicShape
            End Get
            Set(ByVal value As Path)
                myGraphicShape = value
            End Set
        End Property
#End Region
        Private Shared DifferentialBezierCurvePoint As New Point(0,0)
        Sub New(ByVal node1st As GraphicalNode,
                ByVal node2nd As GraphicalNode,
                ByVal alphabetSymbol As String)
            InitializeDataMember(node1st, node2nd, alphabetSymbol)
            InitializeGraphicShape()
        End Sub

        Private Sub InitializeDataMember(ByVal node1st As GraphicalNode,
                                         ByVal node2nd As GraphicalNode,
                                         ByVal alphabetSymbol As String)
            Me.NodeGraphical1st = node1st
            Me.NodeGraphical2nd = node2nd
            Me.AlphabetSymbol = alphabetSymbol
        End Sub

        Private Sub InitializeGraphicShape()
             myGraphicShape = New Path()

            ' Dimension
            'myGraphicShape.Width = 200
            'myGraphicShape.Height = 100

            ' Colors
            myGraphicShape.Stroke = Brushes.Black
            myGraphicShape.StrokeThickness = 2

            ' Geometry
            InitializeGeometry(myGraphicShape)
        End Sub

        Private Sub InitializeGeometry(ByRef graphicPath As Path)
            ' Bezier Segments
            Dim myBezier As New BezierSegment() With { _
                .Point1 = Me.NodeGraphical1st.GraphicShape.Data.Bounds.Location + New Point(35, 35),
                .Point2 = Me.NodeGraphical1st.GraphicShape.Data.Bounds.Location + New Point(60, 60) _
                + DifferentialBezierCurvePoint,
                .Point3 = Me.NodeGraphical2nd.GraphicShape.Data.Bounds.Location + New Point(35, 35)
            }

            ' Path Figure
            Dim myFigure As New PathFigure With { _
                .StartPoint = Me.NodeGraphical1st.GraphicShape.Data.Bounds.Location + New Point(35, 35)
            }
            myFigure.Segments.Add(myBezier)

            ' Geometry
            Dim myGeometry As New PathGeometry()
            myGeometry.Figures.Add(myFigure)

            graphicPath.Data = myGeometry
        End Sub

        Public Sub ReInitializeGeometry()
            Dim dummyPos1 As Point = NodeGraphical1st.GraphicShape.Data.Bounds.Location
            Dim dummyPos2 As Point = NodeGraphical2nd.GraphicShape.Data.Bounds.Location
            ' Bezier Segments
            Dim myBezier As New BezierSegment() With { _
                .Point1 = dummyPos1 + New Point(35, 35),
                .Point2 = dummyPos1 + New Point(60, 60),
                .Point3 = dummyPos2 + New Point(35, 35)
            }

            ' Path Figure
            Dim myFigure As New PathFigure With { _
                .StartPoint = dummyPos1 + New Point(35, 35)
            }
            myFigure.Segments.Add(myBezier)

            ' Geometry
            Dim myGeometry As New PathGeometry()
            myGeometry.Figures.Add(myFigure)

            myGraphicShape.Data = myGeometry
        End Sub

        Public Sub ReInitializeGeometryWithMouseDragging(ByVal newPosition As Point, ByVal startingPoint As Boolean)
            If startingPoint = True Then
                Dim dummyPos2 As Point = NodeGraphical2nd.GraphicShape.Data.Bounds.Location
                ' Bezier Segments
                Dim myBezier As New BezierSegment() With { _
                    .Point1 = newPosition - New Point(10, 10),
                    .Point2 = newPosition - New Point(10, 10),
                    .Point3 = dummyPos2 + New Point(35, 35)
                }

                ' Path Figure
                Dim myFigure As New PathFigure With { _
                    .StartPoint = newPosition - New Point(10, 10)
                }
                myFigure.Segments.Add(myBezier)

                ' Geometry
                Dim myGeometry As New PathGeometry()
                myGeometry.Figures.Add(myFigure)

                myGraphicShape.Data = myGeometry
            Else
                If (NodeGraphical1st.StateIDIndex <> NodeGraphical2nd.StateIDIndex) Then
                    Dim dummyPos1 As Point = NodeGraphical1st.GraphicShape.Data.Bounds.Location
                    ' Bezier Segments
                    Dim myBezier As New BezierSegment() With { _
                        .Point1 = dummyPos1 + New Point(35, 35),
                        .Point2 = dummyPos1 + New Point(60, 60),
                        .Point3 = newPosition - New Point(10, 10)
                    }

                    ' Path Figure
                    Dim myFigure As New PathFigure With { _
                        .StartPoint = dummyPos1 + New Point(35, 35)
                    }
                    myFigure.Segments.Add(myBezier)

                    ' Geometry
                    Dim myGeometry As New PathGeometry()
                    myGeometry.Figures.Add(myFigure)

                    myGraphicShape.Data = myGeometry
                Else
                    Dim dummyPos1 As Point = NodeGraphical1st.GraphicShape.Data.Bounds.Location
                    ' Bezier Segments
                    Dim myBezier As New BezierSegment() With { _
                        .Point1 = newPosition - New Point(10, 10),
                        .Point2 = newPosition - New Point(10, 10),
                        .Point3 = newPosition - New Point(10, 10)
                    }

                    ' Path Figure
                    Dim myFigure As New PathFigure With { _
                        .StartPoint = newPosition - New Point(10, 10)
                    }
                    myFigure.Segments.Add(myBezier)

                    ' Geometry
                    Dim myGeometry As New PathGeometry()
                    myGeometry.Figures.Add(myFigure)

                    myGraphicShape.Data = myGeometry
                End If
            End If

        End Sub
    End Class
End Namespace