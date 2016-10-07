USE master
GO

CREATE CERTIFICATE awEndpointCert
FROM FILE = 'C:\Apress\ProSqlServer\Chapter12\awEndpointCert.cer';
GO

CREATE LOGIN sbLogin
FROM CERTIFICATE awEndpointCert;
GO

GRANT CONNECT ON ENDPOINT::ServiceBrokerEndpoint
TO sbLogin;
GO

CREATE DATABASE Projects;
GO

USE Projects
GO

ALTER DATABASE Projects SET ENABLE_BROKER;
GO

CREATE ROUTE EmployeeProjectDetailsRoute
WITH
   SERVICE_NAME = 'http://schemas.apress.com/prosqlserver/EmployeeProjectDetailsService',
   ADDRESS = 'TCP://ecspi:4022';
GO

CREATE TABLE Project
(
   ProjectID   int IDENTITY PRIMARY KEY,
   ProjectName nvarchar(100),
   StartDate   datetime,
   EndDate     datetime
);

CREATE TABLE Employee
(
   EmployeeID  int IDENTITY PRIMARY KEY,
   FirstName   nvarchar(256),
   LastName    nvarchar(256),
   Email       nvarchar(512)
);

CREATE TABLE ProjectEmployee
(
   ProjectID   int FOREIGN KEY REFERENCES Project(ProjectID),
   EmployeeID  int FOREIGN KEY REFERENCES Employee(EmployeeID)
);
GO

INSERT INTO Project VALUES ('Pro SQL Server 2005', '01/01/2005', '10/15/2005');
INSERT INTO Employee VALUES ('John', 'Doe', 'JohnDoe@apress.com');
INSERT INTO ProjectEmployee VALUES (1, 1);
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
GO

CREATE CONTRACT [http://schemas.apress.com/prosqlserver/ProjectServiceContract]
(
   [http://schemas.apress.com/prosqlserver/ProjectRequestMessage] SENT BY INITIATOR,
   [http://schemas.apress.com/prosqlserver/ProjectResponseMessage] SENT BY TARGET
);
GO

CREATE PROCEDURE usp_GetProjectDetailsForEmployee
AS
DECLARE @msgBody      XML(
           [http://schemas.apress.com/prosqlserver/ProjectRequestSchema]),
        @convID       uniqueidentifier,
        @email        varchar(512),
        @hours        int,
        @startTime    datetime,
        @endTime      datetime,
        @projectCount int,
        @response     XML(
           [http://schemas.apress.com/prosqlserver/ProjectResponseSchema]),
        @respText     nvarchar(1000),
        @msgType      nvarchar(256);

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
      FROM ProjectServiceQueue INTO @msgTable
   ), TIMEOUT 2000;
   
   SET @msgBody = (SELECT TOP (1) CAST(message_body AS XML) FROM @msgTable);
   SET @msgType = (SELECT TOP (1) message_type_name FROM @msgTable);
   SET @convID = (SELECT TOP (1) conversation_handle FROM @msgTable);
   IF @msgType = 'http://schemas.apress.com/prosqlserver/ProjectRequestMessage'
   BEGIN
      SET @email = @msgBody.value('data(//email)[1]', 'varchar(50)');
      SET @hours = @msgBody.value('data(//hours)[1]', 'int');
      SET @startTime = @msgBody.value('data(//startTime)[1]', 'datetime');
      SET @endTime = DATEADD(week, @hours/40, @startTime)

      SET @projectCount = (SELECT COUNT(*)
      FROM Project p
         INNER JOIN ProjectEmployee pe
         ON p.ProjectID = pe.ProjectID
            INNER JOIN Employee e
            ON pe.EmployeeID = e.EmployeeID
      WHERE e.Email = @email
        AND (p.StartDate < @startTime AND p.EndDate > @startTime)
         OR (p.StartDate < @endTime AND p.EndDate > @endTime)
         OR (p.StartDate > @startTime AND p.EndDate < @endTime));

      SET @respText = N'<?xml version="1.0"?>
<projectResponse>
   <email>' + @email + '</email>
   <startTime>' + CONVERT(nvarchar, @startTime, 126) + '+00:00</startTime>
   <activeProjects>' + CAST(@projectCount AS nvarchar) + '</activeProjects>
</projectResponse>';

      SET @response = CAST(@respText AS XML);
      SEND ON CONVERSATION @convID
         MESSAGE TYPE
             [http://schemas.apress.com/prosqlserver/ProjectResponseMessage]
         (@response);

      END CONVERSATION @convID;
   END;
END;
GO

CREATE QUEUE ProjectServiceQueue
WITH
   STATUS = ON,
   RETENTION = OFF,
   ACTIVATION
   (
      STATUS = ON,
      PROCEDURE_NAME = usp_GetProjectDetailsForEmployee,
      MAX_QUEUE_READERS = 5,
      EXECUTE AS SELF
   )

CREATE SERVICE [http://schemas.apress.com/prosqlserver/ProjectDetailsService]
ON QUEUE ProjectServiceQueue
(
   [http://schemas.apress.com/prosqlserver/ProjectServiceContract]
);
GO

CREATE ROUTE EmployeeProjectDetailsRoute
WITH
   SERVICE_NAME =
      'http://schemas.apress.com/prosqlserver/EmployeeProjectDetailsService',
   ADDRESS = 'TCP://ecspi:4022';
GO

CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'gs53&"f"!385';
GO

CREATE CERTIFICATE projUserCert
WITH SUBJECT = 'peiriantprawf.julianskinner.local',
     START_DATE = '01/01/2005',
     EXPIRY_DATE = '01/01/2006'
ACTIVE FOR BEGIN_DIALOG = ON;
GO

BACKUP CERTIFICATE projUserCert TO FILE = 'C:\Apress\ProSqlServer\Chapter12\projUserCert.cer'
