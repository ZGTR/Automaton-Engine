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
Imports AutomataWPF_VBdotNETProject.ProjectEngines.ImporterEngine.FileManipulator
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.RegularExpressions
Imports AutomataWPF_VBdotNETProject.ProjectEngines.GraphicsEngine.CanvasManipulator.MouseManipulator
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata.DFAAutomata
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.BasicAutomata
Imports _3DTools

Class MainWindow

    Public myMouseCanvasManipulator As MouseCanvasManipulator
    Public currentManipulatedAutomata As BaseAutomaton
    Public currentManipulatedRegularExpression As RegularExpression
    Public fileManipulator As FileManipulator
    Public RegStringAlphabetWindow As REWindow

#Region "Properties"




#End Region
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.WindowStartupLocation = Windows.WindowStartupLocation.CenterScreen
        currentManipulatedAutomata = New BaseAutomaton()
        myMouseCanvasManipulator = New MouseCanvasManipulator(mainCanvas, currentManipulatedAutomata)
    End Sub


    Private Sub ClearCanvasFromComponents()
        Dim list As New List(Of UIElement)
        For Each item In Me.mainCanvas.Children
            list.Add(item)
        Next
        For Each item As UIElement In list
            Me.mainCanvas.Children.Remove(item)
        Next item
    End Sub

#Region "ManipulateCanvasMouseEvents"
    Private Sub mainCanvas_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        myMouseCanvasManipulator.myMainCanvas_MouseLeftButtonDown(sender, e)
    End Sub

    Private Sub mainCanvas_MouseLeftButtonUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        myMouseCanvasManipulator.myMainCanvas_MouseLeftButtonUp(sender, e)
    End Sub

    Private Sub mainCanvas_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        myMouseCanvasManipulator.myMainCanvas_MouseMove(sender, e)
    End Sub

    Private Sub mainCanvas_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        myMouseCanvasManipulator.myMainCanvas_MouseRightButtonDown(sender, e)
    End Sub
#End Region

#Region "Manipulate Buttons Clicks Events"

    Private Sub RadButtonDFA_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RadButtonDFA.Click
        fileManipulator = New FileManipulator()
        Dim DFA As DFAAutomaton = fileManipulator.ConvertFileTo_DFAAutomaton()
        currentManipulatedAutomata = DFA
    End Sub

    Private Sub RadButtonNFA_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RadButtonNFA.Click
        fileManipulator = New FileManipulator()
        Dim NFA As NFAAutomaton = fileManipulator.ConvertFileTo_NFAAutomaton()
        currentManipulatedAutomata = NFA
    End Sub

    Private Sub RadButtoneNFA_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RadButtoneNFA.Click
        fileManipulator = New FileManipulator()
        Dim eNFA As eNFAAutomaton = fileManipulator.ConvertFileTo_eNFAAutomaton()
        currentManipulatedAutomata = eNFA
    End Sub

    Private Sub RadButtoneRegularExpression_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RadButtoneRegularExpression.Click
        RegStringAlphabetWindow = New REWindow()
        RegStringAlphabetWindow.Show()
    End Sub

    Private Sub RadButtonDrawAutomaton_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        ClearCanvasFromComponents()
        currentManipulatedAutomata.DrawAutomatonGraphicalModelContainer(Me.mainCanvas)
        myMouseCanvasManipulator = New MouseCanvasManipulator(mainCanvas, currentManipulatedAutomata)
    End Sub

    Private Sub RadButtonCheckChain_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If (currentManipulatedAutomata.CheckChainBelongToLanguage(TextBoxChainInput.Text.ToString(), mainCanvas, Me)) Then
            Dim dialog As New ChainResult()
            dialog.SetMessage("The chain is contained in the Automata")
            dialog.Show()
        Else
            Dim dialog As New ChainResult()
            dialog.SetMessage("The chain is not contained in the Automata")
            dialog.Show()
        End If
    End Sub

    Private Sub RadButtonIllustrate_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub

    Private Sub RadButtonConvertToeNFA_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If (Not (RegStringAlphabetWindow Is Nothing)) Then
            currentManipulatedRegularExpression = RegStringAlphabetWindow.GetRE()
            currentManipulatedAutomata = currentManipulatedRegularExpression.ConvertTo_eNFA()
            TextBoxCurrentRE.Text = currentManipulatedRegularExpression.RegularExpressionString
        Else
            currentManipulatedAutomata = currentManipulatedRegularExpression.ConvertTo_eNFA()
            TextBoxCurrentRE.Text = currentManipulatedRegularExpression.RegularExpressionString
        End If
    End Sub

    Private Sub RadButtonConvertToNFA_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        currentManipulatedAutomata = currentManipulatedAutomata.ConvertTo_NFAAutomaton()
    End Sub

    Private Sub RadButtonConvertToDFA_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        currentManipulatedAutomata = currentManipulatedAutomata.ConvertTo_DFAAutomaton()
    End Sub

    Private Sub RadButtonConvertToMinDFA_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim DummyAutomata As DFAAutomaton = currentManipulatedAutomata.ConvertToMinDFA(currentManipulatedAutomata)
        If (DummyAutomata Is Nothing) Then
            MsgBox("The current DFA Automata is the minimized one", MsgBoxStyle.Information, "Minimized Automata")
        Else
            currentManipulatedAutomata = DummyAutomata
        End If
    End Sub

    Private Sub RadButtonConvertToRE_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        currentManipulatedRegularExpression = currentManipulatedAutomata.ConvertTo_RE()
        MsgBox("The Regular Expression is:" + vbNewLine + currentManipulatedRegularExpression.RegularExpressionString _
               , MsgBoxStyle.Information, "Regular Expression")
    End Sub

#End Region

#Region "MethodsToDelete"
    Public Function Create1stPath(ByVal rawData As ObservableCollection(Of Point)) As Path
        Dim FinalPath As New Path()
        Dim FinalPathGeometry As New PathGeometry()
        Dim PrimaryFigure As New PathFigure()

        'if you want the path to be a shape, you want to close the PathFigure 
        '   that makes up the Path. If you want it to simply by a line, set 
        '   PrimaryFigure.IsClosed = false 
        PrimaryFigure.IsClosed = True
        PrimaryFigure.StartPoint = CType(rawData(0), Point)

        Dim LineSegmentCollection As New PathSegmentCollection()
        For i = 0 To rawData.Count - 1 Step +1
            Dim newSegment As New LineSegment()
            newSegment.Point = CType(rawData([i]), Point)
            LineSegmentCollection.Add(newSegment)
        Next i

        PrimaryFigure.Segments = LineSegmentCollection
        FinalPathGeometry.Figures.Add(PrimaryFigure)
        FinalPath.Data = FinalPathGeometry

        Dim material As New DiffuseMaterial(New SolidColorBrush(Colors.DarkKhaki))
        'Dim triangleModel As New GeometryModel3D(FinalPathGeometry, material)
        Dim model As New ModelUIElement3D()

        'model.Model = triangleModel
        ''model.MouseDown += new MouseButtonEventHandler(model_MouseDown)
        'model.MouseLeftButtonUp += new MouseButtonEventHandler(model_MouseLeftButtonUp)
        'model.MouseLeftButtonDown += new MouseButtonEventHandler(model_MouseLeftButtonDown)
        'model.MouseMove += new MouseEventHandler(model_MouseMove)
        'this.mainViewport.Children.Add(model)

        Return FinalPath
    End Function

    Public Function Create2ndPath() As Path
        ' Create a blue and a black Brush
        Dim blueBrush As New SolidColorBrush()
        blueBrush.Color = Colors.Blue
        Dim blackBrush As New SolidColorBrush()
        blackBrush.Color = Colors.Black

        ' Create a Path with black brush and blue fill
        Dim bluePath As New Path()
        bluePath.Stroke = blackBrush
        bluePath.StrokeThickness = 3
        bluePath.Fill = blueBrush

        ' Create a line geometry
        Dim blackLineGeometry As New LineGeometry()
        blackLineGeometry.StartPoint = New Point(20, 200)
        blackLineGeometry.EndPoint = New Point(300, 200)

        ' Create an ellipse geometry
        'Dim blackEllipseGeometry As New EllipseGeometry()
        'blackEllipseGeometry.Center = New Point(80, 150)
        'blackEllipseGeometry.RadiusX = 50
        'blackEllipseGeometry.RadiusY = 50

        ' Create a rectangle geometry
        'Dim blackRectGeometry As New RectangleGeometry()
        'Dim rct As New Rect()
        'rct.X = 80
        'rct.Y = 167
        'rct.Width = 150
        'rct.Height = 30
        'blackRectGeometry.Rect = rct

        ' Add all the geometries to a GeometryGroup.
        Dim blueGeometryGroup As New GeometryGroup()
        blueGeometryGroup.Children.Add(blackLineGeometry)
        'blueGeometryGroup.Children.Add(blackEllipseGeometry)
        'blueGeometryGroup.Children.Add(blackRectGeometry)

        ' Set Path.Data
        bluePath.Data = blueGeometryGroup
        Return bluePath
    End Function

    Public Shared ReadOnly PositionMeshProperty As DependencyProperty =
             DependencyProperty.Register("PositionMeshProperty", GetType(Point3D), GetType(MeshGeometry3D))

    Public Shared ReadOnly PositionModel3DProperty As DependencyProperty =
             DependencyProperty.Register("PositionModel3DProperty", GetType(Point3D), GetType(Model3D))

    Public Shared Function CreateStaticTriangleModelGroup(ByVal p0 As Point3D, ByVal p1 As Point3D, ByVal p2 As Point3D,
    ByVal myColor As Color, ByVal opacity As Double) As ModelUIElement3D

        Dim mesh As New MeshGeometry3D()
        ' Meshed
        mesh.Positions.Add(p0)
        mesh.Positions.Add(p1)
        mesh.Positions.Add(p2)
        ' Indecies
        mesh.TriangleIndices.Add(0)
        mesh.TriangleIndices.Add(2)
        mesh.TriangleIndices.Add(1)
        mesh.SetValue(PositionMeshProperty, mesh.Positions(0))

        Dim material As New DiffuseMaterial(New SolidColorBrush(myColor))
        Dim triangleModel As New GeometryModel3D(mesh, material)
        Dim model As New ModelUIElement3D()
        model.Model = triangleModel

        Return model
    End Function


#End Region

End Class
