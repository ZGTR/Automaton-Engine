
Imports AutomataWPF_VBdotNETProject.ProjectEngines.AutomataEngine.AutomataProvider.DerivedAutomata

Namespace ProjectEngines.AutomataEngine.AutomataProvider.RegularExpressions
    Public Class RegularExpression

#Region "Properties"
        Private myAlphabet As List(Of String)
        Public Property Alphabet() As List(Of String)
            Get
                Return myAlphabet
            End Get
            Set(ByVal value As List(Of String))
                myAlphabet = value
            End Set
        End Property


        Private myRegularExpressionString As String
        Public Property RegularExpressionString() As String
            Get
                Return myRegularExpressionString
            End Get
            Set(ByVal value As String)
                myRegularExpressionString = value
            End Set
        End Property

#End Region

        Private myExtendedAlphabet As List(Of String)
        Private eNFAAutomatonToReturn As eNFAAutomaton

        Public Sub New(ByVal alphabet As List(Of String), ByRef regularExpressionString As String)
            ' Initialzie Alphabet and Adding epsilon to the Alphabet
            Me.myAlphabet = alphabet
            Me.myAlphabet.Add("ep")
            Me.myRegularExpressionString = regularExpressionString
            Me.myExtendedAlphabet = CreateExtendedAlphabet()
        End Sub

        Private Function CreateExtendedAlphabet() As List(Of String)
            Dim myExtendedAlphabetToReturn As New List(Of String)
            'myExtendedAlphabetToReturn.Add("*")
            'myExtendedAlphabetToReturn.Add("#")

            For Each str As String In myAlphabet
                'If (str <> "ep") Then
                myExtendedAlphabetToReturn.Add("(" & str & ")")
                myExtendedAlphabetToReturn.Add("(" & str & ")*")
                myExtendedAlphabetToReturn.Add("(" & str & ")#")
                'End If
            Next
            Return myExtendedAlphabetToReturn
        End Function

        Private Function AddNewEntrance() As Integer
            eNFAAutomatonToReturn.TransitionTable.Add(New Dictionary(Of String, List(Of Integer)))
            ' Manipulate Alphabet Symbols
            For Each Str As String In myAlphabet
                eNFAAutomatonToReturn.TransitionTable(eNFAAutomatonToReturn.TransitionTable.Count - 1).Add(Str, New List(Of Integer))
            Next
            'eNFAAutomatonToReturn.TransitionTable(eNFAAutomatonToReturn.TransitionTable.Count - 1).Add("ep", New List(Of Integer))
            Return eNFAAutomatonToReturn.TransitionTable.Count - 1
        End Function

        Public Function ConvertTo_eNFA() As eNFAAutomaton
            'Try
            eNFAAutomatonToReturn = New eNFAAutomaton()
            eNFAAutomatonToReturn.Alphabet = myAlphabet
            eNFAAutomatonToReturn.TransitionTable = New List(Of Dictionary(Of String, List(Of Integer)))
            Dim dummyRegularExpressionString As String = Me.myRegularExpressionString
            Dim BraceUpNotation As String = ""
            BuildeNFAAutomatonTransitionTableUp(Me.myRegularExpressionString, 0, 0, 0, 0, "")
            eNFAAutomatonToReturn.SetOfStates = eNFAAutomatonToReturn.TransitionTable.Count
            Return (eNFAAutomatonToReturn)
            'Catch
            ' MsgBox("Regular Expression parsing error, please write an appropriate RE.", MsgBoxStyle.Critical, "Warning")
            'End Try
        End Function

        Private signStack As New Stack(Of String)

        Private Sub BuildeNFAAutomatonTransitionTableUp(ByVal currentString As String,
                                                        ByRef startStateIndexLeft As Integer,
                                                        ByRef finalStateIndexLeft As Integer,
                                                        ByRef startStateIndexRight As Integer,
                                                        ByRef finalStateIndexRight As Integer,
                                                        ByVal flag As String)
            Dim FirstLinkedStartStateIndex As Integer = 0
            Dim BraceUpNotation As String = ""
            If (MinSymbolWithBracesIsAnExtendedAlphabetSymbol(currentString)) Then
                CreateStatesIntoTransitionTable(currentString, FirstLinkedStartStateIndex)
                If (flag = "left") Then
                    startStateIndexLeft = FirstLinkedStartStateIndex
                    finalStateIndexLeft = FirstLinkedStartStateIndex + 1
                Else
                    startStateIndexRight = FirstLinkedStartStateIndex
                    finalStateIndexRight = FirstLinkedStartStateIndex + 1
                End If
            Else
                ' Remove "BIG" braces
                ManipulateBasicBraces(currentString, BraceUpNotation)

                ' Find basic operation
                Dim basicOperationIndex As Integer = FindBasicOperationIndex(currentString)

                ' Call for Left subString
                BuildeNFAAutomatonTransitionTableUp(currentString.Substring(0, basicOperationIndex),
                                                    startStateIndexLeft,
                                                    finalStateIndexLeft,
                                                    startStateIndexRight,
                                                    finalStateIndexRight,
                                                    "left")
                Dim dummystartStateIndexLeft As Integer = startStateIndexLeft
                Dim dummyfinalStateIndexLeft As Integer = finalStateIndexLeft

                ' Call for Right subString
                BuildeNFAAutomatonTransitionTableUp( _
                    currentString.Substring(basicOperationIndex + 1, currentString.Length - basicOperationIndex - 1),
                                                    startStateIndexLeft,
                                                    finalStateIndexLeft,
                                                    startStateIndexRight,
                                                    finalStateIndexRight,
                                                    "right")
                Dim dummystartStateIndexRight As Integer = startStateIndexRight
                Dim dummyfinalStateIndexRight As Integer = finalStateIndexRight

                ' Link all left states package to Right states one
                If (flag = "") Then
                    LinkLeftToRightStatesWithBasicOperation(dummystartStateIndexLeft,
                                                            dummyfinalStateIndexLeft,
                                                            dummystartStateIndexRight,
                                                            dummyfinalStateIndexRight,
                                                            currentString(basicOperationIndex),
                                                            BraceUpNotation,
                                                            flag)
                Else
                    If (flag = "left") Then
                        If (dummystartStateIndexLeft <> startStateIndexRight) Then
                            LinkLeftToRightStatesWithBasicOperation(dummystartStateIndexLeft,
                                                                    dummyfinalStateIndexLeft,
                                                                    startStateIndexRight,
                                                                    finalStateIndexRight,
                                                                    currentString(basicOperationIndex),
                                                                    BraceUpNotation,
                                                                    flag)
                            startStateIndexLeft = dummystartStateIndexLeft
                            finalStateIndexLeft = dummyfinalStateIndexLeft
                        Else
                            If (startStateIndexLeft <> startStateIndexRight) Then
                                LinkLeftToRightStatesWithBasicOperation(startStateIndexLeft,
                                                                        finalStateIndexLeft,
                                                                        startStateIndexRight,
                                                                        finalStateIndexRight,
                                                                        currentString(basicOperationIndex),
                                                                        BraceUpNotation,
                                                                        flag)

                            Else
                                LinkLeftToRightStatesWithBasicOperation(dummystartStateIndexLeft,
                                                                        dummyfinalStateIndexLeft,
                                                                        dummystartStateIndexRight,
                                                                        dummyfinalStateIndexRight,
                                                                        currentString(basicOperationIndex),
                                                                        BraceUpNotation,
                                                                        flag)
                                startStateIndexLeft = dummystartStateIndexLeft
                                finalStateIndexLeft = dummyfinalStateIndexLeft
                                startStateIndexRight = dummystartStateIndexRight
                                finalStateIndexRight = dummyfinalStateIndexRight
                            End If
                        End If
                    Else
                        If (dummystartStateIndexLeft <> dummystartStateIndexRight) Then
                            LinkLeftToRightStatesWithBasicOperation(dummystartStateIndexLeft,
                                                                    dummyfinalStateIndexLeft,
                                                                    dummystartStateIndexRight,
                                                                    dummyfinalStateIndexRight,
                                                                    currentString(basicOperationIndex),
                                                                    BraceUpNotation,
                                                                    flag)
                            startStateIndexLeft = dummystartStateIndexLeft
                            finalStateIndexLeft = dummyfinalStateIndexLeft
                            startStateIndexRight = dummystartStateIndexRight
                            finalStateIndexRight = dummyfinalStateIndexRight

                        Else
                            If (startStateIndexLeft <> startStateIndexRight) Then
                                LinkLeftToRightStatesWithBasicOperation(startStateIndexLeft,
                                                                        finalStateIndexLeft,
                                                                        startStateIndexRight,
                                                                        finalStateIndexRight,
                                                                        currentString(basicOperationIndex),
                                                                        BraceUpNotation,
                                                                        flag)
                            Else
                                LinkLeftToRightStatesWithBasicOperation(startStateIndexLeft,
                                                                        finalStateIndexLeft,
                                                                        dummystartStateIndexRight,
                                                                        dummyfinalStateIndexRight,
                                                                        currentString(basicOperationIndex),
                                                                        BraceUpNotation,
                                                                        flag)
                                startStateIndexRight = dummystartStateIndexRight
                                finalStateIndexRight = dummyfinalStateIndexRight
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function MinSymbolWithBracesIsAnExtendedAlphabetSymbol(ByVal currentString As String) As Boolean
            'currentString = currentString.Remove(0, 1)
            'currentString = currentString.Remove(currentString.Length - 1, 1)
            If (myExtendedAlphabet.Contains(currentString)) Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function FindBasicOperationIndex(ByVal str As String) As Integer
            Dim Scanner As Integer = 0
            Dim NumberOfParenthesesOpened As Integer = 0
            Dim NumberOfParenthesesClosed As Integer = 0
            While (Scanner < str.Length)
                If (str(Scanner) = "(") Then
                    NumberOfParenthesesOpened += 1
                ElseIf (str(Scanner) = ")") Then
                    NumberOfParenthesesClosed += 1
                End If
                If ((NumberOfParenthesesOpened = NumberOfParenthesesClosed)) Then
                    Exit While
                End If
                Scanner += 1
            End While
            If ((str(Scanner + 1)) <> "#" And (str(Scanner + 1) <> "*")) Then
                Return Scanner + 1
            End If
            Return Scanner + 2
        End Function

        Private Sub ManipulateBasicBraces(ByRef str As String, ByRef BraceUpNotation As String)
            If ((str(str.Length - 1) = "#") Or (str(str.Length - 1) = "*")) Then
                BraceUpNotation = str(str.Length - 1)
                str = str.Remove(str.Length - 1)
            End If
            If (str.Length >= 3) Then
                str = str.Remove(0, 1)
                str = str.Remove(str.Length - 1, 1)
            End If
        End Sub

        Private Sub CreateStatesIntoTransitionTable(ByVal symbol As String, ByRef LastLinkedStartStateIndex As Integer)
            If (symbol.Contains("#")) Then
                ManipulateAdditionSymbol(symbol, LastLinkedStartStateIndex)
            Else
                If (symbol.Contains("*")) Then
                    ManipulateStarSymbol(symbol, LastLinkedStartStateIndex)
                Else
                    ManipulateNativeSymbol(symbol, LastLinkedStartStateIndex)
                End If
            End If
        End Sub

        Private Sub ManipulateNativeSymbol(ByVal symbol As String, ByRef LastLinkedStartStateIndex As Integer)
            GetClearSymbol(symbol)
            Dim Index1stEntrance As Integer = AddNewEntrance()
            Dim Index2ndEntrance As Integer = AddNewEntrance()
            eNFAAutomatonToReturn.TransitionTable(Index1stEntrance)(symbol).Add(Index2ndEntrance)
            LastLinkedStartStateIndex = Index1stEntrance
            eNFAAutomatonToReturn.InitialState = Index1stEntrance
            eNFAAutomatonToReturn.FinalStates.Clear()
            eNFAAutomatonToReturn.FinalStates.Add(Index2ndEntrance)
        End Sub

        Private Sub ManipulateAdditionSymbol(ByVal symbol As String, ByRef LastLinkedStartStateIndex As Integer)
            GetClearSymbol(symbol)
            Dim Index1stEntrance As Integer = AddNewEntrance()
            Dim Index2ndEntrance As Integer = AddNewEntrance()
            eNFAAutomatonToReturn.TransitionTable(Index1stEntrance)(symbol).Add(Index2ndEntrance)
            eNFAAutomatonToReturn.TransitionTable(Index2ndEntrance)("ep").Add(Index1stEntrance)
            LastLinkedStartStateIndex = Index1stEntrance
            eNFAAutomatonToReturn.InitialState = Index1stEntrance
            eNFAAutomatonToReturn.FinalStates.Clear()
            eNFAAutomatonToReturn.FinalStates.Add(Index2ndEntrance)
        End Sub

        Private Sub ManipulateStarSymbol(ByVal symbol As String, ByRef LastLinkedStartStateIndex As Integer)
            GetClearSymbol(symbol)
            Dim Index1stEntrance As Integer = AddNewEntrance()
            Dim Index2ndEntrance As Integer = AddNewEntrance()
            eNFAAutomatonToReturn.TransitionTable(Index1stEntrance)(symbol).Add(Index2ndEntrance)
            eNFAAutomatonToReturn.TransitionTable(Index1stEntrance)("ep").Add(Index2ndEntrance)
            eNFAAutomatonToReturn.TransitionTable(Index2ndEntrance)("ep").Add(Index1stEntrance)
            LastLinkedStartStateIndex = Index1stEntrance
            eNFAAutomatonToReturn.InitialState = Index1stEntrance
            eNFAAutomatonToReturn.FinalStates.Clear()
            eNFAAutomatonToReturn.FinalStates.Add(Index2ndEntrance)
        End Sub

        Private Sub GetClearSymbol(ByRef symbol As String)
            symbol = symbol.Remove(0, 1)
            If ((symbol.Contains(")#") Or symbol.Contains(")*"))) Then
                symbol = symbol.Remove(symbol.Length - 2, 2)
            Else
                symbol = symbol.Remove(symbol.Length - 1, 1)
            End If
        End Sub

        Private Sub LinkLeftToRightStatesWithBasicOperation(ByRef startStateIndexOfFirstLeft As Integer,
                                                            ByRef finalStateIndexOfLastLeft As Integer,
                                                            ByRef startStateIndexOfFirstRight As Integer,
                                                            ByRef finalStateIndexOfLastRight As Integer,
                                                            ByVal basicOperation As String,
                                                            ByVal BraceUpNotation As String,
                                                            ByVal flag As String)
            If (basicOperation = ".") Then
                ManipulateConcatination(startStateIndexOfFirstLeft,
                                        finalStateIndexOfLastLeft,
                                        startStateIndexOfFirstRight,
                                        finalStateIndexOfLastRight,
                                        BraceUpNotation,
                                        flag)
            Else
                ' "+" Operation
                ManipulateAddition(startStateIndexOfFirstLeft,
                                    finalStateIndexOfLastLeft,
                                    startStateIndexOfFirstRight,
                                    finalStateIndexOfLastRight,
                                    BraceUpNotation,
                                    flag)
            End If
        End Sub

        Private Sub ManipulateConcatination(ByRef startStateIndexLeft As Integer,
                                            ByRef finalStateIndexLeft As Integer,
                                            ByRef startStateIndexRight As Integer,
                                            ByRef finalStateIndexRight As Integer,
                                            ByVal BraceUpNotation As String,
                                            ByVal flag As String)
            ' Connect the two states packages at the middle of the two connected states packages
            eNFAAutomatonToReturn.TransitionTable(finalStateIndexLeft)("ep").Add(startStateIndexRight)

            ' Manipulate Notation On Parantheses
            If (BraceUpNotation = "*") Then
                eNFAAutomatonToReturn.TransitionTable(startStateIndexLeft)("ep").Add(finalStateIndexRight)
                eNFAAutomatonToReturn.TransitionTable(finalStateIndexRight)("ep").Add(startStateIndexLeft)
            ElseIf (BraceUpNotation = "#") Then
                eNFAAutomatonToReturn.TransitionTable(finalStateIndexRight)("ep").Add(startStateIndexLeft)
            End If

            ' Change Left-Right indeces
            If (flag = "left") Then
                finalStateIndexLeft = finalStateIndexRight
            Else
                startStateIndexRight = startStateIndexLeft
            End If
            ' Set Initial and final States
            eNFAAutomatonToReturn.InitialState = startStateIndexLeft
            eNFAAutomatonToReturn.FinalStates.Clear()
            eNFAAutomatonToReturn.FinalStates.Add(finalStateIndexRight)
        End Sub

        Private Sub ManipulateAddition(ByRef startStateIndexLeft As Integer,
                                       ByRef finalStateIndexLeft As Integer,
                                       ByRef startStateIndexRight As Integer,
                                       ByRef finalStateIndexRight As Integer,
                                       ByVal BraceUpNotation As String,
                                       ByVal flag As String)
            ' Connect the two states packages By a New State at the beginning of the two connected states packages
            Dim NewPreStateIndex As Integer = AddNewEntrance()
            eNFAAutomatonToReturn.TransitionTable(NewPreStateIndex)("ep").Add(startStateIndexLeft)
            eNFAAutomatonToReturn.TransitionTable(NewPreStateIndex)("ep").Add(startStateIndexRight)

            ' Create New State at the end of the two connected states packages
            Dim NewPostStateIndex As Integer = AddNewEntrance()
            eNFAAutomatonToReturn.TransitionTable(finalStateIndexLeft)("ep").Add(NewPostStateIndex)
            eNFAAutomatonToReturn.TransitionTable(finalStateIndexRight)("ep").Add(NewPostStateIndex)

            ' Manipulate Notation On Parantheses
            If (BraceUpNotation = "*") Then
                eNFAAutomatonToReturn.TransitionTable(NewPreStateIndex)("ep").Add(NewPostStateIndex)
                eNFAAutomatonToReturn.TransitionTable(NewPostStateIndex)("ep").Add(NewPreStateIndex)
            ElseIf (BraceUpNotation = "#") Then
                eNFAAutomatonToReturn.TransitionTable(NewPostStateIndex)("ep").Add(NewPreStateIndex)
            End If

            ' Change Left-Right indeces
            If (flag = "left") Then
                startStateIndexLeft = NewPreStateIndex
                finalStateIndexLeft = NewPostStateIndex
            Else
                startStateIndexRight = NewPreStateIndex
                finalStateIndexRight = NewPostStateIndex
            End If
            ' Set Initial and final States
            eNFAAutomatonToReturn.InitialState = NewPreStateIndex
            eNFAAutomatonToReturn.FinalStates.Clear()
            eNFAAutomatonToReturn.FinalStates.Add(NewPostStateIndex)
        End Sub
    End Class
End Namespace