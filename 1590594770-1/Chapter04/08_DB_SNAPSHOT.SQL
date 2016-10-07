--Create a database snapshot

CREATE DATABASE Sales_Snapshot_20050901
ON
  (NAME = SalesPrimary, 
    FILENAME = 'F:\Data\SalesPrimary_20040901.ss'),
  (NAME = SalesPrimary_01, 
    FILENAME = 'F:\Data\SalesPrimary_01_20040901.ss')
AS SNAPSHOT OF Sales





--Restore from a database snapshot

RESTORE DATABASE Sales
FROM
  DATABASE_SNAPSHOT = Sales_Snapshot_20050901
