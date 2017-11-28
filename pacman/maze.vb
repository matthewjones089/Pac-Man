Imports System.IO

Public Class maze
    ' Declare variables to hold the required information of the ghosts home.
    Public homeDoorTile As Point = New Point(13, 11)
    Public homeDoorPixel As Point = New Point((14 * 8) - 1, (11 * 8) + 4)
    Public homeTopPixel As Integer = 14 * 8
    Public homeBottomPixel As Integer = 15 * 8

    ' Declare an enumerator to hold the possible directions of exits.
    Public Enum mazeExits As Integer
        exitNone = 0
        exitUp = 1
        exitDown = 2
        exitLeft = 3
        exitRight = 4
    End Enum

    ' Declare an enumerator to hold the possible maze objects.
    Public Enum mazeObjects As Integer
        blank = 35
        block = 45
        dot = 46
        energizer = 47
    End Enum

    ' Declare an enumerator to hold the possible types of path.
    Public Enum pathType
        blank = 0
        block = 1
        dot = 2
        energizer = 3
    End Enum

    ' Create a structure to hold data relating to the path, for example if a tile is fixed and its type.
    Public Structure pathData
        Dim fixed As Boolean
        Dim pathType As pathType
    End Structure

    ' Declare a 2-D array which hold the initial maze.
    Dim defaultMaze(,) As Integer = {
        {0, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 1},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {2, 9, 9, 9, 9, 17, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 16, 9, 9, 9, 9, 3},
        {35, 35, 35, 35, 35, 10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11, 35, 35, 35, 35, 35},
        {35, 35, 35, 35, 35, 10, 46, 46, 46, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 46, 46, 46, 11, 35, 35, 35, 35, 35},
        {35, 35, 35, 35, 35, 10, 46, 46, 46, 45, 24, 9, 33, 42, 42, 32, 9, 25, 45, 46, 46, 46, 11, 35, 35, 35, 35, 35},
        {8, 8, 8, 8, 8, 19, 46, 46, 46, 45, 11, 35, 35, 35, 35, 35, 35, 10, 45, 46, 46, 46, 18, 8, 8, 8, 8, 8},
        {45, 45, 45, 45, 45, 45, 46, 46, 46, 45, 11, 35, 35, 35, 35, 35, 35, 10, 45, 46, 46, 46, 45, 45, 45, 45, 45, 45},
        {9, 9, 9, 9, 9, 17, 46, 46, 46, 45, 11, 35, 35, 35, 35, 35, 35, 10, 45, 46, 46, 46, 16, 9, 9, 9, 9, 9},
        {35, 35, 35, 35, 35, 10, 46, 46, 46, 45, 26, 8, 8, 8, 8, 8, 8, 27, 45, 46, 46, 46, 11, 35, 35, 35, 35, 35},
        {35, 35, 35, 35, 35, 10, 46, 46, 46, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 46, 46, 46, 11, 35, 35, 35, 35, 35},
        {35, 35, 35, 35, 35, 10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11, 35, 35, 35, 35, 35},
        {0, 8, 8, 8, 8, 19, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 18, 8, 8, 8, 8, 1},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 45, 45, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {10, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 11},
        {2, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 3}
    }

    ' Declare arrays to hold relevant path information.
    Private _path(27, 30) As pathData
    Private _data(27, 30) As Integer
    Private _save(27, 30) As Integer

    ' Declare varialbes to hold information relating to dots.
    Private _dotsTotal As Integer
    Private _dotsEaten As Integer

    ' Gets / Sets the total dot count.
    Property dotCount As Integer
        Get
            dotCount = _dotsTotal
        End Get
        Set(value As Integer)
            _dotsTotal = value
        End Set
    End Property

    ' Gets / Sets the number of dots eaten.
    Property dotEaten As Integer
        Get
            dotEaten = _dotsEaten
        End Get
        Set(value As Integer)
            _dotsEaten = value
        End Set
    End Property

    ' Gets / Sets the type of path in a certain position.
    Property path(p As Point) As pathData
        Get
            If (p.X >= 0 And p.X <= 27) And (p.Y >= 0 And p.Y <= 30) Then
                path = _path(p.X, p.Y)
            Else
                path.pathType = pathType.block
                path.fixed = True
            End If
        End Get
        Set(value As pathData)
            If (p.X >= 0 And p.X <= 27) And (p.Y >= 0 And p.Y <= 30) Then
                _path(p.X, p.Y) = value
            End If
        End Set
    End Property

    ' Gets / Sets the integer value of a tile in a specific position.
    Property data(p As Point) As Integer
        Get
            If (p.X >= 0 And p.X <= 27) And (p.Y >= 0 And p.Y <= 30) Then
                data = _data(p.X, p.Y)
            Else
                data = -1
            End If
        End Get
        Set(value As Integer)
            If (p.X >= 0 And p.X <= 27) And (p.Y >= 0 And p.Y <= 30) Then
                If value = mazeObjects.blank And (_data(p.X, p.Y) = mazeObjects.dot Or _data(p.X, p.Y) = mazeObjects.energizer) Then
                    _dotsEaten += 1
                End If
                _data(p.X, p.Y) = value
            End If
        End Set
    End Property

    ' Gets a list of the exits from a specific tile in the maze.
    Public ReadOnly Property getExits(pos As Point) As List(Of mazeExits)
        Get

            Dim exitList As New List(Of mazeExits)

            If pos.Y - 1 < 0 Then
                exitList.Add(mazeExits.exitUp)
                exitList.Add(mazeExits.exitDown)
            End If
            If pos.Y > 30 Then
                exitList.Add(mazeExits.exitDown)
                exitList.Add(mazeExits.exitUp)
            End If
            If pos.X - 1 < 0 Then
                exitList.Add(mazeExits.exitLeft)
                exitList.Add(mazeExits.exitRight)
            End If
            If pos.X + 1 > 27 Then
                exitList.Add(mazeExits.exitRight)
                exitList.Add(mazeExits.exitLeft)
            End If
            If exitList.Count = 0 Then

                If pos.Y > 0 Then
                    If _path(pos.X, pos.Y - 1).pathType <> pathType.block Then
                        'If path(New Point(pos.X, pos.Y - 1)).pathType <> pathType.block Then
                        exitList.Add(mazeExits.exitUp)
                    End If
                End If
                If pos.Y < 30 Then
                    If _path(pos.X, pos.Y + 1).pathType <> pathType.block Then
                        'If path(New Point(pos.X, pos.Y + 1)).pathType <> pathType.block Then
                        exitList.Add(mazeExits.exitDown)
                    End If
                End If
                If pos.X > 0 Then
                    If _path(pos.X - 1, pos.Y).pathType <> pathType.block Then
                        'If path(New Point(pos.X, pos.Y - 1)).pathType <> pathType.block Then
                        exitList.Add(mazeExits.exitLeft)
                    End If
                End If
                If pos.X < 27 Then
                    If _path(pos.X + 1, pos.Y).pathType <> pathType.block Then
                        'If path(New Point(pos.X, pos.Y + 1)).pathType <> pathType.block Then
                        exitList.Add(mazeExits.exitRight)
                    End If
                End If

            End If

            getExits = exitList

        End Get
    End Property

    ' Gets / Sets the type of path a tile is in a specific position.
    Public Property mazePathType(x As Integer, y As Integer) As pathType
        Get
            If (x < 0 Or x > 27 Or y < 0 Or y > 30) Then
                mazePathType = pathType.blank
            Else
                mazePathType = _path(x, y).pathType
            End If
        End Get
        Set(value As pathType)
            If Not (x <= 0 Or x >= 27 Or y <= 0 Or y >= 30) Then
                _path(x, y).pathType = value
            End If
        End Set
    End Property

    ' Gets / Sets whether a block is fixed or not.
    Public Property mazeBlockFixed(x As Integer, y As Integer) As Boolean
        Get
            If (x < 0 Or x > 27 Or y < 0 Or y > 30) Then
                mazeBlockFixed = True
            Else
                mazeBlockFixed = _path(x, y).fixed
            End If
        End Get
        Set(value As Boolean)
            _path(x, y).fixed = value
        End Set
    End Property

    ' Resets the maze, using the save array.
    Public Sub resetMaze()

        _dotsEaten = 0
        _dotsTotal = 0

        For y = 0 To 30
            For x = 0 To 27
                _data(x, y) = _save(x, y)
                If _save(x, y) = mazeObjects.dot Or _save(x, y) = mazeObjects.energizer Then
                    _dotsTotal += 1
                End If
            Next
        Next

    End Sub

    Public Sub saveMaze(filename As String)

        Dim stream As FileStream

        stream = New FileStream(filename, FileMode.Create)

        For y = 0 To 30
            For x = 0 To 27
                stream.WriteByte(_path(x, y).pathType)
            Next
        Next

        Stream.Close()

    End Sub

    ' Loads maze from a chosen file by the user.
    Public Sub loadMaze(filename As String)

        Dim stream As FileStream

        For y = 0 To 30
            For x = 0 To 27
                Dim blockData As Integer = defaultMaze(y, x)
                _data(x, y) = blockData
                Select Case blockData
                    ' Initializes the type of path in each position and if it is fixed.
                    Case 46
                        _path(x, y).pathType = pathType.dot
                        _path(x, y).fixed = False
                    Case 45
                        _path(x, y).pathType = pathType.blank
                        _path(x, y).fixed = True
                    Case Else
                        _path(x, y).pathType = pathType.block
                        _path(x, y).fixed = True
                End Select
            Next
        Next

        ' Sets fixed position in the map so that in the map editor users cannot manipulate these specific tiles, in order to avoid un-needed complexity.

        For y = 13 To 15
            _path(6, y).fixed = True
            _path(21, y).fixed = True
        Next

        _path(6, 8).fixed = True
        _path(6, 9).fixed = True
        _path(6, 19).fixed = True
        _path(6, 20).fixed = True
        _path(21, 8).fixed = True
        _path(21, 9).fixed = True
        _path(21, 19).fixed = True
        _path(21, 20).fixed = True

        _path(13, 23).fixed = True
        _path(14, 23).fixed = True

        ' Reads the maze from the users chosen file.
        If Trim(filename) <> "" Then
            stream = New FileStream(filename, FileMode.Open)
            For y = 0 To 30
                For x = 0 To 27
                    _path(x, y).pathType = stream.ReadByte()
                Next
            Next
            stream.Close()
        End If

        pathToData()

        For y = 0 To 30
            For x = 0 To 27
                _save(x, y) = _data(x, y)
            Next
        Next

    End Sub

    Public Sub pathToData()

        ' Determines the type of tile that should be placed into each position of the maze.
        For y = 1 To 29
            For x = 1 To 26
                If _path(x, y).pathType = pathType.block And _path(x, y).fixed = False Then
                    ' Retrieves the type of block to be placed using the complex routine getTile.
                    _data(x, y) = getTile(x, y)
                Else
                    If _path(x, y).pathType = pathType.blank And _path(x, y).fixed = False Then
                        _data(x, y) = 45
                    Else
                        If _path(x, y).pathType = pathType.dot And _path(x, y).fixed = False Then
                            _data(x, y) = 46
                        Else
                            If _path(x, y).pathType = pathType.energizer And _path(x, y).fixed = False Then
                                _data(x, y) = 47
                            End If
                        End If
                    End If
                End If
            Next
        Next

        adjustEdges()

    End Sub

    Private Function getTile(x As Integer, y As Integer)

        Dim n, s, e, w, ne, nw, se, sw As Boolean

        ' Is there a block to the north
        If _path(x, y - 1).pathType <> pathType.block Or _path(x, y - 1).fixed = True Then
            n = True
        Else : n = False
        End If

        ' Is there a block to the south
        If _path(x, y + 1).pathType <> pathType.block Or _path(x, y + 1).fixed = True Then
            s = True
        Else : s = False
        End If

        ' Is there a block to the west
        If _path(x - 1, y).pathType <> pathType.block Or _path(x - 1, y).fixed = True Then
            w = True
        Else : w = False
        End If

        ' Is there a block to the east
        If _path(x + 1, y).pathType <> pathType.block Or _path(x + 1, y).fixed = True Then
            e = True
        Else : e = False
        End If

        ' Is there a block to the northeast
        If _path(x + 1, y - 1).pathType <> pathType.block Or _path(x + 1, y - 1).fixed = True Then
            ne = True
        Else : ne = False
        End If

        ' Is there a block to the northwest
        If _path(x - 1, y - 1).pathType <> pathType.block Or _path(x - 1, y - 1).fixed = True Then
            nw = True
        Else : nw = False
        End If

        ' Is there a block to the southeast
        If _path(x + 1, y + 1).pathType <> pathType.block Or _path(x + 1, y + 1).fixed = True Then
            se = True
        Else : se = False
        End If

        ' Is there a block to the southwest
        If _path(x - 1, y + 1).pathType <> pathType.block Or _path(x - 1, y + 1).fixed = True Then
            sw = True
        Else : sw = False
        End If

        ' Set map tile based on block positions

        If Not n And Not s And Not e And Not w And ne And sw Then
            Return 39
        End If

        If Not n And Not s And Not e And Not w And nw And se Then
            Return 38
        End If

        If n Then
            If w Or sw Then Return 16
            If e Or se Then Return 17
            Return 21
        End If

        If s Then
            If w Or nw Then Return 18
            If e Or ne Then Return 19
            Return 20
        End If

        If w And ne Then Return 16
        If e And nw Then Return 17
        If w And se Then Return 18
        If e And sw Then Return 19

        If Not n And Not s And w Then Return 23
        If Not n And Not s And e Then Return 22

        If Not e And Not s And sw Then
            Return 17
        End If

        If Not w And Not s And se Then
            Return 16
        End If

        If Not e And Not n And nw Then
            Return 19
        End If

        If Not w And Not n And ne Then
            Return 18
        End If

        Return 45

    End Function

    Private Sub adjustEdges()
        ' Handles the adjustments required when the user has placed a tile on an edge.

        Dim x, y As Integer
        Dim p1, p2 As Point
        For y = 1 To 29

            ' Find first non-fixed block or first fixed dot on the left hand edge
            For x = 0 To 27
                If _path(x, y).fixed = False Or (_path(x, y).pathType = pathType.dot And _path(x, y).fixed = True) Or (_path(x, y).pathType = pathType.energizer And _path(x, y).fixed = True) Then
                    Exit For
                End If
            Next

            ' Manipulates the edge tile in order to connect the users placed block to the edge of the maze.
            p1 = New Point(x, y)
            p2 = New Point(x - 1, y)
            If _path(x, y).pathType = pathType.block Then
                Dim a = _data(x, y)
                Select Case a
                    Case 16
                        _data(x, y) = 21
                        _data(x - 1, y) = 6
                    Case 23
                        _data(x, y) = 35
                        _data(x - 1, y) = 36
                    Case 18
                        _data(x, y) = 20
                        _data(x - 1, y) = 4
                End Select
            Else
                If x > 0 Then
                    _data(x - 1, y) = defaultMaze(y, x - 1)
                End If
            End If

            ' Find first non-fixed block or first fixed dot on the right hand edge
            For x = 27 To 0 Step -1
                If _path(x, y).fixed = False Or (_path(x, y).pathType = pathType.dot And _path(x, y).fixed = True) Or (_path(x, y).pathType = pathType.energizer And _path(x, y).fixed = True) Then
                    Exit For
                End If
            Next

            ' Manipulates the edge tile in order to connect the users placed block to the edge of the maze.
            p1 = New Point(x, y)
            p2 = New Point(x + 1, y)
            If _path(x, y).pathType = pathType.block Then
                Dim a = _data(x, y)
                Select Case a
                    Case 17
                        _data(x, y) = 21
                        _data(x + 1, y) = 7
                    Case 22
                        _data(x, y) = 35
                        _data(x + 1, y) = 37
                    Case 19
                        _data(x, y) = 20
                        _data(x + 1, y) = 5
                End Select
            Else
                If x < 27 Then
                    _data(x + 1, y) = defaultMaze(y, x + 1)
                End If
            End If

        Next

        ' Handle top of maze
        For x = 2 To 25
            If _path(x, 1).pathType = pathType.block And _path(x - 1, 1).pathType <> pathType.block Then
                _data(x, 0) = 41
                _data(x, 1) = 23
            End If
            If _path(x, 1).pathType = pathType.block And _path(x + 1, 1).pathType <> pathType.block Then
                _data(x, 0) = 40
                _data(x, 1) = 22
            End If
            If _path(x, 1).pathType = pathType.block And _path(x - 1, 1).pathType = pathType.block And _path(x + 1, 1).pathType = pathType.block Then
                _data(x, 0) = 44
                _data(x, 1) = 45
            End If
            If _path(x, 1).pathType <> pathType.block Then
                _data(x, 0) = defaultMaze(0, x)
            End If
        Next

        If _path(1, 1).pathType = pathType.block Then
            _data(0, 0) = 58
            _data(0, 1) = 36
            _data(1, 0) = 44
            _data(1, 1) = 45
        Else
            _data(0, 0) = defaultMaze(0, 0)
            _data(0, 1) = defaultMaze(1, 0)
            _data(1, 0) = defaultMaze(0, 1)
        End If

        If _path(26, 1).pathType = pathType.block Then
            _data(27, 0) = 59
            _data(27, 1) = 37
            _data(26, 0) = 44
            _data(26, 1) = 45
        Else
            _data(27, 0) = defaultMaze(0, 27)
            _data(27, 1) = defaultMaze(1, 27)
            _data(26, 0) = defaultMaze(0, 26)
        End If

        ' Handle bottom of maze
        For x = 2 To 25
            If _path(x, 29).pathType = pathType.block And _path(x - 1, 29).pathType <> pathType.block Then
                _data(x, 30) = 69
                _data(x, 29) = 23
            End If
            If _path(x, 29).pathType = pathType.block And _path(x + 1, 29).pathType <> pathType.block Then
                _data(x, 30) = 57
                _data(x, 29) = 22
            End If
            If _path(x, 29).pathType = pathType.block And _path(x - 1, 29).pathType = pathType.block And _path(x + 1, 29).pathType = pathType.block Then
                _data(x, 30) = 43
                _data(x, 29) = 45
            End If
            If _path(x, 29).pathType <> pathType.block Then
                _data(x, 30) = defaultMaze(30, x)
            End If
        Next

        If _path(1, 29).pathType = pathType.block Then
            _data(0, 30) = 70
            _data(0, 29) = 36
            _data(1, 30) = 43
            _data(1, 29) = 45
        Else
            _data(0, 30) = defaultMaze(30, 0)
            _data(0, 29) = defaultMaze(29, 0)
            _data(1, 30) = defaultMaze(30, 1)
        End If

        If _path(26, 29).pathType = pathType.block Then
            _data(27, 30) = 71
            _data(27, 29) = 37
            _data(26, 30) = 43
            _data(26, 29) = 45
        Else
            _data(27, 30) = defaultMaze(30, 27)
            _data(27, 29) = defaultMaze(29, 27)
            _data(26, 30) = defaultMaze(30, 26)
        End If




    End Sub

End Class
