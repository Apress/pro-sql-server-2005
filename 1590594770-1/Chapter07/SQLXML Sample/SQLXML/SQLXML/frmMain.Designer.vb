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
        Me.btnBulkLoad = New System.Windows.Forms.Button
        Me.txtXMLFile = New System.Windows.Forms.TextBox
        Me.lblXML = New System.Windows.Forms.Label
        Me.lblXMLSchema = New System.Windows.Forms.Label
        Me.txtXMLSchema = New System.Windows.Forms.TextBox
        Me.txtSQLQuery = New System.Windows.Forms.TextBox
        Me.lblSQLQuery = New System.Windows.Forms.Label
        Me.groupQueries = New System.Windows.Forms.GroupBox
        Me.radioDataSet = New System.Windows.Forms.RadioButton
        Me.radioTemplate = New System.Windows.Forms.RadioButton
        Me.radioXPath = New System.Windows.Forms.RadioButton
        Me.radioSQLXMLParameter = New System.Windows.Forms.RadioButton
        Me.radioXMLTextReader = New System.Windows.Forms.RadioButton
        Me.radioStreamReader = New System.Windows.Forms.RadioButton
        Me.radioFORXMLClient = New System.Windows.Forms.RadioButton
        Me.radioForXML = New System.Windows.Forms.RadioButton
        Me.txtResults = New System.Windows.Forms.TextBox
        Me.lblResults = New System.Windows.Forms.Label
        Me.radioUpdateGram = New System.Windows.Forms.RadioButton
        Me.groupQueries.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBulkLoad
        '
        Me.btnBulkLoad.Location = New System.Drawing.Point(363, 12)
        Me.btnBulkLoad.Name = "btnBulkLoad"
        Me.btnBulkLoad.Size = New System.Drawing.Size(147, 36)
        Me.btnBulkLoad.TabIndex = 0
        Me.btnBulkLoad.Text = "&Bulkload XML"
        '
        'txtXMLFile
        '
        Me.txtXMLFile.Location = New System.Drawing.Point(142, 35)
        Me.txtXMLFile.Name = "txtXMLFile"
        Me.txtXMLFile.Size = New System.Drawing.Size(215, 20)
        Me.txtXMLFile.TabIndex = 1
        '
        'lblXML
        '
        Me.lblXML.AutoSize = True
        Me.lblXML.Location = New System.Drawing.Point(58, 41)
        Me.lblXML.Name = "lblXML"
        Me.lblXML.Size = New System.Drawing.Size(78, 14)
        Me.lblXML.TabIndex = 2
        Me.lblXML.Text = "XML File Path:"
        '
        'lblXMLSchema
        '
        Me.lblXMLSchema.AutoSize = True
        Me.lblXMLSchema.Location = New System.Drawing.Point(39, 15)
        Me.lblXMLSchema.Name = "lblXMLSchema"
        Me.lblXMLSchema.Size = New System.Drawing.Size(97, 14)
        Me.lblXMLSchema.TabIndex = 4
        Me.lblXMLSchema.Text = "XML Schema File:"
        '
        'txtXMLSchema
        '
        Me.txtXMLSchema.Location = New System.Drawing.Point(142, 9)
        Me.txtXMLSchema.Name = "txtXMLSchema"
        Me.txtXMLSchema.Size = New System.Drawing.Size(215, 20)
        Me.txtXMLSchema.TabIndex = 3
        '
        'txtSQLQuery
        '
        Me.txtSQLQuery.AutoSize = False
        Me.txtSQLQuery.Location = New System.Drawing.Point(142, 180)
        Me.txtSQLQuery.Multiline = True
        Me.txtSQLQuery.Name = "txtSQLQuery"
        Me.txtSQLQuery.ReadOnly = True
        Me.txtSQLQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSQLQuery.Size = New System.Drawing.Size(474, 131)
        Me.txtSQLQuery.TabIndex = 6
        '
        'lblSQLQuery
        '
        Me.lblSQLQuery.AutoSize = True
        Me.lblSQLQuery.Location = New System.Drawing.Point(58, 180)
        Me.lblSQLQuery.Name = "lblSQLQuery"
        Me.lblSQLQuery.Size = New System.Drawing.Size(64, 14)
        Me.lblSQLQuery.TabIndex = 7
        Me.lblSQLQuery.Text = "SQL Query:"
        '
        'groupQueries
        '
        Me.groupQueries.Controls.Add(Me.radioUpdateGram)
        Me.groupQueries.Controls.Add(Me.radioDataSet)
        Me.groupQueries.Controls.Add(Me.radioTemplate)
        Me.groupQueries.Controls.Add(Me.radioXPath)
        Me.groupQueries.Controls.Add(Me.radioSQLXMLParameter)
        Me.groupQueries.Controls.Add(Me.radioXMLTextReader)
        Me.groupQueries.Controls.Add(Me.radioStreamReader)
        Me.groupQueries.Controls.Add(Me.radioFORXMLClient)
        Me.groupQueries.Controls.Add(Me.radioForXML)
        Me.groupQueries.Location = New System.Drawing.Point(12, 83)
        Me.groupQueries.Name = "groupQueries"
        Me.groupQueries.Size = New System.Drawing.Size(613, 71)
        Me.groupQueries.TabIndex = 10
        Me.groupQueries.TabStop = False
        Me.groupQueries.Text = "Test Queries"
        '
        'radioDataSet
        '
        Me.radioDataSet.AutoSize = True
        Me.radioDataSet.Location = New System.Drawing.Point(415, 42)
        Me.radioDataSet.Name = "radioDataSet"
        Me.radioDataSet.Size = New System.Drawing.Size(80, 17)
        Me.radioDataSet.TabIndex = 7
        Me.radioDataSet.Text = "Use Dataset"
        '
        'radioTemplate
        '
        Me.radioTemplate.AutoSize = True
        Me.radioTemplate.Location = New System.Drawing.Point(415, 19)
        Me.radioTemplate.Name = "radioTemplate"
        Me.radioTemplate.Size = New System.Drawing.Size(88, 17)
        Me.radioTemplate.TabIndex = 6
        Me.radioTemplate.Text = "Run Template"
        '
        'radioXPath
        '
        Me.radioXPath.AutoSize = True
        Me.radioXPath.Location = New System.Drawing.Point(268, 42)
        Me.radioXPath.Name = "radioXPath"
        Me.radioXPath.Size = New System.Drawing.Size(104, 17)
        Me.radioXPath.TabIndex = 5
        Me.radioXPath.Text = "Run XPath Query"
        '
        'radioSQLXMLParameter
        '
        Me.radioSQLXMLParameter.AutoSize = True
        Me.radioSQLXMLParameter.Location = New System.Drawing.Point(268, 19)
        Me.radioSQLXMLParameter.Name = "radioSQLXMLParameter"
        Me.radioSQLXMLParameter.Size = New System.Drawing.Size(134, 17)
        Me.radioSQLXMLParameter.TabIndex = 4
        Me.radioSQLXMLParameter.Text = "Use SQLXMLParameter"
        '
        'radioXMLTextReader
        '
        Me.radioXMLTextReader.AutoSize = True
        Me.radioXMLTextReader.Location = New System.Drawing.Point(135, 42)
        Me.radioXMLTextReader.Name = "radioXMLTextReader"
        Me.radioXMLTextReader.Size = New System.Drawing.Size(121, 17)
        Me.radioXMLTextReader.TabIndex = 3
        Me.radioXMLTextReader.Text = "Use XMLTextReader"
        '
        'radioStreamReader
        '
        Me.radioStreamReader.AutoSize = True
        Me.radioStreamReader.Location = New System.Drawing.Point(135, 19)
        Me.radioStreamReader.Name = "radioStreamReader"
        Me.radioStreamReader.Size = New System.Drawing.Size(111, 17)
        Me.radioStreamReader.TabIndex = 2
        Me.radioStreamReader.Text = "Use StreamReader"
        '
        'radioFORXMLClient
        '
        Me.radioFORXMLClient.AutoSize = True
        Me.radioFORXMLClient.Location = New System.Drawing.Point(6, 42)
        Me.radioFORXMLClient.Name = "radioFORXMLClient"
        Me.radioFORXMLClient.Size = New System.Drawing.Size(116, 17)
        Me.radioFORXMLClient.TabIndex = 1
        Me.radioFORXMLClient.Text = "FOR XML Clientside"
        '
        'radioForXML
        '
        Me.radioForXML.AutoSize = True
        Me.radioForXML.Location = New System.Drawing.Point(6, 19)
        Me.radioForXML.Name = "radioForXML"
        Me.radioForXML.Size = New System.Drawing.Size(68, 17)
        Me.radioForXML.TabIndex = 0
        Me.radioForXML.Text = "FOR XML"
        '
        'txtResults
        '
        Me.txtResults.AutoSize = False
        Me.txtResults.Location = New System.Drawing.Point(142, 347)
        Me.txtResults.Multiline = True
        Me.txtResults.Name = "txtResults"
        Me.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResults.Size = New System.Drawing.Size(474, 250)
        Me.txtResults.TabIndex = 11
        '
        'lblResults
        '
        Me.lblResults.AutoSize = True
        Me.lblResults.Location = New System.Drawing.Point(72, 347)
        Me.lblResults.Name = "lblResults"
        Me.lblResults.Size = New System.Drawing.Size(45, 14)
        Me.lblResults.TabIndex = 12
        Me.lblResults.Text = "Results:"
        '
        'radioUpdateGram
        '
        Me.radioUpdateGram.AutoSize = True
        Me.radioUpdateGram.Location = New System.Drawing.Point(509, 19)
        Me.radioUpdateGram.Name = "radioUpdateGram"
        Me.radioUpdateGram.Size = New System.Drawing.Size(101, 17)
        Me.radioUpdateGram.TabIndex = 8
        Me.radioUpdateGram.Text = "Use Updategram"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(628, 609)
        Me.Controls.Add(Me.lblResults)
        Me.Controls.Add(Me.txtResults)
        Me.Controls.Add(Me.groupQueries)
        Me.Controls.Add(Me.lblSQLQuery)
        Me.Controls.Add(Me.txtSQLQuery)
        Me.Controls.Add(Me.lblXMLSchema)
        Me.Controls.Add(Me.txtXMLSchema)
        Me.Controls.Add(Me.lblXML)
        Me.Controls.Add(Me.txtXMLFile)
        Me.Controls.Add(Me.btnBulkLoad)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SQLXML Sample Application"
        Me.groupQueries.ResumeLayout(False)
        Me.groupQueries.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBulkLoad As System.Windows.Forms.Button
    Friend WithEvents txtXMLFile As System.Windows.Forms.TextBox
    Friend WithEvents lblXML As System.Windows.Forms.Label
    Friend WithEvents lblXMLSchema As System.Windows.Forms.Label
    Friend WithEvents txtXMLSchema As System.Windows.Forms.TextBox
    Friend WithEvents txtSQLQuery As System.Windows.Forms.TextBox
    Friend WithEvents lblSQLQuery As System.Windows.Forms.Label
    Friend WithEvents groupQueries As System.Windows.Forms.GroupBox
    Friend WithEvents radioXMLTextReader As System.Windows.Forms.RadioButton
    Friend WithEvents radioStreamReader As System.Windows.Forms.RadioButton
    Friend WithEvents radioFORXMLClient As System.Windows.Forms.RadioButton
    Friend WithEvents radioForXML As System.Windows.Forms.RadioButton
    Friend WithEvents radioSQLXMLParameter As System.Windows.Forms.RadioButton
    Friend WithEvents txtResults As System.Windows.Forms.TextBox
    Friend WithEvents lblResults As System.Windows.Forms.Label
    Friend WithEvents radioXPath As System.Windows.Forms.RadioButton
    Friend WithEvents radioTemplate As System.Windows.Forms.RadioButton
    Friend WithEvents radioDataSet As System.Windows.Forms.RadioButton
    Friend WithEvents radioUpdateGram As System.Windows.Forms.RadioButton

End Class
