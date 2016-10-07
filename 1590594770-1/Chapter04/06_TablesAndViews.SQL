
--Creating an indexed view with an expression

CREATE VIEW Sales.OrderTotals
WITH SCHEMABINDING
AS
  SELECT
    SalesOrderId, 
    SubTotal + TaxAmt + Freight AS OrderTotal
  FROM Sales.SalesOrderHeader
GO

CREATE UNIQUE CLUSTERED INDEX IX_OrderTotals 
ON Sales.OrderTotals
(OrderTotal, SalesOrderId)




--Persisting a computed column

ALTER TABLE SalesData
  ADD ComplexOutput AS 
    (dbo.VeryComplexFunction(CustomerId, OrderId)) 
    PERSISTED





--Find all persisted columns

SELECT OBJECT_NAME(object_id), name
FROM sys.computed_columns
WHERE is_persisted = 1 





