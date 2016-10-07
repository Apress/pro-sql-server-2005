Imports System.Management
Public Class frmMain
    Dim WmiNamespace As String = "\\" & My.Computer.Name & "\root\Microsoft\SqlServer\ReportServer\v9"
    Dim WmiRSClass As String = "\\" & My.Computer.Name & "\ROOT\Microsoft\SqlServer\ReportServer\v9\Admin:MSReportServer_ConfigurationSetting"
    Dim serverClass As ManagementClass
    Dim scope As ManagementScope
    Private Function ConnecttoWMI() As Boolean
        scope = New ManagementScope(WmiNamespace)

        'Connect to the Reporting Services namespace.
        scope.Connect()
        'Create the server class.
        serverClass = New ManagementClass(WmiRSClass)
        'Connect to the management object.
        serverClass.Get()

        If (serverClass Is Nothing) Then
            ConnecttoWMI = False
        Else
            ConnecttoWMI = True
        End If
    End Function
    
    Private Sub btnExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecute.Click
        Dim name As String = ""
        Dim val As Object
        Dim strItemtoAdd As String = ""

        listResults.Items.Clear()
        listMethods.Items.Clear()

        If (ConnecttoWMI() = False) Then
            MsgBox("Are you sure RS is installed on this machine?")
            Exit Sub
        Else
            'Loop through the instances of the server class.
            Dim instances As ManagementObjectCollection = serverClass.GetInstances()
            Dim instance As ManagementObject
            For Each instance In instances


                Dim instProps As PropertyDataCollection = instance.Properties
                Dim prop As PropertyData
                For Each prop In instProps
                    strItemtoAdd = ""
                    name = prop.Name
                    val = prop.Value

                    strItemtoAdd = name & ": "
                    If Not (val Is Nothing) Then
                        strItemtoAdd += val.ToString()
                    Else
                        strItemtoAdd += "<null>"
                    End If
                    listResults.Items.Add(strItemtoAdd)
                Next
            Next

            Dim methods As MethodDataCollection = serverClass.Methods()
            Dim method As MethodData
            For Each method In methods
                strItemtoAdd = method.Name
                listMethods.Items.Add(strItemtoAdd)
            Next

        End If
    End Sub

    Private Sub btnExecuteMethod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecuteMethod.Click
        'Example executing a method
        If (ConnecttoWMI() = False) Then
            MsgBox("Are you sure RS is installed on this machine?")
            Exit Sub
        Else
            'Backup the encryption key for each isntance using the
            'BackupEncryptionKeyMethod
            Dim instances As ManagementObjectCollection = serverClass.GetInstances()
            Dim instance As ManagementObject

            For Each instance In instances

                Dim inParams As ManagementBaseObject = instance.GetMethodParameters("BackupEncryptionKey")

                inParams("Password") = "Tom15892!!"

                Dim outParams As ManagementBaseObject = instance.InvokeMethod("BackupEncryptionKey", inParams, Nothing)

                Dim strKeyFile As String = ""
                
                Dim arrKeyfile As System.Array

                arrKeyfile = outParams("KeyFile")
                Dim i As Integer = 0
                For i = 0 To arrKeyfile.Length - 1
                    strKeyFile += Hex(arrKeyfile(i))
                Next

                
                MsgBox("HResult: " & outParams("HRESULT") & " Value: " & strKeyFile)
            Next
        End If
    End Sub
End Class
