Imports System.Xml
Public Class frmMain
    Dim strQuery As String = ""
    Dim strConnectionString As String = "Provider=SQLOLEDB;server=localhost;database=pubs;integrated security=SSPI"
    Dim strTable As String = "AuthorsXMLNew"

    Private Sub btnBulkLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBulkLoad.Click
        Try

            'Load the XML into SQL Server using SQLXML Bulkload
            
            Dim oXMLBulkLoad As New SQLXMLBULKLOADLib.SQLXMLBulkLoad4Class()
            oXMLBulkLoad.ErrorLogFile = "c:\myerrors.log"
            oXMLBulkLoad.SchemaGen = True
            oXMLBulkLoad.KeepIdentity = False
            oXMLBulkLoad.BulkLoad = True
            oXMLBulkLoad.SGDropTables = True
            oXMLBulkLoad.XMLFragment = True
            oXMLBulkLoad.ConnectionString = strConnectionString
            oXMLBulkLoad.Execute(txtXMLSchema.Text, txtXMLFile.Text)

            MsgBox("Bulkload Successful.")

        Catch ex As Exception
            MsgBox("Error: " & Err.Number & " Description: " & Err.Description)
        End Try
    End Sub


    Private Sub SetQuery(ByVal strQueryText As String)
        txtSQLQuery.Text = strQueryText
    End Sub

    Private Sub radioForXML_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioForXML.CheckedChanged
        'Set the SQL Query
        strQuery = "SELECT au_lname FROM " & strTable & " FOR XML AUTO, ELEMENTS"
        SetQuery(strQuery)

        'Don't use client-side rendering

        ExecuteQuery(strQuery, False, True, False, False, False, False, False)

    End Sub


    Private Sub radioFORXMLClient_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioFORXMLClient.CheckedChanged
        'Set the SQL Query
        strQuery = "SELECT au_lname, au_fname FROM " & strTable & " FOR XML AUTO"
        SetQuery(strQuery)

        ExecuteQuery(strQuery, True, False, True, False, False, False, False)
    End Sub

    Private Sub radioStreamReader_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioStreamReader.CheckedChanged
        'Set the SQL Query
        strQuery = "SELECT phone, address FROM " & strTable & " FOR XML AUTO"
        SetQuery(strQuery)

        'Set to use the streamreader

        ExecuteQuery(strQuery, True, False, False, False, False, False, False)


    End Sub

    Private Sub radioXMLTextReader_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioXMLTextReader.CheckedChanged
        strQuery = "SELECT city FROM " & strTable & " FOR XML AUTO"
        SetQuery(strQuery)

        'Set to use the XMLReader

        ExecuteQuery(strQuery, False, False, False, False, False, False, False)

    End Sub
    Private Sub radioXPath_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioXPath.CheckedChanged
        'Load up our query

        strQuery = "/AuthorsXMLNew[city='Oakland']"
        SetQuery(strQuery)


        ExecuteQuery(strQuery, True, False, True, True, False, False, False)
    End Sub

    Private Sub radioSQLXMLParameter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioSQLXMLParameter.CheckedChanged
        strQuery = "SELECT * FROM " & strTable & " WHERE city = ? FOR XML AUTO, ELEMENTS"
        SetQuery(strQuery)

        'Set to use the streamreader

        ExecuteQuery(strQuery, True, True, True, False, False, False, False)


    End Sub
    Private Sub radioTemplate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioTemplate.CheckedChanged
        'Load up our query

        strQuery = "<Root><sql:query xmlns:sql=""urn:schemas-microsoft-com:xml-sql"">SELECT * FROM " & strTable & " FOR XML AUTO</sql:query></Root>"
        SetQuery(strQuery)

        ExecuteQuery(strQuery, True, False, True, False, True, False, False)

    End Sub
    Private Sub radioDataSet_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioDataSet.CheckedChanged
        strQuery = "SELECT * FROM " & strTable & " WHERE city = 'oakland' FOR XML AUTO, ELEMENTS"
        SetQuery(strQuery)

        'Set to use the Dataset

        ExecuteQuery(strQuery, False, False, True, False, False, True, False)

    End Sub
    Private Sub radioUpdateGram_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioUpdateGram.CheckedChanged

        Dim strUpdateGram As New System.Text.StringBuilder()
        strUpdateGram.Append("<?xml version='1.0'?><AuthorsXMLNewupdate ")
        strUpdateGram.Append("xmlns:updg='urn:schemas-microsoft-com:xml-updategram'>")
        strUpdateGram.Append("<updg:sync updg:nullvalue='nothing'><updg:before></updg:before>")
        strUpdateGram.Append("<updg:after><AuthorsXMLNew au_id='123-22-1232' au_fname='Tom' state='WA' phone='425-882-8080'/>")
        strUpdateGram.Append("</updg:after>")
        strUpdateGram.Append("<updg:before><AuthorsXMLNew au_id='267-41-2394'/></updg:before>")
        strUpdateGram.Append("<updg:after></updg:after>")
        strUpdateGram.Append("<updg:before><AuthorsXMLNew au_id='238-95-7766'/></updg:before>")
        strUpdateGram.Append("<updg:after><AuthorsXMLNew city='Oakland' phone='212-555-1212'/>")
        strUpdateGram.Append("</updg:after></updg:sync></AuthorsXMLNewupdate>")

        strQuery = strUpdateGram.ToString()

        SetQuery(strQuery)

        'Set to use an UpdateGram

        ExecuteQuery(strQuery, False, False, True, False, False, False, True)


    End Sub

    Private Sub ExecuteQuery(ByVal strQuery As String, ByVal bUseStreamReader As Boolean, ByVal bUseParameter As Boolean, ByVal bUseClientSide As Boolean, ByVal bUseXPath As Boolean, ByVal bUseTemplate As Boolean, ByVal bUseDataSet As Boolean, ByVal bUseUpdateGram As Boolean)
        Err.Clear()
        'Create our SQLXML command
        Dim oSQLXMLCommand As New Microsoft.Data.SqlXml.SqlXmlCommand(strConnectionString)
        Dim oSQLXMLParameter As Microsoft.Data.SqlXml.SqlXmlParameter


        oSQLXMLCommand.CommandType = Microsoft.Data.SqlXml.SqlXmlCommandType.Sql

        'Set our Query
        oSQLXMLCommand.CommandText = strQuery
        If bUseXPath = True Then
            oSQLXMLCommand.CommandType = Microsoft.Data.SqlXml.SqlXmlCommandType.XPath
            oSQLXMLCommand.SchemaPath = txtXMLSchema.Text
            oSQLXMLCommand.RootTag = "ROOT"
        End If

        If bUseUpdateGram = True Then
            oSQLXMLCommand.CommandType = Microsoft.Data.SqlXml.SqlXmlCommandType.UpdateGram
        End If

        If bUseTemplate = True Then
            oSQLXMLCommand.CommandType = Microsoft.Data.SqlXml.SqlXmlCommandType.Template
            oSQLXMLCommand.SchemaPath = txtXMLSchema.Text
            oSQLXMLCommand.RootTag = "ROOT"
        End If


        Try

            'See if we need to render client-side
            If bUseClientSide = True Then
                oSQLXMLCommand.ClientSideXml = True
            End If

            'See if we need to use a parameter
            If bUseParameter Then
                oSQLXMLParameter = oSQLXMLCommand.CreateParameter()
                oSQLXMLParameter.Name = "city"
                oSQLXMLParameter.Value = "Oakland"
            End If

            'See if we need to use a streamreader or XMLreader

            If bUseStreamReader = True Then

                Dim oStream As System.IO.Stream
                oStream = oSQLXMLCommand.ExecuteStream()

                oStream.Position = 0
                Dim oStreamReader As New System.IO.StreamReader(oStream)
                txtResults.Text = oStreamReader.ReadToEnd()
                oStreamReader.Close()

            ElseIf bUseStreamReader = False And bUseDataSet = False Then
                'Use XMLTextReader
                Dim oXMLTextReader As System.Xml.XmlTextReader
                oXMLTextReader = oSQLXMLCommand.ExecuteXmlReader()
                Dim strXML As String = ""

                While oXMLTextReader.Read()
                    'We're on an element
                    If oXMLTextReader.NodeType = XmlNodeType.Element Then
                        strXML += "<" & oXMLTextReader.Name & ""
                    ElseIf oXMLTextReader.NodeType = XmlNodeType.EndElement Then
                        strXML += "</" & oXMLTextReader.Name & ">"
                    End If

                    'Look for attributes
                    If oXMLTextReader.HasAttributes() Then
                        Dim i As Integer = 0
                        Do While (oXMLTextReader.MoveToNextAttribute())
                            i += 1
                            strXML += " " & oXMLTextReader.Name & "=" & oXMLTextReader.Value
                            If oXMLTextReader.AttributeCount = i Then
                                'Last attribute, end the tag
                                strXML += " />"
                            End If
                        Loop

                    Else
                        If (oXMLTextReader.NodeType = XmlNodeType.Text) Then
                            strXML += oXMLTextReader.Value
                        ElseIf oXMLTextReader.NodeType <> XmlNodeType.EndElement Then
                            strXML += ">"
                        End If

                    End If

                End While


                txtResults.Text = strXML
                oXMLTextReader.Close()

            ElseIf bUseDataSet = True Then
                Dim oSQLXMLDataAdapter As New Microsoft.Data.SqlXml.SqlXmlAdapter(oSQLXMLCommand)
                Dim oDS As New System.Data.DataSet()
                oSQLXMLDataAdapter.Fill(oDS)

                'Display the underlying XML
                Dim oMemStream As New System.IO.MemoryStream()
                Dim oStreamWriter As New System.IO.StreamWriter(oMemStream)
                oDS.WriteXml(oMemStream, System.Data.XmlWriteMode.IgnoreSchema)
                oMemStream.Position = 0
                Dim oStreamReader As New System.IO.StreamReader(oMemStream)
                txtResults.Text = oStreamReader.ReadToEnd()
                oMemStream.Close()

            End If

        Catch
            MsgBox("Error Number: " & Err.Number & " Description: " & Err.Description)
            Err.Clear()


        Finally


            oSQLXMLCommand = Nothing
        End Try

    End Sub




End Class
