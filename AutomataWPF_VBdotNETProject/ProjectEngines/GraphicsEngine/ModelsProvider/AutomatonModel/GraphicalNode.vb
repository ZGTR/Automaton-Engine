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
    Public Class GraphicalNode

#Region "Properties"
        Private myStateIDIndex As Integer
        Public Property StateIDIndex() As Integer
            Get
                Return myStateIDIndex
            End Get
            Set(ByVal value As Integer)
                myStateIDIndex = value
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

        Sub New(ByVal stateIDIndex As Integer, ByVal position As Point)
            InitializeDataMember(stateIDIndex, position)
        End Sub

        Private Sub InitializeDataMember(ByVal stateIDIndex As Integer, ByVal position As Point)
            Me.myStateIDIndex = stateIDIndex
            SetGraphicShape(position)
        End Sub

        Public Sub SetGraphicShape(ByVal position As Point)
            myGraphicShape = New Path()

            ' Colors
            myGraphicShape.Fill = Brushes.DarkRed
            myGraphicShape.Stroke = Brushes.Black
            myGraphicShape.StrokeThickness = 3

            ' Geometry
            InitializeGeometry(myGraphicShape, position)
        End Sub

        Private Sub InitializeGeometry(ByRef graphicPath As Path, ByVal position As Point)
            Dim myDefinedGeometry As New EllipseGeometry()
            myDefinedGeometry.Center = position
            myDefinedGeometry.RadiusX = 35
            myDefinedGeometry.RadiusY = 35
            graphicPath.Data = myDefinedGeometry
        End Sub


#Region "MethodsToDelete"
        'Protected Sub Anim(ByVal e As System.Windows.Input.MouseEventArgs)
        '    MyBase.OnMouseMove(e)

        '    Dim TranslateTransform As New TranslateTransform()
        '    TranslateTransform.X = e.GetPosition(CType(Me, IInputElement)).X
        '    TranslateTransform.Y = e.GetPosition(CType(Me, IInputElement)).Y
        '    Me.RenderTransform = TranslateTransform

        '    'MessageBox.Show("s")
        '    'End If
        '    'End If
        'End Sub
#End Region

    End Class
End Namespace