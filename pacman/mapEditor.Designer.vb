<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MJ_MapEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MJ_MapEditor))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.toolStripBlock = New System.Windows.Forms.ToolStripButton()
        Me.toolStripDot = New System.Windows.Forms.ToolStripButton()
        Me.toolStripEnergizer = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolStripReset = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolStripSave = New System.Windows.Forms.ToolStripButton()
        Me.toolStripLoad = New System.Windows.Forms.ToolStripButton()
        Me.toolStripInfo = New System.Windows.Forms.ToolStripButton()
        Me.iconImage = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(40, 40)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripBlock, Me.toolStripDot, Me.toolStripEnergizer, Me.ToolStripSeparator1, Me.toolStripReset, Me.ToolStripSeparator2, Me.toolStripSave, Me.toolStripLoad, Me.toolStripInfo})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
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
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 47)
        '
        'toolStripReset
        '
        Me.toolStripReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolStripReset.Image = CType(resources.GetObject("toolStripReset.Image"), System.Drawing.Image)
        Me.toolStripReset.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolStripReset.Name = "toolStripReset"
        Me.toolStripReset.Size = New System.Drawing.Size(44, 44)
        Me.toolStripReset.Text = "ToolStripButton1"
        Me.toolStripReset.ToolTipText = "Reset Maze"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 47)
        '
        'toolStripSave
        '
        Me.toolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolStripSave.Image = CType(resources.GetObject("toolStripSave.Image"), System.Drawing.Image)
        Me.toolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolStripSave.Name = "toolStripSave"
        Me.toolStripSave.Size = New System.Drawing.Size(44, 44)
        Me.toolStripSave.Text = "ToolStripButton1"
        Me.toolStripSave.ToolTipText = "Save Maze"
        '
        'toolStripLoad
        '
        Me.toolStripLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolStripLoad.Image = CType(resources.GetObject("toolStripLoad.Image"), System.Drawing.Image)
        Me.toolStripLoad.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolStripLoad.Name = "toolStripLoad"
        Me.toolStripLoad.Size = New System.Drawing.Size(44, 44)
        Me.toolStripLoad.Text = "ToolStripButton2"
        Me.toolStripLoad.ToolTipText = "Load Maze"
        '
        'toolStripInfo
        '
        Me.toolStripInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolStripInfo.Image = CType(resources.GetObject("toolStripInfo.Image"), System.Drawing.Image)
        Me.toolStripInfo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolStripInfo.Name = "toolStripInfo"
        Me.toolStripInfo.Size = New System.Drawing.Size(44, 44)
        Me.toolStripInfo.Text = "ToolStripButton3"
        Me.toolStripInfo.ToolTipText = "Map Editor Instructions"
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
        Me.iconImage.Images.SetKeyName(9, "iconInfo.png")
        '
        'MJ_MapEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(359, 261)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "MJ_MapEditor"
        Me.Text = "MJ_MapEditor"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents toolStripBlock As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripDot As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripEnergizer As System.Windows.Forms.ToolStripButton
    Friend WithEvents iconImage As System.Windows.Forms.ImageList
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents toolStripReset As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents toolStripSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripLoad As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripInfo As System.Windows.Forms.ToolStripButton
End Class
