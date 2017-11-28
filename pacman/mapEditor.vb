Imports System.IO
Imports System.Text

Public Class MapEditor

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

    ' New maze object
    Private mapMaze As New maze

    ' New temporary maze object (used to determine whether a block deletion is valid)
    Private mapMazeDelete As New maze

    ' Block placement mode (default to addBlock)
    Dim blockMode As Integer = mode.addBlock

    Private Sub MapEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' On Form Load, initialize the map editor
        initialize()

    End Sub

    Public Sub mapEngine_geGameLogic() Handles mapEngine.geGameLogic

        ' This is the geGameLogic event.
        ' This is triggered once per frame by the gameEngine.

        ' There is no game logic

    End Sub

    Public Sub mapEngine_geRenderScene() Handles mapEngine.geRenderScene

        ' This is the geRenderScene event.
        ' This is triggered once per frame by the gameEngine.

        ' If there is mouse data (i.e. the mouse if over the gameSurface)...
        If mapEngine.getMouse IsNot Nothing Then

            ' Set the mapCursor point (measured in 8x8 tile units)
            mapCursor.point = New Point(Int(mapEngine.getMouse.Location.X / (8 * CLIENT_SCALE)) * 8, Int(mapEngine.getMouse.Location.Y / (8 * CLIENT_SCALE)) * 8)

        End If

    End Sub

    Public Sub mapEngine_geMouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapEngine.geMouseMove

        ' This is the geMouseMove event.
        ' This is triggered once per frame by the gameEngine and reports mouse movement

        ' Get the current mouse position
        Dim pos As Point = New Point(Int(e.Location.X / (8 * CLIENT_SCALE)), Int(e.Location.Y / (8 * CLIENT_SCALE)))

        ' Set the mapCursor point (measured in 8x8 tile units)
        mapCursor.point = New Point(pos.X * 8, pos.Y * 8)

        ' As long as the mouse cursor is within the gameSurface area...
        If pos.X < 27 And pos.Y < 30 Then

            ' Depending upon the mode indicated by the blockMode variable, a different outlining box is shown.
            Select Case blockMode

                Case mode.addBlock

                    ' If addBlock mode is active then check whether the tiles under the cursor are fixed
                    ' If they are then the cursor is red, otherwise it's green
                    If mapMaze.mazeBlockFixed(pos.X, pos.Y) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y) Or mapMaze.mazeBlockFixed(pos.X, pos.Y + 1) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y + 1) Then
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
                    Else
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(1, 1)
                    End If

                Case mode.deleteBlock

                    ' If deleteBlock mode is active then check whether deletion is allowed at this position
                    ' If it is not then the cursor is red, otherwise it's green
                    If checkDelete(pos.X, pos.Y) = False Then
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
                    Else
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(1, 1)
                    End If

                Case mode.addEnergizer

                    ' If addEnergizer mode is active then check whether the tile under the cursor is fixed or a block
                    ' If is is then the cursor is red, otherwise it's green
                    If mapMaze.mazePathType(pos.X, pos.Y) <> maze.pathType.block And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(3, 3)
                    Else
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(2, 2)
                    End If

                Case mode.addDot

                    ' If addDot mode is active then check whether the tile under the cursor is fixed or a block
                    ' If is is then the cursor is red, otherwise it's green
                    If mapMaze.mazePathType(pos.X, pos.Y) <> maze.pathType.block And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                        ' If so the outline box is set to green, to indicate that a dot can be placed in that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(3, 3)
                    Else
                        ' Otherwise, the outline box is set to red, to indicate that a dot cannot be placed in that position.
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(2, 2)
                    End If

                Case mode.deleteDot

                    ' If deleteDot mode is active then check whether the tile under the cursor is fixed or a not a dot
                    ' If is is then the cursor is red, otherwise it's green
                    If mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.dot And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(3, 3)
                    Else
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(2, 2)
                    End If

                Case mode.deleteEnergizer

                    ' If deleteEnergizer mode is active then check whether the tile under the cursor is fixed or a not an energizer
                    ' If is is then the cursor is red, otherwise it's green
                    If mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.energizer And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(3, 3)
                    Else
                        mapCursor.animationRange = New gameEngine.sprite.geAnimationRange(2, 2)
                    End If

            End Select
        End If

        ' If debugging is enabled...
        If ToolStrip_Debug.Checked = True Then

            ' Draw a representation of the 6x6 blocks that surround the cursor
            ' This shows fixed blocks, fixed non-blocks, blocks, and non-blocks.
            Dim off As Integer
            Dim size As Integer
            For y = -2 To 3
                For x = -2 To 3
                    If mapMaze.mazeBlockFixed(pos.X + x, pos.Y + y) = True Then
                        off = 12
                    Else
                        off = 0
                    End If
                    If blockMode = mode.addBlock Or blockMode = mode.deleteBlock Then
                        size = 1
                    Else
                        size = 0
                    End If
                    If mapMaze.path(New Point(pos.X + x, pos.Y + y)).pathType = maze.pathType.block Then
                        If x < 0 Or x > size Or y < 0 Or y > size Then
                            debugMap.value(New Point(2 + x, 2 + y)) = 51 + off
                        Else
                            If mapCursor.animationRange.min = 0 Then
                                debugMap.value(New Point(2 + x, 2 + y)) = 55 + off
                            Else
                                debugMap.value(New Point(2 + x, 2 + y)) = 53 + off
                            End If
                        End If
                    Else
                        If x < 0 Or x > size Or y < 0 Or y > size Then
                            debugMap.value(New Point(2 + x, 2 + y)) = 52 + off
                        Else
                            If mapCursor.animationRange.min = 0 Then
                                debugMap.value(New Point(2 + x, 2 + y)) = 56 + off
                            Else
                                debugMap.value(New Point(2 + x, 2 + y)) = 54 + off
                            End If
                        End If
                    End If
                Next
            Next

        End If

    End Sub

    Public Sub mapEngine_geMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapEngine.geMouseUp

        ' This is the geMouseUp event.
        ' This is triggered once per frame by the gameEngine and reports button releases on the mouse

        ' Get the current mouse position
        Dim pos = New Point(Int(e.Location.X / (8 * CLIENT_SCALE)), Int(e.Location.Y / (8 * CLIENT_SCALE)))

        ' Depending upon the mode indicated by the blockMode variable, a different outlining box is shown.
        Select Case blockMode

            Case mode.addBlock

                ' If addBlock mode is active then check whether the tiles under the cursor are fixed
                ' If they are not then add a block
                If Not (mapMaze.mazeBlockFixed(pos.X, pos.Y) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y) Or mapMaze.mazeBlockFixed(pos.X, pos.Y + 1) Or mapMaze.mazeBlockFixed(pos.X + 1, pos.Y + 1)) Then
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.block
                    mapMaze.mazePathType(pos.X + 1, pos.Y) = maze.pathType.block
                    mapMaze.mazePathType(pos.X, pos.Y + 1) = maze.pathType.block
                    mapMaze.mazePathType(pos.X + 1, pos.Y + 1) = maze.pathType.block
                End If

            Case mode.deleteBlock

                ' If deleteBlock mode is active then check whether deletion is allowed at this position
                ' If it is then delete the block by replacing with dots
                If checkDelete(pos.X, pos.Y) = True Then
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.dot
                    mapMaze.mazePathType(pos.X + 1, pos.Y) = maze.pathType.dot
                    mapMaze.mazePathType(pos.X, pos.Y + 1) = maze.pathType.dot
                    mapMaze.mazePathType(pos.X + 1, pos.Y + 1) = maze.pathType.dot
                End If

            Case mode.addEnergizer

                ' If addEnergizer mode is active then check whether the tile under the cursor is fixed or a block
                ' If is is not then add an energizer
                If mapMaze.mazePathType(pos.X, pos.Y) <> maze.pathType.block And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.energizer
                End If

            Case mode.addDot

                ' If addDot mode is active then check whether the tile under the cursor is fixed or a block
                ' If is is not then add a dot
                If mapMaze.mazePathType(pos.X, pos.Y) <> maze.pathType.block And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.dot
                End If

            Case mode.deleteDot

                ' If deleteDot mode is active then check whether the tile under the cursor is fixed or not a dot
                ' If is is not then add a blank
                If mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.dot And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.blank
                End If

            Case mode.deleteEnergizer

                ' If deleteEnergizer mode is active then check whether the tile under the cursor is fixed or a not an energizer
                ' If is is then add a blank
                If mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.energizer And mapMaze.mazeBlockFixed(pos.X, pos.Y) = False Then
                    mapMaze.mazePathType(pos.X, pos.Y) = maze.pathType.blank
                End If

        End Select

        ' Copy the changes made to the maze into the gameEngine
        copyGameEngineMaze()

    End Sub

    Private Sub copyGameEngineMaze()

        ' Convert maze data into path data
        mapMaze.pathToData()

        ' Copy each element within the mapMaze into the gameEngine
        For y = 0 To 30
            For x = 0 To 27
                mapEngine.mapByName("main").value(New Point(x, y)) = mapMaze.data(New Point(x, y))
            Next
        Next

    End Sub

    Public Sub initialize()

        Me.ClientSize = New Size((CLIENT_WIDTH * CLIENT_SCALE), CLIENT_HEIGHT * CLIENT_SCALE)
        Me.BackColor = Color.Black

        ' Toolstrip defaults to rounded corners, so turn them off as it doesn't look great
        If TypeOf ToolStrip1.Renderer Is ToolStripProfessionalRenderer Then
            CType(ToolStrip1.Renderer, ToolStripProfessionalRenderer).RoundedEdges = False
        End If

        ' Creates a new instance of the game engine.
        mapEngine = New gameEngine(Me, CLIENT_WIDTH, CLIENT_HEIGHT - 40, CLIENT_SCALE, ToolStrip1.Height + MenuStrip1.Height)

        ' Adds a new tileset of size 8 x 8.
        mapEngine.addTile("defaultMaze", New Size(8, 8))
        ' Intitalizes the map varialbe to use the defaul maze tileset and initializes the size.
        map = mapEngine.addMap("main", mapEngine.tileIndexByName("defaultMaze"), New Size(32, 40))

        ' This tileset is used for debugging purposes.
        debugMap = mapEngine.addMap("debug", mapEngine.tileIndexByName("defaultMaze"), New Size(6, 6))    ' DEBUG
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

    Private Sub mapEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        ' The form is closing so end the mapEngine
        mapEngine.endEngine()

    End Sub

    Private Sub toolStripBlock_Click(sender As Object, e As EventArgs) Handles toolStripBlock.Click

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

    Private Function checkDelete(curX As Integer, curY As Integer) As Boolean

        If mapMaze.mazeBlockFixed(curX, curY) = True Or
            mapMaze.mazeBlockFixed(curX + 1, curY) = True Or
            mapMaze.mazeBlockFixed(curX, curY + 1) = True Or
            mapMaze.mazeBlockFixed(curX + 1, curY + 1) = True Then
            Return False
        End If

        ' Iterate through all the maze elements
        For y = 0 To 29
            For x = 0 To 27

                ' Copy the maze to the temporary maze and simulate the deletion of the block
                ' at the current cursor position
                If ((x = curX And y = curY) Or (x = curX + 1 And y = curY) Or (x = curX And y = curY + 1) Or (x = curX + 1 And y = curY + 1)) Then
                    mapMazeDelete.mazePathType(x, y) = maze.pathType.dot
                Else
                    mapMazeDelete.mazePathType(x, y) = mapMaze.mazePathType(x, y)
                End If

            Next
        Next y

        ' Iterate through all the maze elements and check for invalid blocks
        ' As soon as one if found exit the function with a false flag to indicate that
        ' removing the block results in an invalid maze state
        For y = 1 To 28
            For x = 1 To 26

                If mapMazeDelete.mazePathType(x, y) = maze.pathType.block Then
                    If mapMazeDelete.mazePathType(x - 1, y) = maze.pathType.block And mapMazeDelete.mazePathType(x, y - 1) = maze.pathType.block And mapMazeDelete.mazePathType(x - 1, y - 1) = maze.pathType.block Then
                    Else
                        If mapMazeDelete.mazePathType(x + 1, y) = maze.pathType.block And mapMazeDelete.mazePathType(x, y - 1) = maze.pathType.block And mapMazeDelete.mazePathType(x + 1, y - 1) = maze.pathType.block Then
                        Else
                            If mapMazeDelete.mazePathType(x - 1, y) = maze.pathType.block And mapMazeDelete.mazePathType(x, y + 1) = maze.pathType.block And mapMazeDelete.mazePathType(x - 1, y + 1) = maze.pathType.block Then
                            Else
                                If mapMazeDelete.mazePathType(x + 1, y) = maze.pathType.block And mapMazeDelete.mazePathType(x, y + 1) = maze.pathType.block And mapMazeDelete.mazePathType(x + 1, y + 1) = maze.pathType.block Then
                                Else
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If

            Next x
        Next y

        ' All block are valid so exit the function and return true to indicate a valid maze state
        Return True

    End Function

    Private Sub ResetMazeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStrip_ResetMaze.Click

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

    Private Sub ToolStrip_LoadMaze_Click(sender As Object, e As EventArgs) Handles ToolStrip_LoadMaze.Click

        Dim fd As FileDialog

        fd = New OpenFileDialog

        fd.Title = "Load Pacman Maze"
        fd.Filter = "Pacman Mazes (*.pac)|*.pac"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then

            mapMaze.loadMaze(fd.FileName)
            copyGameEngineMaze()

        End If

    End Sub

    Private Sub ToolStrip_SaveMaze_Click(sender As Object, e As EventArgs) Handles ToolStrip_SaveMaze.Click

        Dim fd As FileDialog

        fd = New SaveFileDialog

        fd.Title = "Save Pacman Maze"
        fd.Filter = "Pacman Mazes (*.pac)|*.pac"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then

            mapMaze.saveMaze(fd.FileName)

        End If

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Me.Close()

    End Sub

    Private Sub ToolStrip_Debug_Click(sender As Object, e As EventArgs) Handles ToolStrip_Debug.Click

        If ToolStrip_Debug.Checked = True Then
            ToolStrip_Debug.Checked = False
            debugMap.enabled = False
            copyGameEngineMaze()
        Else
            ToolStrip_Debug.Checked = True
            debugMap.enabled = True
        End If

    End Sub

    Private Sub ToolStrip_EditorInstructions_Click(sender As Object, e As EventArgs) Handles ToolStrip_EditorInstructions.Click

        mapEngine.endEngine()
        MazeEditorInstructions.ShowDialog()
        mapEngine.startEngine()

    End Sub
End Class