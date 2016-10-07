USE AdventureWorks
GO

CREATE ASSEMBLY GetSalesAssem
FROM 'C:\Apress\ProSqlServer\Chapter11\GetSalesAssem.dll'
WITH PERMISSION_SET = EXTERNAL_ACCESS
GO

CREATE PROCEDURE uspGetSalesForNames @filename nvarchar(255)
AS EXTERNAL NAME GetSalesAssem.
     [Apress.SqlServer2005.SecurityChapter.SalesFetcher].GetSalesForNames
GO

EXEC uspGetSalesForNames 'C:\names.txt'