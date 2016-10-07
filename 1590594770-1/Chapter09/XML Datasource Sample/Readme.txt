This sample shows you how to use the XML Datasource with SSRS.  You need to perform the following steps before running the application:


1) Run the CreateSPROC.sql script to create a stored procedure in the AdventureWorks database. (You may not have to do this if you already created the SPROC as part of the Integration Services and Reporting Services samples)

2) Launch Visual Studio 2005 and from the File menu select New and then Website.

3) Select ASP.NET Web Service as the project type.  In the location box, make sure HTTP is selected and for the value type in http://localhost/website2.  Make sure Visual Basic is selected as the language and click OK.

4) After your new project launches, in the Solution Explorer, right-click the App_Code folder and select Add Existing Item from the menu.

5) Browse to where you installed the samples and under the website2 folder, under the App_Code folder, select Service.vb and click Add.  If asked to replace the existing file, click Yes.

6) Click Yes to reload the file into the editor.  On the Build menu, click Build Web Site.  The website should not be built and deployed to the server.

7) You need to go into the IIS Administration program and for the website, turn of anonymous access and turn on only Windows Integrated authentication.  To do this, open the IIS Administration Program and find Website2.  Right-click Website2 and select Properties.

8) On the Directory Security tab, under Authentication and access control, click the Edit button.

9) Uncheck Enable anonymous access and make sure Integrated windows authentication is checked.  Click OK. Click OK.

10) You need to make sure the account ASPNET runs under has permissions to the AdventureWorks database.  You can set these permissions through Management Studio under the Security folder then under Logins and finally by selecting your ASPNET account and select the AdventureWorks database and assigning permissions.  Also, please note that you will also have to GRANT execute permissions for the ASPNET account on the GetProducts stored procedure.  The following T-SQL code shows you how to do this:

use adventureworks
GRANT EXECUTE ON GetProducts TO [DOMAIN\USER]

11) Test the webservice to make sure it works by going to http://localhost/website2/service.asmx

12) Open the project in the Report Project1 folder.  This project should use the web service to return back data to the report.  You may have to refresh the fields in the dataset if you run into errors between naming in the dataset and naming in the report.
