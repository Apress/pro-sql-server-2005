USE AdventureWorks
GO

IF EXISTS(SELECT object_id FROM sys.procedures WHERE name='usp_RequestHoliday')
   DROP PROCEDURE usp_RequestHoliday;

IF EXISTS(SELECT object_id FROM sys.procedures WHERE name='usp_ProcessHolidayRequest')
   DROP PROCEDURE usp_ProcessHolidayRequest;

IF EXISTS(SELECT service_id FROM sys.services WHERE name='http://schemas.apress.com/prosqlserver/HolidayRequestProcessorService')
   DROP SERVICE [http://schemas.apress.com/prosqlserver/HolidayRequestProcessorService];

IF EXISTS(SELECT service_id FROM sys.services WHERE name='http://schemas.apress.com/prosqlserver/HolidayRequestInitiatorService')
   DROP SERVICE [http://schemas.apress.com/prosqlserver/HolidayRequestInitiatorService];

IF EXISTS(SELECT object_id FROM sys.service_queues WHERE name='HolidayRequestQueue')
   DROP QUEUE HolidayRequestQueue;

IF EXISTS(SELECT service_contract_id FROM sys.service_contracts WHERE name='http://schemas.apress.com/prosqlserver/HolidayRequestContract')
   DROP CONTRACT [http://schemas.apress.com/prosqlserver/HolidayRequestContract];

IF EXISTS(SELECT message_type_id FROM sys.service_message_types WHERE name='http://schemas.apress.com/prosqlserver/HolidayRequest')
   DROP MESSAGE TYPE [http://schemas.apress.com/prosqlserver/HolidayRequest];

IF EXISTS(SELECT xml_collection_id FROM sys.xml_schema_collections WHERE name='http://schemas.apress.com/prosqlserver/HolidayRequestSchema')
   DROP XML SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/HolidayRequestSchema];
GO

CREATE XML SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/HolidayRequestSchema]
AS N'<?xml version="1.0" encoding="utf-16"?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
   <xs:element name="vacationRequest">
      <xs:complexType>
         <xs:sequence minOccurs="1" maxOccurs="1">
            <xs:element name="employeeId" type="xs:integer" />
            <xs:element name="email" type="xs:string" />
            <xs:element name="startTime" type="xs:dateTime" />
            <xs:element name="hours" type="xs:integer" />
         </xs:sequence>
      </xs:complexType>
   </xs:element>
</xs:schema>';
GO

CREATE MESSAGE TYPE [http://schemas.apress.com/prosqlserver/HolidayRequest]
VALIDATION = VALID_XML WITH SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/HolidayRequestSchema];

CREATE CONTRACT [http://schemas.apress.com/prosqlserver/HolidayRequestContract]
(
   [http://schemas.apress.com/prosqlserver/HolidayRequest] SENT BY INITIATOR
);
GO

CREATE PROCEDURE usp_ProcessHolidayRequest
AS
DECLARE @msgBody    XML([http://schemas.apress.com/prosqlserver/HolidayRequestSchema]),
        @convID     uniqueidentifier,
        @email      varchar(50),
        @employeeID int,
        @hours      int,
        @startTime  DateTime,
        @hoursTaken int,
        @msgType    nvarchar(256);

DECLARE @msgTable TABLE
(
   message_body        varbinary(max),
   conversation_handle uniqueidentifier,
   message_type_name   nvarchar(256)  
);
BEGIN
   WAITFOR
   (
      RECEIVE TOP (1) message_body, conversation_handle, message_type_name
      FROM HolidayRequestQueue
      INTO @msgTable
   ), TIMEOUT 2000;

   SET @msgBody = (SELECT TOP (1) CAST(message_body AS XML) FROM @msgTable);
   SET @convID = (SELECT TOP (1) conversation_handle FROM @msgTable);
   SET @msgType = (SELECT TOP (1) message_type_name FROM @msgTable);
   END CONVERSATION @convID;
   IF @msgType = 'http://schemas.apress.com/prosqlserver/HolidayRequest'
      BEGIN
         SET @email = @msgBody.value('data(//email)[1]', 'varchar(50)');
         SET @hours = @msgBody.value('data(//hours)[1]', 'int');
         SET @startTime = @msgBody.value('data(//startTime)[1]', 'datetime');
         SET @employeeID = @msgBody.value('data(//employeeId)[1]', 'int');
         SET @hoursTaken = (SELECT VacationHours FROM HumanResources.Employee
                            WHERE EmployeeID = @employeeID);

         IF @hoursTaken + @hours > 160
            EXEC msdb.dbo.sp_send_dbmail
               @profile_name = 'Default Profile',
               @recipients = @email,
               @subject = 'Vacation request',
               @body = 'Your request for vacation has been refused because you
have insufficient hours remaining of your holiday entitlement.';
         ELSE IF @startTime < DATEADD(Week, 1, GETDATE())
            EXEC msdb.dbo.sp_send_dbmail
               @profile_name = 'Default Profile',
               @recipients = @email,
               @subject = 'Vacation request',
               @body = 'Your request for vacation has been refused because you 
have not given sufficient notice. Please request holiday at least a week in 
advance.';
         ELSE
            BEGIN
               UPDATE HumanResources.Employee
                  SET VacationHours = VacationHours + @hours;
               EXEC msdb.dbo.sp_send_dbmail
                  @profile_name = 'Default Profile',
                  @recipients = @email,
                  @subject = 'Vacation request',
                  @body = 'Your request for vacation has been granted.';
            END
      END
END
GO

CREATE QUEUE HolidayRequestQueue
WITH
   STATUS = ON,
   RETENTION = OFF,
   ACTIVATION
   (
      STATUS = ON,
      PROCEDURE_NAME = usp_ProcessHolidayRequest,
      MAX_QUEUE_READERS = 5,
      EXECUTE AS SELF
   );
GO

CREATE SERVICE [http://schemas.apress.com/prosqlserver/HolidayRequestProcessorService]
ON QUEUE HolidayRequestQueue
(
   [http://schemas.apress.com/prosqlserver/HolidayRequestContract]
);

CREATE SERVICE [http://schemas.apress.com/prosqlserver/HolidayRequestInitiatorService]
ON QUEUE HolidayRequestQueue
(
   [http://schemas.apress.com/prosqlserver/HolidayRequestContract]
);
GO

CREATE PROCEDURE usp_RequestHoliday
   @employeeId int,
   @email varchar(50),
   @hours int,
   @startDate varchar(50)
AS
DECLARE @dialogHandle uniqueidentifier,
        @body         nvarchar(1000),
        @msg          XML,
        @date         nvarchar(100)
BEGIN
   SET @body = N'<?xml version="1.0"?>
<vacationRequest>
   <employeeId>' + CAST(@employeeID AS varchar) + '</employeeId>
   <email>' + @email + '</email>
   <startTime>' + @startDate + '</startTime>
   <hours>' + CAST(@hours AS nvarchar) + '</hours>
</vacationRequest>';
   SET @msg = CAST(@body AS XML)

   BEGIN DIALOG CONVERSATION @dialogHandle
      FROM SERVICE
        [http://schemas.apress.com/prosqlserver/HolidayRequestInitiatorService]
      TO SERVICE
        'http://schemas.apress.com/prosqlserver/HolidayRequestProcessorService'
      ON CONTRACT
        [http://schemas.apress.com/prosqlserver/HolidayRequestContract];

   SEND ON CONVERSATION @dialogHandle
      MESSAGE TYPE [http://schemas.apress.com/prosqlserver/HolidayRequest]
      (@msg);
   END CONVERSATION @dialogHandle;
END
GO

EXEC usp_RequestHoliday 140, 'someone@somewhere.com', 8, '2005-11-01T09:00:00+00:00'
