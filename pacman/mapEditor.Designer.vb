<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MapEditor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MapEditor))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.toolStripBlock = New System.Windows.Forms.ToolStripButton()
        Me.toolStripDot = New System.Windows.Forms.ToolStripButton()
        Me.toolStripEnergizer = New System.Windows.Forms.ToolStripButton()
        Me.iconImage = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_EditorInstructions = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_ResetMaze = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStrip_LoadMaze = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_SaveMaze = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_Debug = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripBlock, Me.toolStripDot, Me.toolStripEnergizer})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(359, 47)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'toolStripBlock
        '
        Me.toolStripBlock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolStripBlock.Image = CType(resources.GetObject("toolStripBlock.Image"), System.Drawing.Image)
        Me.toolStripBlock.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolStripBlock.Name = "toolStripBlock"
        Me.toolStripBlock.RightToLeftAutoMirrorImage = True
        Me.toolStripBlock.Size = New System.Drawing.Size(44, 44)
        Me.toolStripBlock.Text = "ToolStripButton1"
        Me.toolStripBlock.ToolTipText = "Add Block"
        '
        'toolStripDot
        '
        Me.toolStripDot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolStripDot.Image = CType(resources.GetObject("toolStripDot.Image"), System.Drawing.Image)
        Me.toolStripDot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolStripDot.Name = "toolStripDot"
        Me.toolStripDot.Size = New System.Drawing.Size(44, 44)
        Me.toolStripDot.Text = "ToolStripButton2"
        Me.toolStripDot.ToolTipText = "Add Dot"
        '
        'toolStripEnergizer
        '
        Me.toolStripEnergizer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolStripEnergizer.Image = CType(resources.GetObject("toolStripEnergizer.Image"), System.Drawing.Image)
        Me.toolStripEnergizer.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolStripEnergizer.Name = "toolStripEnergizer"
        Me.toolStripEnergizer.Size = New System.Drawing.Size(44, 44)
        Me.toolStripEnergizer.Text = "ToolStripButton3"
        Me.toolStripEnergizer.ToolTipText = "Add Energizer"
        '
        'iconImage
        '
        Me.iconImage.ImageStream = CType(resources.GetObject("iconImage.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.iconImage.TransparentColor = System.Drawing.Color.Transparent
        Me.iconImage.Images.SetKeyName(0, "iconBlockOff.png")
        Me.iconImage.Images.SetKeyName(1, "iconBlockAdd.png")
        Me.iconImage.Images.SetKeyName(2, "iconBlockDelete.png")
        Me.iconImage.Images.SetKeyName(3, "iconDotOff.png")
        Me.iconImage.Images.SetKeyName(4, "iconDotAdd.png")
        Me.iconImage.Images.SetKeyName(5, "iconDotDelete.png")
        Me.iconImage.Images.SetKeyName(6, "iconEnergizerOff.png")
        Me.iconImage.Images.SetKeyName(7, "iconEnergizerAdd.png")
        Me.iconImage.Images.SetKeyName(8, "iconEnergizerDelete.png")
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.SettingsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(359, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStrip_ResetMaze, Me.ToolStripMenuItem1, Me.ToolStrip_LoadMaze, Me.ToolStrip_SaveMaze, Me.ToolStripMenuItem2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStrip_EditorInstructions})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'ToolStrip_Instructions
        '
        Me.ToolStrip_EditorInstructions.Name = "ToolStrip_EditorInstructions"
        Me.ToolStrip_EditorInstructions.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_EditorInstructions.Text = "Editor Instructions"
        '
        'ToolStrip_ResetMaze
        '
        Me.ToolStrip_ResetMaze.Name = "ToolStrip_ResetMaze"
        Me.ToolStrip_ResetMaze.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_ResetMaze.Text = "Reset Maze"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(149, 6)
        '
        'ToolStrip_LoadMaze
        '
        Me.ToolStrip_LoadMaze.Name = "ToolStrip_LoadMaze"
        Me.ToolStrip_LoadMaze.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_LoadMaze.Text = "Load Maze"
        '
        'ToolStrip_SaveMaze
        '
        Me.ToolStrip_SaveMaze.Name = "ToolStrip_SaveMaze"
        Me.ToolStrip_SaveMaze.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_SaveMaze.Text = "Save Maze"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(149, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStrip_Debug})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'ToolStrip_Debug
        '
        Me.ToolStrip_Debug.Name = "ToolStrip_Debug"
        Me.ToolStrip_Debug.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_Debug.Text = "Debug"
        '
        'MapEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(359, 261)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MapEditor"
        Me.Text = "Maze Editor"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents toolStripBlock As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripDot As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripEnergizer As System.Windows.Forms.ToolStripButton
    Friend WithEvents iconImage As System.Windows.Forms.ImageList
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_ResetMaze As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStrip_LoadMaze As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_SaveMaze As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_EditorInstructions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_Debug As System.Windows.Forms.ToolStripMenuItem
End Class
