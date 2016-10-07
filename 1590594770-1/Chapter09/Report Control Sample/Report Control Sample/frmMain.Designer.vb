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
        Me.components = New System.ComponentModel.Container
        Dim ReportDataSource7 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource
        Me.GetProductsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.AdventureWorksDataSet = New Report_Control_Sample.AdventureWorksDataSet
        Me.ReportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer
        Me.ReportViewer2 = New Microsoft.Reporting.WinForms.ReportViewer
        Me.GetProductsTableAdapter = New Report_Control_Sample.AdventureWorksDataSetTableAdapters.GetProductsTableAdapter
        Me.Button1 = New System.Windows.Forms.Button
        Me.ReportViewer3 = New Microsoft.Reporting.WinForms.ReportViewer
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        CType(Me.GetProductsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AdventureWorksDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GetProductsBindingSource
        '
        Me.GetProductsBindingSource.DataMember = "GetProducts"
        Me.GetProductsBindingSource.DataSource = Me.AdventureWorksDataSet
        '
        'AdventureWorksDataSet
        '
        Me.AdventureWorksDataSet.DataSetName = "AdventureWorksDataSet"
        '
        'ReportViewer1
        '
        Me.ReportViewer1.AccessibleName = "WinReportServiceViewer"
        Me.ReportViewer1.AllowDrop = True
        Me.ReportViewer1.AutoScroll = True
        Me.ReportViewer1.BackColor = System.Drawing.Color.White
        Me.ReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ReportViewer1.LocalReport.ReportEmbeddedResource = Nothing
        Me.ReportViewer1.LocalReport.ReportPath = Nothing
        Me.ReportViewer1.Location = New System.Drawing.Point(32, 30)
        Me.ReportViewer1.Name = "ReportViewer1"
        Me.ReportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote
        Me.ReportViewer1.PromptAreaCollapsed = False
        Me.ReportViewer1.ServerReport.DisplayName = "company sales"
        Me.ReportViewer1.ServerReport.ReportPath = "/adventureworks sample reports/company sales"
        Me.ReportViewer1.ShowBackButton = True
        Me.ReportViewer1.ShowContextMenu = True
        Me.ReportViewer1.ShowCredentialPrompts = True
        Me.ReportViewer1.ShowDocumentMapButton = True
        Me.ReportViewer1.ShowExportButton = True
        Me.ReportViewer1.ShowPageNavigationControls = True
        Me.ReportViewer1.ShowParameterPrompts = True
        Me.ReportViewer1.ShowPrintButton = True
        Me.ReportViewer1.ShowProgress = True
        Me.ReportViewer1.ShowPromptAreaButton = True
        Me.ReportViewer1.ShowRefreshButton = True
        Me.ReportViewer1.ShowStopButton = True
        Me.ReportViewer1.ShowToolBar = True
        Me.ReportViewer1.ShowZoomControl = True
        Me.ReportViewer1.Size = New System.Drawing.Size(741, 250)
        Me.ReportViewer1.TabIndex = 0
        Me.ReportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent
        Me.ReportViewer1.ZoomPercent = 100
        '
        'ReportViewer2
        '
        Me.ReportViewer2.AccessibleName = "WinReportServiceViewer"
        Me.ReportViewer2.AllowDrop = True
        Me.ReportViewer2.AutoScroll = True
        Me.ReportViewer2.BackColor = System.Drawing.Color.White
        Me.ReportViewer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        ReportDataSource7.Name = "AdventureWorksDataSet_GetProducts"
        ReportDataSource7.Value = Me.GetProductsBindingSource
        Me.ReportViewer2.LocalReport.DataSources.Add(ReportDataSource7)
        Me.ReportViewer2.LocalReport.DisplayName = "Report_Control_Sample.Report1.rdlc"
        Me.ReportViewer2.LocalReport.ReportEmbeddedResource = "Report_Control_Sample.Report1.rdlc"
        Me.ReportViewer2.LocalReport.ReportPath = Nothing
        Me.ReportViewer2.Location = New System.Drawing.Point(32, 303)
        Me.ReportViewer2.Name = "ReportViewer2"
        Me.ReportViewer2.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
        Me.ReportViewer2.PromptAreaCollapsed = False
        Me.ReportViewer2.ShowBackButton = True
        Me.ReportViewer2.ShowContextMenu = True
        Me.ReportViewer2.ShowCredentialPrompts = True
        Me.ReportViewer2.ShowDocumentMapButton = True
        Me.ReportViewer2.ShowExportButton = True
        Me.ReportViewer2.ShowPageNavigationControls = True
        Me.ReportViewer2.ShowParameterPrompts = True
        Me.ReportViewer2.ShowPrintButton = True
        Me.ReportViewer2.ShowProgress = True
        Me.ReportViewer2.ShowPromptAreaButton = True
        Me.ReportViewer2.ShowRefreshButton = True
        Me.ReportViewer2.ShowStopButton = True
        Me.ReportViewer2.ShowToolBar = True
        Me.ReportViewer2.ShowZoomControl = True
        Me.ReportViewer2.Size = New System.Drawing.Size(741, 256)
        Me.ReportViewer2.TabIndex = 1
        Me.ReportViewer2.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent
        Me.ReportViewer2.ZoomPercent = 100
        '
        'GetProductsTableAdapter
        '
        Me.GetProductsTableAdapter.ClearBeforeFill = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(38, 562)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(89, 22)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Programmatic"
        '
        'ReportViewer3
        '
        Me.ReportViewer3.AccessibleName = "WinReportServiceViewer"
        Me.ReportViewer3.AllowDrop = True
        Me.ReportViewer3.AutoScroll = True
        Me.ReportViewer3.BackColor = System.Drawing.Color.White
        Me.ReportViewer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ReportViewer3.LocalReport.ReportEmbeddedResource = Nothing
        Me.ReportViewer3.LocalReport.ReportPath = Nothing
        Me.ReportViewer3.Location = New System.Drawing.Point(32, 587)
        Me.ReportViewer3.Name = "ReportViewer3"
        Me.ReportViewer3.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
        Me.ReportViewer3.PromptAreaCollapsed = False
        Me.ReportViewer3.ShowBackButton = True
        Me.ReportViewer3.ShowContextMenu = True
        Me.ReportViewer3.ShowCredentialPrompts = True
        Me.ReportViewer3.ShowDocumentMapButton = True
        Me.ReportViewer3.ShowExportButton = True
        Me.ReportViewer3.ShowPageNavigationControls = True
        Me.ReportViewer3.ShowParameterPrompts = True
        Me.ReportViewer3.ShowPrintButton = True
        Me.ReportViewer3.ShowProgress = True
        Me.ReportViewer3.ShowPromptAreaButton = True
        Me.ReportViewer3.ShowRefreshButton = True
        Me.ReportViewer3.ShowStopButton = True
        Me.ReportViewer3.ShowToolBar = True
        Me.ReportViewer3.ShowZoomControl = True
        Me.ReportViewer3.Size = New System.Drawing.Size(741, 151)
        Me.ReportViewer3.TabIndex = 3
        Me.ReportViewer3.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent
        Me.ReportViewer3.ZoomPercent = 100
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(37, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Server-side"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(37, 287)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Client-side"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(837, 743)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ReportViewer3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ReportViewer2)
        Me.Controls.Add(Me.ReportViewer1)
        Me.Name = "frmMain"
        Me.Text = "Report Control Sample"
        CType(Me.GetProductsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AdventureWorksDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ReportViewer1 As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents ReportViewer2 As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents GetProductsBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents AdventureWorksDataSet As Report_Control_Sample.AdventureWorksDataSet
    Friend WithEvents GetProductsTableAdapter As Report_Control_Sample.AdventureWorksDataSetTableAdapters.GetProductsTableAdapter
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ReportViewer3 As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label

End Class
