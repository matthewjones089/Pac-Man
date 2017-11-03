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
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MapEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadMazeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DebugToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DebugToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ControlsToolStripMenuitem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ObjectivesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStipMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.statusText = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip2
        '
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.DebugToolStripMenuItem, Me.HelpToolStipMenuItem})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Size = New System.Drawing.Size(649, 24)
        Me.MenuStrip2.TabIndex = 2
        Me.MenuStrip2.Text = "MenuStrip2"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MapEditorToolStripMenuItem, Me.LoadMazeToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'MapEditorToolStripMenuItem
        '
        Me.MapEditorToolStripMenuItem.Name = "MapEditorToolStripMenuItem"
        Me.MapEditorToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.MapEditorToolStripMenuItem.Text = "Map Editor"
        '

        '
        'LoadMazeToolStripMenuItem
        '
        Me.LoadMazeToolStripMenuItem.Name = "LoadMazeToolStripMenuItem"
        Me.LoadMazeToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.LoadMazeToolStripMenuItem.Text = "Load Maze"
        '

        'DebugToolStripMenuItem
        '
        Me.DebugToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DebugToolStripMenuItem1, Me.FPSToolStripMenuItem})
        Me.DebugToolStripMenuItem.Name = "DebugToolStripMenuItem"
        Me.DebugToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.DebugToolStripMenuItem.Text = "Settings"
        '
        'DebugToolStripMenuItem1
        '
        Me.DebugToolStripMenuItem1.Name = "DebugToolStripMenuItem1"
        Me.DebugToolStripMenuItem1.Size = New System.Drawing.Size(109, 22)
        Me.DebugToolStripMenuItem1.Text = "Debug"
        '
        'FPSToolStripMenuItem
        '
        Me.FPSToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.ToolStripMenuItem3, Me.ToolStripMenuItem5, Me.ToolStripMenuItem4})
        Me.FPSToolStripMenuItem.Name = "FPSToolStripMenuItem"
        Me.FPSToolStripMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.FPSToolStripMenuItem.Text = "FPS"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStipMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ControlsToolStripMenuitem, Me.ObjectivesToolStripMenuItem})
        Me.HelpToolStipMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStipMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.HelpToolStipMenuItem.Text = "Help"
        '
        'ControlsToolStripMenuItem
        '
        Me.ControlsToolStripMenuitem.Name = "ControlsToolStripMenuItem"
        Me.ControlsToolStripMenuitem.Size = New System.Drawing.Size(109, 22)
        Me.ControlsToolStripMenuitem.Text = "Controls"
        '
        'ObjectivesToolStripMenuItem
        '
        Me.ObjectivesToolStripMenuItem.Name = "ObjectivesToolStripMenuItem"
        Me.ObjectivesToolStripMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.ObjectivesToolStripMenuItem.Text = "Objectives"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(92, 22)
        Me.ToolStripMenuItem2.Text = "5"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(92, 22)
        Me.ToolStripMenuItem3.Text = "60"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(92, 22)
        Me.ToolStripMenuItem5.Text = "100"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(92, 22)
        Me.ToolStripMenuItem4.Text = "200"
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
    Friend WithEvents DebugToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPSToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MapEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadMazeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents statusText As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents HelpToolStipMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ControlsToolStripMenuitem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ObjectivesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
