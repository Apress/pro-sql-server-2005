Imports System.Data.SqlClient

Public Class frmMain

    Dim sqlConnection As New SqlConnection
    Dim sqlCommand As New SqlCommand
    Dim strConnectionStringPrincipal As String = ""
    Dim strConnectionStringMirror As String = ""
    Dim strConnectionStringWitness As String = ""
    Dim strPrincipalName As String = ""
    Dim strMirrorName As String = ""
    Dim strWitnessName As String = ""
    Dim strDatabaseName As String = ""
    Dim strBackupRestorePath As String = ""
    Dim iPubsCounter As Integer = 9901

    Private Sub btnSelectDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectDB.Click
        'Bring up a list of databases off the Principal Server
        'Launch the Select Database Form
        Me.Hide()
        frmSelectDB.Show()

    End Sub

    Private Sub btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        'Attempt to setup mirroring based on the information provided by the user
        'The application will backup to the root of C:/ and restore the database to
        'the root of C:/

        'Set the cursor so the user knows we're working
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'First, connect to the Principal and make sure it's in Full Recovery Mode


        Dim boolError As Boolean = False
        Dim strSQL As String = ""
        strDatabaseName = lblDatabaseName.Text()
        strPrincipalName = txtPrincipalServer.Text()
        strMirrorName = txtMirrorServer.Text()
        strWitnessName = txtWitnessServer.Text()
        strBackupRestorePath = txtBackupRestorePath.Text()

        'Create our connection string
        strConnectionStringPrincipal = "Initial Catalog = " & strDatabaseName & _
            ";Data Source = " & strPrincipalName & ";Trusted_Connection=Yes;"

        'Attempt to make it full recovery
        strSQL = "ALTER DATABASE [" & strDatabaseName & "] SET RECOVERY FULL;"
        boolError = ExecuteNonQuery(strConnectionStringPrincipal, strSQL)
        If (boolError) Then
            MsgBox("Error making recovery model full for principal. Exiting setup.")
            SetCursorBack()
            Exit Sub
        Else
            MsgBox("Success! Recovery model set to full.")
        End If

        'Backup the database
        strSQL = "BACKUP DATABASE [" & strDatabaseName & "] TO  DISK = N'" & strBackupRestorePath & _
            strDatabaseName & ".bak' WITH NOFORMAT, INIT,  NAME = N'" & strDatabaseName & _
            "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
        boolError = ExecuteNonQuery(strConnectionStringPrincipal, strSQL)
        If (boolError) Then
            MsgBox("Error backing up principal. Exiting setup.")
            SetCursorBack()
            Exit Sub
        Else
            MsgBox("Success! " & strDatabaseName & " successfully backed up.")
        End If

        'Restore the database to the mirror
        strConnectionStringMirror = "Initial Catalog = Master" & _
            ";Data Source = " & strMirrorName & ";Trusted_Connection=Yes;"

        strSQL = "RESTORE DATABASE [" & strDatabaseName & "] FROM  DISK = N'" & strBackupRestorePath & _
            strDatabaseName & ".bak' WITH  FILE = 1, MOVE N'" & _
            strDatabaseName & "' TO N'" & strBackupRestorePath & strDatabaseName & ".mdf',  MOVE N'" & _
            strDatabaseName & "_log' TO N'C:\" & strDatabaseName & ".ldf',  " & _
            "NORECOVERY,  NOUNLOAD, REPLACE, STATS = 10"
        boolError = ExecuteNonQuery(strConnectionStringMirror, strSQL)
        If (boolError) Then
            MsgBox("Error restoring to mirror. Exiting setup.")
            SetCursorBack()
            Exit Sub
        Else
            MsgBox("Success! " & strDatabaseName & " restored to mirror.")
        End If

        'Create the endpoints.  Start on the principal

        'See if the endpoint already exists and if so, drop it
        'since we can only have one mirroring endpoint per server
        strSQL = "SELECT name from sys.endpoints WHERE name = 'MirroringPrincipalSampleEndPoint'"

        Dim strReturnValue As String = CStr(ExecuteScalar(strConnectionStringPrincipal, strSQL))
        If strReturnValue = "MirroringPrincipalSampleEndPoint" Then
            'It exists, drop it
            strSQL = "DROP ENDPOINT MirroringPrincipalSampleEndPoint"
            boolError = ExecuteNonQuery(strConnectionStringPrincipal, strSQL)
            If (boolError) Then
                MsgBox("Error dropping endpoint on the Principal.  Exiting setup.")
                SetCursorBack()
                Exit Sub
            End If
        End If


        Dim strPrincipalPort As String = RetrievePortNumber(txtPrincipalEndPoint.Text)

        strSQL = "CREATE ENDPOINT [MirroringPrincipalSampleEndPoint] STATE = STARTED" & _
            " AS TCP (LISTENER_PORT = " & strPrincipalPort & ", LISTENER_IP = ALL," & _
            " RESTRICT_IP=NONE) FOR DATABASE_MIRRORING (ROLE = PARTNER," & _
            " ENCRYPTION = DISABLED)"

        'Create the endpoint
        boolError = ExecuteNonQuery(strConnectionStringPrincipal, strSQL)
        If (boolError) Then
            MsgBox("Error creating endpoint on the Principal.  Exiting setup.")
            SetCursorBack()
            Exit Sub
        Else
            MsgBox("Success! Created endpoint on Principal.")
        End If

        'Create the mirror endpoint
        'If it exists, drop it
        strSQL = "SELECT name from sys.endpoints WHERE name = 'MirroringMirrorSampleEndPoint'"

        strReturnValue = CStr(ExecuteScalar(strConnectionStringMirror, strSQL))
        If strReturnValue = "MirroringMirrorSampleEndPoint" Then
            'It exists, drop it
            strSQL = "DROP ENDPOINT MirroringMirrorSampleEndPoint"
            boolError = ExecuteNonQuery(strConnectionStringMirror, strSQL)
            If (boolError) Then
                MsgBox("Error dropping endpoint on the Mirror.  Exiting setup.")
                SetCursorBack()
                Exit Sub
            End If
        End If


        Dim strMirrorPort As String = RetrievePortNumber(txtMirrorEndPoint.Text)

        strSQL = "CREATE ENDPOINT [MirroringMirrorSampleEndPoint] STATE = STARTED" & _
            " AS TCP (LISTENER_PORT = " & strMirrorPort & ", LISTENER_IP = ALL," & _
            " RESTRICT_IP=NONE) FOR DATABASE_MIRRORING (ROLE = PARTNER," & _
            " ENCRYPTION = DISABLED)"

        'Create the endpoint
        boolError = ExecuteNonQuery(strConnectionStringMirror, strSQL)
        If (boolError) Then
            MsgBox("Error creating endpoint on the Mirror.  Exiting setup.")
            SetCursorBack()
            Exit Sub
        Else
            MsgBox("Success! Created endpoint on Mirror.")
        End If


        strConnectionStringWitness = "Initial Catalog = Master" & _
            ";Data Source = " & strWitnessName & ";Trusted_Connection=Yes;"

        'Check to see if they want to use a witness.
        If checkWitness.Checked = True Then

            'Create the witness endpoint
            'If it exists, drop it
            strSQL = "SELECT name from sys.endpoints WHERE name = 'MirroringWitnessSampleEndPoint'"

            strReturnValue = CStr(ExecuteScalar(strConnectionStringWitness, strSQL))
            If strReturnValue = "MirroringWitnessSampleEndPoint" Then
                'It exists, drop it
                strSQL = "DROP ENDPOINT MirroringWitnessSampleEndPoint"
                boolError = ExecuteNonQuery(strConnectionStringWitness, strSQL)
                If (boolError) Then
                    MsgBox("Error dropping endpoint on the Witness.  Exiting setup.")
                    SetCursorBack()
                    Exit Sub
                End If
            End If


            Dim strWitnessPort As String = RetrievePortNumber(txtWitnessEndPoint.Text)

            strSQL = "CREATE ENDPOINT [MirroringWitnessSampleEndPoint] STATE = STARTED" & _
                " AS TCP (LISTENER_PORT = " & strWitnessPort & ", LISTENER_IP = ALL," & _
                " RESTRICT_IP=NONE) FOR DATABASE_MIRRORING (ROLE = WITNESS," & _
                " ENCRYPTION = DISABLED)"

            'Create the endpoint
            boolError = ExecuteNonQuery(strConnectionStringWitness, strSQL)
            If (boolError) Then
                MsgBox("Error creating endpoint on the Witness.  Exiting setup.")
                SetCursorBack()
                Exit Sub
            Else
                MsgBox("Success! Created endpoint on Witness.")
            End If

        End If

        'Alter the mirror database to set the partner
        strSQL = "ALTER DATABASE " & strDatabaseName & " SET PARTNER = '" & txtPrincipalEndPoint.Text & "'"

        boolError = ExecuteNonQuery(strConnectionStringMirror, strSQL)
        If (boolError) Then
            MsgBox("Error setting partner on the Mirror.  Exiting setup.")
            SetCursorBack()
            Exit Sub
        Else
            MsgBox("Success! Set partner on the mirror.")
        End If


        'Alter the principal database to set the partner
        strSQL = "ALTER DATABASE " & strDatabaseName & " SET PARTNER = '" & txtMirrorEndPoint.Text & "'"

        boolError = ExecuteNonQuery(strConnectionStringPrincipal, strSQL)
        If (boolError) Then
            MsgBox("Error setting partner on the Principal.  Exiting setup.")
            SetCursorBack()
            Exit Sub
        Else
            MsgBox("Success! Set partner on the principal.")
        End If


        'Check to see if they want to use a witness.
        If checkWitness.Checked = True Then
            'Set Witness
            strSQL = "ALTER DATABASE " & strDatabaseName & " SET WITNESS = '" & txtWitnessEndPoint.Text & "'"

            boolError = ExecuteNonQuery(strConnectionStringPrincipal, strSQL)
            If (boolError) Then
                MsgBox("Error setting witness on the Principal.  Exiting setup.")
                SetCursorBack()
                Exit Sub
            Else
                MsgBox("Success! Set witness on the principal.")
            End If


        End If


        'Set the timer interval and turn it on
        timerMirroring.Interval = Int(listInterval.SelectedItem) * 1000
        timerMirroring.Enabled = True

        'Enable the Fail and Turn Off controls
        btnFail.Enabled = True
        btnTurnOff.Enabled = True

        SetCursorBack()
    End Sub
    Private Function RetrievePortNumber(ByVal strTCPAddress As String) As String
        'Parse out the port from the endpoint string in the UI
        Dim iColon As Integer = InStr(strTCPAddress, ":")
        'That's the first colon, go to the second one
        iColon = InStr(iColon + 1, strTCPAddress, ":")


        'Take everything to the right of the colon
        RetrievePortNumber = Mid(strTCPAddress, iColon + 1)

    End Function
    Private Sub SetCursorBack()
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub
    Private Function ExecuteNonQuery(ByVal strConnectString As String, ByVal strSQLCommand As String) As Boolean
        Dim strSQL As String = ""
        Dim sqlConnection As New SqlConnection(strConnectString)

        Try
            With sqlConnection
                .Open()
                Dim sqlCommand As New SqlCommand(strSQLCommand, sqlConnection)
                sqlCommand.ExecuteNonQuery()
                'Close the connection
                .Close()
                ExecuteNonQuery = False
            End With

        Catch
            MsgBox("Error!  Error#" & Err.Number & " Description: " & Err.Description)
            'Close our connection
            sqlConnection.Close()
            ExecuteNonQuery = True
        End Try

    End Function
    Private Function ExecuteNonQueryForMirror(ByVal strConnectString As String, ByVal strSQLCommand As String) As Boolean
        'Execute non-query after mirror is established

        Dim strSQL As String = ""
        Dim sqlConnection As New SqlConnection(strConnectString)

        Try
            With sqlConnection
                .Open()
                'Write out the datasource of the connection to see
                'if we're talking to the prinicipal or the mirror
                lblConnectedServerText.Text = sqlConnection.DataSource()

                Dim sqlCommand As New SqlCommand(strSQLCommand, sqlConnection)
                sqlCommand.ExecuteNonQuery()

                
                'Close the connection
                .Close()
                ExecuteNonQueryForMirror = False
            End With

        Catch
            MsgBox("Error!  Error#" & Err.Number & " Description: " & Err.Description)
            'Close our connection
            sqlConnection.Close()
            ExecuteNonQueryForMirror = True
        End Try

    End Function
    Private Function ExecuteScalar(ByVal strConnectString As String, ByVal strSQLCommand As String) As Object
        Dim strSQL As String = ""
        Dim sqlConnection As New SqlConnection(strConnectString)

        Try
            With sqlConnection
                .Open()
                Dim sqlCommand As New SqlCommand(strSQLCommand, sqlConnection)
                Dim oReturnValue As Object = sqlCommand.ExecuteScalar()
                'Close the connection
                .Close()
                If IsDBNull(oReturnValue) Then
                    oReturnValue = ""
                End If
                ExecuteScalar = oReturnValue
            End With

        Catch
            MsgBox("Error!  Error#" & Err.Number & " Description: " & Err.Description)
            'Close our connection
            sqlConnection.Close()
            ExecuteScalar = True
        End Try

    End Function
    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'REMOVE THIS!
        txtPrincipalServer.Text = "thomrizwinxp6"
        txtMirrorServer.Text = "thomrizwinxp6\server2"
        txtWitnessServer.Text = "thomrizwinxp6\server3"

        txtPrincipalEndPoint.Text = "TCP://thomrizwinxp6.redmond.corp.microsoft.com:6000"
        txtMirrorEndPoint.Text = "TCP://thomrizwinxp6.redmond.corp.microsoft.com:6001"
        txtWitnessEndPoint.Text = "TCP://thomrizwinxp6.redmond.corp.microsoft.com:6002"
        txtBackupRestorePath.Text = "C:\"

        listInterval.SelectedItem = "5"
    End Sub

    Private Sub timerMirroring_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles timerMirroring.Tick
        SetDBMirrorStatusandPrincipal()
        InsertDataIntoDatabase()

    End Sub
    Private Sub InsertDataIntoDatabase()
        Dim strSQL As String = ""
        Dim boolError As Boolean = False

        strConnectionStringPrincipal = "Initial Catalog = " & strDatabaseName & _
            ";Data Source = " & strPrincipalName & ";Trusted_Connection=Yes;"

        If UCase(strDatabaseName) = "PUBS" Then
            'Insert data into pubs
            'Increment the counter
            iPubsCounter += 1
            strSQL = "INSERT INTO [publishers] (pub_id, pub_name, city, state, country)" & _
                " VALUES ('" & CStr(iPubsCounter) & "','Test','Test','WA','Test')"

            boolError = ExecuteNonQueryForMirror(strConnectionStringPrincipal, strSQL)
            If (boolError) Then
                lblDataFlowText.Text = "Error inserting data."
            Else
                lblDataFlowText.Text = "Added publisher #" & iPubsCounter
            End If

        End If
    End Sub
    Private Sub SetDBMirrorStatusandPrincipal()
        'Try to query the principal to get the status of the database mirroring
        Dim strReturnValue As String = ""
        Dim strSQL As String = ""

        strConnectionStringPrincipal = "Initial Catalog = Master" & _
           ";Data Source = " & strPrincipalName & ";Trusted_Connection=Yes;"


        'First, get the state description
        strSQL = "SELECT mirroring_state_desc from sys.databases WHERE name = '" & strDatabaseName & "'"

        strReturnValue = ExecuteScalar(strConnectionStringPrincipal, strSQL)
        If strReturnValue = "" Then
            strReturnValue = "Unconfigured"
        End If
        lblDBMirrorStatusText.Text = strReturnValue


        'Get the Principal's role
        strSQL = "SELECT mirroring_role_desc from sys.databases WHERE name = '" & strDatabaseName & "'"

        strReturnValue = ExecuteScalar(strConnectionStringPrincipal, strSQL)
        If strReturnValue = "" Then
            strReturnValue = "Unconfigured"
        End If

        lblPrincipalText.Text = strReturnValue

    End Sub
    Private Sub btnTurnOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTurnOff.Click
        Dim boolError As Boolean = False
        Dim strSQL As String = ""
        Dim boolError2 As Boolean = False

        If strMirrorName = "" Then
            'Set the mirror name
            strMirrorName = txtMirrorServer.Text()
        End If

        If strDatabaseName = "" Then
            MsgBox("Please select a database first.")
            Exit Sub
        End If

        'Disable the timer
        timerMirroring.Enabled = False


        'Try to delete the new records in pubs
        strConnectionStringPrincipal = "Initial Catalog = " & strDatabaseName & _
            ";Data Source = " & strPrincipalName & ";Trusted_Connection=Yes;"

        strSQL = "SELECT mirroring_role_desc FROM sys.databases WHERE name = '" & strDatabaseName & "'"
        Dim strRole As String = ExecuteScalar(strConnectionStringPrincipal, strSQL)
        strSQL = "DELETE FROM publishers WHERE pub_name = 'Test'"

        If (boolError) Then
            'Something wrong, must be down
            'Try to failover mirror
            boolError2 = ExecuteNonQuery(strConnectionStringMirror, strSQL)
        Else
            If UCase(strRole) = "PRINCIPAL" Then
                boolError2 = ExecuteNonQuery(strConnectionStringPrincipal, strSQL)
            Else
                boolError2 = ExecuteNonQuery(strConnectionStringMirror, strSQL)
            End If
        End If

        If (boolError2) Then
            MsgBox("Error trying to delete added records to pubs.")
        Else
            MsgBox("Success deleting added records to pubs.")
        End If


        'Attempt to turn off mirroring
        strConnectionStringMirror = "Initial Catalog = Master" & _
           ";Data Source = " & strMirrorName & ";Trusted_Connection=Yes;"

        strSQL = "ALTER DATABASE [" & strDatabaseName & "] SET PARTNER OFF"

        'Talk to the mirror just in case they failed mirror over to principal
        boolError = ExecuteNonQuery(strConnectionStringMirror, strSQL)
        If (boolError) Then
            MsgBox("Error turning off Database Mirroring.  Exiting.")
            Exit Sub
        Else
            MsgBox("Success! Mirroring turned off.")
            lblDBMirrorStatusText.Text = "Unconfigured"
            lblConnectedServerText.Text = "Unconfigured"
            lblPrincipalText.Text = "Unconfigured"
        End If

 


    End Sub
    Friend Sub SetDatabaseName()
        strDatabaseName = lblDatabaseName.Text()
    End Sub

    Private Sub btnFail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFail.Click
        'Fail over to the mirror by figuring out the current principal
        'Connect to the principal and see if it's still the principal
        Dim strSQL As String = ""
        Dim boolError As Boolean = False
        Dim boolError2 As Boolean = False

        strConnectionStringPrincipal = "Initial Catalog = Master" & _
            ";Data Source = " & strPrincipalName & ";Trusted_Connection=Yes;"



        strSQL = "SELECT mirroring_role_desc FROM sys.databases WHERE name = '" & strDatabaseName & "'"
        Dim strRole As String = ExecuteScalar(strConnectionStringPrincipal, strSQL)
        strSQL = "ALTER DATABASE [" & strDatabaseName & "] SET PARTNER FAILOVER"

        If (boolError) Then
            'Something wrong, must be down
            'Try to failover mirror
            boolError2 = ExecuteNonQuery(strConnectionStringMirror, strSQL)
        Else
            If UCase(strRole) = "PRINCIPAL" Then
                boolError2 = ExecuteNonQuery(strConnectionStringPrincipal, strSQL)
            Else
                boolError2 = ExecuteNonQuery(strConnectionStringMirror, strSQL)
            End If
        End If

    End Sub

    
    Private Sub lblDatabaseName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblDatabaseName.TextChanged
        If lblDatabaseName.Text <> "" Then
            'Enable the setup control
            btnSetup.Enabled = True
        End If
    End Sub
End Class
