This sample application shows you how to use Reporting Services and Integration Services together.

Please note that you must have the AdventureWorks sample installed to use this application.

To use the applications, please follow these steps:

1) Double-click the getproducts.sql file to launch Management Studio which will load the file.

2) Once loaded, press F5 to run the code.  The code should create a stored procedure called getproducts in the adventureworks database.

3) Doublic-click the Integration Services 1.sln file to launch Integration Services.  Click F5 to run the package to test it.  Click the data flow tab and you should see 295 rows moved between the source and destination.

4) You must enable the SSIS data processing extension.  To do this, open the file rsreportserver.config.  You can normally find this file at C:\Program Files\Microsoft SQL Server\MSSQL.3\Reporting Services\ReportServer\

5) Find the line 
<!-- <Extension Name="SSIS" Type="Microsoft.SqlServer.Dts.DtsClient.DtsConnection,Microsoft.SqlServer.Dts.DtsClient, Version=9.0.242.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"/> --> 

and remove the comments so that the line reads 

<Extension Name="SSIS" Type="Microsoft.SqlServer.Dts.DtsClient.DtsConnection,Microsoft.SqlServer.Dts.DtsClient, Version=9.0.242.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"/>

6) Save the rsreportserver.config file.

7) To get the SSIS extension to work in Report Designer, open the file RSReportDesigner.config.  You can normally find this file at C:\program files\Microsoft Visual Studio 8\Common7\IDE\PrivateAssemblies

8) Find the line 

<!-- <Extension Name="SSIS" Type="Microsoft.SqlServer.Dts.DtsClient.DtsConnection,Microsoft.SqlServer.Dts.DtsClient, Version=9.0.242.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"/> -->

and remove the comments so it reads 

<Extension Name="SSIS" Type="Microsoft.SqlServer.Dts.DtsClient.DtsConnection,Microsoft.SqlServer.Dts.DtsClient, Version=9.0.242.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"/>

9) Find the line <!-- <Extension Name="SSIS" Type="Microsoft.ReportingServices.QueryDesigners.GenericQueryDesigner,Microsoft.ReportingServices.QueryDesigners"/> --> 

and remove the comments so the line reads       

<Extension Name="SSIS" Type="Microsoft.ReportingServices.QueryDesigners.GenericQueryDesigner,Microsoft.ReportingServices.QueryDesigners"/>

10) Save the file.

11) Go into the SSIS Project folder.  Double-click the SSIS Project.sln to load it into BIDS or Visual Studio depending on what you have installed on your machine.

12) Double-click the DataSource1.rds and change the path in the connection string to the path where the package.dtsx file is and click ok.

13) Open Report1.rdl by clicking on it.  Make sure that the table is on the report.

14) Click the Preview tab at the top.  The report should load and you should see information from SSIS.
