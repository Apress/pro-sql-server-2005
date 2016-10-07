USE master
GO

IF NOT EXISTS(SELECT principal_id FROM sys.symmetric_keys WHERE name='##MS_DatabaseMasterKey##')
   CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'gs53&"f"!385';
GO

CREATE CERTIFICATE projEndpointCert
WITH SUBJECT = 'peiriantprawf.julianskinner.local',
     START_DATE = '01/01/2005',
     EXPIRY_DATE = '01/01/2006'
ACTIVE FOR BEGIN_DIALOG = ON;
GO

CREATE ENDPOINT ServiceBrokerEndpoint
   STATE = STARTED
   AS TCP (LISTENER_PORT = 4022)
   FOR SERVICE_BROKER
   (
      AUTHENTICATION = CERTIFICATE projEndpointCert,
      ENCRYPTION = SUPPORTED
   );
GO

BACKUP CERTIFICATE projEndpointCert TO FILE = 'C:\Apress\ProSqlServer\Chapter12\projEndpointCert.cer';
