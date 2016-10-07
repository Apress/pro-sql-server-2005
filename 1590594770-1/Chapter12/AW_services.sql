USE AdventureWorks;

CREATE ROUTE ProjectDetailsRoute
WITH
   SERVICE_NAME = 'http://schemas.apress.com/prosqlserver/ProjectDetailsService',
   ADDRESS = 'TCP://peiriantprawf:4022';
GO

CREATE XML SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/ProjectRequestSchema]
AS N'<?xml version="1.0" ?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
   <xs:element name="projectRequest">
      <xs:complexType>
         <xs:sequence minOccurs="1" maxOccurs="1">
            <xs:element name="email" type="xs:string" />
            <xs:element name="startTime" type="xs:dateTime" />
            <xs:element name="hours" type="xs:integer" />
         </xs:sequence>
      </xs:complexType>
   </xs:element>
</xs:schema>';

CREATE XML SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/ProjectResponseSchema]
AS N'<?xml version="1.0" ?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
   <xs:element name="projectResponse">
      <xs:complexType>
         <xs:sequence minOccurs="1" maxOccurs="1">
            <xs:element name="email" type="xs:string" />
            <xs:element name="startTime" type="xs:dateTime" />
            <xs:element name="activeProjects" type="xs:integer" />
         </xs:sequence>
      </xs:complexType>
   </xs:element>
</xs:schema>';

CREATE MESSAGE TYPE [http://schemas.apress.com/prosqlserver/ProjectRequestMessage]
VALIDATION = VALID_XML WITH SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/ProjectRequestSchema];

CREATE MESSAGE TYPE [http://schemas.apress.com/prosqlserver/ProjectResponseMessage]
VALIDATION = VALID_XML WITH SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/ProjectResponseSchema];

CREATE CONTRACT [http://schemas.apress.com/prosqlserver/ProjectServiceContract]
(
   [http://schemas.apress.com/prosqlserver/ProjectRequestMessage] SENT BY INITIATOR,
   [http://schemas.apress.com/prosqlserver/ProjectResponseMessage] SENT BY TARGET
);
GO

CREATE ROUTE ProjectDetailsRoute
WITH
   SERVICE_NAME = 'http://schemas.apress.com/prosqlserver/ProjectDetailsService',
   ADDRESS = 'TCP://peiriantprawf:4022';
GO

CREATE XML SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/EmployeeRequestSchema]
AS N'<?xml version="1.0" ?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
   <xs:element name="employeeRequest">
      <xs:complexType>
         <xs:sequence minOccurs="1" maxOccurs="1">
            <xs:element name="id" type="xs:integer" />
            <xs:element name="hours" type="xs:integer" />
         </xs:sequence>
      </xs:complexType>
   </xs:element>
</xs:schema>';

CREATE XML SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/EmployeeResponseSchema]
AS N'<?xml version="1.0" ?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
   <xs:element name="employeeResponse">
      <xs:complexType>
         <xs:sequence minOccurs="1" maxOccurs="1">
            <xs:element name="id" type="xs:integer" />
            <xs:element name="hoursVacation" type="xs:integer" />
         </xs:sequence>
      </xs:complexType>
   </xs:element>
</xs:schema>';

CREATE MESSAGE TYPE [http://schemas.apress.com/prosqlserver/EmployeeRequestMessage]
VALIDATION = VALID_XML WITH SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/EmployeeRequestSchema];

CREATE MESSAGE TYPE [http://schemas.apress.com/prosqlserver/EmployeeResponseMessage]
VALIDATION = VALID_XML WITH SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/EmployeeResponseSchema];

CREATE CONTRACT [http://schemas.apress.com/prosqlserver/EmployeeServiceContract]
(
   [http://schemas.apress.com/prosqlserver/EmployeeRequestMessage] SENT BY INITIATOR,
   [http://schemas.apress.com/prosqlserver/EmployeeResponseMessage] SENT BY TARGET
);
GO

CREATE PROCEDURE usp_GetHoursVacation
AS
DECLARE @msgBody      XML(
           [http://schemas.apress.com/prosqlserver/EmployeeRequestSchema]),
        @response     XML(
           [http://schemas.apress.com/prosqlserver/EmployeeResponseSchema]),
        @convID       uniqueidentifier,
        @empID        int,
        @hours        int,
        @hoursTaken   int,
        @totalHours   int,
        @msgType      nvarchar(256),
        @respText     nvarchar(1000);

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
      FROM EmployeeDetailsQueue INTO @msgTable
   ), TIMEOUT 2000;
   
   SET @msgBody = (SELECT TOP (1) CAST(message_body AS XML) FROM @msgTable);
   SET @convID = (SELECT TOP (1) conversation_handle FROM @msgTable);
   SET @msgType = (SELECT TOP (1) message_type_name FROM @msgTable);

   IF @msgType = 'http://schemas.apress.com/prosqlserver/EmployeeRequestMessage'
   BEGIN
      SET @empID = @msgBody.value('data(//id)[1]', 'int');
      SET @hours = @msgBody.value('data(//id)[1]', 'int');
      SET @hoursTaken = (SELECT VacationHours FROM HumanResources.Employee
                         WHERE EmployeeID = @empID);
      SET @totalHours = @hoursTaken + @hours;
      SET @respText = N'<?xml version="1.0"?>
<employeeResponse>
   <id>' + CAST(@empID AS nvarchar) + '</id>
   <hoursVacation>' + CAST(@totalHours AS nvarchar) + '</hoursVacation>
</employeeResponse>';

      SET @response = CAST(@respText AS XML);
      SEND ON CONVERSATION @convID
         MESSAGE TYPE [http://schemas.apress.com/prosqlserver/EmployeeResponseMessage]
         (@response);

      END CONVERSATION @convID;
   END;
END;
GO

CREATE QUEUE EmployeeDetailsQueue
WITH
   STATUS = ON,
   RETENTION = OFF,
   ACTIVATION
   (
      STATUS = ON,
      PROCEDURE_NAME = usp_GetHoursVacation,
      MAX_QUEUE_READERS = 5,
      EXECUTE AS SELF
   );

CREATE SERVICE [http://schemas.apress.com/prosqlserver/EmployeeDetailsService]
ON QUEUE EmployeeDetailsQueue
(
   [http://schemas.apress.com/prosqlserver/EmployeeServiceContract]
);
GO

CREATE XML SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/VacationRequestSchema]
AS N'<?xml version="1.0" ?>
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

CREATE MESSAGE TYPE [http://schemas.apress.com/prosqlserver/VacationRequest]
VALIDATION = VALID_XML WITH SCHEMA COLLECTION [http://schemas.apress.com/prosqlserver/VacationRequestSchema]

CREATE CONTRACT [http://schemas.apress.com/prosqlserver/VacationRequestContract]
(
   [http://schemas.apress.com/prosqlserver/VacationRequest] SENT BY INITIATOR
);
GO

CREATE PROCEDURE usp_ProcessVacationRequest
AS
DECLARE @msgBody         XML(
           [http://schemas.apress.com/prosqlserver/VacationRequestSchema]),
        @empRequest      XML(
           [http://schemas.apress.com/prosqlserver/EmployeeRequestSchema]),
        @projRequest     XML(
           [http://schemas.apress.com/prosqlserver/ProjectRequestSchema]),
        @empRequestBody  nvarchar(1000),
        @projRequestBody nvarchar(1000),
        @msgType         nvarchar(256),
        @convID          uniqueidentifier,
        @empConvID       uniqueidentifier,
        @projConvID      uniqueidentifier,
        @email           varchar(50),
        @employeeID      int,
        @hours           int,
        @startTime       DateTime;

DECLARE @msgTable TABLE
(
   message_body          varbinary(max),
   conversation_handle   uniqueidentifier,
   message_type_name     nvarchar(256)
);

BEGIN
   WAITFOR
   (
      RECEIVE TOP (1) message_body, conversation_handle, message_type_name
      FROM VacationRequestQueue INTO @msgTable
   ), TIMEOUT 2000;
   
   SET @msgBody = (SELECT TOP (1) CAST(message_body AS XML) FROM @msgTable);
   SET @convID = (SELECT TOP (1) conversation_handle FROM @msgTable);
   SET @msgType = (SELECT TOP (1) message_type_name FROM @msgTable);
   END CONVERSATION @convID;

   IF @msgType = 'http://schemas.apress.com/prosqlserver/VacationRequest'
   BEGIN
      SET @email = @msgBody.value('data(//email)[1]', 'varchar(50)');
      SET @hours = @msgBody.value('data(//hours)[1]', 'int');
      SET @startTime = @msgBody.value('data(//startTime)[1]', 'datetime');
      SET @employeeID = @msgBody.value('data(//employeeId)[1]', 'int');

      SET @empRequestBody = N'<?xml version="1.0"?><employeeRequest>
   <id>' + CAST(@employeeID AS varchar) + '</id>
   <hours>' + CAST(@hours AS varchar) + '</hours>
</employeeRequest>';
      SET @empRequest = CAST(@empRequestBody AS XML)

      SET @projRequestBody = N'<projectRequest>
   <email>' + @email + '</email>
   <startTime>' + CONVERT(nvarchar, @startTime, 126) + '+00:00</startTime>
   <hours>' + CAST(@hours AS varchar) + '</hours>
</projectRequest>';
      SET @projRequest = CAST(@projRequestBody AS XML)

      BEGIN DIALOG CONVERSATION @empConvID
         FROM SERVICE [http://schemas.apress.com/prosqlserver/EmployeeProjectDetailsService]
         TO SERVICE 'http://schemas.apress.com/prosqlserver/EmployeeDetailsService'
         ON CONTRACT [http://schemas.apress.com/prosqlserver/EmployeeServiceContract];

      SEND ON CONVERSATION @empConvID
         MESSAGE TYPE [http://schemas.apress.com/prosqlserver/EmployeeRequestMessage]
         (@empRequest);

      BEGIN DIALOG CONVERSATION @projConvID
         FROM SERVICE [http://schemas.apress.com/prosqlserver/EmployeeProjectDetailsService]
         TO SERVICE 'http://schemas.apress.com/prosqlserver/ProjectDetailsService'
         ON CONTRACT [http://schemas.apress.com/prosqlserver/ProjectServiceContract]
         WITH RELATED_CONVERSATION = @empConvID, ENCRYPTION=OFF;

      SEND ON CONVERSATION @projConvID
         MESSAGE TYPE [http://schemas.apress.com/prosqlserver/ProjectRequestMessage]
         (@projRequest);
   END
END;
GO

CREATE QUEUE EmployeeProjectDetailsQueue
WITH STATUS = ON, RETENTION = OFF;

CREATE SERVICE [http://schemas.apress.com/prosqlserver/EmployeeProjectDetailsService]
ON QUEUE EmployeeProjectDetailsQueue
(
   [http://schemas.apress.com/prosqlserver/EmployeeServiceContract],
   [http://schemas.apress.com/prosqlserver/ProjectServiceContract]
);
GO

CREATE QUEUE VacationRequestQueue
WITH
   STATUS = ON,
   RETENTION = OFF,
   ACTIVATION
   (
      STATUS = ON,
      PROCEDURE_NAME = usp_ProcessVacationRequest,
      MAX_QUEUE_READERS = 5,
      EXECUTE AS SELF
   );

CREATE SERVICE [http://schemas.apress.com/prosqlserver/VacationRequestProcessorService]
ON QUEUE VacationRequestQueue
(
   [http://schemas.apress.com/prosqlserver/VacationRequestContract]
);
GO

CREATE PROCEDURE usp_ReadResponseMessages
AS
DECLARE @empMsgBody   XML([http://schemas.apress.com/prosqlserver/EmployeeResponseSchema]),
        @projMsgBody  XML([http://schemas.apress.com/prosqlserver/ProjectResponseSchema]),
        @groupId      uniqueidentifier,
        @empConvId    uniqueidentifier,
        @projConvId   uniqueidentifier,
        @activeProj   int,
        @hours        int,
        @empId        int,
        @email        nvarchar(50),
        @startTime    datetime;

DECLARE @msgTable TABLE
(
   message_body         varbinary(max),
   message_type_name    nvarchar(256),
   conversation_handle  uniqueidentifier
);
BEGIN
   WAITFOR
   (
      GET CONVERSATION GROUP @groupID
      FROM EmployeeProjectDetailsQueue
   ), TIMEOUT 500;
   WHILE @groupID IS NOT NULL
   BEGIN
      WAITFOR
      (
         RECEIVE message_body, message_type_name, conversation_handle
         FROM EmployeeProjectDetailsQueue INTO @msgTable
      ), TIMEOUT 2000;

      IF (SELECT COUNT(*) FROM @msgTable) > 0
      BEGIN
         SET @empMsgBody = (SELECT TOP (1) CAST(message_body AS XML)
                            FROM @msgTable
                            WHERE message_type_name = 'http://schemas.apress.com/prosqlserver/EmployeeResponseMessage');
         SET @empConvID = (SELECT TOP (1) conversation_handle FROM @msgTable
                           WHERE message_type_name = 'http://schemas.apress.com/prosqlserver/EmployeeResponseMessage');
         SET @hours = @empMsgBody.value('data(//hoursVacation)[1]', 'int');
         SET @empId = @empMsgBody.value('data(//id)[1]', 'int');

         SET @projMsgBody = (SELECT TOP (1) CAST(message_body AS XML)
                             FROM @msgTable
                             WHERE message_type_name = 'http://schemas.apress.com/prosqlserver/ProjectResponseMessage');
         SET @projConvID = (SELECT TOP (1) conversation_handle FROM @msgTable
                            WHERE message_type_name = 'http://schemas.apress.com/prosqlserver/ProjectResponseMessage');
         SET @activeProj = @projMsgBody.value('data(//activeProjects)[1]', 'int');
         SET @email = @projMsgBody.value('data(//email)[1]', 'varchar');
         SET @startTime = @projMsgBody.value('data(//startTime)[1]', 'datetime');

         IF @hours > 160
            EXEC msdb.dbo.sp_send_dbmail
               @profile_name = 'Default Profile',
               @recipients = @email,
               @subject = 'Vacation request',
               @body = 'Your request for vacation has been refused because you have insufficient hours remaining of your holiday entitlement.';
         ELSE IF @startTime < DATEADD(Week, 1, GETDATE())
            EXEC msdb.dbo.sp_send_dbmail
               @profile_name = 'Default Profile',
               @recipients = @email,
               @subject = 'Default Profile',
               @body = 'Your request for vacation has been refused because you have not given sufficient notice. Please request holiday at least a week in advance.';
         ELSE IF @activeProj > 1
            EXEC msdb.dbo.sp_send_dbmail
               @profile_name = 'Default Profile',
               @recipients = @email,
               @subject = 'Vacation request',
               @body = 'Your request for vacation has been refused because you have too many active projects at that time.';
         ELSE
            BEGIN
               UPDATE HumanResources.Employee
                  SET VacationHours = @hours
                  WHERE EmployeeID = @empId;
               EXEC msdb.dbo.sp_send_dbmail
                  @profile_name = 'Default Profile',
                  @recipients = @email,
                  @subject = 'Vacation request',
                  @body = 'Your request for vacation has been granted.';
            END

         END CONVERSATION @empConvID;
         END CONVERSATION @projConvID;
      END;

      WAITFOR
      (
         GET CONVERSATION GROUP @groupID
         FROM EmployeeProjectDetailsQueue
      ), TIMEOUT 500;
   END;
END;
GO

CREATE USER projUser FOR LOGIN [JulianSkinner\Julian];
GO

CREATE CERTIFICATE projUserCert
AUTHORIZATION projUser
FROM FILE = 'C:\Apress\ProSqlServer\Chapter12\projUserCert.cer';
GO

GRANT CONNECT TO projUser;
GRANT SEND ON SERVICE::[http://schemas.apress.com/prosqlserver/EmployeeProjectDetailsService]
TO projUser;

-- If you don't already have a Master Key for the AdventureWorks database, uncomment these lines.
-- You should also change the password.
--CREATE MASTER KEY ENCRYPTION BY PASSWORD = '45Gme*3^&fwu';
--GO

CREATE CERTIFICATE awUserCert
WITH SUBJECT = 'ecspi.julianskinner.local',
     START_DATE = '01/01/2005',
     EXPIRY_DATE = '01/01/2006'

BACKUP CERTIFICATE awUserCert TO FILE = 'C:\Apress\ProSqlServer\Chapter12\awUserCert.cer';

CREATE REMOTE SERVICE BINDING ProjectDetailsServiceBinding
TO SERVICE 'http://schemas.apress.com/prosqlserver/ProjectDetailsService'
WITH USER = projUser;
GO

CREATE SERVICE [http://schemas.apress.com/prosqlserver/VacationRequestInitiatorService]
ON QUEUE VacationRequestQueue
(
   [http://schemas.apress.com/prosqlserver/VacationRequestContract]
);
GO

CREATE PROCEDURE usp_RequestVacation
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
      FROM SERVICE [http://schemas.apress.com/prosqlserver/VacationRequestInitiatorService]
      TO SERVICE 'http://schemas.apress.com/prosqlserver/VacationRequestProcessorService'
      ON CONTRACT [http://schemas.apress.com/prosqlserver/VacationRequestContract];

   SEND ON CONVERSATION @dialogHandle
      MESSAGE TYPE [http://schemas.apress.com/prosqlserver/VacationRequest]
      (@msg);
   END CONVERSATION @dialogHandle;
END;
