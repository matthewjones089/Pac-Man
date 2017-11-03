Imports System.Threading
Imports System.IO

Public Class pacman

    Public Const CLIENT_WIDTH = 224
    Public Const CLIENT_HEIGHT = 288
    Public Const CLIENT_SCALE = 2
    Public Const FPS = 60

    ' Create a new instance of the gameEngine and specify that we want events
    ' GameEngine exposes two public events that get called on each game loop
    ' GeGameLogic (this should contain the game logic)
    ' GeRenderScene (this is used to perform additional rendering, or gameEngine logic)

    Dim WithEvents gameEngine As gameEngine
    Public Shared maze As New maze
    Dim WithEvents actor As New actor

    ' Determines whether the debugging logic is called. The default state is off

    Structure ghostData
        Public name
        Public startPixel As Point
        Public cornerTile As Point
        Public startDirection As actor.actorDirection
        Public startMode As actor.ghostMode
        Public arriveHomeMode As actor.ghostMode
    End Structure

    Structure pacmanData
        Public name
        Public startPixel As Point
        Public startDirection As actor.actorDirection
    End Structure

    Dim ghosts(3) As ghostData
    Dim pacman(0) As pacmanData

    Dim level As Integer
    Dim score As Integer
    Dim highScore As Integer
    Dim lives As Integer
    Dim stepCount As Integer
    Dim state As gameState = gameState.reset

    Enum gameState
        reset = 0
        menu = 1
        getReady = 2
        gameStarted = 3
        playerDied = 4
    End Enum

    Dim debug As Boolean = False

    ' -----------------------------------------------------------------------------------------------------------

    Structure sInput
        Dim left As Boolean
        Dim right As Boolean
        Dim up As Boolean
        Dim down As Boolean
        Dim space As Boolean
    End Structure

    Dim input As sInput

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        initializeGame()

    End Sub


    Public Sub gameEngine_geGameLogic() Handles gameEngine.geGameLogic


        Select Case state

            Case gameState.reset

                ' We are in reset mode
                ' This mode lasts a since frame and its sole purpose is to reset the game variables,
                ' display the pacman logo, border and turn the pacman sprite off.
                ' Although pacman is not displayed it is still active as an actor and the ghosts still#
                ' target based on his starting position.

                resetGame()

                gameEngine.spriteByName("pacmanlogo2").enabled = True
                gameEngine.spriteByName("pacmanborder2").enabled = True
                gameEngine.spriteByName("pacman").enabled = False

                actor.pacmanByName("pacman").direction = actor.actorDirection.None

                state = gameState.menu

            Case gameState.menu

                ' We are in menu mode (attract mode)
                ' In this mode pacman is hidden and the ghosts wander the maze alternating
                ' between scatter and chase mode every 10 seconds

                actor.update(maze)

                If gameEngine.clock > 1800 Then
                    gameEngine.clock = 0
                End If
                If gameEngine.clock < 600 Then
                    actor.state = WindowsApplication1.actor.ghostState.Scatter
                Else
                    actor.state = WindowsApplication1.actor.ghostState.Chase
                End If

                If input.space = True Then
                    gameEngine.spriteByName("pacmanlogo2").enabled = False
                    gameEngine.spriteByName("pacmanborder2").enabled = False
                    gameEngine.spriteByName("pacman").enabled = True

                    actor.pacmanByName("pacman").direction = actor.actorDirection.Left

                    resetLevel()

                    state = gameState.getReady
                End If

            Case gameState.getReady

                If gameEngine.clock > (60 * 2) Then

                    state = gameState.gameStarted

                End If

            Case gameState.gameStarted

                ' Push input into actor Class
                ' The actor class will apply this to Pac-Man movement

                If input.up Then
                    actor.pacmanByName("pacman").nextDirection = actor.actorDirection.Up
                End If
                If input.down Then
                    actor.pacmanByName("pacman").nextDirection = actor.actorDirection.Down
                End If
                If input.left Then
                    actor.pacmanByName("pacman").nextDirection = actor.actorDirection.Left
                End If
                If input.right Then
                    actor.pacmanByName("pacman").nextDirection = actor.actorDirection.Right

                End If

                ' Update the actors
                actor.update(maze)

                ' If Pacman has eaten a dot...

                If maze.data(actor.pacmanByName("pacman").tile) = maze.mazeObjects.dot Then

                    score += 10

                    ' Update the game map

                    maze.data(actor.pacmanByName("pacman").tile) = maze.mazeObjects.blank

                    ' Update the gameEngine Map with a blank tile in place of the dot

                    gameEngine.mapByName("main").value(Point.Add(actor.pacmanByName("pacman").tile, New Point(0, 3))) = maze.mazeObjects.blank

                    ' Inform the releaser that a dot has been eaten

                    actor.ghostReleaser.dotEat()

                    If maze.dotEaten = 70 Or maze.dotEaten = 170 Then
                        If actor.fruitByName("fruit").active = False Then
                            actor.fruitByName("fruit").tick = (9 * 60)
                            actor.fruitByName("fruit").active = True
                        End If
                    End If

                    If maze.dotEaten = maze.dotCount Then
                        level += 1
                        nextLevel()
                        state = gameState.getReady
                    End If

                End If

                ' If Pacman has eaten an energizer...

                If maze.data(actor.pacmanByName("pacman").tile) = maze.mazeObjects.energizer Then

                    score += 50

                    actor.energize()
                    maze.data(actor.pacmanByName("pacman").tile) = maze.mazeObjects.blank
                    gameEngine.mapByName("main").value(Point.Add(actor.pacmanByName("pacman").tile, New Point(0, 3))) = maze.mazeObjects.blank
                End If

                ' If Pacman has died...

                If actor.pacmanByName("pacman").died = True Then

                    lives -= 1
                    If lives > 0 Then
                        resetLevel()
                        actor.ghostReleaser.restartLevel()
                        state = gameState.getReady
                    Else
                        If score > highScore Then
                            highScore = score
                            ' Save highscore after all lives are lost
                            Dim stream As StreamWriter = New StreamWriter("HighScore.txt")

                            Stream.Write(highScore)
                            stream.Close()

                        End If
                        resetGame()
                        state = gameState.reset
                    End If
                End If

                If actor.fruitByName("fruit").eaten = True Then
                    score += actor.fruitByName("fruit").points
                    actor.fruitByName("fruit").eaten = False
                End If

                ' Set the ghost state based on the gameEngine tick

                actor.setGhostState(gameEngine.clock)

            Case gameState.playerDied

        End Select

    End Sub

    Public Sub gameEngine_geRenderScene() Handles gameEngine.geRenderScene

        ' Set ghost positions

        For n = 0 To ghosts.Count - 1

            With actor.ghostByIndex(n)

                ' System.Diagnostics.Trace.Write("Ghost " & Str(n) & " - " & .stateChanged)
                If .scared And (.scaredChanged Or .flashingChanged) Then
                    If .flashing And .flashingChanged = True Then
                        gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(8, 11)
                    Else
                        gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(8, 9)
                    End If
                End If

                If (.directionChanged And Not .scared) Or (.scared = False And .scaredChanged) Then
                    If .mode = actor.ghostMode.ghostGoingHome Or .mode = actor.ghostMode.ghostEnteringHome Then

                        Select Case .direction
                            Case actor.actorDirection.Right
                                gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(12, 12)
                            Case actor.actorDirection.Left
                                gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(13, 13)
                            Case actor.actorDirection.Up
                                gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(14, 14)
                            Case actor.actorDirection.Down
                                gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(15, 15)
                        End Select

                    Else

                        Select Case .direction
                            Case actor.actorDirection.Right
                                gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(0, 1)
                            Case actor.actorDirection.Left
                                gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(2, 3)
                            Case actor.actorDirection.Up
                                gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(4, 5)
                            Case actor.actorDirection.Down
                                gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(6, 7)
                        End Select

                    End If
                End If

                ' If ghost has been eaten then display the appropriate score

                If .eatenTimer > 0 Then

                    ' If ghost has just been eathen then increment the score by the appropriate amount;
                    ' 200 for the 1st ghost, 400 for the 2nd, 800 for the 3rd, and 1600 for the 4th
                    ' The simple calculation for this is;
                    ' 2 ^ [Sequence Eaten] * 100

                    If .eatenTimer = (60 * 3) Then
                        score += (2 ^ (.eatenScore + 1)) * 100
                    End If

                    ' Display the eaten score in the appropriate location

                    gameEngine.spriteByName(.name & "score").animationRange = New gameEngine.sprite.geAnimationRange(.eatenScore, .eatenScore)
                    gameEngine.spriteByName(.name & "score").point = Point.Add(.eatenPixel, New Point(-7, 16))
                    gameEngine.spriteByName(.name & "score").enabled = True
                Else

                    ' The eaten timer has ran out so remove the score

                    gameEngine.spriteByName(.name & "score").enabled = False

                End If

                .directionChanged = False
                .scaredChanged = False
                .flashingChanged = False

            End With

            gameEngine.spriteByName(ghosts(n).name).point = Point.Add(actor.ghostByName(ghosts(n).name).pixel, New Size(-7, 16))

            If debug = True Then
                gameEngine.spriteByName(ghosts(n).name & "debug").point = Point.Add(actor.ghostByName(ghosts(n).name).targetPixel, New Size(-7 + 4, 16 + 4))
            End If

        Next

        ' Set pacman position

        For n = 0 To pacman.Count - 1
            With actor.pacmanByIndex(n)
                If .directionChanged = True Then

                    If .direction = actor.actorDirection.None Then
                        gameEngine.spriteByName(pacman(n).name).animateMode = gameEngine.sprite.geAnimationMode.geNone
                    Else
                        gameEngine.spriteByName(pacman(n).name).animateMode = gameEngine.sprite.geAnimationMode.geBoth
                    End If

                    Select Case .direction
                        Case actor.actorDirection.Right
                            gameEngine.spriteByName(pacman(n).name).animationRange = New gameEngine.sprite.geAnimationRange(0, 2)
                        Case actor.actorDirection.Left
                            gameEngine.spriteByName(pacman(n).name).animationRange = New gameEngine.sprite.geAnimationRange(3, 5)
                        Case actor.actorDirection.Up
                            gameEngine.spriteByName(pacman(n).name).animationRange = New gameEngine.sprite.geAnimationRange(6, 8)
                        Case actor.actorDirection.Down
                            gameEngine.spriteByName(pacman(n).name).animationRange = New gameEngine.sprite.geAnimationRange(9, 11)
                    End Select

                    .directionChanged = False

                End If
            End With
            gameEngine.spriteByName(pacman(n).name).point = Point.Add(actor.pacmanByName(pacman(n).name).pixel, New Size(-7, 16))
        Next

        ' Set fruit position

        If actor.fruitByName("fruit").active Then
            gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(actor.fruitByName("fruit").number, actor.fruitByName("fruit").number)
            gameEngine.spriteByName("fruit").enabled = True
        Else
            gameEngine.spriteByName("fruit").enabled = False
        End If

        ' Render text

        gameEngine.drawTextbyName("font", "1UP", New Point(24, 0))
        gameEngine.drawTextbyName("font", "HIGH SCORE", New Point(72, 0))

        ' Render score
        gameEngine.drawTextbyName("font", String.Format("{0, 6}", score.ToString("####00")), New Point(8, 8))

        ' Render high score
        gameEngine.drawTextbyName("font", String.Format("{0, 6}", highScore.ToString("####00")), New Point(88, 8))

        If state = gameState.menu Then
            gameEngine.drawTextbyName("font", "PACMAN 2017", New Point((8 * 8) + 4, (24 * 8)))
            gameEngine.drawTextbyName("font", "BY MATT JONES", New Point((7 * 8) + 4, (25 * 8)))

            gameEngine.drawTextbyName("font", "PRESS SPACE", New Point((8 * 8) + 4, (27 * 8)))
            gameEngine.drawTextbyName("font", "TO START GAME", New Point((7 * 8) + 4, (28 * 8)))
        End If

        If state = gameState.getReady Then
            gameEngine.drawTextbyName("font", "GET READY", New Point((9 * 8) + 4, (20 * 8)))
        End If

        ' Frame Per Second

        If debug = True Then
            statusText.Text = "FPS: " & Format(gameEngine.fps, "000.0") & " CLOCK: " & Format(gameEngine.clock / 60, "000000") & "." & Format(gameEngine.clock Mod 60, "00") & " DOTS: " & Format(maze.dotEaten, "000") & " / " & Format(maze.dotCount, "000")
        Else
            statusText.Text = "FPS: " & Format(gameEngine.fps, "000.0")
        End If

    End Sub

    Function initializeGame()

        ' Set Form to use double buffering
        ' This prevents flickering as it uses two surfaces and flicks between each

        Me.DoubleBuffered = True

        ' Form is not maximized
        Me.MaximizeBox = False

        ' Set border style to fixed
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle

        ' Set the client width and height
        Me.ClientSize = New Size((CLIENT_WIDTH) * CLIENT_SCALE, CLIENT_HEIGHT * CLIENT_SCALE + MenuStrip2.Height + StatusStrip1.Height)

        ' Centre Form to screen
        Me.CenterToScreen()

        ' We keep the ghost starting states in an array
        ' Initialize those arrays

        With ghosts(0)
            .name = "blinky"
            .cornerTile = New Point(25, -3)
            .startPixel = New Point((14 * 8) - 1, (11 * 8) + 4)
            .startDirection = actor.actorDirection.Left
            .startMode = actor.ghostMode.ghostOutside
            .arriveHomeMode = actor.ghostMode.ghostLeavingHome
        End With

        With ghosts(1)
            .name = "pinky"
            .cornerTile = New Point(2, -3)
            .startPixel = New Point((14 * 8) - 1, (14 * 8) + 4)
            .startDirection = actor.actorDirection.Down
            .startMode = actor.ghostMode.ghostPacingHome
            .arriveHomeMode = actor.ghostMode.ghostPacingHome
        End With

        With ghosts(2)
            .name = "inky"
            .cornerTile = New Point(27, 31)
            .startPixel = New Point((12 * 8) - 1, (14 * 8) + 4)
            .startDirection = actor.actorDirection.Up
            .startMode = actor.ghostMode.ghostPacingHome
            .arriveHomeMode = actor.ghostMode.ghostPacingHome
        End With

        With ghosts(3)
            .name = "clyde"
            .cornerTile = New Point(0, 31)
            .startPixel = New Point((16 * 8) - 1, (14 * 8) + 4)
            .startDirection = actor.actorDirection.Up
            .startMode = actor.ghostMode.ghostPacingHome
            .arriveHomeMode = actor.ghostMode.ghostPacingHome
        End With

        ' We keep pacmans starting state in an array
        ' Initialize those arrays

        With pacman(0)
            .name = "pacman"
            .startPixel = New Point((14 * 8), (23 * 8) + 4)
            .startDirection = actor.actorDirection.Left
        End With

        ' --------------------------------------------------------------------------------------------------
        ' Create Actors
        ' --------------------------------------------------------------------------------------------------

        ' Create a ghost actor with starting positions from the ghost arrays

        For n = 0 To ghosts.Count - 1
            actor.addGhost(ghosts(n).name, ghosts(n).startPixel, ghosts(n).cornerTile, ghosts(n).startDirection, ghosts(n).startMode, ghosts(n).arriveHomeMode)
        Next

        ' Create a pacman actor with starting positions from the pacman arrays

        For n = 0 To pacman.Count - 1
            actor.addPacman(pacman(n).name, pacman(n).startPixel, pacman(n).startDirection)
        Next

        ' Create a fruit actor
        actor.addFruit("fruit", New Point((13 * 8), (17 * 8)))

        ' --------------------------------------------------------------------------------------------------
        ' Initialize GameEngine
        ' --------------------------------------------------------------------------------------------------

        ' Create a new gameEngine instance, pointing it to form
        ' The gameEngine initializes a surface to render to, so we need to define its size
        gameEngine = New gameEngine(Me, CLIENT_WIDTH, CLIENT_HEIGHT, CLIENT_SCALE, MenuStrip2.Height)

        ' Add a new tile set that we will use to render the maze
        gameEngine.addTile("defaultMaze", New Size(8, 8))

        ' Add a new tile set that we will use to display the status information
        gameEngine.addTile("status", New Size(16, 16))

        ' Add a new map called "main" and initialize it to 32 x 40 tiles, and set it to use our "maze" tile map
        gameEngine.addMap("main", gameEngine.tileIndexByName("defaultMaze"), New Size(32, 34))

        ' Add a new map called "status" and initialize it to 16 x 1 tiles, and set it to use our "status" tile map
        gameEngine.addMap("status", gameEngine.tileIndexByName("status"), New Size(16, 1))

        ' Initialize a blank status area (map "status") and enable this map
        gameEngine.mapByName("status").point = New Point(0, (34 * 8))
        For n = 0 To 15
            gameEngine.mapByName("status").value(New Point(n, 0)) = 0
        Next
        gameEngine.mapByName("status").enabled = True

        ' Add ghost sprites to gameEngine
        For n = 0 To ghosts.Count - 1
            With ghosts(n)

                ' Add a new ghost sprite and initialize it
                gameEngine.addSprite(.name, New Size(16, 16))
                With gameEngine.spriteByName(.name)
                    .point = actor.ghostByName(.name).pixel
                    .animateMode = gameEngine.sprite.geAnimationMode.geForward
                    .animateOnFrame = 7
                    .animationRange = New gameEngine.sprite.geAnimationRange(4, 5)
                    .enabled = True
                End With

                ' Add a ghost debug sprite and initialize it
                gameEngine.addSprite(.name & "debug", New Size(16, 16))
                If debug = True Then
                    gameEngine.spriteByName(.name & "debug").enabled = True
                Else
                    gameEngine.spriteByName(.name & "debug").enabled = False
                End If

                ' Add a ghost score sprite and initialize it
                gameEngine.addSprite(.name & "score", New Size(16, 16))
                gameEngine.spriteByName(.name & "score").point = New Point(50 + (n * 20), 50)
                gameEngine.spriteByName(.name & "score").enabled = True

            End With
        Next

        ' Add pacman sprites to gameEngine
        For n = 0 To pacman.Count - 1
            With pacman(n)
                gameEngine.addSprite(.name, New Size(16, 16))
                With gameEngine.spriteByName(.name)
                    .point = actor.pacmanByName(.name).pixel
                    .animateMode = gameEngine.sprite.geAnimationMode.geNone
                    .animateOnFrame = 5
                    .animationRange = New gameEngine.sprite.geAnimationRange(3, 5)
                    .enabled = True
                End With
            End With
        Next

        ' Add pacman logo sprite to the gameEngine
        gameEngine.addSprite("pacmanlogo2", New Size(132, 42))
        With gameEngine.spriteByName("pacmanlogo2")
            .point = New Point(46, 50)
            .animateMode = gameEngine.sprite.geAnimationMode.geNone
            .enabled = False
            .zindex = 2
        End With

        ' Add pacman border sprite to the gameEngine
        gameEngine.addSprite("pacmanborder2", New Size(132, 66))
        With gameEngine.spriteByName("pacmanborder2")
            .point = New Point(46, 180)
            .animateMode = gameEngine.sprite.geAnimationMode.geNone
            .enabled = False
            .zindex = 2
        End With

        ' Add fruit sprite to gameEngine
        gameEngine.addSprite("fruit", New Size(16, 16))
        With gameEngine.spriteByName("fruit")
            .point = New Point((13 * 8), (19 * 8) + 4)
            .animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
            .animateMode = gameEngine.sprite.geAnimationMode.geNone
            .enabled = False
            .zindex = 2
        End With

        ' Add fruit score sprite to gameEngine
        gameEngine.addSprite("fruitscore", New Size(16, 16))

        ' Add a new font to the gameEngine
        gameEngine.addFont("font", New Size(8, 8), 48)

        ' Load the default maze
        'maze.loadMaze("defaultMaze.pac")
        maze.loadMaze("pacmanMaze.pac")

        resetGame()

        ' Reset highscore
        'highScore = 100

        ' Load highscore
        Dim stream As StreamReader = New StreamReader("HighScore.txt")

        highScore = stream.ReadLine()
        stream.Close()

        ' Sett the gameEngine FPS, reset the clock and start the gameEngine process
        gameEngine.fps = FPS
        gameEngine.clock = 0

        gameEngine.startEngine()

        Return True

    End Function

    Public Sub resetGame()

        ' Reset level and score

        level = 1
        score = 0
        lives = 3

        ' Reset ghost actors
        actor.resetGhost()

        ' Reset pacman
        actor.resetPacman()

        ' Reset ghost releaser
        actor.ghostReleaser.newLevel(level)

        ' Reset fruit
        actor.resetFruit()

        ' Reset the maze
        maze.resetMaze()

        ' Copy the maze to the gameEngine class
        copyGameEngineMaze()

        ' Update the status bar
        updateStatus()

        ' Reset the gameEngine tick
        gameEngine.clock = 0

    End Sub

    Public Sub resetLevel()

        ' Reset ghost
        actor.resetGhost()

        ' Reset pacman
        actor.resetPacman()

        ' Reset ghost releaser
        actor.ghostReleaser.newLevel(level)

        ' Update the status bar
        updateStatus()

        ' Reset the gameEngine tick
        gameEngine.clock = 0

    End Sub

    Public Sub nextLevel()

        ' Reset ghost actors
        actor.resetGhost()

        ' Reset pacman
        actor.resetPacman()

        ' Reset ghost releaser
        actor.ghostReleaser.newLevel(level)

        ' Reset fruit
        actor.resetFruit()

        ' Reset the maze
        maze.resetMaze()

        ' Copy the maze to the gameEngine class
        copyGameEngineMaze()

        ' Update the status bar
        updateStatus()

        ' Reset the gameEngine tick
        gameEngine.clock = 0

    End Sub

    ' Updates the status bar at the bottom of the gameEngine screen
    ' This displays the number of lives (represented by pacman sprites), and the current
    ' level (represented by the last 7 fruit bonuses)

    Public Sub updateStatus()

        Dim FruitChar

        ' Process the list of last fruit bonuses. The list is passed as a string of numbers, where
        ' each number represents the fruit that needs rendering. A blank space indicates a blank tile.

        For l = 0 To 6
            FruitChar = Mid(actor.fruitByName("fruit").list, l + 1, 1)
            If FruitChar <> " " Then
                gameEngine.mapByName("status").value(New Point(l + 6, 0)) = Int(FruitChar + 1)
            End If
        Next

        ' Display up to four lives. If the player has more than four lives then only 4 are rendered.

        For l = 1 To 4
            If lives >= l Then
                gameEngine.mapByName("status").value(New Point(l, 0)) = 9
            Else
                gameEngine.mapByName("status").value(New Point(l, 0)) = 0
            End If
        Next

    End Sub

    Public Sub copyGameEngineMaze()

        ' Initialize the first three lines of the gameEngine map with blank tiles.
        ' This is the area that displays the score and high score.

        For y = 0 To 2
            For x = 0 To 30
                gameEngine.mapByName("main").value(New Point(x, y)) = maze.mazeObjects.blank
            Next
        Next

        ' Copy the actual maze data into the gameEngine map.

        For y = 3 To 33
            For x = 0 To 30
                gameEngine.mapByName("main").value(New Point(x, y)) = maze.data(New Point(x, y - 3))
            Next
        Next

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        ' End the gameEngine since the main form is closing

        gameEngine.endEngine()

    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        Select Case e.KeyCode
            Case Keys.O
                input.left = True
                input.right = False
            Case Keys.P
                input.right = True
                input.left = False
            Case Keys.Q
                input.up = True
                input.down = False
            Case Keys.A
                input.down = True
                input.up = False
            Case Keys.Space
                input.space = True
        End Select

    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp

        Select Case e.KeyCode

            Case Keys.O
                input.left = False
            Case Keys.P
                input.right = False
            Case Keys.Q
                input.up = False
            Case Keys.A
                input.down = False
            Case Keys.Space
                input.space = False
        End Select

    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click

        gameEngine.endEngine()
        gameEngine.fps = 5
        gameEngine.startEngine()

    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click

        gameEngine.endEngine()
        gameEngine.fps = 60
        gameEngine.startEngine()

    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click

        gameEngine.endEngine()
        gameEngine.fps = 200
        gameEngine.startEngine()

    End Sub

    Private Sub DebugToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DebugToolStripMenuItem1.Click

        If DebugToolStripMenuItem1.Checked = True Then
            debug = False
            DebugToolStripMenuItem1.Checked = False
            For n = 0 To ghosts.Count - 1
                gameEngine.spriteByName(ghosts(n).name & "debug").enabled = False
            Next
        Else
            debug = True
            DebugToolStripMenuItem1.Checked = True
            For n = 0 To 3
                gameEngine.spriteByName(ghosts(n).name & "debug").enabled = True
            Next
        End If

    End Sub

    Private Sub MapEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MapEditorToolStripMenuItem.Click

        MJ_MapEditor.Show()

    End Sub

    Private Sub LoadMazeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadMazeToolStripMenuItem.Click

        Dim fd As FileDialog
        Dim stream As FileStream
        Dim fileName As String = ""

        fd = New OpenFileDialog

        fd.Title = "Load Pacman Maze"
        fd.Filter = "Pacman Mazes (*.pac)|*.pac"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            stream = New FileStream(fd.FileName, FileMode.Open)

            fileName = fd.FileName

            stream.Close()
        End If
        maze.loadMaze(fileName)
        resetGame()

    End Sub

    Private Sub ControlsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ControlsToolStripMenuitem.Click

        MsgBox("Keyboard:" & vbNewLine & "W - Up" & vbNewLine & "A - Left" & vbNewLine & "S - Down" & vbNewLine & "D - Right", MsgBoxStyle.Information, "Controls")

    End Sub

    Private Sub ObjectivesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ObjectivesToolStripMenuItem.Click

        MsgBox("The aim of the game is to progress through as many levels as possible, gathering as many points as possible, before you lose all of your lives." & vbNewLine &
               "You will lose a life if you collide with a ghost. However, if you have activated an energizer (large dots), upon a collision with a ghost you will eat the ghost and gain points." & vbNewLine &
               "The number of points you gain from eating ghosts is multiplied if you eat multiple ghosts consecutively, within a short period of time." & vbNewLine &
               "You will also gain points for each dot that you eat." & vbNewLine &
               "Once all of the dots have been eaten you will progress onto the next level." & vbNewLine &
               "As you reach higher levels the ghosts will become faster, providing a more difficult game later on.", MsgBoxStyle.Information, "Objectives")

    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click

        gameEngine.endEngine()
        gameEngine.fps = 100
        gameEngine.startEngine()

    End Sub
End Class