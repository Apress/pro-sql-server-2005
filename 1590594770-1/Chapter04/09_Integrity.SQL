
--Determine which databases have page verify checksum turned on

SELECT name, page_verify_option
FROM sys.databases




--Put a database into emergency mode

ALTER DATABASE Sales
SET EMERGENCY




--Put a database back online

ALTER DATABASE Sales
SET ONLINE
