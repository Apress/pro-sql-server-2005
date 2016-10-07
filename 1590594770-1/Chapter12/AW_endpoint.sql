USE master
GO

CREATE CERTIFICATE projEndpointCert
FROM FILE = 'C:\Apress\ProSqlServer\Chapter12\projEndpointCert.cer';

IF NOT EXISTS(SELECT principal_id FROM sys.symmetric_keys WHERE name='##MS_DatabaseMasterKey##')
   CREATE MASTER KEY ENCRYPTION BY PASSWORD = '45Gme*3^&fwu';
GO

CREATE CERTIFICATE awEndpointCert
WITH SUBJECT = 'ecspi.julianskinner.local',
     START_DATE = '01/01/2005',
     EXPIRY_DATE = '01/01/2006'
ACTIVE FOR BEGIN_DIALOG = ON;
GO

CREATE ENDPOINT ServiceBrokerEndpoint
   STATE = STARTED
   AS TCP (LISTENER_PORT=4022)
   FOR SERVICE_BROKER
   (
      AUTHENTICATION = CERTIFICATE awEndpointCert,
      ENCRYPTION = SUPPORTED
   );
GO

CREATE LOGIN sbLogin
FROM CERTIFICATE projEndpointCert;
GO

GRANT CONNECT ON ENDPOINT::ServiceBrokerEndpoint TO sbLogin;
GO

BACKUP CERTIFICATE awEndpointCert
TO FILE = 'C:\Apress\ProSqlServer\Chapter12\awEndpointCert.cer';
