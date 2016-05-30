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
Imports System.Windows.Shapes
Imports System.Windows.Media.Animation
Imports AutomataWPF_VBdotNETProject.ProjectEngines.GraphicsEngine.ModelsProvider.AutomatonModel
Imports System.Windows.Media.Media3D
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.BasicAutomata

Namespace ProjectEngines.GraphicsEngine.AnimationProvider
    Public Module AnimationProvider

        Public Sub Animate_GraphicalNodeMovement(ByRef myGraphicalNode As GraphicalNode)
            Try
                Dim myAnimationX As New DoubleAnimation With { _
                    .From = 0,
                    .To = 50,
                    .DecelerationRatio = 0.5,
                    .Duration = New Duration(TimeSpan.FromSeconds(1))
                }

                myGraphicalNode.GraphicShape.RenderTransform = New TranslateTransform()
                myGraphicalNode.GraphicShape.RenderTransform.BeginAnimation(TranslateTransform.XProperty, myAnimationX)
                myGraphicalNode.GraphicShape.RenderTransform.BeginAnimation(TranslateTransform.YProperty, myAnimationX)
            Catch
            End Try
        End Sub

        Public Sub Animate_GraphicalNodeMovementByPath(ByVal pathList As List(Of Integer),
                                                       ByVal automaton As BaseAutomaton,
                                                       ByRef mainCanvas As Canvas,
                                                       ByVal mainWindow As MainWindow)
            Try
                mainWindow.Margin = New Thickness(20)

                ' Create a NameScope for the page so that
                ' we can use Storyboards.
                NameScope.SetNameScope(mainWindow, New NameScope())

                ' Create a button.
                Dim aButton As New Button()
                aButton.Width = 50
                aButton.Height = 50
                aButton.Content = ""

                ' Create a MatrixTransform. This transform
                ' will be used to move the button.
                Dim buttonMatrixTransform As New MatrixTransform()
                aButton.RenderTransform = buttonMatrixTransform

                ' Register the transform's name with the page
                ' so that it can be targeted by a Storyboard.
                mainWindow.RegisterName("ButtonMatrixTransform", buttonMatrixTransform)

                ' Create a Canvas to contain the button
                ' and add it to the page.
                ' Although this example uses a Canvas,
                ' any type of panel will work.

                mainCanvas.Children.Add(aButton)


                ' Create the animation path.
                Dim animationPath As New PathGeometry()
                Dim pFigure As New PathFigure()
                Dim dummyGraphicalNod As GraphicalNode = (From item As GraphicalNode
                                                              In automaton.AutomatonModel.GraphicalNodesList
                                                              Where item.StateIDIndex = pathList(0)
                                                              Select item)(0)
                pFigure.StartPoint = dummyGraphicalNod.GraphicShape.Data.Bounds.Location
                Dim pBezierSegment As New PolyLineSegment
                For Each nextNodeID As Integer In pathList
                    Dim dummyGraphicalNode As GraphicalNode = (From item As GraphicalNode
                                                              In automaton.AutomatonModel.GraphicalNodesList
                                                              Where item.StateIDIndex = nextNodeID
                                                              Select item)(0)
                    pBezierSegment.Points.Add(dummyGraphicalNode.GraphicShape.Data.Bounds.Location)
                Next
                pFigure.Segments.Add(pBezierSegment)
                animationPath.Figures.Add(pFigure)

                ' Freeze the PathGeometry for performance benefits.
                animationPath.Freeze()

                ' Create a MatrixAnimationUsingPath to move the
                ' button along the path by animating
                ' its MatrixTransform.
                Dim matrixAnimation As New MatrixAnimationUsingPath()
                matrixAnimation.PathGeometry = animationPath
                matrixAnimation.Duration = TimeSpan.FromSeconds(5)
                'matrixAnimation.RepeatBehavior = RepeatBehavior.Forever

                ' Set the animation's DoesRotateWithTangent property
                ' to true so that rotates the rectangle in addition
                ' to moving it.
                matrixAnimation.DoesRotateWithTangent = True

                ' Set the animation to target the Matrix property
                ' of the MatrixTransform named "ButtonMatrixTransform".
                Storyboard.SetTargetName(matrixAnimation, "ButtonMatrixTransform")
                Storyboard.SetTargetProperty(matrixAnimation, New PropertyPath(MatrixTransform.MatrixProperty))

                ' Create a Storyboard to contain and apply the animation.
                Dim pathAnimationStoryboard As New Storyboard()
                pathAnimationStoryboard.Children.Add(matrixAnimation)

                ' Start the storyboard when the button is loaded.
                AddHandler aButton.Loaded, Sub(sender As Object, e As RoutedEventArgs) pathAnimationStoryboard.Begin(mainWindow)



         

            Catch
            End Try
        End Sub

        Public Sub Animate_GraphicalNodeScale(ByRef myGraphicalNode As GraphicalNode)
            Dim myAnimation As New DoubleAnimation With { _
                .From = 50,
                .To = 60,
                .DecelerationRatio = 0.5,
                .Duration = New Duration(TimeSpan.FromSeconds(1))
            }

            'myGraphicalNode.GraphicShape.RenderTransform = New ScaleTransform()
            'myGraphicalNode.GraphicShape.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, myAnimation)
            'myGraphicalNode.GraphicShape.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, myAnimation)
        End Sub

        Public Sub Animate_GraphicalLineStretching(ByVal CurrentLine As GraphicalLine,
                                                  ByVal isStartingPoint As Boolean)
            If (isStartingPoint = True) Then
                Dim Point1 As Point = CurrentLine.NodeGraphical1st.GraphicShape.Data.Bounds.Location
                Dim myAnimation As New PointAnimation With { _
                    .From = Point1 + New Point(35, 35),
                    .To = .From + New Point(50, 50),
                    .DecelerationRatio = 0.5,
                    .Duration = New Duration(TimeSpan.FromSeconds(1))
                }
                Dim dummyFigure As PathFigureCollection = CurrentLine.GraphicShape.Data.GetValue(PathGeometry.FiguresProperty)
                Dim figure As PathFigure = dummyFigure(0)
                Dim bezier As BezierSegment = figure.Segments(0)
                bezier.BeginAnimation(BezierSegment.Point1Property, myAnimation)
                figure.BeginAnimation(PathFigure.StartPointProperty, myAnimation)
            Else
                Dim Point1 As Point = CurrentLine.NodeGraphical2nd.GraphicShape.Data.Bounds.Location
                Dim myAnimation As New PointAnimation With { _
                    .From = Point1 + New Point(35, 35),
                    .To = .From + New Point(50, 50),
                    .DecelerationRatio = 0.5,
                    .Duration = New Duration(TimeSpan.FromSeconds(1))
                }
                Dim dummyFigure As PathFigureCollection = CurrentLine.GraphicShape.Data.GetValue(PathGeometry.FiguresProperty)
                Dim figure As PathFigure = dummyFigure(0)
                Dim bezier As BezierSegment = figure.Segments(0)
                bezier.BeginAnimation(BezierSegment.Point3Property, myAnimation)
            End If
        End Sub

    End Module
End Namespace