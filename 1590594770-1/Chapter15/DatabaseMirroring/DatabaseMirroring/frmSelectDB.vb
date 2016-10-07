Imports System.Data.SqlClient

Public Class frmSelectDB
    Dim strDatabaseServerName As String = ""
    Dim strConnectionString As String = ""
    Dim strSQL As String = ""

       
    Private Sub frmSelectDB_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        frmMain.Show()
    End Sub

    Private Sub frmSelectDB_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus


    End Sub


    Private Sub frmSelectDB_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Attempt to open connection to the principal database from main
        strDatabaseServerName = frmMain.txtPrincipalServer.Text()
        strConnectionString = "Initial Catalog = Master;Data Source = " & strDatabaseServerName & ";Trusted_Connection=Yes;"
        strSQL = "SELECT name FROM sys.databases"
        Dim sqlConnection As New SqlConnection(strConnectionString)
        Dim sqlDataAdapter As New SqlDataAdapter

        sqlDataAdapter.SelectCommand = New SqlCommand(strSQL, sqlConnection)
        Try
            With sqlConnection
                .Open()
                Dim sqlDS As System.Data.DataSet = New System.Data.DataSet("Databases")
                sqlDataAdapter.Fill(sqlDS)
                Dim tRow As System.Data.DataRow
                For Each tRow In sqlDS.Tables.Item(0).Rows
                    'Make sure to not include Master, Tempdb, Model and msdb
                    If Not (UCase(tRow("Name")) = "MASTER" Or UCase(tRow("Name")) = "TEMPDB" Or UCase(tRow("Name")) = "MODEL" Or UCase(tRow("Name")) = "MSDB") Then
                        listDatabases.Items.Add(tRow("Name").ToString())
                    End If
                Next
                'Close the connection
                .Close()
            End With

        Catch
            MsgBox("Error!  Error#" & Err.Number & " Description: " & Err.Description)
            'Close our connection
            sqlConnection.Close()
            listDatabases.Items.Add("Error. Please close form and try again.")
            listDatabases.Enabled = False

        End Try
    End Sub

    Private Sub listDatabases_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles listDatabases.DoubleClick
        If UCase(listDatabases.SelectedItem.ToString()) = "PUBS" Then
        Else
            MsgBox("This application only works with pubs.")
            Exit Sub
        End If
        Me.Hide()
        SetDatabaseNameLabel()
        frmMain.Show()
    End Sub

    Private Sub listDatabases_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listDatabases.SelectedIndexChanged

    End Sub
    Private Sub SetDatabaseNameLabel()
        'Return back the name of the database
        frmMain.lblDatabaseName.Text = listDatabases.Items.Item(listDatabases.SelectedIndex()).ToString()
        frmMain.SetDatabaseName()
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Hide()
        frmMain.Show()
    End Sub

    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If UCase(listDatabases.SelectedItem.ToString()) = "PUBS" Then
        Else
            MsgBox("This application only works with pubs.")
            Exit Sub
        End If

        Me.Hide()
        SetDatabaseNameLabel()
        frmMain.Show()
    End Sub
End Class