The SQLXML samples shows you how to use SQLXML to build applications against SQL Server.

Please note that you need the Pubs database installed on your server to run this application.

To run the application, perform the following steps:

1) Double click the SQLXML.sln file to load the application into Visual Studio 2005.

2) Find the line "Dim strConnectionString As String = "Provider=SQLOLEDB;server=localhost;database=pubs;integrated security=SSPI"" and replace localhost with the name of your server.

3) Hit ctrl-F5 to start the application without debugging.

4) In the XML Schema File textbox, put the full path to the file mapping schema.xsd such as c:\samples\mapping schema.xsd

5) In the XML File Path textbox, put the full path to the file authorsxmlnew.xml such as c:\samples\authorsxmlnew.xml.

6) Click the BulkLoadXML button.  You should get a success message if the application could bulkload your data.

7) Once successful, you can click any of the buttons in the main area of the form to see SQLXML in action.  Please note that after clicking the Use Updategram button you may get errors.  This is expected and should be ignored.