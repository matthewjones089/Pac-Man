Imports System.IO
Imports System.Text

Public Class MJ_MapEditor

    ' Declare constants for dimensions of the GUI, scale and FPS.
    Public Const CLIENT_WIDTH = 224
    Public Const CLIENT_HEIGHT = 288
    Public Const CLIENT_SCALE = 2
    Public Const FPS = 60

    ' Declare enumerator 'mode' to hold the possible actions in the map editor.
    Public Enum mode As Integer
        addBlock = 0
        deleteBlock = 1
        addDot = 2
        deleteDot = 3
        addEnergizer = 4
        deleteEnergizer = 5
    End Enum

    ' Declare global variables for the map editor.
    ' Create, with its events, a variable to hold a new instance of the game engine.
    Dim WithEvents mapEngine As gameEngine
    Dim map As gameEngine.map
    Dim debugMap As gameEngine.map          ' DEBUG
    Dim mapCursor As gameEngine.sprite
    Dim mapPacman As gameEngine.sprite

    Private mapMaze As New maze
    Dim blockMode As Integer = mode.addBlock

    Private Sub MJ_MapEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        initialize()

    End Sub

    Public Sub mapEngine_geGameLogic() Handles mapEngine.geGameLogic

    End Sub

    Public Sub mapEngine_geRenderScene() Handles mapEngine.geRenderScene

        If mapEngine.getMouse IsNot Nothing Then
            mapCursor.point = New Point(Int(mapEngine.getMouse.Location.X / (8 * CLIENT_SCALE)) * 8, Int(mapEngine.getMouse.Location.Y / (8 * CLIENT_SCALE)) * 8)
        End If

    End Sub

    Public Sub mapEngine_geMouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapEngine.geMouseMove
        ' Handles mouse movement.

        Dim pos As Point

        pos = New Point(Int(e.Location.X / (8 * CLIENT_SCALE)), Int(e.Location.Y / (8 * CLIENT_SCALE)))

        mapCursor.point = New Point(pos.X * 8, pos.Y * 8)

        If pos.X < 27 And pos.Y < 30 Then
            ' Depending upon the mode indicated by the blockMode variable, a different outlineing box is shown.
            Select Case blockMode
                Case mode.addBlock
                    ' If the user enables add block.
                    ' Performs checks to see if any tiles within the outlined box are fixed.
                    If mapMaze.mazeBlockFixed(pos.X, pos.Y) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y) Or mapMaze.mazeBlockFixed(pos.X, pos.Y + 1) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y + 1) Then
                        ' If so the outline box is red, to indicate a block cannot be placed there.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
                    Else
                        ' Otherwise, the box is green, to indicate a block can be placed there.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(1, 1)
                    End If
                Case mode.deleteBlock
                    ' If the user enables delete block.
                    ' Once again, performs checks to see if any tiles within the outlined box are fixed.
                    If mapMaze.mazeBlockFixed(pos.X, pos.Y) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y) Or mapMaze.mazeBlockFixed(pos.X, pos.Y + 1) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y + 1) Then
                        'If so the outlined box is red, to indicate the block cannot be deleted.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
                    Else

                        For y = 1 To 28
                            For x = 1 To 26
                                If (x = pos.X And y = pos.Y) Then

                                    If ((mapMaze.mazeBlockFixed(x - 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y) = True) And (mapMaze.mazeBlockFixed(x + 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x, y - 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 1) = True) Or
                                                (mapMaze.mazeBlockFixed(x + 2, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 2, y) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 1) = True) And (mapMaze.mazeBlockFixed(x, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 1, y + 2) = True) Or
                                                (mapMaze.mazeBlockFixed(x, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 1) = True) Or
                                                (mapMaze.mazeBlockFixed(x - 1, y) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 2) = True) And (mapMaze.mazeBlockFixed(x, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 1, y + 2) = True) Or
                                                (mapMaze.mazeBlockFixed(x - 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y - 1) = True) Or
                                                (mapMaze.mazeBlockFixed(x - 1, y + 2) = True) And (mapMaze.mazeBlockFixed(x, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 1, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 2) = True) Or
                                                (mapMaze.mazeBlockFixed(x - 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 2) = True) Or
                                                (mapMaze.mazeBlockFixed(x + 2, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 2) = True)) Then
                                        ' Otherwise, the outline box is set to green, to indicate the current block can be deleted.
                                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(1, 1)

                                    'ElseIf (mapMaze.mazePathType(x + 2, y) = maze.pathType.block And mapMaze.mazeBlockFixed(x + 2, y) = False) And mapMaze.mazePathType(x + 3, y) <> maze.pathType.block Then
                                    '    ' Otherwise, the outline box is set to green, to indicate the current block can be deleted.
                                    '    mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
                                    Else
                                        ' Otherwise, the outline box is set to green, to indicate the current block can be deleted.
                                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
                                    End If
                                    If ((x > 2 And x < 25) And (y > 2 And y < 27)) Then
                                        If (mapMaze.mazePathType(x, y - 1) = maze.pathType.block And mapMaze.mazePathType(x, y - 2) <> maze.pathType.block) Or
                                (mapMaze.mazePathType(x + 1, y - 1) = maze.pathType.block And mapMaze.mazePathType(x + 1, y - 2) <> maze.pathType.block) Or
                                (mapMaze.mazePathType(x, y + 2) = maze.pathType.block And mapMaze.mazePathType(x, y + 3) <> maze.pathType.block) Or
                                (mapMaze.mazePathType(x + 1, y + 2) = maze.pathType.block And mapMaze.mazePathType(x + 1, y + 3) <> maze.pathType.block) Or
                                (mapMaze.mazePathType(x + 2, y) = maze.pathType.block And mapMaze.mazePathType(x + 3, y) <> maze.pathType.block) Or
                                (mapMaze.mazePathType(x + 2, y + 1) = maze.pathType.block And mapMaze.mazePathType(x + 3, y + 1) <> maze.pathType.block) Or
                                (mapMaze.mazePathType(x - 1, y) = maze.pathType.block And mapMaze.mazePathType(x - 2, y) <> maze.pathType.block) Or
                                (mapMaze.mazePathType(x - 1, y + 1) = maze.pathType.block And mapMaze.mazePathType(x - 2, y + 1) <> maze.pathType.block) Then
                                            ' Otherwise, the outline box is set to green, to indicate the current block can be deleted.
                                            mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
                                        Else
                                            ' Otherwise, the outline box is set to green, to indicate the current block can be deleted.
                                            mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(1, 1)
                                        End If
                                    End If
                                End If

                            Next
                        Next
                        ' Otherwise, checks the surrounding blocks to ensure no single, not connected tiles will be left after the delete.
                        'If (mapMaze.mazePathType(pos.X, pos.Y - 1) = maze.pathType.block And mapMaze.mazePathType(pos.X, pos.Y - 2) <> maze.pathType.block) Or
                        '(mapMaze.mazePathType(pos.X + 1, pos.Y - 1) = maze.pathType.block And mapMaze.mazePathType(pos.X + 1, pos.Y - 2) <> maze.pathType.block) Or
                        '(mapMaze.mazePathType(pos.X, pos.Y + 2) = maze.pathType.block And mapMaze.mazePathType(pos.X, pos.Y + 3) <> maze.pathType.block) Or
                        '(mapMaze.mazePathType(pos.X + 1, pos.Y + 2) = maze.pathType.block And mapMaze.mazePathType(pos.X + 1, pos.Y + 3) <> maze.pathType.block) Or
                        '(mapMaze.mazePathType(pos.X + 2, pos.Y) = maze.pathType.block And mapMaze.mazePathType(pos.X + 3, pos.Y) <> maze.pathType.block) Or
                        '(mapMaze.mazePathType(pos.X + 2, pos.Y + 1) = maze.pathType.block And mapMaze.mazePathType(pos.X + 3, pos.Y + 1) <> maze.pathType.block) Or
                        '(mapMaze.mazePathType(pos.X - 1, pos.Y) = maze.pathType.block And mapMaze.mazePathType(pos.X - 2, pos.Y) <> maze.pathType.block) Or
                        '(mapMaze.mazePathType(pos.X - 1, pos.Y + 1) = maze.pathType.block And mapMaze.mazePathType(pos.X - 2, pos.Y + 1) <> maze.pathType.block) Then

                        'If (mapMaze.mazePathType(pos.X, pos.Y - 1) = maze.pathType.block And mapMaze.mazePathType(pos.X, pos.Y - 2) <> maze.pathType.block) Or
                        '        (mapMaze.mazePathType(pos.X + 1, pos.Y - 1) = maze.pathType.block And mapMaze.mazePathType(pos.X + 1, pos.Y - 2) <> maze.pathType.block) Or
                        '        (mapMaze.mazePathType(pos.X, pos.Y + 2) = maze.pathType.block And mapMaze.mazePathType(pos.X, pos.Y + 3) <> maze.pathType.block) Or
                        '        (mapMaze.mazePathType(pos.X + 1, pos.Y + 2) = maze.pathType.block And mapMaze.mazePathType(pos.X + 1, pos.Y + 3) <> maze.pathType.block) Or
                        '        (mapMaze.mazePathType(pos.X + 2, pos.Y) = maze.pathType.block And mapMaze.mazePathType(pos.X + 3, pos.Y) <> maze.pathType.block) Or
                        '        (mapMaze.mazePathType(pos.X + 2, pos.Y + 1) = maze.pathType.block And mapMaze.mazePathType(pos.X + 3, pos.Y + 1) <> maze.pathType.block) Or
                        '        (mapMaze.mazePathType(pos.X - 1, pos.Y) = maze.pathType.block And mapMaze.mazePathType(pos.X - 2, pos.Y) <> maze.pathType.block) Or
                        '        (mapMaze.mazePathType(pos.X - 1, pos.Y + 1) = maze.pathType.block And mapMaze.mazePathType(pos.X - 2, pos.Y + 1) <> maze.pathType.block) Then
                        ' If tiles will be left unconnected, the outline box will be set to red, to indicate nthe current block cannot be deleted.


                    End If
                Case mode.addEnergizer
                    ' If the uer enables add energizer.
                    ' Performs checks to see if any tiles within the outlined box are fixed and are not labeled as blocks.
                    If mapMaze.mazePathType(pos.X, pos.Y) <> maze.pathType.block And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                        ' If so the outline box is set to green, to indicate that an energizer can be placed in that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(3, 3)
                    Else
                        ' Otherwise, the outline box is set to red, to indicate that an energizer cannot be placed in that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(2, 2)
                    End If
                Case mode.addDot
                    ' If the user enables add dot.
                    ' Performs checks to see if any tiles within the outlined box are fixed and are not labeled as blocks.
                    If mapMaze.mazePathType(pos.X, pos.Y) <> maze.pathType.block And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                        ' If so the outline box is set to green, to indicate that a dot can be placed in that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(3, 3)
                    Else
                        ' Otherwise, the outline box is set to red, to indicate that a dot cannot be placed in that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(2, 2)
                    End If
                Case mode.deleteDot
                    ' If the user enables delete dot.
                    ' Performs checks to see if any tiles within the outlined box are fixed and are labeled as dots.
                    If mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.dot And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                        ' If so the outline box is set to green, to indicate that a dot can be deleted from that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(3, 3)
                    Else
                        ' Otherwise, the outline box is set to red, to indicate that a dot cannot be deleted from that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(2, 2)
                    End If
                Case mode.deleteEnergizer
                    ' If the user enables delete energizer.
                    ' Performs checks to see if any tiles within the outlined box are fixed and are labeled as energizers.
                    If mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.energizer And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                        ' If so the outline box is set to green, to indicate that an energizer can be deleted from that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(3, 3)
                    Else
                        ' Otherwise, the outline box is set to red, to indicate that an energizer cannot be deleted from that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(2, 2)
                    End If

            End Select
        End If

        ' Debug
        Dim off As Integer
        For y = -2 To 3
            For x = -2 To 3
                If mapMaze.mazeBlockFixed(pos.X + x, pos.Y + y) = True Then
                    off = 12
                Else
                    off = 0
                End If
                If mapMaze.path(New Point(pos.X + x, pos.Y + y)).pathType = maze.pathType.block Then
                    If x < 0 Or x > 1 Or y < 0 Or y > 1 Then
                        mapEngine.mapByName("debug").value(New Point(2 + x, 2 + y)) = 51 + off
                    Else
                        mapEngine.mapByName("debug").value(New Point(2 + x, 2 + y)) = 53 + off
                    End If
                Else
                    If x < 0 Or x > 1 Or y < 0 Or y > 1 Then
                        mapEngine.mapByName("debug").value(New Point(2 + x, 2 + y)) = 52 + off
                    Else
                        mapEngine.mapByName("debug").value(New Point(2 + x, 2 + y)) = 54 + off
                    End If
                End If
            Next
        Next

    End Sub

    Public Sub mapEngine_geMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapEngine.geMouseUp
        ' Handles the action performed on a mouse click.

        Dim pos As Point

        pos = New Point(Int(e.Location.X / (8 * CLIENT_SCALE)), Int(e.Location.Y / (8 * CLIENT_SCALE)))

        ' Depending upon the mode indicated by the blockMode variable, a different action will be performed.
        Select Case blockMode
            Case mode.addBlock
                ' If the user enables add block.
                ' Performs a check to ensure the tiles within the outlined box are not fixed.
                If Not (mapMaze.mazeBlockFixed(pos.X, pos.Y) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y) Or mapMaze.mazeBlockFixed(pos.X, pos.Y + 1) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y + 1)) Then
                    ' If true then each of the tiles within the outlined box are set to blocks.
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.block
                    mapMaze.mazePathType(pos.X + 1, pos.Y) = maze.pathType.block
                    mapMaze.mazePathType(pos.X, pos.Y + 1) = maze.pathType.block
                    mapMaze.mazePathType(pos.X + 1, pos.Y + 1) = maze.pathType.block
                End If
            Case mode.deleteBlock
                ' If the user enables delete block.
                ' Performs a check to ensure the tiles within the outlined box are not fixed.
                If Not (mapMaze.mazeBlockFixed(pos.X, pos.Y) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y) Or mapMaze.mazeBlockFixed(pos.X, pos.Y + 1) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y + 1)) Then
                    ' Performs a check to ensure that once the current block is deleted no single or unconnected tiles are left.

                    For y = 1 To 28
                        For x = 1 To 26
                            If (x = pos.X And y = pos.Y) Then
                                If ((x > 2 And x < 25) And (y > 2 And y < 27)) Then
                                    If (mapMaze.mazePathType(x, y - 1) = maze.pathType.block And mapMaze.mazePathType(x, y - 2) <> maze.pathType.block) Or
                            (mapMaze.mazePathType(x + 1, y - 1) = maze.pathType.block And mapMaze.mazePathType(x + 1, y - 2) <> maze.pathType.block) Or
                            (mapMaze.mazePathType(x, y + 2) = maze.pathType.block And mapMaze.mazePathType(x, y + 3) <> maze.pathType.block) Or
                            (mapMaze.mazePathType(x + 1, y + 2) = maze.pathType.block And mapMaze.mazePathType(x + 1, y + 3) <> maze.pathType.block) Or
                            (mapMaze.mazePathType(x + 2, y) = maze.pathType.block And mapMaze.mazePathType(x + 3, y) <> maze.pathType.block) Or
                            (mapMaze.mazePathType(x + 2, y + 1) = maze.pathType.block And mapMaze.mazePathType(x + 3, y + 1) <> maze.pathType.block) Or
                            (mapMaze.mazePathType(x - 1, y) = maze.pathType.block And mapMaze.mazePathType(x - 2, y) <> maze.pathType.block) Or
                            (mapMaze.mazePathType(x - 1, y + 1) = maze.pathType.block And mapMaze.mazePathType(x - 2, y + 1) <> maze.pathType.block) Then

                                    Else
                                        alterOutlinedTiles(x, y)
                                    End If
                                Else

                                    If ((mapMaze.mazeBlockFixed(x - 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y) = True) And (mapMaze.mazeBlockFixed(x + 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x, y - 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 1) = True) Or
                                        (mapMaze.mazeBlockFixed(x + 2, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 2, y) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 1) = True) And (mapMaze.mazeBlockFixed(x, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 1, y + 2) = True) Or
                                        (mapMaze.mazeBlockFixed(x, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 1) = True) Or
                                        (mapMaze.mazeBlockFixed(x - 1, y) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 2) = True) And (mapMaze.mazeBlockFixed(x, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 1, y + 2) = True) Or
                                        (mapMaze.mazeBlockFixed(x - 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y - 1) = True) Or
                                        (mapMaze.mazeBlockFixed(x - 1, y + 2) = True) And (mapMaze.mazeBlockFixed(x, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 1, y + 2) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 2) = True) Or
                                        (mapMaze.mazeBlockFixed(x - 1, y - 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 1) = True) And (mapMaze.mazeBlockFixed(x - 1, y + 2) = True) Or
                                        (mapMaze.mazeBlockFixed(x + 2, y - 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 1) = True) And (mapMaze.mazeBlockFixed(x + 2, y + 2) = True)) Then

                                        alterOutlinedTiles(x, y)

                                        If (mapMaze.mazePathType(x + 2, y) = maze.pathType.block And mapMaze.mazeBlockFixed(x + 2, y) = False) And mapMaze.mazePathType(x + 3, y) <> maze.pathType.block Then
                                            If mapMaze.data(New Point(x + 2, y)) = maze.mazeObjects.blank Then
                                                mapMaze.mazePathType(x + 2, y) = maze.pathType.blank
                                            Else
                                                mapMaze.mazePathType(x + 2, y) = maze.pathType.dot
                                            End If
                                            If mapMaze.data(New Point(x + 2, y + 1)) = maze.mazeObjects.blank Then
                                                mapMaze.mazePathType(x + 2, y + 1) = maze.pathType.blank
                                            Else
                                                mapMaze.mazePathType(x + 2, y + 1) = maze.pathType.dot
                                            End If
                                        End If
                                        If (mapMaze.mazePathType(x - 1, y) = maze.pathType.block And mapMaze.mazeBlockFixed(x - 1, y) = False) And mapMaze.mazePathType(x - 2, y) <> maze.pathType.block Then
                                            If mapMaze.data(New Point(x - 1, y)) = maze.mazeObjects.blank Then
                                                mapMaze.mazePathType(x - 1, y) = maze.pathType.blank
                                            Else
                                                mapMaze.mazePathType(x - 1, y) = maze.pathType.dot
                                            End If
                                            If mapMaze.data(New Point(x - 1, y + 1)) = maze.mazeObjects.blank Then
                                                mapMaze.mazePathType(x - 1, y + 1) = maze.pathType.blank
                                            Else
                                                mapMaze.mazePathType(x - 1, y + 1) = maze.pathType.dot
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    Next

                    'If (mapMaze.mazePathType(pos.X, pos.Y - 1) = maze.pathType.block And mapMaze.mazePathType(pos.X, pos.Y - 2) <> maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X + 1, pos.Y - 1) = maze.pathType.block And mapMaze.mazePathType(pos.X + 1, pos.Y - 2) <> maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X, pos.Y + 2) = maze.pathType.block And mapMaze.mazePathType(pos.X, pos.Y + 3) <> maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X + 1, pos.Y + 2) = maze.pathType.block And mapMaze.mazePathType(pos.X + 1, pos.Y + 3) <> maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X + 2, pos.Y) = maze.pathType.block And mapMaze.mazePathType(pos.X + 3, pos.Y) <> maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X + 2, pos.Y + 1) = maze.pathType.block And mapMaze.mazePathType(pos.X + 3, pos.Y + 1) <> maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X - 1, pos.Y) = maze.pathType.block And mapMaze.mazePathType(pos.X - 2, pos.Y) <> maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X - 1, pos.Y + 1) = maze.pathType.block And mapMaze.mazePathType(pos.X - 2, pos.Y + 1) <> maze.pathType.block) Then

                    'If (mapMaze.mazePathType(pos.X, pos.Y - 1) = maze.pathType.block And mapMaze.mazePathType(pos.X, pos.Y - 2) = maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X + 1, pos.Y - 1) = maze.pathType.block And mapMaze.mazePathType(pos.X + 1, pos.Y - 2) = maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X, pos.Y + 2) = maze.pathType.block And mapMaze.mazePathType(pos.X, pos.Y + 3) = maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X + 1, pos.Y + 2) = maze.pathType.block And mapMaze.mazePathType(pos.X + 1, pos.Y + 3) = maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X + 2, pos.Y) = maze.pathType.block And mapMaze.mazePathType(pos.X + 3, pos.Y) = maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X + 2, pos.Y + 1) = maze.pathType.block And mapMaze.mazePathType(pos.X + 3, pos.Y + 1) = maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X - 1, pos.Y) = maze.pathType.block And mapMaze.mazePathType(pos.X - 2, pos.Y) = maze.pathType.block) Or
                    '    (mapMaze.mazePathType(pos.X - 1, pos.Y + 1) = maze.pathType.block And mapMaze.mazePathType(pos.X - 2, pos.Y + 1) = maze.pathType.block) Then
                    'Else
                    '    ' The next If Statements determine what each tile within the outlined box become once deleted.
                    '    If mapMaze.data(New Point(pos.X, pos.Y)) = maze.mazeObjects.blank Then
                    '        mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.blank
                    '    Else
                    '        mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.dot
                    '    End If
                    '    If mapMaze.data(New Point(pos.X + 1, pos.Y)) = maze.mazeObjects.blank Then
                    '        mapMaze.mazePathType(pos.X + 1, pos.Y) = maze.pathType.blank
                    '    Else
                    '        mapMaze.mazePathType(pos.X + 1, pos.Y) = maze.pathType.dot
                    '    End If
                    '    If mapMaze.data(New Point(pos.X, pos.Y + 1)) = maze.mazeObjects.blank Then
                    '        mapMaze.mazePathType(pos.X, pos.Y + 1) = maze.pathType.blank
                    '    Else
                    '        mapMaze.mazePathType(pos.X, pos.Y + 1) = maze.pathType.dot
                    '    End If
                    '    If mapMaze.data(New Point(pos.X + 1, pos.Y + 1)) = maze.mazeObjects.blank Then
                    '        mapMaze.mazePathType(pos.X + 1, pos.Y + 1) = maze.pathType.blank
                    '    Else
                    '        mapMaze.mazePathType(pos.X + 1, pos.Y + 1) = maze.pathType.dot
                    '    End If
                    'End If
                End If
            Case mode.addEnergizer
                ' If the user enables add energizer.
                ' Performs a check to ensure that the block the user would like to place the energizer in does not contain a block and is fixed.
                If mapMaze.mazePathType(pos.X, pos.Y) <> maze.pathType.block And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                    ' If so an enetgizer is placed in the current block.
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.energizer
                End If
            Case mode.addDot
                ' If the user enables add dot.
                ' Performs a check to ensure that the block the user would like to place the dot in does not contain a block and is not fixed.
                If mapMaze.mazePathType(pos.X, pos.Y) <> maze.pathType.block And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                    ' If so a dot is placed in the current block.
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.dot
                End If
            Case mode.deleteDot
                ' If the user enables delete dot.
                ' Performs a check to ensure that the dot the user would like to delete is in fact a dot and also that it is not fixed.
                If mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.dot And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                    ' If so the dot is turned into a blank.
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.blank
                End If
            Case mode.deleteEnergizer
                ' If the uesr enables delete energizer.
                ' Performs a check to ensure that the energizer the user would like to delete is in fact an energizer and also that it is not fixed.
                If mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.energizer And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                    ' If so the energizer is turned into a blank.
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.blank
                End If

        End Select

        copyGameEngineMaze()

    End Sub

    Private Sub copyGameEngineMaze()

        mapMaze.pathToData()

        ' Actually displays the maze. 
        ' Each time an action is performed the map engine map is updated.
        For y = 0 To 30
            For x = 0 To 27
                mapEngine.mapByName("main").value(New Point(x, y)) = mapMaze.data(New Point(x, y))
            Next
        Next

    End Sub

    Public Sub initialize()

        Me.ClientSize = New Size((CLIENT_WIDTH * CLIENT_SCALE), CLIENT_HEIGHT * CLIENT_SCALE)
        Me.BackColor = Color.Black

        ' Creates a new instance of the game engine.
        mapEngine = New gameEngine(Me, CLIENT_WIDTH, CLIENT_HEIGHT - 40, CLIENT_SCALE, ToolStrip1.Height)

        ' Adds a new tileset of size 8 x 8.
        mapEngine.addTile("defaultMaze", New Size(8, 8))
        ' Intitalizes the map varialbe to use the defaul maze tileset and initializes the size.
        map = mapEngine.addMap("main", mapEngine.tileIndexByName("defaultMaze"), New Size(32, 40))

        ' Adds a new tileset of size 8 x 8.
        mapEngine.addTile("mapEditor", New Size(8, 8))
        ' This tileset is used for debugging purposes.
        debugMap = mapEngine.addMap("debug", mapEngine.tileIndexByName("mapEditor"), New Size(6, 6))    ' DEBUG
        ' Intitalizes the positioin of the debugger.
        debugMap.point = New Point(88, 94)

        ' Adds a new instance of the Pacman sprite to the map engine, and initializes required attributes.
        mapPacman = mapEngine.addSprite("pacman", New Size(16, 16))
        mapPacman.animateMode = gameEngine.sprite.geAnimationMode.geBoth
        mapPacman.animateOnFrame = 5
        mapPacman.animationRange = New gameEngine.sprite.geAnimationRange(3, 5)
        mapPacman.enabled = True
        mapPacman.point = New Point(13 * 8, (22 * 8) + 4)

        ' Adds a new sprite to the map engine or the map cursor, and initializes required attributes.
        mapCursor = mapEngine.addSprite("newmapcur", New Size(16, 16))
        mapCursor.animateMode = gameEngine.sprite.geAnimationMode.geNone
        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(1, 1)
        mapCursor.enabled = True

        ' Loads a blank maze.
        mapMaze.loadMaze("")

        copyGameEngineMaze()

        mapEngine.drawMap()

        mapEngine.startEngine()

    End Sub

    Private Sub MJ_mapEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        mapEngine.endEngine()

    End Sub

    Private Sub toolStripBlock_Click(sender As Object, e As EventArgs) Handles toolStripBlock.Click
        ' Handles the creation of the buttons and the event of clicking the add block button.

        If blockMode <> mode.addBlock And blockMode <> mode.deleteBlock Then
            toolStripBlock.Image = iconImage.Images(1)
            toolStripDot.Image = iconImage.Images(3)
            toolStripEnergizer.Image = iconImage.Images(6)
            toolStripBlock.ToolTipText = "Add Block"
            toolStripDot.ToolTipText = "Add Dot"
            toolStripEnergizer.ToolTipText = "Add Energizer"
            blockMode = mode.addBlock
        Else
            If blockMode = mode.addBlock Then
                toolStripBlock.Image = iconImage.Images(2)
                toolStripDot.Image = iconImage.Images(3)
                toolStripEnergizer.Image = iconImage.Images(6)
                toolStripBlock.ToolTipText = "Delete Block"
                blockMode = mode.deleteBlock
            Else
                toolStripBlock.Image = iconImage.Images(1)
                toolStripDot.Image = iconImage.Images(3)
                toolStripEnergizer.Image = iconImage.Images(6)
                toolStripBlock.ToolTipText = "Add Block"
                blockMode = mode.addBlock
            End If
        End If

        ToolStrip1.Refresh()

    End Sub

    Private Sub toolStripDot_Click(sender As Object, e As EventArgs) Handles toolStripDot.Click
        ' Handles the creation of the buttons and the event of clicking the add dot button. 

        If blockMode <> mode.addDot And blockMode <> mode.deleteBlock Then
            toolStripBlock.Image = iconImage.Images(0)
            toolStripDot.Image = iconImage.Images(4)
            toolStripEnergizer.Image = iconImage.Images(6)
            toolStripBlock.ToolTipText = "Add Block"
            toolStripDot.ToolTipText = "Add Dot"
            toolStripEnergizer.ToolTipText = "Add Energizer"
            blockMode = mode.addDot
        Else
            If blockMode = mode.addDot Then
                toolStripBlock.Image = iconImage.Images(0)
                toolStripDot.Image = iconImage.Images(5)
                toolStripEnergizer.Image = iconImage.Images(6)
                toolStripDot.ToolTipText = "Delete Dot"
                blockMode = mode.deleteDot
            Else
                toolStripBlock.Image = iconImage.Images(0)
                toolStripDot.Image = iconImage.Images(4)
                toolStripEnergizer.Image = iconImage.Images(6)
                toolStripDot.ToolTipText = "Add Dot"
                blockMode = mode.addDot
            End If
        End If

        ToolStrip1.Refresh()

    End Sub

    Private Sub toolStripEnergizer_Click(sender As Object, e As EventArgs) Handles toolStripEnergizer.Click
        ' Handles the creation of the buttons and the event of clicking the add energizer button.

        If blockMode <> mode.addEnergizer And blockMode <> mode.deleteEnergizer Then
            toolStripBlock.Image = iconImage.Images(0)
            toolStripDot.Image = iconImage.Images(3)
            toolStripEnergizer.Image = iconImage.Images(7)
            toolStripBlock.ToolTipText = "Add Block"
            toolStripDot.ToolTipText = "Add Dot"
            toolStripEnergizer.ToolTipText = "Add Energizer"
            blockMode = mode.addEnergizer
        Else
            If blockMode = mode.addEnergizer Then
                toolStripBlock.Image = iconImage.Images(0)
                toolStripDot.Image = iconImage.Images(3)
                toolStripEnergizer.Image = iconImage.Images(8)
                toolStripEnergizer.ToolTipText = "Delete Energizer"
                blockMode = mode.deleteEnergizer
            Else
                toolStripBlock.Image = iconImage.Images(0)
                toolStripDot.Image = iconImage.Images(3)
                toolStripEnergizer.Image = iconImage.Images(7)
                toolStripEnergizer.ToolTipText = "Add Energizer"
                blockMode = mode.addEnergizer
            End If
        End If

        ToolStrip1.Refresh()

    End Sub

    Private Sub toolStripReset_Click(sender As Object, e As EventArgs) Handles toolStripReset.Click
        ' Handles the creation of the button and the event of clicking the reset button.

        Dim reply As DialogResult

        reply = MessageBox.Show("Are you sure you want to reset the maze?", "Reset Maze", MessageBoxButtons.YesNo)

        If reply = Windows.Forms.DialogResult.Yes Then

            mapMaze.loadMaze("")
            copyGameEngineMaze()

            toolStripBlock.Image = iconImage.Images(1)
            toolStripDot.Image = iconImage.Images(3)
            toolStripEnergizer.Image = iconImage.Images(6)
            toolStripBlock.ToolTipText = "Add Block"
            toolStripDot.ToolTipText = "Add Dot"
            toolStripEnergizer.ToolTipText = "Add Energizer"
            blockMode = mode.addBlock

        End If

    End Sub

    Public Sub alterOutlinedTiles(x As Integer, y As Integer)
        If mapMaze.data(New Point(x, y)) = maze.mazeObjects.blank Then
            mapMaze.mazePathType(x, y) = maze.pathType.blank
        Else
            mapMaze.mazePathType(x, y) = maze.pathType.dot
        End If
        If mapMaze.data(New Point(x + 1, y)) = maze.mazeObjects.blank Then
            mapMaze.mazePathType(x + 1, y) = maze.pathType.blank
        Else
            mapMaze.mazePathType(x + 1, y) = maze.pathType.dot
        End If
        If mapMaze.data(New Point(x, y + 1)) = maze.mazeObjects.blank Then
            mapMaze.mazePathType(x, y + 1) = maze.pathType.blank
        Else
            mapMaze.mazePathType(x, y + 1) = maze.pathType.dot
        End If
        If mapMaze.data(New Point(x + 1, y + 1)) = maze.mazeObjects.blank Then
            mapMaze.mazePathType(x + 1, y + 1) = maze.pathType.blank
        Else
            mapMaze.mazePathType(x + 1, y + 1) = maze.pathType.dot
        End If
    End Sub

    Private Sub toolStripSave_Click(sender As Object, e As EventArgs) Handles toolStripSave.Click
        ' Handles the creation of the button and the event of clicking the save button.

        Dim fd As FileDialog
        Dim stream As FileStream

        fd = New SaveFileDialog

        fd.Title = "Save Pacman Maze"
        fd.Filter = "Pacman Mazes (*.pac)|*.pac"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            stream = New FileStream(fd.FileName, FileMode.Create)

            For y = 0 To 30
                For x = 0 To 27
                    stream.WriteByte(mapMaze.mazePathType(x, y))
                Next
            Next
            stream.Close()
        End If

    End Sub

    Private Sub toolStripLoad_Click(sender As Object, e As EventArgs) Handles toolStripLoad.Click
        ' Handles the creation of the button and the event of clicking the load button.

        Dim fd As FileDialog
        Dim stream As FileStream

        fd = New OpenFileDialog

        fd.Title = "Load Pacman Maze"
        fd.Filter = "Pacman Mazes (*.pac)|*.pac"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            stream = New FileStream(fd.FileName, FileMode.Open)

            Dim fileName As String = fd.FileName

            For y = 0 To 30
                For x = 0 To 27
                    mapMaze.mazePathType(x, y) = stream.ReadByte()
                Next
            Next

            stream.Close()
        End If

    End Sub

    Private Sub toolStripInfo_Click(sender As Object, e As EventArgs) Handles toolStripInfo.Click

        MsgBox("To use the map editor simply select the object you would like to place from the tool strip and place using the mouse." & vbNewLine &
               "A red outlined box will indicate where objects cannot be placed, whereas a green outlined box will indicate where objects can be placed." & vbNewLine &
               "To delete objects, simply, select the objects icon again until the delete sign appears on the icon." & vbNewLine &
               "The same rules apply for what you can and cannot delete, a green box means you can and a red box means you cannot delete the object.", MsgBoxStyle.Information, "Map Editor Instructions")

    End Sub

End Class