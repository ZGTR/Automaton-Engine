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

Namespace HelperModules
    Module HelperModule

        Public Function GetSummedPoint(ByVal p1 As Point3D, ByVal p2 As Point3D) As Point3D
            Dim myPoint As New Point3D
            ' Manipulate Coordinates
            myPoint.X = p1.X + p2.X
            myPoint.Y = p1.Y + p2.Y
            myPoint.Z = p1.Z + p2.Z
            Return myPoint
        End Function


#Region "MethodsToDelete"

#End Region
    End Module
End Namespace