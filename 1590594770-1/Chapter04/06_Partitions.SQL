--Creating a partition function

CREATE PARTITION FUNCTION pf_FiscalQuarter2005 (DATETIME)
AS RANGE RIGHT FOR VALUES
('20050401', '20050701', '20051001', '20060101')





--Creating a partition scheme, mapping all partitions to PRIMARY

CREATE PARTITION SCHEME ps_FiscalQuarter2005_PRIMARY
AS PARTITION pf_FiscalQuarter2005
ALL TO ([PRIMARY])





--Another partition scheme, mapping partitions to different filegroups

CREATE PARTITION SCHEME ps_FiscalQuarter2005_Split
AS PARTITION pf_FiscalQuarter2005
TO (Q1Q2_2005, Q1Q2_2005, Q3Q4_2005, Q3Q4_2005, Q3Q4_2005)





--Creating a partitioned table

CREATE TABLE SalesAmounts
(
  SalesAmountId INT NOT NULL PRIMARY KEY NONCLUSTERED,
  SalesAmount NUMERIC(9,2) NOT NULL,
  SalesDate DATETIME NOT NULL
)
GO

CREATE CLUSTERED INDEX IX_SalesAmounts_SalesDate
ON SalesAmounts (SalesDate)
  ON ps_FiscalQuarter2005_Split (SalesDate)





--Creating a partitioned index

CREATE INDEX IX_Amount
ON SalesAmounts
(
  SalesAmount
)
ON ps_FiscalQuarter2005_PRIMARY (SalesDate)





--Creating the Visitors table

CREATE TABLE Visitors
(
  VisitorId INT NOT NULL,
  VisitDate DATETIME NOT NULL,
  CONSTRAINT PK_Visitors
    PRIMARY KEY (VisitorId, VisitDate)
)





--Modifying the partition scheme on the Visitors table

SET XACT_ABORT ON

BEGIN TRANSACTION
  ALTER TABLE Visitors
  DROP CONSTRAINT PK_Visitors

  ALTER TABLE Visitors
  ADD CONSTRAINT PK_Visitors
    PRIMARY KEY (VisitorId, VisitDate)
  ON ps_FiscalQuarter2005_Split (VisitDate)
COMMIT





--Merging a partition range

ALTER PARTITION FUNCTION
pf_FiscalQuarter2005()
MERGE RANGE ('20051001') 





--Altering a partition scheme

ALTER PARTITION SCHEME ps_FiscalQuarter2005_Split
NEXT USED [PRIMARY]





--Altering the partition function

ALTER PARTITION FUNCTION
pf_FiscalQuarter2005()
SPLIT RANGE ('20070101')





--Creating the VisitorStaging_2006 table

CREATE TABLE VisitorStaging_2006
(
  VisitorId INT NOT NULL,
  VisitDate DATETIME NOT NULL
)




--The table must have the same PK as the partitioned table

ALTER TABLE VisitorStaging_2006
ADD CONSTRAINT PK_Visitors_2006
  PRIMARY KEY (VisitorId, VisitDate)





--Must also create a CHECK constraint to make sure no invalid data is inserted

ALTER TABLE VisitorStaging_2006
ADD CONSTRAINT CK_Visitors_06012006_12012007
  CHECK (VisitDate >= '20060101' AND VisitDate < '20070101')





--Find the next boundary

SELECT rv.boundary_id
FROM sys.partition_functions f
JOIN sys.partition_range_values rv ON f.function_id = rv.function_id
WHERE rv.value = CONVERT(datetime, '20070101')
  AND f.name = 'pf_FiscalQuarter2005'




--Switch in the new partition

ALTER TABLE VisitorStaging_2006
SWITCH TO Visitors PARTITION 4




--Switch out the partition

ALTER TABLE Visitors
SWITCH PARTITION 4 TO VisitorStaging_2006





--Rebuild only one partition

ALTER INDEX PK_Visitors
ON Visitors
REBUILD
PARTITION = 4

