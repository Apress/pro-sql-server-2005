Public Class frmMain
    Dim con As System.Data.SqlClient.SqlConnection

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'AdventureWorksDataSet.GetProducts' table. You can move, or remove it, as needed.
        Me.GetProductsTableAdapter.Fill(Me.AdventureWorksDataSet.GetProducts)

        Me.ReportViewer1.RefreshReport()
        Me.ReportViewer2.RefreshReport()
    End Sub

    Private Function GetData() As System.Data.DataSet

        con = New System.Data.SqlClient.SqlConnection("server=localhost;Database=AdventureWorks;Integrated Security=SSPI")

        Using con

            con.Open()


            Dim sda As New System.Data.SqlClient.SqlDataAdapter("EXEC dbo.GetProducts", con)

            Dim ds As New System.Data.DataSet
            sda.Fill(ds)
            con.Close()
            Return ds
        End Using
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        
        Dim ds As System.Data.DataSet
        ds = GetData()
        Dim dt As System.Data.DataTable
        dt = ds.Tables(0)

        ReportViewer3.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        ReportViewer3.LocalReport.ReportEmbeddedResource = "Report_Control_Sample.Report1.rdlc"
        ReportViewer3.LocalReport.ReportPath = Nothing

        ReportViewer3.ShowToolBar = True
        ReportViewer3.ShowDocumentMapButton = True
        ReportViewer3.ShowContextMenu = True
        ReportViewer3.ShowParameterPrompts = True
        ReportViewer3.Name = "My Report"
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource("AdventureWorksDataSet_GetProducts", dt)
        ReportViewer3.LocalReport.DataSources.Add(rds)
        ReportViewer3.RefreshReport()

    End Sub


End Class
