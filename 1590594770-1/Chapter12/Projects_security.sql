USE Projects
GO

CREATE USER awUser FOR LOGIN [JulianSkinner\Administrator];
GO

CREATE CERTIFICATE awUserCert
AUTHORIZATION awUser
FROM FILE = 'C:\Apress\ProSqlServer\Chapter12\awUserCert.cer';
GO

GRANT CONNECT TO awUser;
GRANT SEND ON SERVICE::[http://schemas.apress.com/prosqlserver/ProjectDetailsService]
TO awUser;
GO
