--Enabling SNAPSHOT isolation

ALTER DATABASE Sales
SET ALLOW_SNAPSHOT_ISOLATION ON




--Enabling READ COMMITTED SNAPSHOT

ALTER DATABASE Sales
SET READ_COMMITTED_SNAPSHOT ON




--Disabling SNAPSHOT isolation

ALTER DATABASE Sales
SET ALLOW_SNAPSHOT_ISOLATION OFF




--Determining which databases have SNAPSHOT isolation enabled

SELECT
    name,
  snapshot_isolation_state,
  is_read_committed_snapshot_on
FROM sys.databases





--Create a table to test SNAPSHOT isolation

CREATE TABLE TestSnapshot
(
  ColA INT,
  ColB VARCHAR(20)
)

INSERT TestSnapshot (ColA, ColB)
VALUES (1, 'Original Value')





--Begin a SNAPSHOT transaction on one connection

SET TRANSACTION ISOLATION LEVEL SNAPSHOT

BEGIN TRANSACTION

SELECT ColB
FROM TestSnapshot
WHERE ColA = 1





--In another connection, the following UPDATE is issued:

UPDATE TestSnapshot
SET ColB = 'New Value'
WHERE ColA = 1





--Selecting into a temp table in a SNAPSHOT transaction

SET TRANSACTION ISOLATION LEVEL SNAPSHOT

BEGIN TRANSACTION

SELECT ColB
INTO #Temp
FROM TestSnapshot
WHERE ColA = 1




--Updating the table, after the select, on another connection

UPDATE TestSnapshot
SET ColB = 'New Value'
WHERE ColA = 1





--Back in the first connection, an error will occur

UPDATE TestSnapshot
SET ColB = 'Even Newer Value'
WHERE ColA = 1





