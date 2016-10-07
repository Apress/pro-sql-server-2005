USE AdventureWorks  --all of the code is to be executed in adventureworks
GO

----------------------------------------------------------------------
-- DML Enhancements - Old-Style Outer Joins Deprecated
----------------------------------------------------------------------

--causes error
SELECT	*
FROM   sales.salesPerson as salesPerson,
       sales.salesTerritory as salesTerritory
WHERE  salesperson.territoryId *= salesTerritory.territoryId
go

SELECT *
FROM   sales.salesPerson as salesPerson
           LEFT OUTER JOIN sales.salesTerritory as salesTerritory
               on salesperson.territoryId = salesTerritory.territoryId
go

SELECT
FROM     sales.salesPerson as salesPerson,
         sales.salesTerritory as salesTerritory
WHERE    salesperson.territoryId = salesTerritory.territoryId
go


----------------------------------------------------------------------
-- DML Enhancements - Common Table Expressions
----------------------------------------------------------------------
WITH simpleExample as
(
     SELECT 'hi' AS columnName
)

SELECT  columnName
FROM    simpleExample
GO
SELECT  columnName
FROM    (SELECT 'hi' AS columnName) AS simpleExample
GO

--In SQL Server 2000
SELECT  cast(c.LastName + ', ' + c.FirstName as varchar(30)) as SalesPerson
    ,
--YEAR TO DATE SALES
       (SELECT amount
        FROM (  SELECT soh.SalesPersonID, sum(sod.LineTotal) as amount
                FROM   sales.SalesOrderHeader soh
                        JOIN sales.SalesOrderDetail sod
                        ON sod.SalesOrderID = soh.SalesOrderID
                WHERE  soh.Status = 5 -- complete
                and  soh.OrderDate >= '20040101'
                GROUP  by soh.SalesPersonID) as YTDSalesPerson
        where YTDSalesPerson.salesPersonId = salesperson.SalesPersonID) as YTDSales,

--PERCENT OF TOTAL
        (SELECT amount
        FROM (SELECT soh.SalesPersonID, sum(sod.LineTotal) as amount
                FROM   sales.SalesOrderHeader soh
                        JOIN sales.SalesOrderDetail sod
                        ON sod.SalesOrderID = soh.SalesOrderID
                WHERE  soh.Status = 5 -- complete
                and  soh.OrderDate >= '20040101'
                GROUP  by soh.SalesPersonID) as YTDSalesPerson
        where YTDSalesPerson.salesPersonId = salesperson.SalesPersonID) /
            (SELECT sum(amount)
            FROM (SELECT soh.SalesPersonID, sum(sod.LineTotal) as amount
                FROM   sales.SalesOrderHeader soh
                        JOIN sales.SalesOrderDetail sod
                        ON sod.SalesOrderID = soh.SalesOrderID
                WHERE  soh.Status = 5 -- complete
                and  soh.OrderDate >= '20040101'
                GROUP  by soh.SalesPersonID) as YTDSalesPerson
            ) as percentOfTotal,

--COMPARE TO QUOTA
       (SELECT amount
        FROM (SELECT soh.SalesPersonID, sum(sod.LineTotal) as amount
                FROM   sales.SalesOrderHeader soh
                        JOIN sales.SalesOrderDetail sod
                        ON sod.SalesOrderID = soh.SalesOrderID
                WHERE  soh.Status = 5 -- complete
                and  soh.OrderDate >= '20040101'
                GROUP  by soh.SalesPersonID)as YTDSalesPerson
        where YTDSalesPerson.salesPersonId = salesperson.SalesPersonID) -
        salesPerson.SalesQuota         as MetQuota

    FROM    sales.SalesPerson as salesPerson
             join HumanResources.Employee as e
                on salesPerson.salesPersonId = e.employeeId
             join Person.Contact as c
                on c.contactId = e.contactId
GO

-- SQL Server 2005 CTE syntax
WITH YTDSalesPerson
AS
(
    SELECT soh.SalesPersonID, sum(sod.LineTotal) as amount
    FROM   sales.SalesOrderHeader soh
             JOIN sales.SalesOrderDetail sod
               ON sod.SalesOrderID = soh.SalesOrderID
    WHERE  soh.Status = 5 -- complete
      and  soh.OrderDate >= '20040101'
    GROUP  by soh.SalesPersonID
),
SalesPersonInfo
AS
(
    SELECT  salesPersonId, SalesQuota as salesQuota,
            cast(c.LastName + ', ' + c.FirstName as varchar(30)) as SalesPerson
    FROM    sales.SalesPerson as s
             JOIN HumanResources.Employee as e
                on s.salesPersonId = e.employeeId
             JOIN Person.Contact as c
                on c.contactId = e.contactId
)
SELECT SalesPersonInfo.SalesPerson,
       (SELECT amount
        FROM YTDSalesPerson
        WHERE YTDSalesPerson.salesPersonId = salespersonInfo.SalesPersonID)
             as YTDSales,

        (SELECT amount
        FROM YTDSalesPerson
        WHERE YTDSalesPerson.salesPersonId = salespersonInfo.SalesPersonID)
        /  (SELECT sum(amount) FROM YTDSalesPerson) as percentOfTotal,

       (SELECT amount
        FROM YTDSalesPerson
        WHERE YTDSalesPerson.salesPersonId = salespersonInfo.SalesPersonID) -
        salesPersonInfo.SalesQuota         as MetQuota

FROM   SalesPersonInfo
Go

WITH YTDSalesPerson
AS
(
    SELECT soh.SalesPersonID, sum(sod.LineTotal) as amount
    FROM   sales.SalesOrderHeader soh
             JOIN sales.SalesOrderDetail sod
               ON sod.SalesOrderID = soh.SalesOrderID
    WHERE  soh.Status = 5 -- complete
      and  soh.OrderDate >= '20040101'
    GROUP  by soh.SalesPersonID
)
SELECT  *
FROM    YTDSalesPerson
GO

-- SQL Server 2000 example of the recursive query

DECLARE @managerId int
SET @managerId = 140

 --holds the output treelevel, which lets us isolate a level in the looped query
DECLARE @outTable table (employeeId int, managerId int, treeLevel int)

 --used to hold the level of the tree we are currently at in the loop
DECLARE @treeLevel as int
SET     @treelevel = 1

 --get the top level
 INSERT INTO @outTable
 SELECT employeeId, managerId, @treelevel as treelevel
 FROM   HumanResources.employee as employee
 WHERE  (employee.managerId = @managerId)

 WHILE (1 = 1) --imitates do...until construct
  BEGIN

     INSERT INTO @outTable
     SELECT employee.employeeId, employee.managerId,
            treelevel + 1 as treelevel
     FROM   HumanResources.employee as employee
              JOIN @outTable as ht
                  ON employee.managerId = ht.employeeId
     --this where isolates a given level of the tree
     WHERE  EXISTS(     SELECT  *
                        FROM    @outTable AS holdTree
                        WHERE   treelevel = @treelevel
                          AND   employee.managerId = holdtree.employeeId)

     IF @@rowcount = 0
       BEGIN
          BREAK
       END

     SET @treelevel = @treelevel + 1
  END

SELECT   Employee.EmployeeID,Contact.LastName,Contact.FirstName
FROM     HumanResources.Employee as Employee
           INNER JOIN @outTable ot
               ON Employee.EmployeeID = ot.EmployeeID
           INNER JOIN Person.Contact as Contact
              ON Contact.contactId = Employee.contactId
go
--recursive query using CTE
-- SQL Server 2005 syntax
DECLARE @managerId int
SET @managerId = 140;

WITH EmployeeHierarchy (EmployeeId, ManagerId)
AS
(
     SELECT EmployeeID, ManagerID
     FROM   HumanResources.Employee as Employee
     WHERE  ManagerID=@managerId

     UNION ALL

     SELECT Employee.EmployeeID, Employee.ManagerID
     FROM   HumanResources.Employee as Employee
              INNER JOIN EmployeeHierarchy
                on Employee.ManagerID= EmployeeHierarchy.EmployeeID)

SELECT  Employee.EmployeeID,Contact.LastName,Contact.FirstName
FROM     HumanResources.Employee as Employee
         INNER JOIN EmployeeHierarchy
              ON Employee.EmployeeID = EmployeeHierarchy.EmployeeID
           INNER JOIN Person.Contact as Contact
                ON Contact.contactId = Employee.contactId
GO

--enhanced version of the CTE query
DECLARE @managerId int
SET @managerId = 140;

WITH EmployeeHierarchy(EmployeeID, ManagerID, treelevel, heirarchy)
AS
(
     SELECT EmployeeID, ManagerID,
            1 as treeevel, CAST(EmployeeId as varchar(max)) as heirarchy
     FROM   HumanResources.Employee as Employee
     WHERE ManagerID=@managerId

     UNION ALL

     SELECT Employee.EmployeeID, Employee.ManagerID,
            treelevel + 1 as treelevel,
            heirarchy + '\' +cast(Employee.EmployeeId as varchar(20)) as heirarchy
     FROM   HumanResources.Employee as Employee
              INNER JOIN EmployeeHierarchy
                on Employee.ManagerID= EmployeeHierarchy.EmployeeID
)

SELECT  Employee.EmployeeID,Contact.LastName,Contact.FirstName,
        EmployeeHierarchy.treelevel, EmployeeHierarchy.heirarchy
FROM     HumanResources.Employee as Employee
         INNER JOIN EmployeeHierarchy
              ON Employee.EmployeeID = EmployeeHierarchy.EmployeeID
           INNER JOIN Person.Contact as Contact
                ON Contact.contactId = employee.contactId
ORDER BY heirarchy
go

----------------------------------------------------------------------
-- DML Enhancements - TOP
----------------------------------------------------------------------

DECLARE @rowsToReturn int

SELECT  @rowsToReturn = 10

SELECT  TOP(@rowsToReturn) * --note that TOP requires parentheses to accept
                             --parameters but not for constant
FROM    HumanResources.Employee
GO
CREATE TABLE testTop
(
	value	int primary Key
)
INSERT TOP (5) into testTop
SELECT  *  --this derived table returns seven rows
FROM    (SELECT 1  as value  union SELECT 2  union SELECT 3 union SELECT 4
		union SELECT 5 union SELECT 6 union SELECT 7) as sevenRows
go
SELECT	*
FROM	testTop
go

UPDATE TOP (2) testTop
SET	 value = value * 100

SELECT	*
FROM	testTop
GO
DELETE TOP(3) testTop
go

select * from testTop
go

/* this statement does not refer to any actual table
INSERT TOP(10) otherTable (batchTableId, value)
SELECT batchTableId, value
FROM   batchTable
WHERE  not exists ( SELECT *
                    FROM   otherTable
                    WHERE  otherTable.batchTableId = batchTable.batchTableId)
*/

create table top10sales
(
 salesOrderId int,
 totalDue  money
)

insert  TOP (10) top10sales
SELECT salesOrderId, totalDue
FROM sales.salesOrderHeader
ORDER BY totalDue desc
go

--added
truncate table top10sales
go
INSERT top10sales
SELECT TOP (10) salesOrderId, totalDue
FROM   sales.salesOrderHeader
ORDER     BY totalDue desc
GO
BEGIN TRANSACTION
DECLARE @rowcount int
SET @rowcount = 100
WHILE (@rowcount = 100)  --if it is less than 100, we are done, greater than 100
  BEGIN			      --cannot happen
             DELETE TOP(100) sales.salesOrderHeader
             SET @rowcount = @@rowcount
  END
ROLLBACK TRANSACTION --we don't want to actually delete the rows
GO

----------------------------------------------------------------------
-- DML Enhancements - FROM Clause extensions
----------------------------------------------------------------------

/* causes error
SELECT Product.productNumber, SalesOrderAverage.averageTotal
FROM   Production.Product as Product
          JOIN       (     SELECT AVG(lineTotal) as averageTotal
                            FROM Sales.SalesOrderDetail as SalesOrderDetail
                            WHERE product.ProductID=SalesOrderDetail.ProductID
                            HAVING COUNT(*) > 1
                            ) as SalesOrderAverage

SELECT Product.productNumber, SalesOrderAverage.averageTotal
FROM   Production.Product as Product
         JOIN dbo.getAverageTotalPerProduct(product.productId)
                                                            as SalesOrderAverage

*/

--Cross Apply
SELECT Product.productNumber, SalesOrderAverage.averageTotal
FROM   Production.Product as Product
          CROSS APPLY (     SELECT AVG(lineTotal) as averageTotal
                            FROM Sales.SalesOrderDetail as SalesOrderDetail
                            WHERE product.ProductID=SalesOrderDetail.ProductID
                            HAVING COUNT(*) > 0
                            ) as SalesOrderAverage
go

CREATE FUNCTION production.getAverageTotalPerProduct
(
     @productId int
)
RETURNS @output TABLE (unitPrice decimal(10,4))
AS
     BEGIN
          INSERT  INTO @output (unitPrice)
          SELECT AVG(lineTotal) as averageTotal
          FROM Sales.SalesOrderDetail as SalesOrderDetail
          WHERE SalesOrderDetail.ProductID = @productId
          HAVING COUNT(*) > 0

          RETURN
     END
go
SELECT Product.ProductNumber, AverageSale.UnitPrice
FROM   Production.Product as Product
            CROSS APPLY
                      production.getAverageTotalPerProduct(product.productId)
                                                                      as AverageSale
go

SELECT Product.ProductNumber, AverageSale.UnitPrice
FROM   Production.Product as Product
            OUTER APPLY
                      production.getAverageTotalPerProduct(product.productId)
                                                                      as AverageSale
GO

----------------------------------------------------------------------
-- DML Enhancements - Random Data Sampling
----------------------------------------------------------------------

SELECT  *
FROM    sales.salesOrderDetail TABLESAMPLE SYSTEM (2 PERCENT)
go
SELECT  *
FROM    sales.salesOrderDetail TABLESAMPLE SYSTEM (500 rows)
go
SELECT  *
FROM    sales.salesOrderDetail TABLESAMPLE SYSTEM (500 rows) REPEATABLE (123456)
go

----------------------------------------------------------------------
-- DML Enhancements - Pivoting Data
----------------------------------------------------------------------

CREATE TABLE sales.salesByMonth
(
     year char(4),
     month char(3),
     amount money,
     PRIMARY KEY (year, month)
)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Jan',   789.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Feb',   389.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Mar',   8867.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Apr',   778.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','May',   78.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Jun',   9.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Jul',   987.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Aug',   866.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Sep',   7787.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Oct',   85576.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Nov',   855.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2004','Dec',   5878.0000)

INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2005','Jan',   7.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2005','Feb',   6868.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2005','Mar',   688.0000)
INSERT INTO sales.salesByMonth (year, month, amount)
VALUES('2005','Apr',   9897.0000)
go

SELECT year,
       SUM(case when month = 'Jan' then amount else 0 end) AS 'Jan',
       SUM(case when month = 'Feb' then amount else 0 end) AS 'Feb',
       SUM(case when month = 'Mar' then amount else 0 end) AS 'Mar',
       SUM(case when month = 'Apr' then amount else 0 end) AS 'Apr',
       SUM(case when month = 'May' then amount else 0 end) AS 'May',
       SUM(case when month = 'Jun' then amount else 0 end) AS 'Jun',
       SUM(case when month = 'Jul' then amount else 0 end) AS 'Jul',
       SUM(case when month = 'Aug' then amount else 0 end) AS 'Aug',
       SUM(case when month = 'Sep' then amount else 0 end) AS 'Sep',
       SUM(case when month = 'Oct' then amount else 0 end) AS 'Oct',
       SUM(case when month = 'Nov' then amount else 0 end) AS 'Nov',
       SUM(case when month = 'Dec' then amount else 0 end) AS 'Dec'
FROM   sales.salesByMonth
GROUP  by year
go
SELECT Year,[Jan],[Feb],[Mar],[Apr],[May],[Jun],
            [Jul],[Aug],[Sep],[Oct],[Nov],[Dec]
FROM (
     SELECT year, amount, month
     FROM       sales.salesByMonth ) AS salesByMonth
     PIVOT  ( SUM(amount) FOR month IN
               ([Jan],[Feb],[Mar],[Apr],[May],[Jun],
                [Jul],[Aug],[Sep],[Oct],[Nov],[Dec])
     ) AS ourPivot
ORDER BY Year
go
SELECT [Jan],[Feb],[Mar],[Apr],[May],[Jun],
        [Jul],[Aug],[Sep],[Oct],[Nov],[Dec]
FROM (      SELECT amount, month
            FROM   sales.salesByMonth ) AS salesByMonth
     PIVOT  ( SUM(amount) FOR month IN
               ([Jan],[Feb],[Mar],[Apr],[May],[Jun],
                [Jul],[Aug],[Sep],[Oct],[Nov],[Dec])
     ) AS ourPivot
go

CREATE TABLE Person.ContactProperty
(
     ContactPropertyId int identity(1,1) PRIMARY KEY,
     ContactId      int REFERENCES Person.Contact(ContactId),
     PropertyName  varchar(20),
     PropertyValue sql_variant,
     UNIQUE (ContactID, PropertyName)
)

INSERT Person.ContactProperty (ContactId, PropertyName, PropertyValue)
VALUES(1,'dog name','Fido')
INSERT Person.ContactProperty (ContactId, PropertyName, PropertyValue)
VALUES(1,'hair color','brown')
INSERT Person.ContactProperty (ContactId, PropertyName, PropertyValue)
VALUES(1,'car style','sedan')

INSERT Person.ContactProperty (ContactId, PropertyName, PropertyValue)
VALUES(2,'dog name','Rufus')
INSERT Person.ContactProperty (ContactId, PropertyName, PropertyValue)
VALUES(2,'hair color','blonde')

INSERT Person.ContactProperty (ContactId, PropertyName, PropertyValue)
VALUES(3,'dog name','Einstein')
INSERT Person.ContactProperty (ContactId, PropertyName, PropertyValue)
VALUES(3,'car style','coupe')
go
SELECT  cast(Contact.FirstName + ' ' + Contact.LastName as varchar(30)) as Name,
        ContactProperty.PropertyName, ContactProperty.PropertyValue
FROM    Person.Contact as Contact
          INNER JOIN Person.ContactProperty as ContactProperty
               ON  ContactProperty.ContactId = Contact.ContactID
go

SELECT  cast(Contact.FirstName + ' ' + Contact.LastName as varchar(30)) as Name,
        pivotColumns.* --demonstrating that * works, it should not
                       --be done this way in production code
FROM    Person.Contact as Contact
         INNER JOIN (SELECT ContactId, PropertyName,PropertyValue
                     FROM   Person.ContactProperty as property)
                                                                 as PivotTable
            PIVOT( MAX(PropertyValue)
                      FOR PropertyName IN ([dog name],[hair color],[car style]))
                                                                 AS PivotColumns
                ON Contact.ContactId = PivotColumns.ContactId
go

----------------------------------------------------------------------
-- DML Enhancements - Unpivoting data
----------------------------------------------------------------------

SELECT Year,[Jan],[Feb],[Mar],[Apr],[May],[Jun],
           [Jul],[Aug],[Sep],[Oct],[Nov],[Dec]
INTO  sales.salesByYear
FROM (
     SELECT year, amount, month
     FROM   sales.salesByMonth ) AS salesByMonth
     PIVOT  ( SUM(amount) FOR month IN
               ([Jan],[Feb],[Mar],[Apr],[May],[Jun],
                [Jul],[Aug],[Sep],[Oct],[Nov],[Dec])
     ) AS ourPivot
ORDER BY Year
go

SELECT  Year, cast(Month as char(3)) as Month, Amount
FROM    sales.salesByYear
            UNPIVOT (Amount FOR Month IN
               ([Jan],[Feb],[Mar],[Apr],[May],[Jun],
                [Jul],[Aug],[Sep],[Oct],[Nov],[Dec])) as unPivoted
go

----------------------------------------------------------------------
-- DML Enhancements - Output
----------------------------------------------------------------------

BEGIN TRANSACTION

DECLARE @changes table (change varchar(2000))

UPDATE TOP (10) Person.Contact
SET    FirstName = Reverse(FirstName)
OUTPUT 'Was: ''' + DELETED.FirstName +
                ''' Is: ''' + INSERTED.FirstName + ''''
INTO   @changes

SELECT *
FROM   @changes

ROLLBACK TRANSACTION

----------------------------------------------------------------------
-- DML Enhancements - Ranking functions
----------------------------------------------------------------------
CREATE VIEW contactSubset
as
     Select  TOP 20 *
     FROM    Person.Contact
     WHERE   FirstName like 'b%'
go

SELECT  FirstName,
          (    SELECT count(*)
               FROM   contactSubset as c
               WHERE  c.FirstName < contactSubset.FirstName) + 1 as RANK
FROM    contactSubset
ORDER   BY FirstName
go

SELECT   FirstName, LastName,
           (   SELECT count(*)
               FROM   contactSubset as c
               WHERE  c.LastName < contactSubset.LastName
                  OR  (c.LastName = contactSubset.LastName
                       AND c.FirstName< contactSubset.FirstName)
                    ) + 1 as orderNumber
FROM     contactSubset
ORDER    BY LastName, FirstName
GO
SELECT  FirstName,
        ROW_NUMBER() over (order by FirstName) as 'ROW_NUMBER',
        RANK() over (order by FirstName) as 'RANK',
        DENSE_RANK() over (order by FirstName) as 'DENSE_RANK',
        NTILE(4) over (order by FirstName) as 'NTILE(4)'
FROM    contactSubSet
ORDER   BY FirstName
GO
SELECT    FirstName, LastName,
          ROW_NUMBER() over (order by FirstName, LastName) as 'ROW_NUMBER',
          RANK() over (order by FirstName,LastName) as 'RANK',
          DENSE_RANK() over (order by FirstName,LastName) as 'DENSE_RANK',
          NTILE(4) over (order by FirstName,LastName) as 'NTILE(4)'
FROM      contactSubSet
ORDER     BY FirstName
GO
SELECT    firstName,
          ROW_NUMBER() over (order by FirstName) as ascending,
          ROW_NUMBER() over (order by FirstName desc) as descending
FROM      contactSubSet
ORDER     BY FirstName
GO
SELECT    FirstName, LastName,
          ROW_NUMBER()over ( partition by FirstName order by lastName) as
                                                               'ROW_NUMBER',
          RANK()over ( partition by FirstName order by LastName) as 'RANK',
          DENSE_RANK()over ( partition by FirstName order by LastName) as
                                                               'DENSE_RANK',
          NTILE(2) over ( partition by FirstName order by LastName) as
                                                                   'NTILE(2)'
FROM      contactSubSet
ORDER     BY FirstName
GO
WITH salesSubset AS
(
     SELECT  product.name as product,  sum(salesOrderDetail.lineTotal) as total
     FROM    sales.salesOrderDetail  as salesOrderDetail
			   JOIN sales.salesOrderHeader as salesOrderHeader
                    ON salesOrderHeader.salesOrderId = salesOrderDetail.salesOrderId
			   JOIN production.product as product
				ON product.productId = salesOrderDetail.productId
	 WHERE   orderDate >= '1/1/2004' and orderDate < '1/1/2005'
	 GROUP   BY product.name
)
SELECT	*
FROM    (SELECT  product, total,
		        RANK() over (order by total desc) as 'RANK',
		        NTILE(10) over (order by total desc) as 'NTILE(10)'
		FROM    salesSubset) as productOrders
WHERE	[NTILE(10)] = 10
gO

----------------------------------------------------------------------
-- DML Enhancements - EXCEPT and INTERSECT
----------------------------------------------------------------------
CREATE TABLE projectPerson
(
	personId     VARCHAR(10),
	projectId    VARCHAR(10),
	PRIMARY KEY (personId, projectId)
)
go

INSERT INTO projectPerson VALUES ('joeb','projBig')
INSERT INTO projectPerson VALUES ('joeb','projLittle')
INSERT INTO projectPerson VALUES ('fredf','projBig')
INSERT INTO projectPerson VALUES ('homerr','projLittle')
INSERT INTO projectPerson VALUES ('stevegr','projBig')
INSERT INTO projectPerson VALUES ('stevegr','projLittle')
go

SELECT	personId
FROM	projectPerson
WHERE	projectId = 'projBig'
UNION
SELECT	personId
FROM	projectPerson
WHERE	projectId = 'projLittle'
GO

--worked only on projectLittle, but not projectBig
SELECT	personId
FROM	projectPerson as projLittle
WHERE	projectId = 'projLittle'
  AND NOT EXISTS (	 SELECT	*
			 FROM	projectPerson as projBig
			 WHERE	projBig.projectId = 'projBig'
			   and	projBig.personId = projLittle.personId)
GO

--worked only on projectLittle, but not projectBig
SELECT	personId
FROM	projectPerson
WHERE	projectId = 'projLittle'
EXCEPT
SELECT	personId
FROM	projectPerson
WHERE	projectId = 'projBig'
go

--worked on both projects.
SELECT	personId
FROM	projectPerson
WHERE	projectId = 'projBig'
INTERSECT
SELECT	personId
FROM	projectPerson
WHERE	projectId = 'projLittle'
go

----------------------------------------------------------------------
-- DML Enhancements - Synonyms
----------------------------------------------------------------------
CREATE SYNONYM Emp FOR AdventureWorks.HumanResources.Employee
go
SELECT * from Emp
go
DROP SYNONYM Emp
go
CREATE LOGIN ANDREW WITH PASSWORD = 'ANDREW1'
CREATE USER ANDREW FOR LOGIN ANDREW
go
CREATE SYNONYM synSecure FOR sales.customer
GO
CREATE VIEW viewSecure --as a contrasting example
AS
SELECT *
FROM sales.customer
GO
GRANT SELECT,INSERT,UPDATE,DELETE  ON synSecure to ANDREW
GRANT SELECT,INSERT,UPDATE,DELETE ON viewSecure to ANDREW
go
EXECUTE AS LOGIN='ANDREW'
--returns permission error
SELECT * from sales.customer
go
SELECT * from viewSecure
SELECT * from synSecure
go
revert --not in text, reverts back to security context of primary user

----------------------------------------------------------------------
-- General Development Enhancements - Error Handling
----------------------------------------------------------------------
CREATE SCHEMA Entertainment
    CREATE TABLE TV
	(	
		TVid int primary key,
		location varchar(20),
		diagonalWidth int
                CONSTRAINT CKEntertainment_tv_checkWidth CHECK (diagonalWidth >= 30)
	)

go

CREATE TABLE dbo.error_log --(adventureworks already has their own error log table)
(	
     tableName sysname,
     userName sysname,
     errorNumber int,
     errorSeverity int,
     errorState int,
     errorMessage varchar(4000)
)
GO

CREATE PROCEDURE entertainment.tv$insert
(
        @TVid             int,    
        @location         varchar(30),
	 @diagonalWidth    int
)
AS
declare @Error int

     BEGIN TRANSACTION

     --Insert a row
     INSERT entertainment.TV (TVid, location, diagonalWidth)
     VALUES(@TVid, @location, @diagonalWidth)

     --save @@ERROR so we don't lose it.
     SET @Error=@@ERROR
     IF @Error<>0
        BEGIN
          -- an error has occurred
          GOTO ErrorHandler
        END

     COMMIT TRANSACTION

GOTO ExitProc

ErrorHandler:
     -- Rollback the transaction
     ROLLBACK TRANSACTION
     -- log the error into the errorlog table
    INSERT dbo.error_log (tableName, userName, 
                         errorNumber, errorSeverity ,errorState ,
                         errorMessage)
     VALUES('TV',suser_sname(),@Error,0,0,'We do not know the message!')
select * from dbo.errorlog

ExitProc:
GO
exec entertainment.tv$insert @TVid = 1, @location = 'Bed Room', @diagonalWidth = 29
go
SELECT * FROM dbo.error_log
go
BEGIN TRY
     RAISERROR ('Something is amiss',16,1)
END TRY
BEGIN CATCH
     select ERROR_NUMBER() as ERROR_NUMBER,
            ERROR_SEVERITY() as ERROR_SEVERITY,
            ERROR_STATE() as ERROR_STATE,
            ERROR_MESSAGE() as ERROR_MESSAGE
END CATCH
go
DELETE FROM entertainment.TV --in case you have added rows
DELETE FROM dbo.error_log
go

ALTER PROCEDURE entertainment.tv$insert
(
     @TVid          int,
     @location	      varchar(30),
     @diagonalWidth int
)
AS
    SET NOCOUNT ON
	BEGIN TRY
		   BEGIN TRANSACTION
		   INSERT TV (TVid, location, diagonalWidth)
                  VALUES(@TVid, @location, @diagonalWidth)
		   COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		    ROLLBACK TRANSACTION
            INSERT dbo.error_log (tableName, userName, 
                                 errorNumber, errorSeverity ,errorState ,
                                 errorMessage)
            VALUES('TV',suser_sname(),ERROR_NUMBER(),
                   ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE())
            RAISERROR ('Error creating new TV row',16,1)
	END CATCH
GO

exec entertainment.tv$insert @TVid = 1, @location = 'Bed Room',
                             @diagonalWidth = 29
GO
SELECT * FROM dbo.error_log
GO

CREATE PROCEDURE dbo.raise_an_error
AS
 BEGIN
       BEGIN TRY
             raiserror ('Boom, boom, boom, boom',16,1)
       END TRY
       BEGIN CATCH  --just catch it, return it as a select,
                   --and raise another error
          SELECT ERROR_NUMBER() AS ErrorNumber,
                 ERROR_SEVERITY() AS ErrorSeverity,
                 ERROR_STATE() as ErrorState, ERROR_LINE() as ErrorLine,
                 ERROR_PROCEDURE() as ErrorProcedure,
                 ERROR_MESSAGE() as ErrorMessage
                 RAISERROR ('Error in procedure raise_an_error',16,1)
       END CATCH
 END
go
SET NOCOUNT ON
BEGIN TRY
    exec raise_an_error --@no_parm = 1 (we will uncomment this for a test
    select 'I am never getting here'
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_SEVERITY() AS ErrorSeverity,
            ERROR_STATE() as ErrorState, ERROR_LINE() as ErrorLine,
            cast(ERROR_PROCEDURE() as varchar(30)) as ErrorProcedure,
            cast(ERROR_MESSAGE() as varchar(40))as ErrorMessage
END CATCH
go
SET NOCOUNT ON
BEGIN TRY
    exec raise_an_error @no_parm = 1
	select 'hi'
END TRY
BEGIN CATCH
END CATCH
go
SET NOCOUNT ON
BEGIN TRY
    exeec procedure --error here is on purpose!

END TRY
BEGIN CATCH
END CATCH
go
SET NOCOUNT ON
BEGIN TRY
    exec ('seeelect *')

END TRY
BEGIN CATCH
      SELECT  ERROR_NUMBER() AS ErrorNumber, ERROR_SEVERITY() AS ErrorSeverity,
              ERROR_STATE() as ErrorState, ERROR_LINE() as ErrorLine,
              cast(ERROR_PROCEDURE() as varchar(60)) as ErrorProcedure,
              cast(ERROR_MESSAGE() as varchar(550))as ErrorMessage
END CATCH
go

ALTER PROCEDURE entertainment.tv$insert
(
        @TVid                int,
        @location            varchar(30),
        @diagonalWidth       int
)
AS
    SET NOCOUNT ON
    DECLARE @errorMessage varchar(2000)
	BEGIN TRY
                 BEGIN TRANSACTION
                 SET @errorMessage = 'Error inserting TV with diagonalWidth / 1'
                 INSERT TV (TVid, location, diagonalWidth)
                 VALUES(@TVid, @location, @diagonalWidth)

                 --second insert:
                 SET @errorMessage = 'Error inserting TV with diagonalWidth / 2'
                 INSERT TV (TVid, location, diagonalWidth)
                 VALUES(@TVid, @location, @diagonalWidth / 2 )

                 COMMIT TRANSACTION
       END TRY
       BEGIN CATCH
                 ROLLBACK TRANSACTION
                 INSERT dbo.error_log VALUES('TV',suser_sname(),
                        ERROR_NUMBER(),ERROR_SEVERITY(),
                        ERROR_STATE(), ERROR_MESSAGE())
                 RAISERROR (@errorMessage,16,1)
       END CATCH
GO

exec entertainment.tv$insert @TVid = 10, @location = 'Bed Room',
                             @diagonalWidth = 29

exec entertainment.tv$insert @TVid = 11, @location = 'Bed Room',
                             @diagonalWidth = 60

----------------------------------------------------------------------
-- General Development Enhancements - .WRITE Extension to the UPDATE Statement
----------------------------------------------------------------------

create table testBIGtext
(
    testBIGtextId  int PRIMARY KEY,
    value          varchar(max)
)
insert into testBIGtext
values(1,'')
go

DECLARE @offset int
SET @offset = 0
WHILE @offset < 26
 BEGIN
        UPDATE testBIGtext
            --the text I am writing is just starting at the letter A --> char(97)
            --and increasing.  the offset is the how may we are in the loop
            --times the number of bytes we are putting in the string, and again
            --the length of the data we are writing
        SET  value.write(replicate(char(97 + @offset),1000),@offset*1000, 1000)
        WHERE  testBIGTextId = 1

        SET @offset = @offset + 1
 END
go
select testBIGtextId, len(value) as CharLength
from  testBIGtext
go

----------------------------------------------------------------------
-- General Development Enhancements - EXECUTE
----------------------------------------------------------------------

--note, if you are not running SQL Server as the default instance, you may
--have to change where I have specified localhost to point to your server instance
EXECUTE sp_addlinkedserver   @server='LocalLinkedServer', @srvproduct='',
                             @provider='SQLOLEDB', @datasrc='.\yukon'

EXECUTE  sp_serveroption 'LocalLinkedServer','RPC OUT',True
go
EXECUTE('SELECT * FROM AdventureWorks.Production.Culture') AT LocalLinkedServer

EXECUTE sp_dropserver LocalLinkedServer
----------------------------------------------------------------------
-- General Development Enhancements - Code security enhancements
----------------------------------------------------------------------

--this user will be the owner of the primary schema
CREATE LOGIN mainOwner WITH PASSWORD = 'mainOwnery'
CREATE USER mainOwner FOR LOGIN mainOwner
GRANT CREATE SCHEMA to mainOwner
GRANT CREATE TABLE to mainOwner

--this will be the procedure creator
CREATE LOGIN secondaryOwner WITH PASSWORD = 'secondaryOwnery'
CREATE USER secondaryOwner FOR LOGIN secondaryOwner
GRANT CREATE SCHEMA to secondaryOwner
GRANT CREATE PROCEDURE to secondaryOwner
GRANT CREATE TABLE to secondaryOwner

--this will be the average user who needs to access data
CREATE LOGIN aveSchlub WITH PASSWORD = 'aveSchluby'
CREATE USER aveSchlub FOR LOGIN aveSchlub
go
EXECUTE AS USER='mainOwner'
Go
CREATE SCHEMA mainOwnersSchema
GO
CREATE TABLE  mainOwnersSchema.person
(
    personId    int constraint PKtestAccess_person primary key,
    firstName   varchar(20),
    lastName    varchar(20)
)
GO

INSERT INTO mainOwnersSchema.person
VALUES (1, 'Paul','McCartney')
INSERT INTO mainOwnersSchema.person
VALUES (2, 'Pete','Townshend')
GO
GRANT SELECT on mainOwnersSchema.person to secondaryOwner
REVERT  --we can step back on the stack of principals,
               --but we can't change directly to secondaryOwner
go
EXECUTE AS USER = 'secondaryOwner'
go

CREATE SCHEMA secondaryOwnerSchema
GO
CREATE TABLE secondaryOwnerSchema.otherPerson
(
    personId    int constraint PKtestAccess_person primary key,
    firstName   varchar(20),
    lastName    varchar(20)
)
GO

INSERT INTO secondaryOwnerSchema.otherPerson
VALUES (1, 'Rocky','Racoon')
INSERT INTO secondaryOwnerSchema.otherPerson
VALUES (2, 'Sally','Simpson')
GO
CREATE PROCEDURE  secondaryOwnerSchema.person$asCaller
WITH EXECUTE AS CALLER --this is the default
AS
SELECT  personId, firstName, lastName
FROM    secondaryOwnerSchema.otherPerson --<-- ownership same as proc
SELECT  personId, firstName, lastName
FROM    mainOwnersSchema.person  --<-- breaks ownership chain
GO

CREATE PROCEDURE secondaryOwnerSchema.person$asSelf
WITH EXECUTE AS SELF  --now this runs in context of secondaryOwner,
                      --since it created it
AS
SELECT  personId, firstName, lastName
FROM    secondaryOwnerSchema.otherPerson --<-- ownership same as proc

SELECT  personId, firstName, lastName
FROM    mainOwnersSchema.person  --<-- breaks ownership chain
GO
GRANT EXECUTE ON secondaryOwnerSchema.person$asCaller to aveSchlub
GRANT EXECUTE ON secondaryOwnerSchema.person$asSelf to aveSchlub
go
REVERT
GO
EXECUTE AS USER = 'aveSchlub'
GO
--this proc is in contet of the caller, in this case, aveSchlub
execute secondaryOwnerSchema.person$asCaller
go
--secondaryOwner, so it works
execute secondaryOwnerSchema.person$asSelf

o a