

Public Class actor

    Public Enum actorDirection As Integer
        None = 0
        Up = 1
        Down = 2
        Left = 3
        Right = 4
    End Enum

    Public Enum ghostMode As Integer
        ghostOutside = 0
        ghostEaten = 1
        ghostGoingHome = 2
        ghostEnteringHome = 3
        ghostPacingHome = 4
        ghostLeavingHome = 5
        ghostStart = 6
    End Enum

    Public Enum ghostState As Integer
        Scatter = 0
        Chase = 1
    End Enum

    Public Enum releaserMode
        modePersonal = 0
        modeGlobal = 1
    End Enum

    Private Shared _ghost As New List(Of ghost)
    Private Shared _pacman As New List(Of pacman)
    Private Shared _fruit As New List(Of fruit)

    Private Shared _ghostState As ghostState = ghostState.Chase

    Public ghostReleaser As New releaser

    Private Structure stepSizeData
        Public pacmanNormal As String
        Public ghostsNormal As String
        Public pacmanFright As String
        Public ghostsFright As String
        Public ghostsTunnel As String
        Public ghostsPacing As String
        Public elroy1 As String
        Public elroy2 As String
    End Structure

    Private Shared _stepSize As stepSizeData
    Private Shared _stepCounter As Integer
    Private Shared _level As Integer

    Private Shared _energizeTime() = {6, 5, 4, 3, 2, 5, 2, 2, 1, 5, 2, 1, 1, 3, 1, 1, 0, 1}
    Private Shared _energizeFlashTime() = {5, 5, 5, 5, 5, 5, 5, 5, 3, 5, 5, 3, 3, 5, 3, 3, 0, 3}

    Private _energizedTimer As Integer
    Private _energizedFlashTimer As Integer
    Private _energizedScore As Integer

    Public Sub New()

    End Sub


    ' =============================================================================================================================
    '
    ' fruit Class
    '
    ' =============================================================================================================================

    Public Class fruit

        Private _name As String
        Private _pixel As Point
        Private _number As Integer
        Private _active As Integer
        Private _tick As Integer
        Private _points As Integer
        Private _eaten As Boolean
        Private _list As String

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.name(as string)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property name As String
            Get
                name = _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.pixel(as string)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property pixel As Point
            Get
                pixel = _pixel
            End Get
            Set(value As Point)
                _pixel = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.tile() as Point
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property tile As Point
            Get

                tile.X = Int(_pixel.X / 8)
                tile.Y = Int(_pixel.Y / 8)

            End Get
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.number(n as number) 
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property number As Integer
            Get
                number = _number
            End Get
            Set(value As Integer)
                If value < 8 Then
                    _number = value
                End If
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.active() as boolean
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property active As Boolean
            Get
                active = _active
            End Get
            Set(value As Boolean)
                _active = value
                If _active = False Then
                    _tick = 0
                End If
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.tick() as integer
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property tick As Integer
            Get
                tick = _tick
            End Get
            Set(value As Integer)
                _tick = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.points() as integer
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property points As Integer
            Get
                points = _points
            End Get
            Set(value As Integer)
                _points = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.eaten() as boolean
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property eaten As Boolean
            Get
                eaten = _eaten
            End Get
            Set(value As Boolean)
                _eaten = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.fruit.list() as string
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property list As String
            Get
                list = _list
            End Get
            Set(value As String)
                _list = value
            End Set
        End Property

    End Class

    ' =============================================================================================================================
    '
    ' ghost Class
    '
    ' =============================================================================================================================

    Public Class ghost

        Private _name As String
        Private _mode As ghostMode
        Private _pixel As Point
        Private _direction As actorDirection
        Private _nextDirection As actorDirection
        Private _signalReverse As Boolean
        Private _signalLeaveHome As Boolean
        Private _scared As Boolean
        Private _flashing As Boolean
        Private _targetting As Boolean
        Private _targetTile As Point
        Private _startPixel As Point
        Private _cornerTile As Point
        Private _startDirection As actorDirection
        Private _startMode As ghostMode
        Private _arriveHomeMode As ghostMode
        Private _personalDotLimit As Integer
        Private _globalDotLimit As Integer
        Private _dotsCounter As Integer
        Private _directionChanged As Boolean
        Private _scaredChanged As Boolean
        Private _flashingChanged As Boolean
        Private _eatenPixel As Point
        Private _eatenTimer As Integer
        Private _eatenScore As Integer

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.name(as string)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property name As String
            Get
                name = _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.state(as ghostMode)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property mode As ghostMode
            Get
                mode = _mode
            End Get
            Set(value As ghostMode)
                _mode = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.pixel(as Point)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property pixel As Point
            Get
                pixel = _pixel
            End Get
            Set(value As Point)
                _pixel = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.direction(as actorDirection)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property direction As actorDirection
            Get
                direction = _direction
            End Get
            Set(value As actorDirection)
                If _direction <> value Then
                    _directionChanged = True
                End If
                _direction = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.nextDirection(as actorDirection)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property nextDirection As actorDirection
            Get
                nextDirection = _nextDirection
            End Get
            Set(value As actorDirection)
                _nextDirection = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.signalReverse(as boolean)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property signalReverse As Boolean
            Get
                signalReverse = _signalReverse
            End Get
            Set(value As Boolean)
                _signalReverse = value
                _directionChanged = True
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.signalLeaveHome(as boolean)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property signalLeaveHome As Boolean
            Get
                signalLeaveHome = _signalLeaveHome
            End Get
            Set(value As Boolean)
                _signalLeaveHome = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.scared(as boolean)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property scared As Boolean
            Get
                scared = _scared
            End Get
            Set(value As Boolean)
                If _scared <> value Then
                    _scaredChanged = True
                End If
                _scared = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.flashing(as boolean)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property flashing As Boolean
            Get
                flashing = _flashing
            End Get
            Set(value As Boolean)
                If _flashing <> value Then
                    _flashingChanged = True
                End If
                _flashing = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.targetting(as boolean)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property targetting As Boolean
            Get
                targetting = _targetting
            End Get
            Set(value As Boolean)
                _targetting = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.targetTile(as Point)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property targetTile As Point
            Get
                targetTile = _targetTile
            End Get
            Set(value As Point)
                _targetTile = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.targetPixel(as Point)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property targetPixel As Point
            Get
                targetPixel = _targetTile
                targetPixel.X = targetPixel.X * 8
                targetPixel.Y = targetPixel.Y * 8
            End Get
            Set(value As Point)
                _targetTile.X = Int(targetPixel.X / 8)
                _targetTile.Y = Int(targetPixel.Y / 8)
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.startPixel(as Point)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property startPixel As Point
            Get
                startPixel = _startPixel
            End Get
            Set(value As Point)
                _startPixel = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.cornerTile(as Point)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property cornerTile As Point
            Get
                cornerTile = _cornerTile
            End Get
            Set(value As Point)
                _cornerTile = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.startDirection as actorDirection
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property startDirection As actorDirection
            Get
                startDirection = _startDirection
            End Get
            Set(value As actorDirection)
                _startDirection = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.startMode as ghostMode
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property startMode As ghostMode
            Get
                startMode = _startMode
            End Get
            Set(value As ghostMode)
                _startMode = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.arriveHomeMode as ghostMode
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property arriveHomeMode As ghostMode
            Get
                arriveHomeMode = _arriveHomeMode
            End Get
            Set(value As ghostMode)
                _arriveHomeMode = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.personalDotLimit as integer
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property personalDotLimit As Integer
            Get
                personalDotLimit = _personalDotLimit
            End Get
            Set(value As Integer)
                _personalDotLimit = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.globalDotLimit as integer
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property globalDotLimit As Integer
            Get
                globalDotLimit = _globalDotLimit
            End Get
            Set(value As Integer)
                _globalDotLimit = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.globalCounter as integer
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property dotsCounter As Integer
            Get
                dotsCounter = _dotsCounter
            End Get
            Set(value As Integer)
                _dotsCounter = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.directionChanged as boolean
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property directionChanged As Integer
            Get
                directionChanged = _directionChanged
            End Get
            Set(value As Integer)
                _directionChanged = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.directionChanged as boolean
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property scaredChanged As Integer
            Get
                scaredChanged = _scaredChanged
            End Get
            Set(value As Integer)
                _scaredChanged = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.eatenPixel as point
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property eatenPixel As Point
            Get
                eatenPixel = _eatenPixel
            End Get
            Set(value As Point)
                _eatenPixel = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.eatenTimer as integer
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property eatenTimer As Integer
            Get
                eatenTimer = _eatenTimer
            End Get
            Set(value As Integer)
                _eatenTimer = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.eatenScore as integer
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property eatenScore As Integer
            Get
                eatenScore = _eatenScore
            End Get
            Set(value As Integer)
                _eatenScore = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.directionChanged as boolean
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property flashingChanged As Integer
            Get
                flashingChanged = _flashingChanged
            End Get
            Set(value As Integer)
                _flashingChanged = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.atTileCenter(as boolean)
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property atTileCenter As Boolean
            Get
                atTileCenter = False
                If _pixel.X Mod 8 = 3 And _pixel.Y Mod 8 = 4 Then
                    atTileCenter = True
                End If
            End Get
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.justPassedCenter(as boolean)
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property justPassedCenter As Boolean
            Get
                justPassedCenter = False
                If (_direction = actorDirection.Right And pixel.X Mod 8 = 4) Or
                    (_direction = actorDirection.Left And pixel.X Mod 8 = 2) Or
                    (_direction = actorDirection.Up And pixel.Y Mod 8 = 3) Or
                    (_direction = actorDirection.Down And pixel.Y Mod 8 = 5) Then
                    justPassedCenter = True
                End If

            End Get
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.reverseDirection(as boolean)
        ' -----------------------------------------------------------------------------------------------------------------------------

        WriteOnly Property reverseDirection As Boolean
            Set(value As Boolean)
                If value = True Then

                    Select Case _direction
                        Case actorDirection.Up
                            _nextDirection = actorDirection.Down
                        Case actorDirection.Down
                            _nextDirection = actorDirection.Up
                        Case actorDirection.Left
                            _nextDirection = actorDirection.Right
                        Case actorDirection.Right
                            _nextDirection = actorDirection.Left
                    End Select

                End If
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.tile() as Point
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property tile As Point
            Get

                tile.X = Int(_pixel.X / 8)
                tile.Y = Int(_pixel.Y / 8)

            End Get
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.nextTile() as Point
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property nextTile As Point
            Get

                Dim tile As Point

                tile.X = Int(_pixel.X / 8)
                tile.Y = Int(_pixel.Y / 8)

                Select Case _direction
                    Case actorDirection.Left
                        tile.X -= 1
                    Case actorDirection.Right
                        tile.X += 1
                    Case actorDirection.Up
                        tile.Y -= 1
                    Case actorDirection.Down
                        tile.Y += 1
                End Select

                nextTile = tile

            End Get
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.ghost.randomDirection() as actorDirection
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property randomDirection As actorDirection
            Get

                Dim rndDirection As Integer
                randomDirection = actorDirection.None

                Randomize()
                rndDirection = CInt(Math.Ceiling(Rnd() * 4))

                Select Case rndDirection
                    Case 0
                        randomDirection = actorDirection.Up
                    Case 1
                        randomDirection = actorDirection.Down
                    Case 2
                        randomDirection = actorDirection.Left
                    Case 3
                        randomDirection = actorDirection.Right
                End Select

            End Get
        End Property

    End Class

    ' =============================================================================================================================
    '
    ' Pacman Class
    '
    ' =============================================================================================================================

    Public Class pacman

        Private _name As String
        Private _pixel As Point
        Private _startPixel As Point
        Private _direction As actorDirection
        Private _nextDirection As actorDirection
        Private _facing As actorDirection
        Private _directionChanged As Boolean
        Private _died As Boolean

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.name(as string)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property name As String
            Get
                name = _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.pixel(as Point)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property pixel As Point
            Get
                pixel = _pixel
            End Get
            Set(value As Point)
                _pixel = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.startPixel(as Point)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property startPixel As Point
            Get
                startPixel = _startPixel
            End Get
            Set(value As Point)
                _startPixel = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.direction(as actorDirection)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property direction As actorDirection
            Get
                direction = _direction
            End Get
            Set(value As actorDirection)
                If _direction <> value Then
                    _directionChanged = True
                End If
                _direction = value
                If _direction <> actorDirection.None Then
                    _facing = _direction
                End If
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.nextDirection(as actorDirection)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property nextDirection As actorDirection
            Get
                nextDirection = _nextDirection
            End Get
            Set(value As actorDirection)
                _nextDirection = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.facing(as actorDirection)
        ' -----------------------------------------------------------------------------------------------------------------------------

        Public Property facing As actorDirection
            Get
                facing = _facing
            End Get
            Set(value As actorDirection)
                _facing = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.directionChanged as boolean
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property directionChanged As Integer
            Get
                directionChanged = _directionChanged
            End Get
            Set(value As Integer)
                _directionChanged = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.died as boolean
        ' -----------------------------------------------------------------------------------------------------------------------------

        Property died As Integer
            Get
                died = _died
            End Get
            Set(value As Integer)
                _died = value
            End Set
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.tile() as Point
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property tile As Point
            Get

                tile.X = Int(_pixel.X / 8)
                tile.Y = Int(_pixel.Y / 8)

            End Get
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.getNextTile() as Point
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property nextTile As Point
            Get

                Dim tile As Point

                tile.X = Int(_pixel.X / 8)
                tile.Y = Int(_pixel.Y / 8)

                Select Case _direction
                    Case actorDirection.Left
                        tile.X -= 1
                    Case actorDirection.Right
                        tile.X += 1
                    Case actorDirection.Up
                        tile.Y -= 1
                    Case actorDirection.Down
                        tile.Y += 1
                End Select

                nextTile = tile

            End Get
        End Property

        ' -----------------------------------------------------------------------------------------------------------------------------
        ' actor.pacman.getDistanceFromCenter() as Point
        ' -----------------------------------------------------------------------------------------------------------------------------

        ReadOnly Property distanceFromCenter As Point
            Get

                Dim p As Point
                p = New Point(0, 0)

                p.X = ((pixel.X Mod 8) - 3)
                p.Y = ((pixel.Y Mod 8) - 4)

                distanceFromCenter = p

            End Get
        End Property

    End Class

    ' =============================================================================================================================
    '
    ' actor Class
    '
    ' =============================================================================================================================

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.addGhost(n as String)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub addGhost(name As String, startPixel As Point, cornerTile As Point, startDirection As actor.actorDirection, startMode As actor.ghostMode, arriveHomeMode As actor.ghostMode)

        Dim gh As New ghost

        gh.name = name
        gh.mode = ghostMode.ghostPacingHome
        gh.pixel = startPixel
        gh.direction = startDirection
        gh.nextDirection = startDirection
        gh.signalReverse = False
        gh.signalLeaveHome = False
        gh.scared = False
        gh.flashing = False
        gh.targetting = False
        gh.startPixel = startPixel
        gh.cornerTile = cornerTile
        gh.startDirection = startDirection
        gh.startMode = startMode
        gh.arriveHomeMode = arriveHomeMode
        gh.personalDotLimit = 0
        gh.globalDotLimit = 0
        gh.dotsCounter = 0
        gh.eatenPixel = New Point(0, 0)
        gh.eatenTimer = 0

        _ghost.Add(gh)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.ghostByName(name as String) as ghost
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function ghostByName(name As String) As ghost

        Dim index
        index = _ghost.FindIndex((Function(f) f.name = name))
        Return _ghost(index)

    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.ghostByIndex(index as integer) as ghost
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function ghostByIndex(index As Integer) As ghost

        Return _ghost(index)

    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.ghostIndexByName(name as String) as integer
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function ghostIndexByName(name As String) As Integer

        Return _ghost.FindIndex((Function(f) f.name = name))

    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.setGhostState() as ghostState
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Property state As ghostState
        Get
            state = _ghostState
        End Get
        Set(value As ghostState)
            _ghostState = value
        End Set
    End Property

    ' =============================================================================================================================
    '
    '  Class
    '
    ' =============================================================================================================================

    Public Class releaser

        Private Enum mode
            modePersonal = 0
            modeGlobal = 1
        End Enum

        Private _mode As Integer
        Private _framesSinceLastDot As Integer
        Private _globalCount As Integer

        Public Sub newLevel(level As Integer)

            _level = level
            _mode = mode.modePersonal
            _framesSinceLastDot = 0

            For n = 0 To _ghost.Count - 1
                With _ghost(n)
                    Select Case .name
                        Case "blinky"
                            .personalDotLimit = 0
                            .globalDotLimit = 0
                        Case "pinky"
                            .personalDotLimit = 0
                            .globalDotLimit = 7
                        Case "inky"
                            If level = 1 Then
                                .personalDotLimit = 30
                            Else
                                .personalDotLimit = 0
                            End If
                            .globalDotLimit = 17
                        Case "clyde"
                            If level = 1 Then .personalDotLimit = 30
                            If level = 2 Then .personalDotLimit = 60
                            If level > 2 Then .personalDotLimit = 0
                            .globalDotLimit = 32
                        Case Else
                            .personalDotLimit = 0
                            .globalDotLimit = 0
                    End Select

                    .dotsCounter = 0

                End With
            Next

            ' Set step size for each actor & state
            ' The step sizes cover 16 frame of movement in total, and the current step is tracked with the StepCounter variable
            ' When the end of the step size list is reached, the StepCounter is reset to 1 to start at the beginning of the list

            ' A step size of 0 indicates that no steps are taken in this frame
            ' A step size of 1 indicates that 1 step is taken in this frame
            ' A step size of 2 indicates that 2 steps are taken in this frame

            Select Case level

                Case Is = 1

                    _stepSize.pacmanNormal = "1111111111111111"
                    _stepSize.ghostsNormal = "0111111111111111"
                    _stepSize.pacmanFright = "1111211111112111"
                    _stepSize.ghostsFright = "0110110101101101"
                    _stepSize.ghostsTunnel = "0101010101010101"
                    _stepSize.ghostsPacing = "0101010101010101"
                    _stepSize.elroy1 = "1111111111111111"
                    _stepSize.elroy2 = "1111111121111111"

                Case Is <= 4

                    _stepSize.pacmanNormal = "1111211111112111"
                    _stepSize.ghostsNormal = "1111111121111111"
                    _stepSize.pacmanFright = "1111211112111121"
                    _stepSize.ghostsFright = "0110110110110111"
                    _stepSize.ghostsTunnel = "0110101011010101"
                    _stepSize.ghostsPacing = "0101010101010101"
                    _stepSize.elroy1 = "1111211111112111"
                    _stepSize.elroy2 = "1111211112111121"

                Case Is <= 20

                    _stepSize.pacmanNormal = "1121112111211121"
                    _stepSize.ghostsNormal = "1111211112111121"
                    _stepSize.pacmanFright = "1121112111211121"
                    _stepSize.ghostsFright = "0111011101110111"
                    _stepSize.ghostsTunnel = "0110110101101101"
                    _stepSize.ghostsPacing = "0101010101010101"
                    _stepSize.elroy1 = "1121112111211121"
                    _stepSize.elroy2 = "1121121121121121"

                Case Is > 20

                    _stepSize.pacmanNormal = "1111211111112111"
                    _stepSize.ghostsNormal = "1111211112111121"
                    _stepSize.pacmanFright = "0000000000000000"
                    _stepSize.ghostsFright = "0000000000000000"
                    _stepSize.ghostsTunnel = "0110110101101101"
                    _stepSize.ghostsPacing = "0101010101010101"
                    _stepSize.elroy1 = "1121112111211121"
                    _stepSize.elroy2 = "1121121121121121"

            End Select

            _stepCounter = 1

        End Sub

        Public Sub restartLevel()

            _mode = mode.modeGlobal
            _framesSinceLastDot = 0
            _globalCount = 0

        End Sub

        Public Sub dotEat()

            _framesSinceLastDot = 0

            If _mode = mode.modeGlobal Then
                _globalCount += 1
            Else
                For n = 0 To _ghost.Count - 1
                    With _ghost(n)
                        If .mode = ghostMode.ghostPacingHome Then
                            .dotsCounter += 1
                            Exit For
                        End If
                    End With
                Next
            End If

        End Sub

        Public Sub update()

            Dim _timeoutLimit As Integer

            If _mode = mode.modePersonal Then

                For n = 0 To _ghost.Count - 1
                    With _ghost(n)
                        If .mode = ghostMode.ghostPacingHome Then
                            If .dotsCounter >= .personalDotLimit Then
                                .signalLeaveHome = True
                                Exit Sub
                            End If
                            Exit For
                        End If
                    End With
                Next

            Else

                If _mode = mode.modeGlobal Then

                    For n = 0 To _ghost.Count - 1
                        With _ghost(n)

                            Select Case .name

                                Case "pinky"
                                    If _globalCount > .globalDotLimit And .mode = ghostMode.ghostPacingHome Then
                                        .signalLeaveHome = True
                                        Exit Sub
                                    End If

                                Case "inky"
                                    If _globalCount > .globalDotLimit And .mode = ghostMode.ghostPacingHome Then
                                        .signalLeaveHome = True
                                        Exit Sub
                                    End If

                                Case "clyde"
                                    If _globalCount > .globalDotLimit And .mode = ghostMode.ghostPacingHome Then
                                        _globalCount = 0
                                        _mode = mode.modePersonal
                                        .signalLeaveHome = True
                                        Exit Sub
                                    End If

                            End Select

                        End With
                    Next

                End If
            End If

            If _level < 5 Then
                _timeoutLimit = 4 * 60
            Else
                _timeoutLimit = 3 * 60
            End If

            If _framesSinceLastDot > _timeoutLimit Then

                _framesSinceLastDot = 0
                For n = 0 To _ghost.Count - 1
                    With _ghost(n)
                        If .mode = ghostMode.ghostPacingHome Then
                            .signalLeaveHome = True
                            Exit For
                        End If
                    End With
                Next
            Else

                _framesSinceLastDot += 1

            End If

        End Sub

    End Class

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.addPacman(n as String)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub addPacman(n As String, position As Point, startDirection As actor.actorDirection)

        Dim pm As New pacman

        pm.name = n
        pm.pixel = position
        pm.startPixel = position
        pm.direction = startDirection
        pm.nextDirection = actorDirection.None
        pm.facing = actorDirection.Left
        pm.directionChanged = False
        pm.died = False

        _pacman.Add(pm)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.pacmanByName(name as String) as pacman
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function pacmanByName(name As String) As pacman

        Dim index
        index = _pacman.FindIndex((Function(f) f.name = name))
        Return _pacman(index)

    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.pacmanByIndex(index as integer) as pacman
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function pacmanByIndex(index As Integer) As pacman

        Return _pacman(index)

    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.getPacmanIndexByName(name as String) as integer
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function getPacmanIndexByName(name As String) As Integer

        Return _pacman.FindIndex((Function(f) f.name = name))

    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.resetPacman()
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub resetPacman()

        For n = 0 To _pacman.Count - 1
            With _pacman(n)

                .pixel = .startPixel
                .direction = actorDirection.Left
                .nextDirection = actorDirection.None
                .facing = actorDirection.Left
                .directionChanged = True
                .died = False

            End With
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.resetGhosts()
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub resetGhost()

        For n = 0 To _ghost.Count - 1
            With _ghost(n)

                .signalReverse = False
                .signalLeaveHome = False

                .mode = .startMode
                .scared = False
                .flashing = False

                .direction = .startDirection
                .pixel = .startPixel
                .targetting = False

                .directionChanged = True
                .scaredChanged = True
                .flashingChanged = True

            End With
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.setGhostState(timer as long)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Sub setGhostState(timer As Long)

        If _level = 1 Then

            Select Case timer
                Case Is < (7 * 60)
                    state = actor.ghostState.Scatter
                Case Is < (27 * 60)
                    state = actor.ghostState.Chase
                Case Is < (34 * 60)
                    state = actor.ghostState.Scatter
                Case Is < (54 * 60)
                    state = actor.ghostState.Chase
                Case Is < (59 * 60)
                    state = actor.ghostState.Scatter
                Case Is < (79 * 60)
                    state = actor.ghostState.Chase
                Case Is < (84 * 60)
                    state = actor.ghostState.Scatter
                Case Is >= (84 * 60)
                    state = actor.ghostState.Chase
            End Select

        Else
            If _level > 2 And _level < 5 Then

                Select Case timer
                    Case Is < (7 * 60)
                        state = actor.ghostState.Scatter
                    Case Is < (27 * 60)
                        state = actor.ghostState.Chase
                    Case Is < (34 * 60)
                        state = actor.ghostState.Scatter
                    Case Is < (54 * 60)
                        state = actor.ghostState.Chase
                    Case Is < (59 * 60)
                        state = actor.ghostState.Scatter
                    Case Is < (1092 * 60)
                        state = actor.ghostState.Chase
                    Case Is < (1092 * 60) + 1
                        state = actor.ghostState.Scatter
                    Case Is >= (1092 * 60) + 1
                        state = actor.ghostState.Chase
                End Select

            Else

                Select Case timer
                    Case Is < (5 * 60)
                        state = actor.ghostState.Scatter
                    Case Is < (25 * 60)
                        state = actor.ghostState.Chase
                    Case Is < (30 * 60)
                        state = actor.ghostState.Scatter
                    Case Is < (50 * 60)
                        state = actor.ghostState.Chase
                    Case Is < (55 * 60)
                        state = actor.ghostState.Scatter
                    Case Is < (1092 * 60)
                        state = actor.ghostState.Chase
                    Case Is < (1092 * 60) + 1
                        state = actor.ghostState.Scatter
                    Case Is >= (1092 * 60) + 1
                        state = actor.ghostState.Chase
                End Select

            End If
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.update(ByRef as maze)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub update(ByRef maze As maze)

        For f = 1 To 2
            updateGhost(maze, f)
            updatePacman(maze, f)
        Next

        ghostReleaser.update()

        updateFruit()

        If _energizedTimer > 0 Then
            If _energizedTimer = _energizedFlashTimer Then
                For n = 0 To _ghost.Count - 1
                    If _ghost(n).scared = True Then
                        _ghost(n).flashing = True
                    End If
                Next
            End If
            _energizedTimer -= 1
        Else
            For n = 0 To _ghost.Count - 1
                _ghost(n).scared = False
                _ghost(n).flashing = False
            Next
        End If

        _stepCounter += 1
        If _stepCounter > 16 Then _stepCounter = 1

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.updateGhost(ByRef as maze)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub updateGhost(ByRef maze As maze, f As Integer)

        Dim steps As Integer

        ' Loop through all ghost items

        For n = 0 To _ghost.Count - 1

            ' Using the current ghost item...

            With _ghost(n)

                If .eatenTimer > 0 Then
                    .eatenTimer -= 1
                End If

                If .mode = ghostMode.ghostEaten Then
                    .eatenPixel = .pixel
                    .eatenTimer = (60 * 3)
                    .mode = ghostMode.ghostGoingHome
                End If

                If .mode = ghostMode.ghostGoingHome Or .mode = ghostMode.ghostEnteringHome Then
                    steps = 2
                Else
                    If .mode = ghostMode.ghostLeavingHome Or .mode = ghostMode.ghostPacingHome Then
                        steps = Int(Mid(_stepSize.ghostsPacing, _stepCounter, 1))
                    Else
                        If .tile.Y = 14 And (.pixel.X < 40 Or .pixel.X > 184) Then
                            steps = Int(Mid(_stepSize.ghostsTunnel, _stepCounter, 1))
                        Else
                            If .scared Then
                                steps = Int(Mid(_stepSize.ghostsFright, _stepCounter, 1))
                            Else
                                ' ELROY TODO
                                steps = Int(Mid(_stepSize.ghostsNormal, _stepCounter, 1))
                            End If
                        End If
                    End If
                End If

                If f <= steps Then

                    ' Perform all home logic

                    Select Case .mode
                        Case ghostMode.ghostGoingHome
                            If .tile = maze.homeDoorTile Then
                                .direction = actorDirection.Down
                                .targetting = False
                                If .pixel.X = maze.homeDoorPixel.X Then
                                    .mode = ghostMode.ghostEnteringHome
                                    .direction = actorDirection.Down
                                Else
                                    .direction = actorDirection.Right
                                End If
                            End If

                        Case ghostMode.ghostEnteringHome
                            If .pixel.Y = maze.homeBottomPixel Then
                                If .pixel.X = .startPixel.X Then
                                    .direction = actorDirection.Up
                                    .mode = .arriveHomeMode
                                Else
                                    If .startPixel.X < .pixel.X Then
                                        .direction = actorDirection.Left
                                    Else
                                        .direction = actorDirection.Right
                                    End If
                                End If
                            End If

                        Case ghostMode.ghostPacingHome
                            If .signalLeaveHome = True Then
                                .signalLeaveHome = False
                                .mode = ghostMode.ghostLeavingHome
                                If .pixel.X = maze.homeDoorPixel.X Then
                                    .direction = actorDirection.Up
                                Else
                                    If .pixel.X < maze.homeDoorPixel.X Then
                                        .direction = actorDirection.Right
                                    Else
                                        .direction = actorDirection.Left
                                    End If
                                End If
                            Else
                                If .pixel.Y = maze.homeTopPixel Then
                                    .direction = actorDirection.Down
                                Else
                                    If .pixel.Y = maze.homeBottomPixel Then
                                        .direction = actorDirection.Up
                                    End If
                                End If
                            End If

                        Case ghostMode.ghostLeavingHome
                            If .pixel.X = maze.homeDoorPixel.X Then
                                If .pixel.Y = maze.homeDoorPixel.Y Then
                                    .mode = ghostMode.ghostOutside
                                    Randomize()
                                    If CInt(Math.Ceiling(Rnd() * 2 - 1)) = 0 Then
                                        .direction = actorDirection.Left
                                        .nextDirection = actorDirection.Left
                                    Else
                                        .direction = actorDirection.Right
                                        .nextDirection = actorDirection.Right
                                    End If
                                Else
                                    .direction = actorDirection.Up
                                End If
                            End If

                    End Select

                    ' If we are not pursuing a target tile then exit the subroutine as we are done

                    If (.mode <> ghostMode.ghostOutside And .mode <> ghostMode.ghostGoingHome) Then
                        .targetting = False
                    Else

                        ' Are we are at the middle of a tile?

                        If .atTileCenter = True Then

                            ' If reversal has been triggered then do so

                            If .signalReverse = True Then
                                .reverseDirection = True
                                .signalReverse = False
                            End If

                            ' Commit the new direction

                            .direction = .nextDirection

                        Else

                            ' Have we just passed the mid-tile?

                            If .justPassedCenter = True Then

                                ' Get next tile

                                Dim tilePos As Point
                                tilePos = .nextTile

                                ' Get exits from next tile

                                'Dim ex As maze.exits
                                'ex = maze.getExits(tilePos)

                                Dim ex As List(Of maze.mazeExits)
                                ex = maze.getExits(tilePos)

                                ' With the standard Pac-man map there are always at least two exits from each tile,
                                ' however, a user created map may have a dead-end. For this reason, we must allow a
                                ' ghost to exit the way it came...but ONLY if there is a single exit.
                                ' Otherwise we prohibit a ghost choosing an exit that is in the opposite direction
                                ' of travel.

                                If ex.Count > 1 Then
                                    Select Case .direction
                                        Case actorDirection.Up
                                            For i = ex.Count - 1 To 0 Step -1
                                                If ex(i) = maze.mazeExits.exitDown Then
                                                    ex.RemoveAt(i)
                                                End If
                                            Next
                                            'ex.down = False
                                        Case actorDirection.Down
                                            For i = ex.Count - 1 To 0 Step -1
                                                If ex(i) = maze.mazeExits.exitUp Then
                                                    ex.RemoveAt(i)
                                                End If
                                            Next
                                            'ex.up = False
                                        Case actorDirection.Left
                                            For i = ex.Count - 1 To 0 Step -1
                                                If ex(i) = maze.mazeExits.exitRight Then
                                                    ex.RemoveAt(i)
                                                End If
                                            Next
                                            'ex.right = False
                                        Case actorDirection.Right
                                            For i = ex.Count - 1 To 0 Step -1
                                                If ex(i) = maze.mazeExits.exitLeft Then
                                                    ex.RemoveAt(i)
                                                End If
                                            Next
                                            'ex.left = False
                                    End Select
                                End If

                                ' If a ghost is scared then its moves are random;

                                If .scared = True Then

                                    ' Get Random Exit

                                    Randomize()

                                    Dim randomExit As Integer
                                    randomExit = CInt(Math.Ceiling(Rnd() * (ex.Count - 1)))
                                    .nextDirection = ex(randomExit)

                                Else

                                    If .mode = ghostMode.ghostGoingHome Then

                                        .targetTile = maze.homeDoorTile

                                    Else

                                        If state = ghostState.Scatter Then

                                            .targetTile = .cornerTile
                                            .targetting = True

                                        Else

                                            Select Case _ghost(n).name
                                                Case "blinky"
                                                    .targetTile = blinkyGetTargetTile()
                                                Case "pinky"
                                                    .targetTile = pinkyGetTargetTile()
                                                Case "inky"
                                                    .targetTile = inkyGetTargetTile()
                                                Case "clyde"
                                                    .targetTile = clydeGetTargetTile()
                                            End Select

                                        End If


                                    End If

                                    ' Find shortest path to target tile

                                    Dim dist As Point
                                    Dim distance As Long
                                    Dim distanceSelected As Long

                                    distanceSelected = 1000000
                                    For i = 0 To ex.Count - 1
                                        Select Case ex(i)
                                            Case maze.mazeExits.exitUp
                                                dist = Point.Subtract(Point.Add(.tile(), New Point(0, -1)), .targetTile)
                                            Case maze.mazeExits.exitDown
                                                dist = Point.Subtract(Point.Add(.tile(), New Point(0, 1)), .targetTile)
                                            Case maze.mazeExits.exitLeft
                                                dist = Point.Subtract(Point.Add(.tile(), New Point(-1, 0)), .targetTile)
                                            Case maze.mazeExits.exitRight
                                                dist = Point.Subtract(Point.Add(.tile(), New Point(1, 0)), .targetTile)
                                        End Select

                                        distance = (dist.X * dist.X) + (dist.Y * dist.Y)
                                        If distance < distanceSelected Then
                                            distanceSelected = distance
                                            .nextDirection = ex(i)
                                        End If

                                    Next

                                End If

                            End If

                        End If
                    End If

                    Select Case .direction
                        Case actorDirection.Up
                            .pixel = Point.Add(.pixel, New Size(0, -1))
                        Case actorDirection.Down
                            .pixel = Point.Add(.pixel, New Size(0, 1))
                        Case actorDirection.Left
                            .pixel = Point.Add(.pixel, New Size(-1, 0))
                        Case actorDirection.Right
                            .pixel = Point.Add(.pixel, New Size(1, 0))
                    End Select

                    ' Deal with tunnel

                    If .tile.Y = 14 Then
                        If .pixel.X < -16 Then
                            .pixel = New Point(239, .pixel.Y)
                        Else
                            If .pixel.X > 239 Then
                                .pixel = New Point(-16, .pixel.Y)
                            End If
                        End If
                    End If

                    For i = 0 To _pacman.Count - 1
                        If .tile = _pacman(i).tile Then
                            collision(i, n)
                        End If
                    Next

                End If

            End With

        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.updatePacman(ByRef as maze)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub updatePacman(ByRef maze As maze, f As Integer)

        Dim steps As Integer

        steps = Int(Mid(_stepSize.pacmanNormal, _stepCounter, 1))
        If f > steps Then Exit Sub

        ' Loop through all pacman items

        For n = 0 To _pacman.Count - 1

            ' Using the current ghost item...

            With _pacman(n)

                ' Get next tile

                Dim tilePos As Point
                tilePos = .tile

                Dim ex As List(Of maze.mazeExits)
                ex = maze.getExits(tilePos)

                Dim distFromCenter As Point
                distFromCenter = .distanceFromCenter

                Select Case .nextDirection

                    Case actorDirection.Left
                        If ex.IndexOf(actorDirection.Left) < 0 Then
                            If distFromCenter.X = 0 Then
                                .nextDirection = actorDirection.None
                            End If
                        End If

                    Case actorDirection.Right
                        If ex.IndexOf(actorDirection.Right) < 0 Then
                            If distFromCenter.X = 0 Then
                                .nextDirection = actorDirection.None
                            End If
                        End If

                    Case actorDirection.Up
                        If ex.IndexOf(actorDirection.Up) < 0 Then
                            If distFromCenter.Y = 0 Then
                                .nextDirection = actorDirection.None
                            End If
                        End If

                    Case actorDirection.Down
                        If ex.IndexOf(actorDirection.Down) < 0 Then
                            If distFromCenter.Y = 0 Then
                                .nextDirection = actorDirection.None
                            End If
                        End If

                End Select

                ' Stop moving if the exit is blocked in the direction of travel

                If .direction = actorDirection.Left Or .direction = actorDirection.Right Then
                    If ex.IndexOf(.direction) < 0 Then
                        If distFromCenter.X = 0 Then
                            .direction = actorDirection.None
                        End If
                    End If
                End If

                If .direction = actorDirection.Up Or .direction = actorDirection.Down Then
                    If ex.IndexOf(.direction) < 0 Then
                        If distFromCenter.Y = 0 Then
                            .direction = actorDirection.None
                        End If
                    End If
                End If

                ' Update direction if it has changed

                If .nextDirection <> actorDirection.None Then
                    .direction = .nextDirection
                End If

                Select Case .direction
                    Case actorDirection.Up
                        .pixel = Point.Add(.pixel, New Size(0, -1))
                        If distFromCenter.X < 0 Then
                            .pixel = Point.Add(.pixel, New Size(1, 0))
                        Else
                            If distFromCenter.X > 0 Then
                                .pixel = Point.Add(.pixel, New Size(-1, 0))
                            End If
                        End If
                    Case actorDirection.Down
                        .pixel = Point.Add(.pixel, New Size(0, 1))
                        If distFromCenter.X < 0 Then
                            .pixel = Point.Add(.pixel, New Size(1, 0))
                        Else
                            If distFromCenter.X > 0 Then
                                .pixel = Point.Add(.pixel, New Size(-1, 0))
                            End If
                        End If
                    Case actorDirection.Left
                        .pixel = Point.Add(.pixel, New Size(-1, 0))
                        If distFromCenter.Y < 0 Then
                            .pixel = Point.Add(.pixel, New Size(0, 1))
                        Else
                            If distFromCenter.Y > 0 Then
                                .pixel = Point.Add(.pixel, New Size(0, -1))
                            End If
                        End If
                    Case actorDirection.Right
                        .pixel = Point.Add(.pixel, New Size(1, 0))
                        If distFromCenter.Y < 0 Then
                            .pixel = Point.Add(.pixel, New Size(0, 1))
                        Else
                            If distFromCenter.Y > 0 Then
                                .pixel = Point.Add(.pixel, New Size(0, -1))
                            End If
                        End If
                End Select

                ' Deal with tunnel

                If .tile.Y = 14 Then
                    If .pixel.X < -16 Then
                        .pixel = New Point(239, .pixel.Y)
                    Else
                        If .pixel.X > 239 Then
                            .pixel = New Point(-16, .pixel.Y)
                        End If
                    End If
                End If

                For i = 0 To _ghost.Count - 1
                    If .tile = _ghost(i).tile Then
                        collision(n, i)
                    End If
                Next

                For i = 0 To _fruit.Count - 1
                    If _fruit(i).active Then
                        If .tile = _fruit(i).tile Then
                            _fruit(i).active = False
                            _fruit(i).eaten = True
                        End If
                    End If
                Next

            End With

        Next

    End Sub

    ' Blinky direction targets Pacman 

    Private Function blinkyGetTargetTile() As Point

        Dim index As Integer
        Dim pos As Point

        index = getPacmanIndexByName("pacman")

        If index <> -1 Then
            pos = _pacman(index).tile()
        Else
            pos = New Point(1, 1)
        End If

        Return pos

    End Function

    ' Pinky 

    Private Function pinkyGetTargetTile() As Point

        Dim index As Integer
        Dim pos As Point

        index = getPacmanIndexByName("pacman")

        If index <> -1 Then
            pos = _pacman(index).tile()

            Select Case _pacman(index).facing
                Case actorDirection.Left
                    pos = Point.Add(pos, New Size(-4, 0))
                Case actorDirection.Right
                    pos = Point.Add(pos, New Size(4, 0))
                Case actorDirection.Up
                    pos = Point.Add(pos, New Size(-4, -4))
                Case actorDirection.Down
                    pos = Point.Add(pos, New Size(0, 4))

            End Select
        Else
            pos = New Point(1, 1)
        End If

        Return pos

    End Function

    Private Function inkyGetTargetTile() As Point

        Dim index1 As Integer
        Dim index2 As Integer
        Dim pos1 As Point
        Dim pos2 As Point
        Dim pos3 As Point

        index1 = getPacmanIndexByName("pacman")
        index2 = ghostIndexByName("blinky")

        If index1 <> -1 And index2 <> -1 Then

            pos1 = _pacman(index1).tile()
            pos2 = _ghost(index2).tile()

            Select Case _pacman(index1).facing
                Case actorDirection.Left
                    pos1 = Point.Add(pos1, New Size(-2, 0))
                Case actorDirection.Right
                    pos1 = Point.Add(pos1, New Size(2, 0))
                Case actorDirection.Up
                    pos1 = Point.Add(pos1, New Size(-2, -2))
                Case actorDirection.Down
                    pos1 = Point.Add(pos1, New Size(0, 2))
            End Select

            pos3 = Point.Subtract(pos2, pos1)
            pos3 = Point.Add(pos3, pos3)
            pos1 = Point.Subtract(pos1, pos3)

        Else

            pos1 = New Point(1, 1)

        End If

        Return pos1

    End Function

    Private Function clydeGetTargetTile() As Point

        Dim index1 As Integer
        Dim index2 As Integer
        Dim pos1 As Point
        Dim pos2 As Point
        Dim pos3 As Point
        Dim dist As Long

        index1 = getPacmanIndexByName("pacman")
        index2 = ghostIndexByName("clyde")

        If index1 <> -1 And index2 <> -1 Then

            pos1 = _pacman(index1).tile()
            pos2 = _ghost(index2).tile()
            pos3 = Point.Subtract(pos1, pos2)

            dist = pos3.X * pos3.X + pos3.Y * pos3.Y
            If dist < 64 Then
                pos1 = _ghost(index2).cornerTile
            End If
        End If

        Return pos1

    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.collision(pacmanNumber as integer, ghostNumber as integer)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Private Sub collision(pacmanNumber As Integer, ghostNumber As Integer)

        If _energizedTimer > 0 Then
            If _ghost(ghostNumber).scared = True Then
                _ghost(ghostNumber).mode = ghostMode.ghostEaten
                _ghost(ghostNumber).scared = False
                _ghost(ghostNumber).flashing = False
                _ghost(ghostNumber).eatenScore = _energizedScore
                _energizedScore += 1
            Else
                If _ghost(ghostNumber).mode <> ghostMode.ghostEaten And _ghost(ghostNumber).mode <> ghostMode.ghostGoingHome Then
                    _pacman(pacmanNumber).died = True
                End If
            End If
        Else
            If _ghost(ghostNumber).mode <> ghostMode.ghostEaten And _ghost(ghostNumber).mode <> ghostMode.ghostGoingHome Then
                _pacman(pacmanNumber).died = True
            End If
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.energize()
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub energize()
        If _level < 18 Then
            _energizedFlashTimer = _energizeFlashTime(_level - 1) * 14 * 2
            _energizedTimer = (_energizeTime(_level - 1) * 60) + _energizedFlashTimer
        Else
            _energizedFlashTimer = _energizeFlashTime(17) * 14 * 2
            _energizedTimer = (_energizeTime(17) * 60) + _energizedFlashTimer
        End If
        For n = 0 To _ghost.Count - 1
            If _ghost(n).mode <> ghostMode.ghostGoingHome Then
                _ghost(n).scared = True
                _ghost(n).flashing = False
                _ghost(n).reverseDirection = True
            End If
        Next
        _energizedScore = 0
    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.addFruit(name as string, pixel as point)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub addFruit(name As String, position As Point)

        Dim fr As New fruit

        fr.name = name
        fr.pixel = position
        fr.number = 1
        fr.active = False
        fr.tick = 0
        fr.eaten = False
        fr.list = ""

        _fruit.Add(fr)

    End Sub

    Public Sub resetFruit()

        Dim _fruitSequence() = {0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7}
        Dim _fruitPoints() = {100, 300, 500, 700, 1000, 2000, 3000, 5000}

        For n = 0 To _fruit.Count - 1
            With _fruit(n)
                If _level < 14 Then
                    .number = _fruitSequence(_level - 1)
                Else
                    .number = _fruitSequence(13)
                End If
                .points = _fruitPoints(.number)

                .list = ""
                For l = 0 To 6
                    If _level - l - 1 >= 0 Then
                        If _level - l - 1 < 14 Then
                            .list = Trim(Str(_fruitSequence(_level - l - 1))) & .list
                        Else
                            .list = Trim(Str(_fruitSequence(13))) & .list
                        End If
                    Else
                        .list = " " & .list
                    End If
                Next

            End With
        Next

    End Sub

    Public Sub updateFruit()

        For n = 0 To _fruit.Count - 1
            With _fruit(n)
                If .active Then
                    If .tick > 0 Then
                        .tick -= 1
                    Else
                        .active = False
                    End If
                End If
            End With
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' actor.fruitByName(name as String) as fruit
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function fruitByName(name As String) As fruit

        Dim index
        index = _fruit.FindIndex((Function(f) f.name = name))
        Return _fruit(index)

    End Function

    Public Shared Event pacmanChanged(n As Integer)
    Public Shared Event ghostChanged(n As Integer)

End Class
