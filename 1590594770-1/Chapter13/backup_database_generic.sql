File: backup_database_generic.sql
:CONNECT $(myConnection)
BACKUP DATABASE $(myDatabase) TO DISK='C:\backups\$(myDatabase).bak'

