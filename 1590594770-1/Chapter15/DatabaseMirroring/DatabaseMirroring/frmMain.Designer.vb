Partial Public Class frmMain
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

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
        Me.components = New System.ComponentModel.Container
        Me.lblPrinicpal = New System.Windows.Forms.Label
        Me.LblMirror = New System.Windows.Forms.Label
        Me.lblWitness = New System.Windows.Forms.Label
        Me.txtPrincipalEndPoint = New System.Windows.Forms.TextBox
        Me.txtMirrorEndPoint = New System.Windows.Forms.TextBox
        Me.txtWitnessEndPoint = New System.Windows.Forms.TextBox
        Me.checkWitness = New System.Windows.Forms.CheckBox
        Me.lblStep1 = New System.Windows.Forms.Label
        Me.lblStep2 = New System.Windows.Forms.Label
        Me.btnSelectDB = New System.Windows.Forms.Button
        Me.btnSetup = New System.Windows.Forms.Button
        Me.lblStep3 = New System.Windows.Forms.Label
        Me.lblDataFlow = New System.Windows.Forms.Label
        Me.txtWitnessServer = New System.Windows.Forms.TextBox
        Me.txtMirrorServer = New System.Windows.Forms.TextBox
        Me.txtPrincipalServer = New System.Windows.Forms.TextBox
        Me.lblWitnessServer = New System.Windows.Forms.Label
        Me.lblMirrorServer = New System.Windows.Forms.Label
        Me.lblPrincipalServer = New System.Windows.Forms.Label
        Me.lblServerNames = New System.Windows.Forms.Label
        Me.lblServerAddresses = New System.Windows.Forms.Label
        Me.btnFail = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.listInterval = New System.Windows.Forms.ListBox
        Me.lblDatabaseName = New System.Windows.Forms.Label
        Me.lblDBMirrorStatus = New System.Windows.Forms.Label
        Me.lblDBMirrorStatusText = New System.Windows.Forms.Label
        Me.lblPrincipalText = New System.Windows.Forms.Label
        Me.lblPrincipal = New System.Windows.Forms.Label
        Me.lblBackupRestorePath = New System.Windows.Forms.Label
        Me.txtBackupRestorePath = New System.Windows.Forms.TextBox
        Me.timerMirroring = New System.Windows.Forms.Timer(Me.components)
        Me.btnTurnOff = New System.Windows.Forms.Button
        Me.lblConnectedServer = New System.Windows.Forms.Label
        Me.lblConnectedServerText = New System.Windows.Forms.Label
        Me.lblDataFlowText = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblPrinicpal
        '
        Me.lblPrinicpal.AutoSize = True
        Me.lblPrinicpal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrinicpal.Location = New System.Drawing.Point(17, 125)
        Me.lblPrinicpal.Name = "lblPrinicpal"
        Me.lblPrinicpal.Size = New System.Drawing.Size(137, 14)
        Me.lblPrinicpal.TabIndex = 0
        Me.lblPrinicpal.Text = "Principal Server &Address:"
        '
        'LblMirror
        '
        Me.LblMirror.AutoSize = True
        Me.LblMirror.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMirror.Location = New System.Drawing.Point(17, 145)
        Me.LblMirror.Name = "LblMirror"
        Me.LblMirror.Size = New System.Drawing.Size(123, 14)
        Me.LblMirror.TabIndex = 1
        Me.LblMirror.Text = "Mi&rror Server Address:"
        '
        'lblWitness
        '
        Me.lblWitness.AutoSize = True
        Me.lblWitness.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWitness.Location = New System.Drawing.Point(17, 165)
        Me.lblWitness.Name = "lblWitness"
        Me.lblWitness.Size = New System.Drawing.Size(189, 14)
        Me.lblWitness.TabIndex = 2
        Me.lblWitness.Text = "W&itness Server Address (Optional):"
        '
        'txtPrincipalEndPoint
        '
        Me.txtPrincipalEndPoint.Location = New System.Drawing.Point(216, 125)
        Me.txtPrincipalEndPoint.Name = "txtPrincipalEndPoint"
        Me.txtPrincipalEndPoint.Size = New System.Drawing.Size(400, 20)
        Me.txtPrincipalEndPoint.TabIndex = 4
        Me.txtPrincipalEndPoint.Text = "TCP://Server:Port"
        '
        'txtMirrorEndPoint
        '
        Me.txtMirrorEndPoint.Location = New System.Drawing.Point(216, 142)
        Me.txtMirrorEndPoint.Name = "txtMirrorEndPoint"
        Me.txtMirrorEndPoint.Size = New System.Drawing.Size(400, 20)
        Me.txtMirrorEndPoint.TabIndex = 5
        Me.txtMirrorEndPoint.Text = "TCP://Server:Port"
        '
        'txtWitnessEndPoint
        '
        Me.txtWitnessEndPoint.Location = New System.Drawing.Point(216, 159)
        Me.txtWitnessEndPoint.Name = "txtWitnessEndPoint"
        Me.txtWitnessEndPoint.Size = New System.Drawing.Size(400, 20)
        Me.txtWitnessEndPoint.TabIndex = 6
        Me.txtWitnessEndPoint.Text = "TCP://Server:Port"
        '
        'checkWitness
        '
        Me.checkWitness.AutoSize = True
        Me.checkWitness.Checked = True
        Me.checkWitness.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkWitness.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.checkWitness.Location = New System.Drawing.Point(17, 185)
        Me.checkWitness.Name = "checkWitness"
        Me.checkWitness.Size = New System.Drawing.Size(93, 17)
        Me.checkWitness.TabIndex = 7
        Me.checkWitness.Text = "&Use Witness"
        '
        'lblStep1
        '
        Me.lblStep1.AutoSize = True
        Me.lblStep1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep1.Location = New System.Drawing.Point(17, 220)
        Me.lblStep1.Name = "lblStep1"
        Me.lblStep1.Size = New System.Drawing.Size(178, 14)
        Me.lblStep1.TabIndex = 7
        Me.lblStep1.Text = "Step 3: Select Database to Mirror"
        '
        'lblStep2
        '
        Me.lblStep2.AutoSize = True
        Me.lblStep2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2.Location = New System.Drawing.Point(17, 290)
        Me.lblStep2.Name = "lblStep2"
        Me.lblStep2.Size = New System.Drawing.Size(362, 14)
        Me.lblStep2.TabIndex = 8
        Me.lblStep2.Text = "Step 4: Setup Mirroring (Backup, Restore, Endpoints, start mirroring)"
        '
        'btnSelectDB
        '
        Me.btnSelectDB.Location = New System.Drawing.Point(21, 240)
        Me.btnSelectDB.Name = "btnSelectDB"
        Me.btnSelectDB.Size = New System.Drawing.Size(110, 23)
        Me.btnSelectDB.TabIndex = 8
        Me.btnSelectDB.Text = "&Select Database"
        '
        'btnSetup
        '
        Me.btnSetup.Enabled = False
        Me.btnSetup.Location = New System.Drawing.Point(17, 365)
        Me.btnSetup.Name = "btnSetup"
        Me.btnSetup.Size = New System.Drawing.Size(110, 23)
        Me.btnSetup.TabIndex = 11
        Me.btnSetup.Text = "Se&tup Mirroring"
        '
        'lblStep3
        '
        Me.lblStep3.AutoSize = True
        Me.lblStep3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3.Location = New System.Drawing.Point(17, 419)
        Me.lblStep3.Name = "lblStep3"
        Me.lblStep3.Size = New System.Drawing.Size(188, 14)
        Me.lblStep3.TabIndex = 11
        Me.lblStep3.Text = "Step 5: Fail Mirroring over to Mirror"
        '
        'lblDataFlow
        '
        Me.lblDataFlow.AutoSize = True
        Me.lblDataFlow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDataFlow.Location = New System.Drawing.Point(21, 507)
        Me.lblDataFlow.Name = "lblDataFlow"
        Me.lblDataFlow.Size = New System.Drawing.Size(57, 14)
        Me.lblDataFlow.TabIndex = 12
        Me.lblDataFlow.Text = "Data Flow"
        '
        'txtWitnessServer
        '
        Me.txtWitnessServer.Location = New System.Drawing.Point(216, 64)
        Me.txtWitnessServer.Name = "txtWitnessServer"
        Me.txtWitnessServer.Size = New System.Drawing.Size(400, 20)
        Me.txtWitnessServer.TabIndex = 3
        Me.txtWitnessServer.Text = "Server\InstanceName"
        '
        'txtMirrorServer
        '
        Me.txtMirrorServer.Location = New System.Drawing.Point(216, 47)
        Me.txtMirrorServer.Name = "txtMirrorServer"
        Me.txtMirrorServer.Size = New System.Drawing.Size(400, 20)
        Me.txtMirrorServer.TabIndex = 2
        Me.txtMirrorServer.Text = "Server\InstanceName"
        '
        'txtPrincipalServer
        '
        Me.txtPrincipalServer.Location = New System.Drawing.Point(216, 30)
        Me.txtPrincipalServer.Name = "txtPrincipalServer"
        Me.txtPrincipalServer.Size = New System.Drawing.Size(400, 20)
        Me.txtPrincipalServer.TabIndex = 1
        Me.txtPrincipalServer.Text = "Server\InstanceName"
        '
        'lblWitnessServer
        '
        Me.lblWitnessServer.AutoSize = True
        Me.lblWitnessServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWitnessServer.Location = New System.Drawing.Point(17, 70)
        Me.lblWitnessServer.Name = "lblWitnessServer"
        Me.lblWitnessServer.Size = New System.Drawing.Size(189, 14)
        Me.lblWitnessServer.TabIndex = 16
        Me.lblWitnessServer.Text = "&Witness Server Address (Optional):"
        '
        'lblMirrorServer
        '
        Me.lblMirrorServer.AutoSize = True
        Me.lblMirrorServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMirrorServer.Location = New System.Drawing.Point(17, 50)
        Me.lblMirrorServer.Name = "lblMirrorServer"
        Me.lblMirrorServer.Size = New System.Drawing.Size(123, 14)
        Me.lblMirrorServer.TabIndex = 15
        Me.lblMirrorServer.Text = "&Mirror Server Address:"
        '
        'lblPrincipalServer
        '
        Me.lblPrincipalServer.AutoSize = True
        Me.lblPrincipalServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrincipalServer.Location = New System.Drawing.Point(17, 30)
        Me.lblPrincipalServer.Name = "lblPrincipalServer"
        Me.lblPrincipalServer.Size = New System.Drawing.Size(137, 14)
        Me.lblPrincipalServer.TabIndex = 14
        Me.lblPrincipalServer.Text = "&Principal Server Address:"
        '
        'lblServerNames
        '
        Me.lblServerNames.AutoSize = True
        Me.lblServerNames.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerNames.Location = New System.Drawing.Point(17, 7)
        Me.lblServerNames.Name = "lblServerNames"
        Me.lblServerNames.Size = New System.Drawing.Size(329, 14)
        Me.lblServerNames.TabIndex = 20
        Me.lblServerNames.Text = "Step 1: Enter SQL Server Names for your Mirror Configuration"
        '
        'lblServerAddresses
        '
        Me.lblServerAddresses.AutoSize = True
        Me.lblServerAddresses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerAddresses.Location = New System.Drawing.Point(17, 99)
        Me.lblServerAddresses.Name = "lblServerAddresses"
        Me.lblServerAddresses.Size = New System.Drawing.Size(312, 14)
        Me.lblServerAddresses.TabIndex = 21
        Me.lblServerAddresses.Text = "Step 2: Enter Server Endpoint Addresses you want created"
        '
        'btnFail
        '
        Me.btnFail.Enabled = False
        Me.btnFail.Location = New System.Drawing.Point(21, 439)
        Me.btnFail.Name = "btnFail"
        Me.btnFail.Size = New System.Drawing.Size(110, 23)
        Me.btnFail.TabIndex = 12
        Me.btnFail.Text = "&Fail!"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(17, 345)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(334, 14)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Please select an interval to insert data into database (in seconds):"
        '
        'listInterval
        '
        Me.listInterval.FormattingEnabled = True
        Me.listInterval.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"})
        Me.listInterval.Location = New System.Drawing.Point(343, 342)
        Me.listInterval.Name = "listInterval"
        Me.listInterval.ScrollAlwaysVisible = True
        Me.listInterval.Size = New System.Drawing.Size(49, 17)
        Me.listInterval.TabIndex = 10
        '
        'lblDatabaseName
        '
        Me.lblDatabaseName.AutoSize = True
        Me.lblDatabaseName.Location = New System.Drawing.Point(152, 245)
        Me.lblDatabaseName.Name = "lblDatabaseName"
        Me.lblDatabaseName.Size = New System.Drawing.Size(0, 0)
        Me.lblDatabaseName.TabIndex = 25
        '
        'lblDBMirrorStatus
        '
        Me.lblDBMirrorStatus.AutoSize = True
        Me.lblDBMirrorStatus.Location = New System.Drawing.Point(133, 370)
        Me.lblDBMirrorStatus.Name = "lblDBMirrorStatus"
        Me.lblDBMirrorStatus.Size = New System.Drawing.Size(124, 14)
        Me.lblDBMirrorStatus.TabIndex = 26
        Me.lblDBMirrorStatus.Text = "Database Mirror Status: "
        '
        'lblDBMirrorStatusText
        '
        Me.lblDBMirrorStatusText.AutoSize = True
        Me.lblDBMirrorStatusText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDBMirrorStatusText.Location = New System.Drawing.Point(255, 370)
        Me.lblDBMirrorStatusText.Name = "lblDBMirrorStatusText"
        Me.lblDBMirrorStatusText.Size = New System.Drawing.Size(74, 14)
        Me.lblDBMirrorStatusText.TabIndex = 27
        Me.lblDBMirrorStatusText.Text = "Unconfigured"
        '
        'lblPrincipalText
        '
        Me.lblPrincipalText.AutoSize = True
        Me.lblPrincipalText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrincipalText.Location = New System.Drawing.Point(255, 390)
        Me.lblPrincipalText.Name = "lblPrincipalText"
        Me.lblPrincipalText.Size = New System.Drawing.Size(74, 14)
        Me.lblPrincipalText.TabIndex = 29
        Me.lblPrincipalText.Text = "Unconfigured"
        '
        'lblPrincipal
        '
        Me.lblPrincipal.AutoSize = True
        Me.lblPrincipal.Location = New System.Drawing.Point(133, 390)
        Me.lblPrincipal.Name = "lblPrincipal"
        Me.lblPrincipal.Size = New System.Drawing.Size(77, 14)
        Me.lblPrincipal.TabIndex = 28
        Me.lblPrincipal.Text = "Principal Role:"
        '
        'lblBackupRestorePath
        '
        Me.lblBackupRestorePath.AutoSize = True
        Me.lblBackupRestorePath.Location = New System.Drawing.Point(17, 322)
        Me.lblBackupRestorePath.Name = "lblBackupRestorePath"
        Me.lblBackupRestorePath.Size = New System.Drawing.Size(189, 14)
        Me.lblBackupRestorePath.TabIndex = 30
        Me.lblBackupRestorePath.Text = "Path for Backup/Restore Operations:"
        '
        'txtBackupRestorePath
        '
        Me.txtBackupRestorePath.Location = New System.Drawing.Point(216, 319)
        Me.txtBackupRestorePath.Name = "txtBackupRestorePath"
        Me.txtBackupRestorePath.Size = New System.Drawing.Size(400, 20)
        Me.txtBackupRestorePath.TabIndex = 9
        Me.txtBackupRestorePath.Text = "Local path (such as C:\) or \\server\share\"
        '
        'timerMirroring
        '
        '
        'btnTurnOff
        '
        Me.btnTurnOff.Enabled = False
        Me.btnTurnOff.Location = New System.Drawing.Point(21, 468)
        Me.btnTurnOff.Name = "btnTurnOff"
        Me.btnTurnOff.Size = New System.Drawing.Size(110, 23)
        Me.btnTurnOff.TabIndex = 13
        Me.btnTurnOff.Text = "Turn off M&irroring"
        '
        'lblConnectedServer
        '
        Me.lblConnectedServer.AutoSize = True
        Me.lblConnectedServer.Location = New System.Drawing.Point(21, 527)
        Me.lblConnectedServer.Name = "lblConnectedServer"
        Me.lblConnectedServer.Size = New System.Drawing.Size(109, 14)
        Me.lblConnectedServer.TabIndex = 33
        Me.lblConnectedServer.Text = "Connected to server:"
        '
        'lblConnectedServerText
        '
        Me.lblConnectedServerText.AutoSize = True
        Me.lblConnectedServerText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConnectedServerText.Location = New System.Drawing.Point(131, 527)
        Me.lblConnectedServerText.Name = "lblConnectedServerText"
        Me.lblConnectedServerText.Size = New System.Drawing.Size(74, 14)
        Me.lblConnectedServerText.TabIndex = 34
        Me.lblConnectedServerText.Text = "Unconfigured"
        '
        'lblDataFlowText
        '
        Me.lblDataFlowText.AutoSize = True
        Me.lblDataFlowText.Location = New System.Drawing.Point(22, 560)
        Me.lblDataFlowText.Name = "lblDataFlowText"
        Me.lblDataFlowText.Size = New System.Drawing.Size(0, 0)
        Me.lblDataFlowText.TabIndex = 35
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(659, 599)
        Me.Controls.Add(Me.lblDataFlowText)
        Me.Controls.Add(Me.lblConnectedServerText)
        Me.Controls.Add(Me.lblConnectedServer)
        Me.Controls.Add(Me.btnTurnOff)
        Me.Controls.Add(Me.txtBackupRestorePath)
        Me.Controls.Add(Me.lblBackupRestorePath)
        Me.Controls.Add(Me.lblPrincipalText)
        Me.Controls.Add(Me.lblPrincipal)
        Me.Controls.Add(Me.lblDBMirrorStatusText)
        Me.Controls.Add(Me.lblDBMirrorStatus)
        Me.Controls.Add(Me.lblDatabaseName)
        Me.Controls.Add(Me.listInterval)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnFail)
        Me.Controls.Add(Me.lblServerAddresses)
        Me.Controls.Add(Me.lblServerNames)
        Me.Controls.Add(Me.txtWitnessServer)
        Me.Controls.Add(Me.txtMirrorServer)
        Me.Controls.Add(Me.txtPrincipalServer)
        Me.Controls.Add(Me.lblWitnessServer)
        Me.Controls.Add(Me.lblMirrorServer)
        Me.Controls.Add(Me.lblPrincipalServer)
        Me.Controls.Add(Me.lblDataFlow)
        Me.Controls.Add(Me.lblStep3)
        Me.Controls.Add(Me.btnSetup)
        Me.Controls.Add(Me.btnSelectDB)
        Me.Controls.Add(Me.lblStep2)
        Me.Controls.Add(Me.lblStep1)
        Me.Controls.Add(Me.checkWitness)
        Me.Controls.Add(Me.txtWitnessEndPoint)
        Me.Controls.Add(Me.txtMirrorEndPoint)
        Me.Controls.Add(Me.txtPrincipalEndPoint)
        Me.Controls.Add(Me.lblWitness)
        Me.Controls.Add(Me.LblMirror)
        Me.Controls.Add(Me.lblPrinicpal)
        Me.Location = New System.Drawing.Point(13, 14)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Database Mirroring Sample Application"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPrinicpal As System.Windows.Forms.Label
    Friend WithEvents LblMirror As System.Windows.Forms.Label
    Friend WithEvents lblWitness As System.Windows.Forms.Label
    Friend WithEvents txtPrincipalEndPoint As System.Windows.Forms.TextBox
    Friend WithEvents txtMirrorEndPoint As System.Windows.Forms.TextBox
    Friend WithEvents txtWitnessEndPoint As System.Windows.Forms.TextBox
    Friend WithEvents checkWitness As System.Windows.Forms.CheckBox
    Friend WithEvents lblStep1 As System.Windows.Forms.Label
    Friend WithEvents lblStep2 As System.Windows.Forms.Label
    Friend WithEvents btnSelectDB As System.Windows.Forms.Button
    Friend WithEvents btnSetup As System.Windows.Forms.Button
    Friend WithEvents lblStep3 As System.Windows.Forms.Label
    Friend WithEvents lblDataFlow As System.Windows.Forms.Label
    Friend WithEvents txtWitnessServer As System.Windows.Forms.TextBox
    Friend WithEvents txtMirrorServer As System.Windows.Forms.TextBox
    Friend WithEvents txtPrincipalServer As System.Windows.Forms.TextBox
    Friend WithEvents lblWitnessServer As System.Windows.Forms.Label
    Friend WithEvents lblMirrorServer As System.Windows.Forms.Label
    Friend WithEvents lblPrincipalServer As System.Windows.Forms.Label
    Friend WithEvents lblServerNames As System.Windows.Forms.Label
    Friend WithEvents lblServerAddresses As System.Windows.Forms.Label
    Friend WithEvents btnFail As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents listInterval As System.Windows.Forms.ListBox
    Friend WithEvents lblDatabaseName As System.Windows.Forms.Label
    Friend WithEvents lblDBMirrorStatus As System.Windows.Forms.Label
    Friend WithEvents lblDBMirrorStatusText As System.Windows.Forms.Label
    Friend WithEvents lblPrincipalText As System.Windows.Forms.Label
    Friend WithEvents lblPrincipal As System.Windows.Forms.Label
    Friend WithEvents lblBackupRestorePath As System.Windows.Forms.Label
    Friend WithEvents txtBackupRestorePath As System.Windows.Forms.TextBox
    Friend WithEvents timerMirroring As System.Windows.Forms.Timer
    Friend WithEvents btnTurnOff As System.Windows.Forms.Button
    Friend WithEvents lblConnectedServer As System.Windows.Forms.Label
    Friend WithEvents lblConnectedServerText As System.Windows.Forms.Label
    Friend WithEvents lblDataFlowText As System.Windows.Forms.Label

End Class
