
--Disallow schema changes in the current database

CREATE TRIGGER NoChanges
ON DATABASE
FOR DDL_DATABASE_LEVEL_EVENTS
AS
    SELECT 'DDL IS NOT ALLOWED IN THE CURRENT DATABASE!'
    SELECT 'TO ALLOW DDL, DROP THE NoChanges trigger.'
    ROLLBACK



--Find all DDL triggers in the current database

SELECT
  name,
  create_date
FROM sys.triggers
WHERE parent_class_desc = 'DATABASE'




--Find all active triggers in the current database

SELECT
  tr.name,
  ev.type_desc
FROM sys.server_triggers tr
JOIN sys.server_trigger_events ev ON tr.object_id = ev.object_id
WHERE tr.is_disabled = 0




--Return event information from a DDL trigger

CREATE TRIGGER ReturnEventData
ON DATABASE
FOR CREATE_TABLE, ALTER_TABLE
AS
  DECLARE @eventData XML
  SET @eventData = eventdata()

  SELECT 
    @eventData.query('data(/EVENT_INSTANCE/UserName)') AS UserName,
    @eventData.query('data(/EVENT_INSTANCE/ObjectName)') AS ObjectName,
    @eventData.query('data(/EVENT_INSTANCE/TSQLCommand/CommandText)') AS CommandText



--Log event information in a DDL trigger

CREATE TABLE DDLEventLog
(
  EventDate DATETIME NOT NULL,
  UserName SYSNAME NOT NULL,
  ObjectName SYSNAME NOT NULL,
  CommandText VARCHAR(MAX) NOT NULL
)
GO

CREATE TRIGGER ReturnEventData
ON DATABASE
FOR CREATE_TABLE, ALTER_TABLE
AS
  DECLARE @eventData XML
  SET @eventData = eventdata()

  INSERT DDLEventLog (EventDate, UserName, ObjectName, CommandText)
  SELECT 
    GETDATE() AS EventDate,
    @eventData.value('data(/EVENT_INSTANCE/UserName)[1]', 'SYSNAME')
      AS UserName,
    @eventData.value('data(/EVENT_INSTANCE/ObjectName)[1]', 'SYSNAME')
      AS ObjectName,
    @eventData.value('data(/EVENT_INSTANCE/TSQLCommand/CommandText)[1]',
      'VARCHAR(MAX)') AS CommandText




--Keep a certain object from being dropped
CREATE TRIGGER DontDropDontDropMe
ON DATABASE
FOR DROP_TABLE
AS
  DECLARE @eventData XML
  SET @eventData = eventdata()

  DECLARE @objectName VARCHAR(MAX)
  SET @objectName = 
    CONVERT(VARCHAR(MAX), @eventData.query('data(/EVENT_INSTANCE/ObjectName)'))

  IF @objectName = 'DontDropMe'
  BEGIN
    PRINT 'You can not drop DontDropMe!'
    ROLLBACK
  END
