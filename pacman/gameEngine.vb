Public Class gameEngine

    Declare Function QueryPerformanceCounter Lib "Kernel32" (ByRef X As Long) As Short
    Declare Function QueryPerformanceFrequency Lib "Kernel32" (ByRef X As Long) As Short

    Private Const MARGIN_X = 0
    Private Const MARGIN_Y = 0

    Private _bw As New System.ComponentModel.BackgroundWorker

    Dim _cTimer As Long
    Dim _cTimer2 As Long
    Dim _freq As Long
    Dim _interval As Double
    Dim _fpsActual As Double

    ' This structure holds the information relating to the surface we're targetting

    Private Structure geSurface
        Public _client As PictureBox
        Public _mouseEventArgs As MouseEventArgs
        Public _image As Image
        Public _scale As Integer
        Public _fps As Double
        Public _clock As Integer
        Public _surfaceLocked As Boolean
    End Structure

    ' -----------------------------------------------------------------------------------------------------------------------------

    Private _geSurface As New geSurface
    Private _geTile As New List(Of tile)
    Private _geSprite As New List(Of sprite)
    Private _geMap As New List(Of map)
    Private _geFont As New List(Of font)

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' geSurface
    ' -----------------------------------------------------------------------------------------------------------------------------

    ' Create a new surface

    Public Sub New(client As Form, x As Integer, y As Integer, Optional scale As Integer = 1, Optional offset As Integer = 24)

        ' Create a new picturebox to hold the gameEngine surface

        _geSurface._client = New PictureBox
        With _geSurface._client
            .Size = New Size(x * scale, y * scale)
            .Location = New Point(0, offset)
        End With
        AddHandler _geSurface._client.MouseMove, AddressOf mouseMove
        AddHandler _geSurface._client.MouseDown, AddressOf mouseDown
        AddHandler _geSurface._client.MouseUp, AddressOf mouseUp

        ' Add the picturebox to the client (form)

        client.Controls.Add(_geSurface._client)

        ' Create a surface into which the gameEngine will render

        _geSurface._image = New Bitmap((x + (MARGIN_X * 2)) * scale, (y + (MARGIN_Y * 2)) * scale)

        ' Set the scaling
        ' Game engine pre-scales everything in advance rather than at render time
        ' This increases performance drastically

        _geSurface._scale = scale

        ' Set the default frames per second

        _geSurface._fps = 60

        ' Set up the background worker
        ' The background worker supports cancellation and reports progress (we do the rendering in the progress event)

        _bw.WorkerSupportsCancellation = True
        _bw.WorkerReportsProgress = True
        AddHandler _bw.DoWork, AddressOf runEngine
        AddHandler _bw.ProgressChanged, AddressOf runEngineProcess
        AddHandler _bw.RunWorkerCompleted, AddressOf runEngineComplete

    End Sub

    Public Sub startEngine()

        ' Wait until the background worker is ended

        While _bw.IsBusy = True
            Application.DoEvents()
        End While

        ' Start the background worker

        _bw.RunWorkerAsync()

    End Sub

    Public Sub endEngine()

        ' Cancel the background worker

        _bw.CancelAsync()

    End Sub

    Private Sub runEngineProcess(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs)

        _geSurface._surfaceLocked = True
        RaiseEvent geGameLogic()
        drawMap()
        drawSprites()
        RaiseEvent geRenderScene()

        _geSurface._client.Image = _geSurface._image
        _geSurface._clock += 1
        _geSurface._surfaceLocked = False

    End Sub

    Private Sub runEngine(sender As Object, e As System.ComponentModel.DoWorkEventArgs)

        Dim _oldClock As Integer = -1

        ' Get the performance frequency

        QueryPerformanceFrequency(_freq)

        ' Calculate the clock interval at the given frames per second

        _interval = _freq / _geSurface._fps

        Do

            ' If the background worker has been cancelled (by gameEngine.endEngine) then cancel and exit

            If _bw.CancellationPending = True Then
                e.Cancel = True
                Exit Do
            End If

            ' Get current time
            QueryPerformanceCounter(_cTimer2)

            ' Compare the current time with the previous time to see whether enough time has elapsed

            If _cTimer2 >= _cTimer + _interval Then

                ' Get the actual frames per second

                _fpsActual = Math.Round(_freq / (_cTimer2 - _cTimer), 1)

                ' Get the current time

                QueryPerformanceCounter(_cTimer)

                ' Ensure that the previous ReportProgress has completed
                ' We do this be checking that the surfaceLocked state

                '_bw.ReportProgress(1, _geSurface._image)

                If _geSurface._surfaceLocked = False Then
                    _bw.ReportProgress(1, _geSurface._image)
                End If

            End If

        Loop

    End Sub

    Private Sub runEngineComplete(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)

    End Sub

    Private Sub mouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        RaiseEvent geMouseMove(sender, e)

    End Sub

    Private Sub mouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        RaiseEvent geMouseDown(sender, e)

    End Sub

    Private Sub mouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        RaiseEvent geMouseUp(sender, e)

    End Sub



    Public Event geGameLogic()
    Public Event geRenderScene()

    Public Event geMouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    Public Event geMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    Public Event geMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

    Public Property scale
        Get
            scale = _geSurface._scale
        End Get
        Set(value)
            _geSurface._scale = scale
        End Set
    End Property

    Public Property fps As Double
        Get
            fps = _fpsActual
        End Get
        Set(value As Double)
            _geSurface._fps = value
        End Set
    End Property

    Public ReadOnly Property running As Boolean
        Get
            If _bw.IsBusy Then
                running = True
            Else
                running = False
            End If
        End Get
    End Property

    Public Property clock As Integer
        Get
            clock = _geSurface._clock '/ _geSurface._fps
        End Get
        Set(value As Integer)
            _geSurface._clock = value '* _geSurface._fps
        End Set
    End Property

    Public ReadOnly Property getMouse As MouseEventArgs
        Get
            getMouse = _geSurface._mouseEventArgs
        End Get
    End Property

    Public ReadOnly Property getSurface As PictureBox
        Get
            getSurface = _geSurface._client
        End Get
    End Property

    Public ReadOnly Property getImage
        Get
            getImage = _geSurface._image
        End Get
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.addSprite(as gameEngine.geSprite)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Function addSprite(filename As String, spriteSize As Size) As sprite
        _geSprite.Add(New sprite(Me, filename, spriteSize))
        Return _geSprite(_geSprite.Count - 1)
    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.spriteByIndex(as integer)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Property spriteByIndex(index As Integer) As sprite
        Get
            Return _geSprite(index)
        End Get
        Set(value As sprite)
            _geSprite(index) = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.spriteByName(as string)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Property spriteByName(name As String) As sprite
        Get
            Dim index As Integer
            index = _geSprite.FindIndex((Function(f) f.name = name))
            Return _geSprite(index)
        End Get
        Set(value As sprite)
            Dim index As Integer
            index = _geSprite.FindIndex((Function(f) f.name = name))
            _geSprite(index) = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.spriteIndexByName(as string) as integer
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public ReadOnly Property spriteIndexByName(name As String) As Integer
        Get
            Dim index As Integer
            index = _geSprite.FindIndex((Function(f) f.name = name))
            Return index
        End Get
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.addTile(as gameEngine.geTile)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function addTile(filename As String, tileSize As Size) As tile

        _geTile.Add(New tile(Me, filename, tileSize))
        Return _geTile(_geTile.Count - 1)

    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.tileByIndex(as integer) as tile
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Property tileByIndex(index As Integer) As tile
        Get
            Return _geTile(index)
        End Get
        Set(value As tile)
            _geTile(index) = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.tileByName(as string) as tile
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Property tileByName(name As String) As tile
        Get
            Dim index As Integer
            index = _geTile.FindIndex((Function(f) f.name = name))
            Return _geTile(index)
        End Get
        Set(value As tile)
            Dim index As Integer
            index = _geTile.FindIndex((Function(f) f.name = name))
            _geTile(index) = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.tileIndexByName(as string) as integer
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public ReadOnly Property tileIndexByName(name As String) As Integer
        Get
            Dim index As Integer
            index = _geTile.FindIndex((Function(f) f.name = name))
            Return index
        End Get
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.addMap(as gameEngine.geMap)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function addMap(name As String, tilesetIndex As Integer, mapSize As Size) As map
        _geMap.Add(New map(Me, name, tilesetIndex, mapSize))
        Return _geMap(_geMap.Count - 1)
    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.mapByIndex(as integer)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Property mapByIndex(index As Integer) As map
        Get
            Return _geMap(index)
        End Get
        Set(value As map)
            _geMap(index) = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.mapByName(as string) as map
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Property mapByName(name As String) As map
        Get
            Dim index As Integer
            index = _geMap.FindIndex((Function(f) f.name = name))
            Return _geMap(index)
        End Get
        Set(value As map)
            Dim index As Integer
            index = _geMap.FindIndex((Function(f) f.name = name))
            _geMap(index) = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.mapIndexByName(as string) as integer
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public ReadOnly Property mapIndexByName(name As String) As Integer
        Get
            Dim index As Integer
            index = _geMap.FindIndex((Function(f) f.name = name))
            Return index
        End Get
    End Property

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' gameEngine.addFont(as gameEngine.geFont)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Function addFont(filename As String, tileSize As Size, asciiStart As Integer) As font
        _geFont.Add(New font(Me, filename, tileSize, asciiStart))
        Return _geFont(_geFont.Count - 1)
    End Function

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' drawSprites()
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub drawSprites()

        _geSprite.Sort(Function(x, y) x.zindex.CompareTo(y.zindex))

        For n = 0 To _geSprite.Count - 1
            spriteByIndex(n).drawSprite()
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' drawTileByName(name as string, t as integer, p as point)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub drawTileByName(name As String, t As Integer, p As Point)

        tileByName(name).drawTile(t, p)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' drawTileByIndex(index as integer, t as integer, p as point)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub drawTileByIndex(index As Integer, t As Integer, p As Point)

        tileByIndex(index).drawTile(t, p)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' drawMap()
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub drawMap()

        For m = 0 To _geMap.Count - 1

            If _geMap(m).enabled = True Then

                For y1 = _geMap(m).mapClip.Y To _geMap(m).mapClip.Bottom - 1
                    For x1 = _geMap(m).mapClip.X To _geMap(m).mapClip.Right - 1

                        Dim tileSize As Size
                        tileSize = _geTile(_geMap(m).tilesetIndex).size

                        If _geMap(m).invalidated(New Point(x1, y1)) = True Then
                            If _geMap(m).value(New Point(x1, y1)) <> -1 Then
                                drawTileByIndex(_geMap(m).tilesetIndex, _geMap(m).value(New Point(x1, y1)), New Point(_geMap(m).point.X + ((x1 - _geMap(m).mapClip.X) * tileSize.Width), _geMap(m).point.Y + ((y1 - _geMap(m).mapClip.Y) * tileSize.Height)))
                            End If
                            _geMap(m).invalidated(New Point(x1, y1)) = False
                        End If

                    Next
                Next

            End If

        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------------------------
    ' drawTextByName(name as string, text as string, p as point)
    ' -----------------------------------------------------------------------------------------------------------------------------

    Public Sub drawTextbyName(name As String, text As String, p As Point)

        Dim index As Integer
        index = _geFont.FindIndex((Function(f) f.name = name))

        For s = 1 To Len(text)

            _geFont(index).drawTile(Asc(Mid(text, s, 1)), New Point(p.X + ((s - 1) * _geFont(index).size.Width), p.Y))

        Next

        ' Invalidate tiles that are overlapped by the sprites

        For n = 0 To _geMap.Count - 1

            Dim tileSize As Size
            tileSize = _geTile(_geMap(n).tilesetIndex).size

            Dim tilePos As Point
            tilePos = New Point(Int(p.X / tileSize.Width), Int(p.Y / tileSize.Height))

            For yy = tilePos.Y To tilePos.Y + (_geFont(index).size.Height / tileSize.Height) + 1
                For xx = tilePos.X To tilePos.X + ((_geFont(index).size.Width * Len(text)) / tileSize.Width) + 1
                    _geMap(n).invalidated(New Point(xx, yy)) = True
                Next
            Next
        Next

    End Sub

    ' =============================================================================================================================
    '
    ' geTile Class
    '
    ' =============================================================================================================================

    Public Class tile

        Private _parent As gameEngine               ' Parent class
        Private _name As String                     ' Name of tileset
        Private _geTileset As List(Of Image)        ' List of tile images
        Private _count As Integer                   ' Number of tiles in the tileset
        Private _size As Size                       ' Tile size

        ' New subroutine
        ' This handles the creation of the geTile item

        Public Sub New(parent As gameEngine, filename As String, tileSize As Size)

            _parent = parent
            _name = filename
            _geTileset = New List(Of Image)
            _count = 0
            _size = tileSize

            ' Define a new picturebox to hold the unformatted tileset

            Dim pbTileSet As New PictureBox

            ' Allocate an empty bitmap the same size as an individual tile

            Dim bm As New Bitmap(_size.Width * _parent._geSurface._scale, _size.Height * _parent._geSurface._scale, Imaging.PixelFormat.Format32bppPArgb)

            ' Define two variables to hold the number of tiles in the X and Y direction

            Dim numberTilesX As Integer
            Dim numberTilesY As Integer

            ' Load image from file

            pbTileSet.Image = Image.FromFile(filename & ".png")

            ' Get number of tiles in X and Y direction

            numberTilesX = pbTileSet.Image.Width / _size.Width
            numberTilesY = pbTileSet.Image.Height / _size.Height

            ' Loop through each row and column per individual tile

            For y1 = 0 To numberTilesY - 1
                For x1 = 0 To numberTilesX - 1

                    ' Allocate a graphics surface to the empty bitmap

                    Using gr As Graphics = Graphics.FromImage(bm)

                        ' Set the source rectangle size

                        Dim srcRect As New Rectangle(x1 * _size.Width, y1 * _size.Height, _size.Width, _size.Height)

                        ' Set the destination rectangle size

                        Dim dstRect As New Rectangle(0, 0, _size.Width * _parent._geSurface._scale, _size.Height * _parent._geSurface._scale)

                        ' Copy from the source rectangle co-ordinates in the tileset bitmap
                        ' into the destination rectangle co-ordinates in the empty bitmap

                        gr.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
                        gr.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                        gr.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                        gr.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                        gr.CompositingMode = Drawing2D.CompositingMode.SourceCopy
                        gr.DrawImage(pbTileSet.Image, dstRect, srcRect, GraphicsUnit.Pixel)

                    End Using

                    _geTileset.Add(bm.Clone)
                    _count += 1

                Next
            Next

        End Sub

        ' geTile.drawTile

        Public Sub drawTile(t As Integer, p As Point)

            If t < _count Then
                Using gr As Graphics = Graphics.FromImage(_parent._geSurface._image)
                    gr.DrawImage(_geTileset(t), p.X * _parent._geSurface._scale, p.Y * _parent._geSurface._scale)
                End Using
            End If

        End Sub

        ' geTile.name

        Public Property name As String
            Get
                name = _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        ' geTile.count

        Public ReadOnly Property count As Integer
            Get
                count = _count
            End Get
        End Property

        ' geTile.size

        Public ReadOnly Property size As Size
            Get
                size = _size
            End Get
        End Property

    End Class

    ' =============================================================================================================================
    '
    ' geMap Class
    '
    ' =============================================================================================================================

    Public Class map

        Private _parent As gameEngine
        Private _name As String
        Private _tilesetIndex As Integer
        Private _geMap(,) As Integer
        Private _gemapInvalidated(,) As Boolean
        Private _geMapSize As Size
        Private _geMapClip As Rectangle
        Private _point As Point
        Private _enabled As Boolean

        Public Sub New(parent As gameEngine, name As String, tilesetIndex As Integer, mapSize As Size)

            _parent = parent
            _name = name
            _tilesetIndex = tilesetIndex
            _geMapSize = mapSize
            _geMapClip = New Rectangle(0, 0, _geMapSize.Width, _geMapSize.Height)
            _point = New Point(0, 0)
            _enabled = True

            ReDim _geMap(_geMapSize.Width, _geMapSize.Height)
            ReDim _gemapInvalidated(_geMapSize.Width, _geMapSize.Height)
            For y1 = 0 To _geMapSize.Height
                For x1 = 0 To _geMapSize.Width
                    _geMap(x1, y1) = -1
                    _gemapInvalidated(x1, y1) = True
                Next
            Next

        End Sub

        Property name As String
            Get
                name = _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        Property tilesetIndex As Integer
            Get
                tilesetIndex = _tilesetIndex
            End Get
            Set(value As Integer)
                _tilesetIndex = value
            End Set
        End Property

        Property mapSize As Size
            Get
                mapSize = _geMapSize
            End Get
            Set(value As Size)
                _geMapSize = value
                ReDim Preserve _geMap(_geMapSize.Width - 1, _geMapSize.Height - 1)
            End Set
        End Property

        Property mapClip As Rectangle
            Get
                mapClip = _geMapClip
            End Get
            Set(value As Rectangle)
                If value.X < 0 Then value.X = 0
                If value.X > _geMapSize.Width Then value.X = _geMapSize.Width
                If value.Y < 0 Then value.Y = 0
                If value.Y > _geMapSize.Height Then value.Y = _geMapSize.Height
                _geMapClip = value
            End Set
        End Property

        Public Property point As Point
            Get
                point = _point
            End Get
            Set(value As Point)
                _point = value
            End Set
        End Property

        Public Property enabled As Boolean
            Get
                enabled = _enabled
            End Get
            Set(value As Boolean)
                _enabled = value
            End Set
        End Property

        Public Property value(p As Point) As Integer
            Get
                If p.X < 0 Or p.X >= _geMapSize.Width Or p.Y < 0 Or p.Y >= _geMapSize.Height Then
                    value = -1
                Else
                    value = _geMap(p.X, p.Y)
                End If
            End Get
            Set(value As Integer)
                If p.X >= 0 And p.X < _geMapSize.Width And p.Y >= 0 And p.Y < _geMapSize.Height Then
                    _geMap(p.X, p.Y) = value
                    _gemapInvalidated(p.X, p.Y) = True
                End If
            End Set
        End Property

        Public Property invalidated(p As Point) As Boolean
            Get
                If p.X < 0 Or p.X >= _geMapSize.Width Or p.Y < 0 Or p.Y >= _geMapSize.Height Then
                    invalidated = False
                Else
                    invalidated = _gemapInvalidated(p.X, p.Y)
                End If
            End Get
            Set(value As Boolean)
                If p.X >= 0 And p.X < _geMapSize.Width And p.Y >= 0 And p.Y < _geMapSize.Height Then
                    _gemapInvalidated(p.X, p.Y) = value
                End If
            End Set
        End Property

    End Class

    ' =============================================================================================================================
    '
    ' geSprite Class
    '
    ' =============================================================================================================================

    Public Class sprite

        Public Enum geAnimationMode As Integer
            geNone = 0
            geForward = 1
            geBackwards = 2
            geBoth = 3
        End Enum

        Public Class geAnimationRange
            Private _min As Integer
            Private _max As Integer
            Sub New(min As Integer, max As Integer)
                _min = min
                _max = max
            End Sub
            Public Property min As Integer
                Get
                    min = _min
                End Get
                Set(value As Integer)
                    _min = value
                End Set
            End Property
            Public Property max As Integer
                Get
                    max = _max
                End Get
                Set(value As Integer)
                    _max = value
                End Set
            End Property
        End Class

        Private _parent As gameEngine               ' Parent Class
        Private _name As String                     ' Name of sprite
        Private _geTileset As List(Of Image)        ' List of sprite images
        Private _point As Point                     ' X,Y position
        Private _size As Size                       ' size in pixels
        Private _enabled As Boolean                 ' Enabled/disabled
        Private _totalframes As Integer             ' Total number of frames available
        Private _animateMode As geAnimationMode     ' Animation mode
        Private _animateRange As geAnimationRange   ' Animation minimum and maximum frames
        Private _frameCount As Integer              ' Global frame counter
        Private _animateOnFrame As Integer          ' Increment animation frame when the global frame counter hits this value
        Private _animateFrame As Integer            ' Current animation frame
        Private _animateDirection As Integer        ' Current animation direction
        Private _zindex As Integer                  ' Z-Order when rendering

        ' New subroutine
        ' This handles the creation of the geSprite item

        Public Sub New(parent As gameEngine, filename As String, spriteSize As Size)

            _parent = parent
            _name = filename
            _geTileset = New List(Of Image)
            _point = New Point(0, 0)
            _size = spriteSize
            _enabled = False
            _totalframes = 0
            _animateMode = geAnimationMode.geNone
            _animateRange = New geAnimationRange(0, 0)
            _frameCount = 0
            _animateOnFrame = 0
            _animateFrame = 0
            _animateDirection = 1
            _zindex = 1

            ' We need to load the tileset from the file specified

            ' Define a new picturebox to hold the unformatted tileset

            Dim pbTileSet As New PictureBox

            ' Allocation an empty bitmap the same size as an individual tile

            Dim bm As New Bitmap(_size.Width * _parent._geSurface._scale, _size.Height * _parent._geSurface._scale, Imaging.PixelFormat.Format32bppPArgb)

            ' Define two variables to hold the number of tiles in the X and Y direction

            Dim numberTilesX As Integer
            Dim numberTilesY As Integer

            ' Load image from file

            pbTileSet.Image = Image.FromFile(filename & ".png")

            ' Get number of tiles in X and Y direction

            numberTilesX = pbTileSet.Image.Width / _size.Width
            numberTilesY = pbTileSet.Image.Height / _size.Height

            ' Loop through each row and column per individual tile

            Dim x As Integer
            Dim y As Integer

            For y = 0 To numberTilesY - 1
                For x = 0 To numberTilesX - 1

                    ' Allocate a graphics surface to the empty bitmap

                    Using gr As Graphics = Graphics.FromImage(bm)

                        ' Set the source rectangle size

                        Dim srcRect As New Rectangle(x * _size.Width, y * _size.Height, _size.Width, _size.Height)

                        ' Set the destination rectangle size

                        Dim dstRect As New Rectangle(0, 0, _size.Width * _parent._geSurface._scale, _size.Height * _parent._geSurface._scale)

                        ' Copy from the source rectangle co-ordinates in the tileset bitmap
                        ' into the destination rectangle co-ordinates in the empty bitmap

                        gr.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
                        gr.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                        gr.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                        gr.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                        gr.CompositingMode = Drawing2D.CompositingMode.SourceCopy
                        gr.DrawImage(pbTileSet.Image, dstRect, srcRect, GraphicsUnit.Pixel)

                    End Using

                    ' Add the current tile into our list of tiles and increment the frame count

                    _geTileset.Add(bm.Clone)
                    _totalframes += 1

                Next
            Next
        End Sub

        ' DrawSprite subroutine
        ' This handle the drawing of the sprite instance

        Public Sub drawSprite()

            If _enabled = True Then
                Using gr As Graphics = Graphics.FromImage(_parent._geSurface._image)
                    gr.DrawImage(_geTileset(_animateFrame), _point.X * _parent._geSurface._scale, _point.Y * _parent._geSurface._scale)
                End Using

                If _animateMode <> geAnimationMode.geNone Then
                    If _frameCount >= _animateOnFrame Then
                        _animateFrame += _animateDirection
                        If _animateFrame > _animateRange.max Then
                            If _animateMode = geAnimationMode.geBoth Then
                                _animateDirection *= -1
                                _animateFrame = _animateRange.max + _animateDirection
                            Else
                                _animateFrame = _animateRange.min
                            End If
                        End If
                        If _animateFrame < _animateRange.min Then
                            If _animateMode = geAnimationMode.geBoth Then
                                _animateDirection *= -1
                                _animateFrame = _animateRange.min + _animateDirection
                            Else
                                _animateFrame = _animateRange.max
                            End If
                        End If
                        _frameCount = 0
                    Else
                        _frameCount += 1
                    End If
                End If

                ' Invalidate tiles that are overlapped by the sprites

                For n = 0 To _parent._geMap.Count - 1

                    Dim tileSize As Size
                    tileSize = _parent._geTile(_parent._geMap(n).tilesetIndex).size

                    Dim tilePos As Point
                    tilePos = New Point(Int(point.X / tileSize.Width), Int(point.Y / tileSize.Height))

                    For yy = tilePos.Y To tilePos.Y + (_size.Height / tileSize.Height) + 1
                        For xx = tilePos.X To tilePos.X + (_size.Width / tileSize.Width) + 1
                            _parent._geMap(n).invalidated(New Point(xx, yy)) = True
                        Next
                    Next
                Next

            End If

        End Sub

        ' geSprite.name

        Public Property name As String
            Get
                name = _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        ' geSprite.point 

        Public Property point As Point
            Get
                point = _point
            End Get
            Set(value As Point)
                _point = value
            End Set
        End Property

        ' geSprite.enabled

        Public Property enabled As Boolean
            Get
                enabled = _enabled
            End Get
            Set(value As Boolean)
                _enabled = value
            End Set
        End Property

        ' geSprite.totalFrames

        Public ReadOnly Property totalFrames As Integer
            Get
                totalFrames = _totalframes
            End Get
        End Property

        ' geSprite.animationMode

        Public Property animateMode As geAnimationMode
            Get
                animateMode = _animateMode
            End Get
            Set(value As geAnimationMode)
                _animateMode = value
            End Set
        End Property

        ' geSprite.animationRange

        Public Property animationRange As geAnimationRange
            Get
                animationRange = _animateRange
            End Get
            Set(value As geAnimationRange)
                _animateRange = value
                _animateFrame = value.min
                _frameCount = 0
            End Set
        End Property

        ' geSprite.animateOnFrame

        Public Property animateOnFrame As Integer
            Get
                animateOnFrame = _animateOnFrame
            End Get
            Set(value As Integer)
                _animateOnFrame = value
                _frameCount = 0
            End Set
        End Property

        ' geSprite.zindex

        Public Property zindex As Integer
            Get
                zindex = _zindex
            End Get
            Set(value As Integer)
                _zindex = value
            End Set
        End Property

    End Class


    ' =============================================================================================================================
    '
    ' geFont Class
    '
    ' =============================================================================================================================

    Public Class font

        Private _parent As gameEngine               ' Parent class
        Private _name As String                     ' Name of tileset
        Private _geTileset As List(Of Image)        ' List of tile images
        Private _count As Integer                   ' Number of tiles in the tileset
        Private _asciiStart As Integer              ' ASCII start position
        Private _tileSize As Size                   ' Tile Size

        ' New subroutine
        ' This handles the creation of the geFont item

        Public Sub New(parent As gameEngine, filename As String, tileSize As Size, asciiStart As Integer)

            _parent = parent
            _name = filename
            _geTileset = New List(Of Image)
            _count = 0
            _asciiStart = asciiStart
            _tileSize = tileSize

            ' Define a new picturebox to hold the unformatted tileset

            Dim pbTileSet As New PictureBox

            ' Allocation an empty bitmap the same size as an individual tile

            Dim bm As New Bitmap(_tileSize.Width * _parent._geSurface._scale, _tileSize.Height * _parent._geSurface._scale, Imaging.PixelFormat.Format32bppPArgb)

            ' Define two variables to hold the number of tiles in the X and Y direction

            Dim numberTilesX As Integer
            Dim numberTilesY As Integer

            ' Load image from file

            pbTileSet.Image = Image.FromFile(filename & ".png")

            ' Get number of tiles in X and Y direction

            numberTilesX = pbTileSet.Image.Width / tileSize.Width
            numberTilesY = pbTileSet.Image.Height / tileSize.Height

            ' Loop through each row and column per individual tile

            For y1 = 0 To numberTilesY - 1
                For x1 = 0 To numberTilesX - 1

                    ' Allocate a graphics surface to the empty bitmap

                    Using gr As Graphics = Graphics.FromImage(bm)

                        ' Set the source rectangle size

                        Dim srcRect As New Rectangle(x1 * tileSize.Width, y1 * tileSize.Height, tileSize.Width, tileSize.Height)

                        ' Set the destination rectangle size

                        Dim dstRect As New Rectangle(0, 0, tileSize.Width * _parent._geSurface._scale, tileSize.Height * _parent._geSurface._scale)

                        ' Copy from the source rectangle co-ordinates in the tileset bitmap
                        ' into the destination rectangle co-ordinates in the empty bitmap

                        gr.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
                        gr.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                        gr.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                        gr.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                        gr.CompositingMode = Drawing2D.CompositingMode.SourceCopy
                        gr.DrawImage(pbTileSet.Image, dstRect, srcRect, GraphicsUnit.Pixel)

                    End Using

                    _geTileset.Add(bm.Clone)
                    _count += 1

                Next
            Next

        End Sub

        Public Sub drawTile(t As Integer, p As Point)

            If t - _asciiStart < _count And t - asciiStart >= 0 Then
                Using gr As Graphics = Graphics.FromImage(_parent._geSurface._image)
                    gr.DrawImage(_geTileset(t - _asciiStart), p.X * _parent._geSurface._scale, p.Y * _parent._geSurface._scale)
                End Using
            End If

        End Sub

        ' geFont.name

        Public Property name As String
            Get
                name = _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        ' geFont.count

        Public ReadOnly Property count As Integer
            Get
                count = _count
            End Get
        End Property

        ' geFount.asciiStart

        Public Property asciiStart As Integer
            Get
                asciiStart = _asciiStart
            End Get
            Set(value As Integer)
                If asciiStart > 0 Then
                    If asciiStart > (128 - _count) Then
                        _asciiStart = 1
                    End If
                    _asciiStart = value
                Else
                    _asciiStart = 1
                End If
            End Set
        End Property

        ' geFont.getSize

        Public ReadOnly Property size As Size
            Get
                size = _tileSize
            End Get
        End Property

    End Class

End Class


