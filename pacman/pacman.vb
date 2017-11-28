Imports System.Threading
Imports System.IO

Public Class pacman

    Public Const TILE_SIZE = 8
    Public Const SPRITE_SIZE = 16
    Public Const CLIENT_WIDTH = 224
    Public Const CLIENT_HEIGHT = 288
    Public Const CLIENT_SCALE = 2
    Public Const FPS = 60

    ' Create a new instance of the gameEngine and specify that we want events.
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
    Dim invincibile As Boolean = False
    Dim redrawStatus As Boolean = False
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

        Dim last_score As Integer

        last_score = score

        ' This is the geGameLogic event.
        ' This is triggered once per frame by the gameEngine.
        ' It is meant to handle game logic ONLY.

        Select Case state

            Case gameState.reset

                ' We are in reset mode
                ' This mode lasts a since frame and its sole purpose is to reset the game variables,
                ' display the pacman logo, border and turn the pacman sprite off.
                ' Although pacman is not displayed it is still active as an actor and the ghosts still
                ' target based on his starting position.

                resetGame()

                ' Turn on the Pac-Man Logo
                gameEngine.spriteByName("pacmanlogo2").enabled = True

                'Turn on the Border (a transparent window)
                gameEngine.spriteByName("pacmanborder2").enabled = True

                ' Disable the Pac-Man sprite
                gameEngine.spriteByName("pacman").enabled = False

                ' Don't move Pac-Man in any direction
                actor.pacmanByName("pacman").direction = actor.actorDirection.None

                ' Set the state to "Menu"
                state = gameState.menu

            Case gameState.menu

                ' We are in menu mode (attract mode)
                ' In this mode pacman is hidden and the ghosts wander the maze alternating
                ' between scatter and chase mode every 10 seconds

                ' Update the actors.
                ' Because Pac-Man is disabled, we only update the ghosts
                actor.update(maze, invincibile)

                ' If 1800 frames have passed (30 seconds), then reset the internal clock
                If gameEngine.clock > 1800 Then
                    gameEngine.clock = 0
                End If

                ' If 600 frames have passed (10 seconds), then the ghosts are in scatter mode.
                ' Otherwise, the ghosts are in chase mode.
                If gameEngine.clock < 600 Then
                    actor.state = WindowsApplication1.actor.ghostState.Scatter
                Else
                    actor.state = WindowsApplication1.actor.ghostState.Chase
                End If

                ' If the Spacebar has been pressed then the player wants to start the game.
                If input.space = True Then

                    ' Turn off the Pac-Man logo
                    gameEngine.spriteByName("pacmanlogo2").enabled = False

                    ' Turn off the border
                    gameEngine.spriteByName("pacmanborder2").enabled = False

                    ' Enable the Pac-Man sprite
                    gameEngine.spriteByName("pacman").enabled = True

                    ' Set Pac-Man's initial direction to left
                    actor.pacmanByName("pacman").direction = actor.actorDirection.Left

                    ' Reset the level
                    resetLevel()

                    ' Set the state to "getReady"
                    state = gameState.getReady
                End If

            Case gameState.getReady

                ' We are in getReady mode.
                ' In this mode the text "Get Ready!" is displayed for 120 frames (2 seconds)

                ' If 120 frames have passed (2 seconds) then we have displayed the 
                ' "Get Ready!" messages for long enough, so move the game state onwards.
                If gameEngine.clock > (60 * 2) Then

                    ' Set the state to "gameStarted"
                    state = gameState.gameStarted

                End If

            Case gameState.gameStarted

                ' We are in the gameStarted mode.
                ' In this mode the player is actually playing the game, so this handles the
                ' main game logic.

                ' Push input into actor Class
                ' The actor class will apply this to Pac-Man movement

                ' If the input resolves to up then set the Pac-Man actor's direction to up
                If input.up Then
                    actor.pacmanByName("pacman").nextDirection = actor.actorDirection.Up
                End If

                ' If the input resolves to down then set the Pac-Man actor's direction to down
                If input.down Then
                    actor.pacmanByName("pacman").nextDirection = actor.actorDirection.Down
                End If

                ' If the input resolves to left then set the Pac-Man actor's direction to left
                If input.left Then
                    actor.pacmanByName("pacman").nextDirection = actor.actorDirection.Left
                End If

                ' If the input resolves to right then set the Pac-Man actor's direction to right
                If input.right Then
                    actor.pacmanByName("pacman").nextDirection = actor.actorDirection.Right
                End If

                ' Update the actors
                actor.update(maze, invincibile)

                ' If Pacman has eaten a dot...
                If maze.data(actor.pacmanByName("pacman").tile) = maze.mazeObjects.dot Then

                    ' Increase the score by 10 points
                    score += 10

                    ' Update the maze, replacing the dot with a blank tile
                    maze.data(actor.pacmanByName("pacman").tile) = maze.mazeObjects.blank

                    ' Update the gameEngine map, replacing the dot with a blank tile
                    gameEngine.mapByName("main").value(Point.Add(actor.pacmanByName("pacman").tile, New Point(0, 3))) = maze.mazeObjects.blank

                    ' Inform the ghost releaser that a dot has been eaten
                    actor.ghostReleaser.dotEat()

                    ' When 70 or 170 dots have been eaten, we should update the fruit state
                    If maze.dotEaten = 70 Or maze.dotEaten = 170 Then

                        ' If the fruit is not already active we can active it
                        If actor.fruitByName("fruit").active = False Then

                            ' Fruit stays active for 540 frames (9 seconds)
                            actor.fruitByName("fruit").tick = (9 * 60)

                            ' Enable the fruit
                            actor.fruitByName("fruit").active = True
                        End If
                    End If

                    ' If the number of dots eaten equal the total maze dot count then
                    ' the maze is complete and we need to start the next level
                    If maze.dotEaten = maze.dotCount Then

                        ' Increment the level counter
                        level += 1

                        ' Perform next level actions
                        nextLevel()

                        ' Set the state to "getReady"
                        state = gameState.getReady
                    End If

                End If

                ' If Pacman has eaten an energizer...
                If maze.data(actor.pacmanByName("pacman").tile) = maze.mazeObjects.energizer Then

                    ' Increase the score by 50 points
                    score += 50

                    ' Inform the actor class that an energizer is active
                    actor.energize()

                    ' Update the maze, replacing the energizer with a blank tile
                    maze.data(actor.pacmanByName("pacman").tile) = maze.mazeObjects.blank

                    ' Update the gameEngine map, replacing the energizer with a blank tile
                    gameEngine.mapByName("main").value(Point.Add(actor.pacmanByName("pacman").tile, New Point(0, 3))) = maze.mazeObjects.blank

                    ' If the number of dots eaten equal the total maze dot count then
                    ' the maze is complete and we need to start the next level
                    If maze.dotEaten = maze.dotCount Then

                        ' Increment the level counter
                        level += 1

                        ' Perform next level actions
                        nextLevel()

                        ' Set the state to "getReady"
                        state = gameState.getReady
                    End If

                End If

                ' If Pacman has died...
                If actor.pacmanByName("pacman").died = True Then

                    ' Reset the game engine clock
                    gameEngine.clock = 0

                    ' Set the state to "playerDied"
                    state = gameState.playerDied

                End If

                ' If Pac-Man has eaten a fruit...
                If actor.fruitByName("fruit").eaten = True Then

                    ' Increase the score by whatever the fruit was worth
                    score += actor.fruitByName("fruit").points

                    ' Reset the fruit eaten state within the actor class
                    actor.fruitByName("fruit").eaten = False

                End If

                ' Set the ghost state based on the current gameEngine clock (current tick)
                actor.setGhostState(gameEngine.clock)

            Case gameState.playerDied

                ' We are in the playerDied mode.
                ' In this mode, game play stops and an animation of Pac-Man dying is played

                ' If the game engine clock exceeds 70 then the animation has completed...
                If gameEngine.clock > 70 Then

                    ' Decrement the lives counter
                    lives -= 1

                    ' If the lives are still greater than zero then we simply reset the level,
                    ' otherwise the game is over
                    If lives > 0 Then

                        ' Reset the current level
                        resetLevel()

                        ' Inform the releaser class that the level has restarted
                        actor.ghostReleaser.restartLevel()

                        ' Set the state to "getReady"
                        state = gameState.getReady

                    Else

                        ' Save the current highscore
                        ' Open the highscore stream file
                        Dim stream As StreamWriter = New StreamWriter("HighScore.txt")

                        ' Write the new highscore
                        stream.Write(highScore)

                        ' Close the highscore stream file
                        stream.Close()

                        ' Set the state to "Reset"
                        state = gameState.reset
                    End If

                End If

        End Select

        If score >= 10000 And last_score < 10000 Then
            redrawStatus = True
            lives += 1
        End If

    End Sub

    Public Sub gameEngine_geRenderScene() Handles gameEngine.geRenderScene

        Dim last_score As Integer

        last_score = score

        ' This is the geRenderScene event.
        ' This is triggered once per frame by the gameEngine.
        ' It is meant to handle rendering type logic.

        ' If the state is not "playerDied"...
        If state <> gameState.playerDied Then

            ' Iterate through the ghosts and update their positions
            For n = 0 To ghosts.Count - 1

                ' For each ghost actor...
                With actor.ghostByIndex(n)

                    gameEngine.spriteByName(ghosts(n).name).enabled = True

                    ' Update the ghost sprite based on whether it's scared...
                    ' If the ghost is scared and the ghost has either only just become scared, or is flashing...
                    If .scared And (.scaredChanged Or .flashingChanged) Then

                        ' If the ghost is flashing and only just changed to a flashing state then
                        ' set it's animation range to alternate between white and blue,
                        ' otherwise the ghost is simply blue
                        If .flashing And .flashingChanged = True Then
                            gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(8, 11)
                        Else
                            gameEngine.spriteByName(ghosts(n).name).animationRange = New gameEngine.sprite.geAnimationRange(8, 9)
                        End If
                    End If

                    ' Update the ghost sprite based on whether it's not scared...
                    ' If the ghost is not scared and the ghost has either changed direction or is just changing back from scared...
                    If Not .scared And (.directionChanged Or .scaredChanged) Then

                        ' If the ghost is going home or entering home...
                        If .mode = actor.ghostMode.ghostGoingHome Or .mode = actor.ghostMode.ghostEnteringHome Then

                            ' Change the animation based on the direction that the ghost is facing
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

                            ' If the ghost is not going home and not entering home...

                            ' Change the animation based on the direction that the ghost is facing
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

                    ' If eaten timer has started...
                    If .eatenTimer > 0 Then

                        ' If 3 seconds (180 frames) have passed since the ghost has been eaten then
                        ' update the score.
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

                    ' Reset the direction changed, scared changed and flashing changed flags as we
                    ' have processed them now
                    .directionChanged = False
                    .scaredChanged = False
                    .flashingChanged = False

                End With

                ' Calculate and set the ghost position within the game surface.
                ' This is done by offsetting the ghost by -7 in the x direction and +16 in the y direction.
                gameEngine.spriteByName(ghosts(n).name).point = Point.Add(actor.ghostByName(ghosts(n).name).pixel, New Size(-7, 16))

                ' If debugging is turned on then calculate the debug squares position.
                ' This is done by offsetting the debug square by -3 in the x direction and 20 in the y direction.
                If debug = True Then
                    gameEngine.spriteByName(ghosts(n).name & "debug").point = Point.Add(actor.ghostByName(ghosts(n).name).targetPixel, New Size(-7 + 4, 16 + 4))
                End If
            Next

            ' Iterate through the Pac-Man actors.
            ' Although the game is played with only a single Pac-Man, this logic supports
            ' multiple Pac-Man Sprites. Potentially, the game could be extended to have multiple
            ' players with seperate Pac-Man sprites, all playing simultaneously.

            For n = 0 To pacman.Count - 1

                ' For each Pac-Man actor...
                With actor.pacmanByIndex(n)

                    ' If the Pac-Man direction has changed...
                    If .directionChanged = True Then

                        ' If Pac-Man is no longer moving then stop the Pac-Man animation,
                        ' otherwise the animation mode is geBoth which means the animation plays
                        ' forwards to the last frame and then backwards to the first frame, then repeats.
                        If .direction = actor.actorDirection.None Then
                            gameEngine.spriteByName(pacman(n).name).animateMode = gameEngine.sprite.geAnimationMode.geNone
                        Else
                            gameEngine.spriteByName(pacman(n).name).animateMode = gameEngine.sprite.geAnimationMode.geBoth
                        End If

                        ' Update the animation based on the direction that Pac-Man is facing.
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

                        ' Reset the direction changed flag as we have processed it now
                        .directionChanged = False

                    End If
                End With

                ' Calculate and set the Pac-Man position within the game surface.
                ' This is done by offsetting Pac-Man by -7 in the x direction and +16 in the y direction.
                gameEngine.spriteByName(pacman(n).name).point = Point.Add(actor.pacmanByName(pacman(n).name).pixel, New Size(-7, 16))

            Next

            ' Set the fruit animation...
            With actor.fruitByName("fruit")

                ' If a fruit is active then set the fruit type and enable it...
                If .active And .tick > 0 Then
                    gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(.number, .number)
                    gameEngine.spriteByName("fruit").enabled = True
                Else

                    ' If a fruit is not active then check the eatenTick to determine whether we
                    ' should be displaying a fruit score or not...
                    If Not .active And .eatenTick > 0 Then

                        ' If we are displaying a fruit score then show the appropriate tile
                        Select Case .points
                            Case 100
                                gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(8, 8)
                            Case 300
                                gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(9, 9)
                            Case 500
                                gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(10, 10)
                            Case 700
                                gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(11, 11)
                            Case 1000
                                gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(12, 12)
                            Case 2000
                                gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(13, 13)
                            Case 3000
                                gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(14, 14)
                            Case 5000
                                gameEngine.spriteByName("fruit").animationRange = New gameEngine.sprite.geAnimationRange(15, 15)
                                gameEngine.spriteByName("fruit").enabled = True
                        End Select
                    Else
                        ' If the fruit is not active and we are not showing a fruit score then
                        ' disable the fruit
                        gameEngine.spriteByName("fruit").enabled = False
                    End If
                End If

            End With

            ' If the score is greater than the current highscore then update the highscore
            If score > highScore Then
                highScore = score
            End If

        Else

            ' The state is "playerDied"...

            ' If the gameEngine clock is zero...
            If gameEngine.clock = 0 Then

                ' Play Pac-Man animation forwards
                gameEngine.spriteByName(pacman(0).name).animateMode = gameEngine.sprite.geAnimationMode.geForward

                ' Play the death animation
                gameEngine.spriteByName(pacman(0).name).animationRange = New gameEngine.sprite.geAnimationRange(11, 22)

                ' Turn all the ghosts off
                For n = 0 To ghosts.Count - 1
                    gameEngine.spriteByName(ghosts(n).name).enabled = False
                Next
            End If

        End If

        ' The text at the top of the screen is not part of the map, it is rendered seperately.
        ' Render the "1UP" and "HIGHSCORE" text
        gameEngine.drawTextbyName("font", "1UP", New Point(24, 0))
        gameEngine.drawTextbyName("font", "HIGH SCORE", New Point(72, 0))

        ' Render the current score
        gameEngine.drawTextbyName("font", String.Format("{0, 6}", score.ToString("####00")), New Point((1 * TILE_SIZE), (1 * TILE_SIZE)))

        ' Render current highscore
        gameEngine.drawTextbyName("font", String.Format("{0, 6}", highScore.ToString("####00")), New Point((11 * TILE_SIZE), (1 * TILE_SIZE)))

        ' If the game state is "menu" then we should render the opening credits and instructions
        ' about how to start the game...
        If state = gameState.menu Then
            gameEngine.drawTextbyName("font", "PACMAN 2017", New Point((8 * TILE_SIZE) + 4, (24 * TILE_SIZE)))
            gameEngine.drawTextbyName("font", "BY MATT JONES", New Point((7 * TILE_SIZE) + 4, (25 * TILE_SIZE)))
            gameEngine.drawTextbyName("font", "PRESS SPACE", New Point((8 * TILE_SIZE) + 4, (27 * TILE_SIZE)))
            gameEngine.drawTextbyName("font", "TO START GAME", New Point((7 * TILE_SIZE) + 4, (28 * TILE_SIZE)))
        End If

        ' If the game state is "getReady" then we should render the "GET READY" text
        If state = gameState.getReady Then
            gameEngine.drawTextbyName("font", "GET READY", New Point((9 * TILE_SIZE) + 4, (20 * TILE_SIZE)))
        End If

        ' If debugging is enabled then we should show the current FPS and game engine clock,
        ' otherwise just display the current FPS
        If debug = True Then
            statusText.Text = "FPS: " & Format(gameEngine.fps, "000.0") & " CLOCK: " & Format(gameEngine.clock / 60, "000000") & "." & Format(gameEngine.clock Mod 60, "00") & " DOTS: " & Format(maze.dotEaten, "000") & " / " & Format(maze.dotCount, "000")
        Else
            statusText.Text = "FPS: " & Format(gameEngine.fps, "000.0")
        End If

        If score >= 10000 And last_score < 10000 Then
            redrawStatus = True
            lives += 1
        End If

        If redrawStatus = True Then
            updateStatus()
            redrawStatus = False
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

        ' Center Form to screen
        Me.CenterToScreen()

        ' We keep the ghost starting states in an array as this allows us to reset them quicker.
        ' Initialize those arrays...
        With ghosts(0)
            .name = "blinky"
            .cornerTile = New Point(25, -3)
            .startPixel = New Point((14 * TILE_SIZE) - 1, (11 * TILE_SIZE) + 4)
            .startDirection = actor.actorDirection.Left
            .startMode = actor.ghostMode.ghostOutside
            .arriveHomeMode = actor.ghostMode.ghostLeavingHome
        End With

        With ghosts(1)
            .name = "pinky"
            .cornerTile = New Point(2, -3)
            .startPixel = New Point((14 * TILE_SIZE) - 1, (14 * TILE_SIZE) + 4)
            .startDirection = actor.actorDirection.Down
            .startMode = actor.ghostMode.ghostPacingHome
            .arriveHomeMode = actor.ghostMode.ghostPacingHome
        End With

        With ghosts(2)
            .name = "inky"
            .cornerTile = New Point(27, 31)
            .startPixel = New Point((12 * TILE_SIZE) - 1, (14 * TILE_SIZE) + 4)
            .startDirection = actor.actorDirection.Up
            .startMode = actor.ghostMode.ghostPacingHome
            .arriveHomeMode = actor.ghostMode.ghostPacingHome
        End With

        With ghosts(3)
            .name = "clyde"
            .cornerTile = New Point(0, 31)
            .startPixel = New Point((16 * TILE_SIZE) - 1, (14 * TILE_SIZE) + 4)
            .startDirection = actor.actorDirection.Up
            .startMode = actor.ghostMode.ghostPacingHome
            .arriveHomeMode = actor.ghostMode.ghostPacingHome
        End With

        ' We keep pacmans starting state in an array
        ' Initialize those arrays...
        With pacman(0)
            .name = "pacman"
            .startPixel = New Point((14 * TILE_SIZE), (23 * TILE_SIZE) + 4)
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
        actor.addFruit("fruit", New Point((13 * TILE_SIZE), (17 * TILE_SIZE)))

        ' --------------------------------------------------------------------------------------------------
        ' Initialize GameEngine
        ' --------------------------------------------------------------------------------------------------

        ' Create a new gameEngine instance, pointing it to our main form
        ' The gameEngine initializes a surface to render to, so we need to define its size
        gameEngine = New gameEngine(Me, CLIENT_WIDTH, CLIENT_HEIGHT, CLIENT_SCALE, MenuStrip2.Height)

        ' Add a new tile set that we will use to render the maze
        gameEngine.addTile("defaultMaze", New Size(TILE_SIZE, TILE_SIZE))

        ' Add a new tile set that we will use to display the status information
        gameEngine.addTile("status", New Size(16, 16))

        ' Add a new map called "main" and initialize it to 32 x 40 tiles, and set it to use our "maze" tile map
        gameEngine.addMap("main", gameEngine.tileIndexByName("defaultMaze"), New Size(32, 34))

        ' Add a new map called "status" and initialize it to 16 x 1 tiles, and set it to use our "status" tile map
        gameEngine.addMap("status", gameEngine.tileIndexByName("status"), New Size(16, 1))

        ' Initialize a blank status area (map "status") and enable this map
        gameEngine.mapByName("status").point = New Point(0, (34 * TILE_SIZE))
        For n = 0 To 15
            gameEngine.mapByName("status").value(New Point(n, 0)) = 0
        Next
        gameEngine.mapByName("status").enabled = True

        ' Add the ghost sprites to gameEngine
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
            .point = New Point((13 * TILE_SIZE), (19 * TILE_SIZE) + 4)
            .animationRange = New gameEngine.sprite.geAnimationRange(0, 0)
            .animateMode = gameEngine.sprite.geAnimationMode.geNone
            .enabled = False
            .zindex = 2
        End With

        ' Add fruit score sprite to gameEngine
        gameEngine.addSprite("fruitscore", New Size(16, 16))

        ' Add a new font to the gameEngine
        gameEngine.addFont("font", New Size(TILE_SIZE, TILE_SIZE), 48)

        ' Load the the default maze
        maze.loadMaze("pacmanMaze.pac")

        ' Reset the game
        resetGame()

        ' Load the highscore
        Dim stream As StreamReader = New StreamReader("HighScore.txt")

        highScore = stream.ReadLine()
        stream.Close()

        ' Set the gameEngine FPS, reset the clock and (finally, phew!) start the gameEngine process
        gameEngine.fps = FPS
        gameEngine.clock = 0

        gameEngine.startEngine()

        Return True

    End Function

    Public Sub resetGame()

        ' This function resets the entire game.
        ' It us usually called when a new game is started

        ' Reset level, score and lives
        level = 1
        score = 0
        lives = 3

        ' Reset ghost actors
        actor.resetGhost()

        ' Reset pacman actor
        actor.resetPacman()

        ' Reset ghost releaser
        actor.ghostReleaser.newLevel(level)

        ' Reset fruit
        actor.resetFruit()

        ' Reset the maze
        maze.resetMaze()

        ' Copy the maze to the gameEngine class.
        ' We do this because the game engine keeps all of it's resources internally
        ' because it runs in a seperate thread. Accessing variables outside of this
        ' thread is a big no-no as it can cause protection faults, so we need to 
        ' provide a copy.
        copyGameEngineMaze()

        ' Update the status bar (fruits)
        updateStatus()

        ' Reset the gameEngine tick
        gameEngine.clock = 0

    End Sub

    Public Sub resetLevel()

        ' This function resets the level.
        ' It is usually called when Pac-Man dies but has more lives.

        ' Reset ghost actors
        actor.resetGhost()

        ' Reset pacman actor
        actor.resetPacman()

        ' Reset ghost releaser
        actor.ghostReleaser.newLevel(level)

        ' Update the status bar (fruits)
        updateStatus()

        ' Reset the gameEngine tick
        gameEngine.clock = 0

    End Sub

    Public Sub nextLevel()

        ' This function moves to the next level.
        ' It is usually called when Pac-Man has eaten all of the dots on a level.

        ' Reset ghost actors
        actor.resetGhost()

        ' Reset pacman actor
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

    Public Sub updateStatus()

        ' This function updates the status bar at the bottom of the gameEngine screen
        ' This displays the number of lives (represented by pacman sprites), and the current
        ' level (represented by the last 7 fruit bonuses)

        Dim FruitChar

        ' Process the list of last fruit bonuses. The list is passed as a string of numbers, where
        ' each number represents the fruit that needs rendering. A blank space indicates a blank tile.
        For l = 0 To 6

            ' Get a character from the fruit list
            FruitChar = Mid(actor.fruitByName("fruit").list, l + 1, 1)

            ' If the fruit character is not blank then render the fruit
            If FruitChar <> " " Then
                gameEngine.mapByName("status").value(New Point(l + 6, 0)) = Int(FruitChar + 1)
            Else
                gameEngine.mapByName("status").value(New Point(l + 6, 0)) = 0
            End If

        Next

        ' Display up to four lives. Each life is represented by a Pac-Man sprite.
        ' If the player has more than four lives then only 4 are rendered.
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

        ' Based on the keyCode pressed, we can pass data into the input class...
        Select Case e.KeyCode

            Case Keys.A, Keys.Left
                input.left = True
                input.right = False
            Case Keys.D, Keys.Right
                input.right = True
                input.left = False
            Case Keys.W, Keys.Up
                input.up = True
                input.down = False
            Case Keys.S, Keys.Down
                input.down = True
                input.up = False
            Case Keys.Space
                input.space = True
        End Select

    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp

        ' Based on the keyCode released, we can pass data into the input class...
        Select Case e.KeyCode

            Case Keys.A, Keys.Left
                input.left = False
            Case Keys.D, Keys.Right
                input.right = False
            Case Keys.W, Keys.Up
                input.up = False
            Case Keys.S, Keys.Down
                input.down = False
            Case Keys.Space
                input.space = False

        End Select

    End Sub

    Private Sub ToolStrip_FPS5_Click(sender As Object, e As EventArgs) Handles ToolStrip_FPS5.Click

        ' This function is called when the FPS is set to 5 from the tool strip menu.

        ' End the game engine
        gameEngine.endEngine()

        ' Set the FPS to 5
        gameEngine.fps = 5

        ' Restart the game engine
        gameEngine.startEngine()

        ToolStrip_FPS5.Checked = True
        ToolStrip_FPS50.Checked = False
        ToolStrip_FPS60.Checked = False
        ToolStrip_FPS200.Checked = False
        ToolStrip_FPS100.Checked = False

    End Sub

    Private Sub ToolStrip_FPS50_Click(sender As Object, e As EventArgs) Handles ToolStrip_FPS50.Click

        ' This function is called when the FPS is set to 200 from the tool strip menu.

        gameEngine.endEngine()
        gameEngine.fps = 50
        gameEngine.startEngine()

        ToolStrip_FPS5.Checked = False
        ToolStrip_FPS50.Checked = True
        ToolStrip_FPS60.Checked = False
        ToolStrip_FPS200.Checked = False
        ToolStrip_FPS100.Checked = False

    End Sub

    Private Sub ToolStrip_FPS60_Click(sender As Object, e As EventArgs) Handles ToolStrip_FPS60.Click

        ' This function is called when the FPS is set to 60 from the tool strip menu.
        gameEngine.endEngine()
        gameEngine.fps = 60
        gameEngine.startEngine()

        ToolStrip_FPS5.Checked = False
        ToolStrip_FPS50.Checked = False
        ToolStrip_FPS60.Checked = True
        ToolStrip_FPS200.Checked = False
        ToolStrip_FPS100.Checked = False

    End Sub

    Private Sub ToolStrip_FPS100_Click(sender As Object, e As EventArgs) Handles ToolStrip_FPS100.Click

        ' This function is called when the FPS is set to 100 from the tool strip menu.

        gameEngine.endEngine()
        gameEngine.fps = 100
        gameEngine.startEngine()

        ToolStrip_FPS5.Checked = False
        ToolStrip_FPS50.Checked = False
        ToolStrip_FPS60.Checked = False
        ToolStrip_FPS100.Checked = True
        ToolStrip_FPS200.Checked = False

    End Sub

    Private Sub ToolStrip_FPS200_Click(sender As Object, e As EventArgs) Handles ToolStrip_FPS200.Click

        ' This function is called when the FPS is set to 200 from the tool strip menu.

        gameEngine.endEngine()
        gameEngine.fps = 200
        gameEngine.startEngine()

        ToolStrip_FPS5.Checked = False
        ToolStrip_FPS50.Checked = False
        ToolStrip_FPS60.Checked = False
        ToolStrip_FPS100.Checked = False
        ToolStrip_FPS200.Checked = True

    End Sub

    Private Sub ToolStrip_Debug_Click(sender As Object, e As EventArgs) Handles ToolStrip_Debug.Click

        ' This function is called when the debugging is enable or disabled in the tool strip menu.

        If ToolStrip_Debug.Checked = True Then
            debug = False
            ToolStrip_Debug.Checked = False
            For n = 0 To ghosts.Count - 1
                gameEngine.spriteByName(ghosts(n).name & "debug").enabled = False
            Next
        Else
            debug = True
            ToolStrip_Debug.Checked = True
            For n = 0 To 3
                gameEngine.spriteByName(ghosts(n).name & "debug").enabled = True
            Next
        End If

    End Sub

    Private Sub ToolStrip_Instructions_Click(sender As Object, e As EventArgs) Handles ToolStrip_Instructions.Click

        gameEngine.endEngine()
        Instructions.ShowDialog()
        gameEngine.startEngine()

    End Sub

    Private Sub ToolStrip_LoadMaze_Click(sender As Object, e As EventArgs) Handles ToolStrip_LoadMaze.Click

        Dim fd As FileDialog
        Dim fileName As String = ""
        Dim ans As MsgBoxResult

        gameEngine.endEngine()

        fd = New OpenFileDialog

        fd.Title = "Load Pacman Maze"
        fd.Filter = "Pacman Mazes (*.pac)|*.pac"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        If state = gameState.gameStarted Or state = gameState.getReady Or state = gameState.playerDied Then
            ans = MsgBox("Loading a maze will end the current game." & Chr(13) & "Are you sure that you want to continue?", MsgBoxStyle.YesNo, "Load Maze")
        Else
            ans = vbYes
        End If

        If ans = vbYes Then

            If fd.ShowDialog() = DialogResult.OK Then

                fileName = fd.FileName

                maze.loadMaze(fileName)

                state = gameState.reset

            End If

        End If

        gameEngine.startEngine()

    End Sub

    Private Sub ToolStrip_MapEditor_Click(sender As Object, e As EventArgs) Handles ToolStrip_MapEditor.Click

        MapEditor.Show()

    End Sub

    Private Sub ToolStrip_NewGame_Click(sender As Object, e As EventArgs) Handles ToolStrip_NewGame.Click

        gameEngine.endEngine()
        If MsgBox("Are you sure that you want to start a new game?", MsgBoxStyle.YesNo, "New Game") = MsgBoxResult.Yes Then
            state = gameState.reset
        End If
        gameEngine.startEngine()

    End Sub

    Private Sub ToolStrip_ResetHighscore_Click(sender As Object, e As EventArgs) Handles ToolStrip_ResetHighscore.Click

        If MsgBox("Are you sure that you want to reset the highscore?", MsgBoxStyle.YesNo, "Reset Highscore") = MsgBoxResult.Yes Then

            ' Save the current highscore
            ' Open the highscore stream file
            Dim stream As StreamWriter = New StreamWriter("HighScore.txt")

            ' Write the new highscore
            stream.Write(0)

            ' Close the highscore stream file
            stream.Close()

            ' Reset highscore
            highScore = 0

        End If

    End Sub

    Private Sub ToolStrip_EnableInvincibility_Click(sender As Object, e As EventArgs) Handles ToolStrip_EnableInvincibility.Click

        If ToolStrip_EnableInvincibility.Checked = False Then
            ToolStrip_EnableInvincibility.Checked = True
            invincibile = True
        Else
            ToolStrip_EnableInvincibility.Checked = False
            invincibile = False
        End If

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Me.Close()

    End Sub
End Class