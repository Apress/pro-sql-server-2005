--Create an index online

CREATE INDEX ix_Table
ON Table (Column)
WITH (ONLINE = ON)




--Create an index, disallowing row-level locks

CREATE INDEX ix_Table
ON Table (Column)
WITH (ALLOW_ROW_LOCKS = OFF)




--Create an index, disallowing page-level locks

CREATE INDEX ix_Table
ON Table (Column)
WITH (ALLOW_PAGE_LOCKS = OFF)




--Create an index online, disallowing page and row-level locks

CREATE INDEX ix_Table
ON Table (Column)
WITH (ONLINE = ON, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)




--Create a test table for included indexes

CREATE TABLE DatabaseSystems
(
  DatabaseSystemId INT,
  Name VARCHAR(35),
  IsRelational CHAR(1),
  IsObjectOriented CHAR(1),
  SupportsXML CHAR(1),
  FullSpecifications VARCHAR(MAX)
)

CREATE NONCLUSTERED INDEX IX_Name
ON DatabaseSystems (Name)





--Test query for observing execution plans for the DatabaseSystems table


SELECT Name, SupportsXML
FROM DatabaseSystems
WHERE Name LIKE ‘S%’
AND SupportsXML = ‘Y’




--Typical covering index

DROP INDEX IX_Name
CREATE NONCLUSTERED INDEX IX_Name_SupportsXML
ON DatabaseSystems(Name, SupportsXML)




--Index including one non-key column

CREATE UNIQUE NONCLUSTERED INDEX IX_Name
ON DatabaseSystems(Name)
INCLUDE (SupportsXML)




--Index including two non-key columns

CREATE UNIQUE NONCLUSTERED INDEX IX_Name
ON DatabaseSystems(Name)
INCLUDE (SupportsXML, FullSpecifications)





--Find fragmented indexes

SELECT
  OBJECT_NAME(i.object_id) AS TableName, 
  i.name AS IndexName,
  ips.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'DETAILED') ips
JOIN sys.indexes i ON 
  i.object_id = ips.object_id 
  AND i.index_id = ips.index_id
WHERE ips.avg_fragmentation_in_percent > 10




--Defragment an index

ALTER INDEX IX_CustomerName
ON Customers
REORGANIZE





--Rebuild an index

ALTER INDEX IX_CustomerName
ON Customers
REBUILD
WITH (ONLINE=ON)





--Disable an index

ALTER INDEX IX_CustomerName
ON Customers
DISABLE





--Re-enable an index 

ALTER INDEX IX_CustomerName
ON Customers
REBUILD





