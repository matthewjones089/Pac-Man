<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class pacman
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
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_NewGame = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_LoadMaze = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_MapEditor = New System.Windows.Forms.ToolStripMenuItem()
        Me.DebugToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_Debug = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_FPS5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_FPS60 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_FPS100 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_FPS200 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_ResetHighscore = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_EnableInvincibility = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStipMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_Instructions = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.statusText = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStrip_FPS50 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip2
        '
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ViewToolStripMenuItem, Me.DebugToolStripMenuItem, Me.HelpToolStipMenuItem})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Size = New System.Drawing.Size(649, 24)
        Me.MenuStrip2.TabIndex = 2
        Me.MenuStrip2.Text = "MenuStrip2"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStrip_NewGame, Me.ToolStrip_LoadMaze, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ToolStrip_NewGame
        '
        Me.ToolStrip_NewGame.Name = "ToolStrip_NewGame"
        Me.ToolStrip_NewGame.Size = New System.Drawing.Size(132, 22)
        Me.ToolStrip_NewGame.Text = "New Game"
        '
        'ToolStrip_LoadMaze
        '
        Me.ToolStrip_LoadMaze.Name = "ToolStrip_LoadMaze"
        Me.ToolStrip_LoadMaze.Size = New System.Drawing.Size(132, 22)
        Me.ToolStrip_LoadMaze.Text = "Load Maze"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStrip_MapEditor})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'ToolStrip_MapEditor
        '
        Me.ToolStrip_MapEditor.Name = "ToolStrip_MapEditor"
        Me.ToolStrip_MapEditor.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_MapEditor.Text = "Maze Editor"
        '
        'DebugToolStripMenuItem
        '
        Me.DebugToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStrip_Debug, Me.FPSToolStripMenuItem, Me.ToolStrip_ResetHighscore, Me.ToolStrip_EnableInvincibility})
        Me.DebugToolStripMenuItem.Name = "DebugToolStripMenuItem"
        Me.DebugToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.DebugToolStripMenuItem.Text = "Settings"
        '
        'ToolStrip_Debug
        '
        Me.ToolStrip_Debug.Name = "ToolStrip_Debug"
        Me.ToolStrip_Debug.Size = New System.Drawing.Size(173, 22)
        Me.ToolStrip_Debug.Text = "Debug"
        '
        'FPSToolStripMenuItem
        '
        Me.FPSToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStrip_FPS5, Me.ToolStrip_FPS50, Me.ToolStrip_FPS60, Me.ToolStrip_FPS100, Me.ToolStrip_FPS200})
        Me.FPSToolStripMenuItem.Name = "FPSToolStripMenuItem"
        Me.FPSToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.FPSToolStripMenuItem.Text = "FPS"
        '
        'ToolStrip_FPS5
        '
        Me.ToolStrip_FPS5.Name = "ToolStrip_FPS5"
        Me.ToolStrip_FPS5.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_FPS5.Text = "5"
        '
        'ToolStrip_FPS60
        '
        Me.ToolStrip_FPS60.Checked = True
        Me.ToolStrip_FPS60.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStrip_FPS60.Name = "ToolStrip_FPS60"
        Me.ToolStrip_FPS60.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_FPS60.Text = "60 [NTSC]"
        '
        'ToolStrip_FPS100
        '
        Me.ToolStrip_FPS100.Name = "ToolStrip_FPS100"
        Me.ToolStrip_FPS100.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_FPS100.Text = "100"
        '
        'ToolStrip_FPS200
        '
        Me.ToolStrip_FPS200.Name = "ToolStrip_FPS200"
        Me.ToolStrip_FPS200.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_FPS200.Text = "200"
        '
        'ToolStrip_ResetHighscore
        '
        Me.ToolStrip_ResetHighscore.Name = "ToolStrip_ResetHighscore"
        Me.ToolStrip_ResetHighscore.Size = New System.Drawing.Size(173, 22)
        Me.ToolStrip_ResetHighscore.Text = "Reset Highscore"
        '
        'ToolStrip_EnableInvincibility
        '
        Me.ToolStrip_EnableInvincibility.Name = "ToolStrip_EnableInvincibility"
        Me.ToolStrip_EnableInvincibility.Size = New System.Drawing.Size(173, 22)
        Me.ToolStrip_EnableInvincibility.Text = "Enable Invincibility"
        '
        'HelpToolStipMenuItem
        '
        Me.HelpToolStipMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStrip_Instructions})
        Me.HelpToolStipMenuItem.Name = "HelpToolStipMenuItem"
        Me.HelpToolStipMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStipMenuItem.Text = "Help"
        '
        'ToolStrip_Instructions
        '
        Me.ToolStrip_Instructions.Name = "ToolStrip_Instructions"
        Me.ToolStrip_Instructions.Size = New System.Drawing.Size(136, 22)
        Me.ToolStrip_Instructions.Text = "Instructions"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.statusText})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 296)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(649, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'statusText
        '
        Me.statusText.Name = "statusText"
        Me.statusText.Size = New System.Drawing.Size(120, 17)
        Me.statusText.Text = "ToolStripStatusLabel1"
        '
        'ToolStrip_FPS50
        '
        Me.ToolStrip_FPS50.Name = "ToolStrip_FPS50"
        Me.ToolStrip_FPS50.Size = New System.Drawing.Size(152, 22)
        Me.ToolStrip_FPS50.Text = "50 [PAL]"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(149, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'pacman
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(649, 318)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Name = "pacman"
        Me.Text = "Pac-Man by M. Jones 2017"
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip2 As System.Windows.Forms.MenuStrip
    Friend WithEvents DebugToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_Debug As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPSToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_FPS5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_FPS60 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_FPS200 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_MapEditor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_FPS100 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents statusText As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents HelpToolStipMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_Instructions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_NewGame As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_LoadMaze As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_ResetHighscore As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_EnableInvincibility As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_FPS50 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
