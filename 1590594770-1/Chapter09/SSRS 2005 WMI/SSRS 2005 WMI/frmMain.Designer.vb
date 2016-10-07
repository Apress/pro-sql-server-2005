<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnExecute = New System.Windows.Forms.Button
        Me.listResults = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.listMethods = New System.Windows.Forms.ListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnExecuteMethod = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(18, 39)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(99, 35)
        Me.btnExecute.TabIndex = 0
        Me.btnExecute.Text = "Get props + methods"
        '
        'listResults
        '
        Me.listResults.FormattingEnabled = True
        Me.listResults.Location = New System.Drawing.Point(18, 113)
        Me.listResults.Name = "listResults"
        Me.listResults.Size = New System.Drawing.Size(261, 394)
        Me.listResults.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "RS Instance Properties"
        '
        'listMethods
        '
        Me.listMethods.FormattingEnabled = True
        Me.listMethods.Location = New System.Drawing.Point(285, 113)
        Me.listMethods.Name = "listMethods"
        Me.listMethods.Size = New System.Drawing.Size(261, 394)
        Me.listMethods.Sorted = True
        Me.listMethods.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(284, 97)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "RS Instance Methods"
        '
        'btnExecuteMethod
        '
        Me.btnExecuteMethod.Location = New System.Drawing.Point(142, 39)
        Me.btnExecuteMethod.Name = "btnExecuteMethod"
        Me.btnExecuteMethod.Size = New System.Drawing.Size(99, 35)
        Me.btnExecuteMethod.TabIndex = 6
        Me.btnExecuteMethod.Text = "Execute Method"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(635, 546)
        Me.Controls.Add(Me.btnExecuteMethod)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.listMethods)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.listResults)
        Me.Controls.Add(Me.btnExecute)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SSRS 2005 WMI Sample"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnExecute As System.Windows.Forms.Button
    Friend WithEvents listResults As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents listMethods As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExecuteMethod As System.Windows.Forms.Button

End Class
